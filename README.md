# Approval System

**A configurable, multi-step approval workflow API built with Clean Architecture on ASP.NET Core.**

Employees submit requests (purchases, expenses, any amount-based approval). The system routes each request through an ordered chain of approvers, picks the right approver by role and amount rules, notifies them in-app and by email, and records every action in an audit trail.

![.NET](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-Web%20API-512BD4)
![EF Core](https://img.shields.io/badge/EF%20Core-10-512BD4)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-Npgsql-4169E1?logo=postgresql&logoColor=white)
![Auth](https://img.shields.io/badge/Auth-JWT-000000?logo=jsonwebtokens&logoColor=white)

> **Demo video:** _add your YouTube link here_

---

## Table of contents

1. [Features](#features)
2. [Tech stack](#tech-stack)
3. [Architecture](#architecture)
4. [How the workflow engine works](#how-the-workflow-engine-works)
5. [Database design](#database-design)
6. [Project structure](#project-structure)
7. [Getting started](#getting-started)
8. [API overview](#api-overview)
9. [Documentation](#documentation)
10. [Roadmap](#roadmap)
11. [Author](#author)

---

## Features

- **Configurable workflows**: define workflow templates made of ordered steps, each mapped to a role.
- **Rule-based routing**: rules on the request `Amount` (`>`, `<`, `>=`, `<=`, `=`) decide which role approves.
- **Approve / reject / escalate** actions with comments, tracked per approval step.
- **Audit trail**: workflow start, approvals, step changes, rejections, escalations and completion are logged.
- **Notifications**: in-app notifications stored in the database, plus email through SMTP (MailKit). Email failures never block the workflow.
- **Users, roles and departments** management, with many-to-many user-to-role assignment.
- **Reports and dashboard**: totals, request trend, requests by status and department, approval time statistics, user activity.
- **JWT authentication** and interactive **Swagger** documentation.
- **Soft-cancel**: deleting a request marks it `Cancelled` instead of removing it.

## Tech stack

| Area | Technology |
|---|---|
| Runtime / framework | .NET 10, ASP.NET Core Web API |
| ORM | Entity Framework Core 10 (code-first migrations) |
| Database | PostgreSQL via Npgsql |
| Authentication | JWT Bearer (HMAC-SHA256) |
| Validation | FluentValidation |
| Email | MailKit (SMTP, STARTTLS) |
| API docs | Swashbuckle (Swagger UI) |
| Architecture | Clean Architecture, Repository pattern, dependency injection |

## Architecture

The solution is split into four projects. Dependencies point inward: the domain (`Core`) knows nothing about frameworks, and the outer layers depend on it.

```mermaid
flowchart LR
    Client["API clients<br/>Swagger UI, React app"]

    subgraph API["ApprovalSystem.Api"]
        MW["CORS and JWT authentication"]
        CTRL["Controllers"]
    end

    subgraph APP["ApprovalSystem.Application"]
        SVC["Services"]
        ENG["WorkflowEngine"]
        DTO["DTOs and validators"]
    end

    subgraph CORE["ApprovalSystem.Core"]
        ENT["Entities and enums"]
        IREPO["Repository interfaces"]
    end

    subgraph INFRA["ApprovalSystem.Infrastructure"]
        REPO["EF Core repositories"]
        CTX["ApprovalDbContext"]
        MAIL["EmailService"]
    end

    PG[("PostgreSQL")]
    SMTP["SMTP server"]

    Client --> MW --> CTRL --> SVC
    SVC --> ENG
    SVC --> IREPO
    ENG --> IREPO
    IREPO -.implemented by.-> REPO
    REPO --> CTX --> PG
    ENG -.IEmailService.-> MAIL --> SMTP
```

| Project | Responsibility |
|---|---|
| `ApprovalSystem.Core` | Entities, enums, repository interfaces. No external dependencies. |
| `ApprovalSystem.Application` | Business logic: services, the workflow engine, DTOs, validators, settings classes. Depends on Core only. |
| `ApprovalSystem.Infrastructure` | EF Core `DbContext`, entity configurations, migrations, repositories, SMTP email service. |
| `ApprovalSystem.Api` | Controllers, JWT and CORS setup, Swagger, composition root (`Program.cs`). |

More detail in [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md).

## How the workflow engine works

When a request is submitted, the engine loads the workflow's active steps, picks an approver for the first step, creates an `ApprovalStep`, and notifies the approver. Each approval creates the next step until none remain, at which point the request is `Approved`.

```mermaid
sequenceDiagram
    actor R as Requester
    participant API as RequestController
    participant RS as RequestService
    participant WE as WorkflowEngine
    participant DB as PostgreSQL
    actor A as Approver

    R->>API: POST /api/request (JWT)
    API->>RS: CreateAsync(dto, userId from token)
    RS->>DB: insert Request (Pending)
    RS->>WE: StartWorkflowAsync(request)
    WE->>DB: load active steps and rules
    WE->>WE: determine approver (amount rules, else step role)
    WE->>DB: insert ApprovalStep (Pending)
    WE-->>A: in-app notification and email
    WE->>DB: insert AuditLog
    A->>API: POST /api/approval/approve
    API->>WE: ApproveStepAsync
    WE->>DB: mark step Approved
    alt more steps remain
        WE->>DB: insert next ApprovalStep
        WE-->>A: notify next approver
    else final step
        WE->>DB: set Request to Approved
        WE-->>R: notification and email
    end
```

Request status lifecycle:

```mermaid
stateDiagram-v2
    [*] --> Pending: request submitted
    Pending --> Pending: step approved, next step created
    Pending --> Approved: final step approved
    Pending --> Rejected: approver rejects
    Pending --> Escalated: approver escalates
    Pending --> Cancelled: requester cancels
    Approved --> [*]
    Rejected --> [*]
```

Full explanation, including approver selection rules: [docs/WORKFLOW_ENGINE.md](docs/WORKFLOW_ENGINE.md).

## Database design

PostgreSQL, 12 tables, managed with EF Core migrations.

```mermaid
erDiagram
    Departments ||--o{ Users : has
    Users ||--o{ UserRoles : assigned
    Roles ||--o{ UserRoles : granted
    WorkflowDefinitions ||--o{ WorkflowStepDefinitions : contains
    WorkflowDefinitions ||--o{ WorkflowRules : "routed by"
    Roles ||--o{ WorkflowStepDefinitions : "approves step"
    Roles ||--o{ WorkflowRules : "rule target"
    WorkflowDefinitions ||--o{ Requests : governs
    Users ||--o{ Requests : submits
    Requests ||--o{ ApprovalSteps : "has steps"
    Users ||--o{ ApprovalSteps : "approver"
    Users ||--o{ ApprovalSteps : "assigned to"
    Users ||--o{ Notifications : receives
    Users ||--o{ FileUploads : uploads

    Departments {
        uuid Id PK
        varchar Name
        text Description
        boolean IsActive
    }
    Users {
        uuid Id PK
        varchar FirstName
        varchar LastName
        varchar Email UK
        text Password
        boolean IsActive
        uuid DepartmentId FK
    }
    Roles {
        uuid Id PK
        varchar Name UK
        text Description
        boolean IsActive
    }
    UserRoles {
        uuid UserId PK
        uuid RoleId PK
    }
    WorkflowDefinitions {
        uuid Id PK
        varchar Name
        varchar Description
        boolean IsActive
    }
    WorkflowStepDefinitions {
        uuid Id PK
        uuid WorkflowDefinitionId FK
        integer StepOrder
        varchar StepName
        uuid RoleId FK
        boolean IsActive
    }
    WorkflowRules {
        uuid Id PK
        uuid WorkflowDefinitionId FK
        varchar FieldName
        varchar Operator
        varchar Value
        uuid TargetRoleId FK
        boolean IsActive
    }
    Requests {
        uuid Id PK
        varchar Title
        text Description
        decimal Amount
        integer Status
        uuid CurrentApprovalStepId
        uuid WorkflowDefinitionId FK
        uuid RequestedById FK
        timestamptz CreatedAt
    }
    ApprovalSteps {
        uuid Id PK
        uuid RequestId FK
        integer StepOrder
        uuid AssignedToUserId FK
        uuid ApproverId FK
        integer Status
        timestamptz ActionDate
        varchar Comment
    }
    AuditLogs {
        uuid Id PK
        varchar EntityName
        uuid EntityId
        varchar Action
        text OldValue
        text NewValue
        uuid PerformedById
        timestamptz Timestamp
    }
    Notifications {
        uuid Id PK
        uuid UserId FK
        text Title
        text Message
        boolean IsRead
        timestamptz CreatedAt
    }
    FileUploads {
        uuid Id PK
        text FileName
        text StoredFileName
        text ContentType
        bigint FileSize
        text FilePath
        uuid UploadedBy
        timestamptz UploadedAt
    }
```

Column-level data dictionary, enums, indexes and migration history: [docs/DATABASE.md](docs/DATABASE.md).

## Project structure

```
ApprovalSystem/
├── ApprovalSystem.slnx
├── ApprovalSystem.Api/
│   ├── Controllers/              # REST controllers (one per resource)
│   ├── Program.cs                # DI, JWT, CORS, Swagger
│   ├── appsettings.example.json  # copy to appsettings.json and fill in
│   └── Properties/launchSettings.json
├── ApprovalSystem.Application/
│   ├── Services/                 # business logic + WorkflowEngine
│   ├── Interfaces/               # service contracts
│   ├── DTOs/                     # request/response models by feature
│   ├── Validators/               # FluentValidation rules
│   └── Common/                   # JwtSettings, SmtpSettings
├── ApprovalSystem.Core/
│   ├── Entities/                 # domain entities
│   ├── Enums/                    # RequestStatus, ApprovalStatus
│   └── Interfaces/               # repository contracts
├── ApprovalSystem.Infrastructure/
│   ├── Persistence/              # DbContext, design-time factory, Migrations/
│   ├── Configuration/            # EF entity type configurations
│   ├── Repositories/             # repository implementations
│   └── Services/EmailService.cs  # MailKit SMTP sender
├── docs/                         # detailed documentation
└── promo-video/                  # Remotion project that renders the demo video
```

## Getting started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- PostgreSQL (local install or Docker)
- `dotnet-ef` tool: `dotnet tool install --global dotnet-ef`
- An SMTP account if you want email notifications (optional; in-app notifications work without it)

### 1. Clone

```bash
git clone https://github.com/collarfil/ApprovalSystem.git
cd ApprovalSystem
```

### 2. Configure

```bash
cp ApprovalSystem.Api/appsettings.example.json ApprovalSystem.Api/appsettings.json
```

Edit `appsettings.json` and set your PostgreSQL connection string, a long random JWT `SecretKey` (at least 32 characters), and your SMTP details. **Never commit this file.** It is listed in `.gitignore`.

Generate a key:

```bash
openssl rand -base64 32
```

### 3. Create the database

Run from the Infrastructure project folder (the design-time factory reads `../ApprovalSystem.Api/appsettings.json`):

```bash
cd ApprovalSystem.Infrastructure
dotnet ef database update --startup-project ../ApprovalSystem.Api
```

### 4. Run

```bash
cd ..
dotnet run --project ApprovalSystem.Api
```

The API listens on `http://localhost:5242` (and `https://localhost:7234` with the https profile). Open Swagger UI at **http://localhost:5242/swagger**.

### 5. Try it

A step-by-step walkthrough (create roles, users, a workflow, submit and approve a request) is in [docs/API.md](docs/API.md#end-to-end-walkthrough).

## API overview

All routes are under `/api`. Full reference with request bodies: [docs/API.md](docs/API.md).

| Resource | Base route | Highlights |
|---|---|---|
| Auth | `/api/auth` | `POST register`, `POST login` (returns JWT) |
| Requests | `/api/request` | create, list, get, update, cancel |
| Approvals | `/api/approval` | `POST approve`, `POST reject`, `POST escalate`, `GET pending/{approverId}` |
| Workflow setup | `/api/workflowsetup`, `/api/workflowdefinition`, `/api/workflowstepdefinition`, `/api/workflowrule` | define templates, steps and routing rules |
| Directory | `/api/user`, `/api/role`, `/api/userrole`, `/api/department` | users, roles, assignments, departments |
| Notifications | `/api/notification` | per-user list, unread, count, mark as read |
| Audit | `/api/auditlog` | read the audit trail |
| Files | `/api/fileupload` | file metadata records |
| Reports | `/api/report`, `/api/dashboard` | dashboard, trend, by status, by department, approval time, user activity |

Authenticate by sending `Authorization: Bearer <token>`.

## Documentation

| Document | What it covers |
|---|---|
| [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md) | Layers, dependency rule, request lifecycle, design decisions |
| [docs/WORKFLOW_ENGINE.md](docs/WORKFLOW_ENGINE.md) | Step processing, approver selection, status transitions |
| [docs/DATABASE.md](docs/DATABASE.md) | ERD, data dictionary, enums, indexes, migrations |
| [docs/API.md](docs/API.md) | Endpoint reference and end-to-end walkthrough |
| [docs/SECURITY.md](docs/SECURITY.md) | Current security posture and hardening checklist |
| [CONTRIBUTING.md](CONTRIBUTING.md) | Branching, commit style, how to add a feature |
| [CHANGELOG.md](CHANGELOG.md) | Notable changes |

## Roadmap

- [ ] Password hashing and role-based authorization policies (see [docs/SECURITY.md](docs/SECURITY.md))
- [ ] Take the approver identity from the JWT instead of the request body
- [ ] Global exception handling with `ProblemDetails` responses
- [ ] Register and enforce FluentValidation validators
- [ ] Real file storage behind the file upload endpoints
- [ ] Unit tests (workflow engine) and integration tests (API + database)
- [ ] Dockerfile and `docker-compose` for API + PostgreSQL
- [ ] CI pipeline (build, test) with GitHub Actions
- [ ] React front end (CORS is already configured for a dev client on ports 5173 and 5177)

## Author

**Collins**, Backend Software Engineer. Clean Architecture and multi-tenant SaaS with ASP.NET Core.

GitHub: [@collarfil](https://github.com/collarfil)

## License

Released under the [MIT License](LICENSE).
