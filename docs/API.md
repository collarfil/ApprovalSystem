# API reference

Base URL (development): `http://localhost:5242`. Interactive docs: `/swagger`.
Routes follow `api/[controller]` and are case-insensitive (`/api/Request` = `/api/request`).

**Auth column:** `JWT` = the controller has `[Authorize]`. `None` = the controller currently has no `[Authorize]` attribute (see [SECURITY.md](SECURITY.md)).

## Auth
| Method | Route | Auth | Description |
|---|---|---|---|
| POST | `/api/auth/register` | None | Create a user (`firstName`, `lastName`, `email`, `password`, `departmentId`) |
| POST | `/api/auth/login` | None | Returns `id`, `email`, `token` |

## Requests
| Method | Route | Auth | Description |
|---|---|---|---|
| POST | `/api/request` | JWT | Submit a request and start its workflow. The requester id comes from the token. |
| GET | `/api/request` | JWT | List requests |
| GET | `/api/request/{id}` | JWT | Get one request |
| PUT | `/api/request/{id}` | JWT | Update `title`, `description`, `amount` |
| DELETE | `/api/request/{id}` | JWT | Cancel (sets status `Cancelled`) |

## Approvals
| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/api/approval/pending/{approverId}` | JWT | Pending steps for an approver |
| POST | `/api/approval/approve` | JWT | Body: `requestId`, `approverId`, `comment` |
| POST | `/api/approval/reject` | JWT | Body: `requestId`, `approverId`, `reason` |
| POST | `/api/approval/escalate` | JWT | Body: `requestId`, `approverId`, `reason` |
| POST | `/api/workflow/approve` \| `reject` \| `escalate` | JWT | Same actions, calling the engine directly |

## Workflow configuration
| Method | Route | Auth | Description |
|---|---|---|---|
| POST | `/api/workflowsetup/create-template` | None | Create a workflow with its steps in one call |
| POST/GET/GET{id}/DELETE | `/api/workflowdefinition` | JWT | Workflow templates |
| POST/GET/GET{id}/DELETE | `/api/workflowstepdefinition` | JWT | Steps (`workflowDefinitionId`, `stepOrder`, `stepName`, `roleId`, `isActive`) |
| POST/GET/GET{id}/DELETE | `/api/workflowrule` | JWT | Routing rules (`workflowDefinitionId`, `fieldName`, `operator`, `value`, `targetRoleId`) |

## Directory
| Method | Route | Auth | Description |
|---|---|---|---|
| POST/GET/GET{id}/DELETE | `/api/department` | None | Departments (`name`, `description`) |
| POST/GET/GET{id}/DELETE | `/api/role` | None | Roles (`name`, `description`) |
| POST/GET/GET{id}/DELETE | `/api/user` | None | Users (delete deactivates the user) |
| POST/GET/DELETE | `/api/userrole` | None | Assign or remove a role (`userId`, `roleId`) |

## Notifications
| Method | Route | Auth | Description |
|---|---|---|---|
| POST | `/api/notification` | JWT | Create (`userId`, `title`, `message`) |
| GET | `/api/notification` | JWT | All |
| GET | `/api/notification/{id}` | JWT | One |
| GET | `/api/notification/user/{userId}` | JWT | For a user |
| GET | `/api/notification/user/{userId}/unread` | JWT | Unread for a user |
| GET | `/api/notification/user/{userId}/count` | JWT | Unread count |
| PATCH | `/api/notification/{id}/read` | JWT | Mark as read |
| DELETE | `/api/notification/{id}` | JWT | Delete |

## Audit and files
| Method | Route | Auth | Description |
|---|---|---|---|
| POST/GET/GET{id} | `/api/auditlog` | None | Audit entries |
| POST/GET/GET{id}/DELETE | `/api/fileupload` | None | File metadata (`fileName`, `storedFileName`, `filePath`, `contentType`, `fileSize`, `uploadedBy`) |

## Reports
| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/api/dashboard/summary` | JWT | Counts for users, requests by status, notifications, audit logs, files |
| GET | `/api/report/dashboard` | JWT | Dashboard statistics |
| GET | `/api/report/trend?days=30` | JWT | Requests per day |
| GET | `/api/report/by-status` | JWT | Requests grouped by status |
| GET | `/api/report/by-department` | JWT | Requests grouped by department |
| GET | `/api/report/approval-time` | JWT | Average, minimum and maximum approval time in hours |
| GET | `/api/report/user-activity` | JWT | Requests and approvals per user |

## End-to-end walkthrough

Set a variable for convenience, then follow the steps. Replace the `<...>` placeholders with ids returned by earlier calls.

```bash
BASE=http://localhost:5242
```

**1. Create a department and roles**

```bash
curl -X POST $BASE/api/department -H "Content-Type: application/json" \
  -d '{"name":"Operations","description":"Operations team"}'

curl -X POST $BASE/api/role -H "Content-Type: application/json" \
  -d '{"name":"Manager","description":"First-level approver"}'

curl -X POST $BASE/api/role -H "Content-Type: application/json" \
  -d '{"name":"Finance","description":"Second-level approver"}'
```

**2. Register users** (a requester, a manager and a finance officer)

```bash
curl -X POST $BASE/api/auth/register -H "Content-Type: application/json" \
  -d '{"firstName":"Ada","lastName":"Obi","email":"ada@example.com","password":"ChangeMe123!","departmentId":"<departmentId>"}'
```

Repeat for the other two users.

**3. Assign roles**

```bash
curl -X POST $BASE/api/userrole -H "Content-Type: application/json" \
  -d '{"userId":"<managerUserId>","roleId":"<managerRoleId>"}'
```

Do the same for the finance user and role.

**4. Create the workflow with two steps**

```bash
curl -X POST $BASE/api/workflowsetup/create-template -H "Content-Type: application/json" \
  -d '{
    "name":"Purchase approval",
    "description":"Manager then Finance",
    "steps":[
      {"stepOrder":1,"stepName":"Manager review","roleId":"<managerRoleId>"},
      {"stepOrder":2,"stepName":"Finance review","roleId":"<financeRoleId>"}
    ]
  }'
```

The response contains `workflowDefinitionId`.

**5. Log in as the requester**

```bash
curl -X POST $BASE/api/auth/login -H "Content-Type: application/json" \
  -d '{"email":"ada@example.com","password":"ChangeMe123!"}'
```

Copy the `token`.

**6. Submit a request**

```bash
curl -X POST $BASE/api/request -H "Content-Type: application/json" -H "Authorization: Bearer <token>" \
  -d '{"title":"Office laptops","description":"5 laptops for the new team","amount":2500000,"workflowDefinitionId":"<workflowDefinitionId>"}'
```

**7. Approve as the manager** (log in as the manager to get a token first)

```bash
curl $BASE/api/approval/pending/<managerUserId> -H "Authorization: Bearer <managerToken>"

curl -X POST $BASE/api/approval/approve -H "Content-Type: application/json" -H "Authorization: Bearer <managerToken>" \
  -d '{"requestId":"<requestId>","approverId":"<managerUserId>","comment":"Looks good"}'
```

The request moves to the Finance step. Repeat as the finance user. After the last approval, `GET /api/request/<requestId>` shows status `Approved`.

### Optional: routing rules

```bash
curl -X POST $BASE/api/workflowrule -H "Content-Type: application/json" -H "Authorization: Bearer <token>" \
  -d '{"workflowDefinitionId":"<workflowDefinitionId>","fieldName":"Amount","operator":">","value":"1000000","targetRoleId":"<directorRoleId>"}'
```

Rules apply to the whole workflow, not to a single step. When a rule matches, **every** step of that request is assigned to a user holding the rule's target role. Use rules for "large amounts go to a senior approver" and keep that in mind when designing multi-step workflows. See [WORKFLOW_ENGINE.md](WORKFLOW_ENGINE.md#choosing-the-approver).
