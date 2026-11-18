# Contributing

Thanks for your interest in improving Approval System.

## Getting set up

Follow the [Getting started](README.md#getting-started) section. Never commit `appsettings.json` or any secret.

## Workflow

1. Fork the repository and create a branch from `main`:
   `feature/<short-name>`, `fix/<short-name>` or `docs/<short-name>`.
2. Make focused changes. Keep controllers thin and put business rules in `ApprovalSystem.Application`.
3. Build and run before opening a pull request:
   ```bash
   dotnet build
   ```
4. If you changed an entity, add a migration (from `ApprovalSystem.Infrastructure`):
   ```bash
   dotnet ef migrations add <DescriptiveName> --startup-project ../ApprovalSystem.Api
   ```
5. Open a pull request describing what changed and why.

## Commit messages

Use short, imperative messages, optionally with a prefix:

```
feat: add parallel approval steps
fix: validate approver against JWT claim
docs: update workflow engine guide
refactor: move DI registration to extension methods
```

## Code style

- Follow standard C# conventions and the existing project layout.
- `Nullable` is enabled; avoid suppressing warnings.
- One class per file. Name services `XService` with an `IXService` interface in `Application/Interfaces`.
- Do not return entities from controllers; use DTOs.

## Adding a feature (checklist)

- [ ] Entity and repository interface in `Core`
- [ ] EF configuration and repository in `Infrastructure`
- [ ] DTOs, service interface and implementation in `Application`
- [ ] Controller in `Api` with `[Authorize]`
- [ ] DI registration
- [ ] Migration
- [ ] Documentation updated in `docs/`
