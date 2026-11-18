# Security notes and hardening checklist

This document records what the project does today, what should be fixed before a production deployment, and how. It was written from a code review of the repository.

## What is in place

- JWT bearer authentication with issuer, audience, signing key and lifetime validation, zero clock skew, 60-minute tokens (configurable).
- `[Authorize]` on request, approval, workflow configuration, notification, dashboard and report controllers.
- Requester identity is read from the token (`NameIdentifier` claim) when creating a request.
- Unique indexes on user email and role name.
- Parameterised data access through EF Core (no raw SQL).
- Email failures are isolated from the workflow.

## Do this first: secrets and repository hygiene

The repository history contains `ApprovalSystem.Api/appsettings.json` (committed in several commits) with a JWT signing key and SMTP and database settings, and the `bin/` and `obj/` build folders are tracked (about 300 files). There is no `.gitignore`.

If the repository is public, or ever was, treat those values as exposed:

1. **Rotate** the JWT `SecretKey`, the SMTP password (use an app password and revoke the old one), and the database password.
2. Add the provided `.gitignore`, then stop tracking the files:
   ```bash
   git rm -r --cached .
   git add .
   git commit -m "Apply .gitignore and remove build output and local config"
   ```
3. Keep real settings out of git. Use `appsettings.example.json` as a template, and for real values use environment variables (`ConnectionStrings__DefaultConnection`, `JwtSettings__SecretKey`, `SMTP__Password`) or `dotnet user-secrets`.
4. Optional: remove the old values from history with [`git filter-repo`](https://github.com/newren/git-filter-repo) and force-push. Rotation is what actually protects you; rewriting history only hides the old values.

## Hardening checklist

| # | Finding | Where | Recommended fix |
|---|---|---|---|
| 1 | Passwords are stored and compared as plain text | `AuthService`, `UserService`, `User.Password` | Hash with `PasswordHasher<User>` (Microsoft.AspNetCore.Identity) or BCrypt/Argon2, store only the hash, and migrate existing users by forcing a reset. |
| 2 | No role-based authorization; tokens carry only id and email | `TokenService`, controllers | Add role claims at login and protect admin endpoints with `[Authorize(Roles = "Admin")]` or policies. |
| 3 | Several controllers have no `[Authorize]`: User, Role, UserRole, Department, AuditLog, FileUpload, WorkflowSetup | `ApprovalSystem.Api/Controllers` | Add `[Authorize]` (or a global fallback policy) and restrict administration to an admin role. |
| 4 | Approver identity comes from the request body (`approverId`), so a logged-in user could act as another approver | `ApprovalController`, `WorkflowController` | Read the user id from the JWT claim and ignore the body field. |
| 5 | Any authenticated user can read another user's data by changing the id in the URL | `approval/pending/{approverId}`, `notification/user/{userId}` | Compare the route id with the token's user id, or drop the parameter and use the token. |
| 6 | Registration is anonymous and any caller can create users | `AuthController.Register` | Restrict to admins, or add email verification and a default least-privilege role. |
| 7 | `IsActive` is not checked at login, and `Register` does not set it | `AuthService` | Reject inactive users and set the flag explicitly. |
| 8 | Swagger is exposed in every environment | `Program.cs` | Wrap `UseSwagger` and `UseSwaggerUI` in `IsDevelopment()`, or protect them. |
| 9 | CORS origins are hard-coded to localhost | `Program.cs` | Read allowed origins from configuration per environment. |
| 10 | Generic `Exception` messages can reach clients | services, `RequestController` | Add exception-handling middleware returning `ProblemDetails` and log details server-side. |
| 11 | FluentValidation validators exist but are not registered or invoked | `Validators/` | Register them (for example with `AddValidatorsFromAssemblyContaining`) and validate in controllers or a filter. |
| 12 | Cascading deletes on every foreign key | EF configurations | Use `Restrict` for users and roles and rely on `IsActive` to retain history. |
| 13 | No rate limiting on login | `AuthController.Login` | Add ASP.NET Core rate limiting and account lockout. |
| 14 | No refresh tokens; 60-minute access tokens only | `TokenService` | Add refresh tokens or shorten and rotate as needed. |
| 15 | HTTPS redirection and HSTS are not configured | `Program.cs` | Add `UseHttpsRedirection()` and `UseHsts()` for production. |

## Correctness issues found during review

These are not security problems but affect behaviour:

- **File records:** `FileUpload` has a `User` navigation with no matching foreign key property, so EF created a required shadow column `UserId`, while `FileUploadService` only sets `UploadedBy`. Inserting a record is likely to fail the foreign key. Fix by mapping the relationship to `UploadedBy`: `builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UploadedBy);` and adding a migration. Also note that the endpoint stores metadata only; no file bytes are saved.
- **Silent workflow failure:** `RequestService.CreateAsync` catches and swallows errors from `StartWorkflowAsync`. If no user holds the step's role, the request is saved as `Pending` with no approval step, and the API still returns success. Return an error or mark the request so it can be retried.
- **Rules apply to all steps:** routing rules are evaluated for the whole workflow, so a matching rule assigns every step to the same target role.
- **Escalation:** `EscalateStepAsync` does not verify that the caller is the current approver and does not reassign the request.
- **Unmapped classes:** `Comment` and `AuditHistory` have no table.
- **Dependency injection:** several services are registered in both `Program.cs` and the `AddApplication` / `AddInfrastructure` extension methods. Consolidating registration in the extension methods would remove the duplication.

## Reporting a vulnerability

If you find a security issue, please open a private security advisory on GitHub (Security tab) or contact the maintainer directly rather than filing a public issue.
