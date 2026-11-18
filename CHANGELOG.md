# Changelog

All notable changes to this project are documented here. Format based on [Keep a Changelog](https://keepachangelog.com/).

## [Unreleased]

### Added
- Project documentation: README, architecture, workflow engine, database, API and security guides.
- `appsettings.example.json` and `.gitignore`.

## [0.1.0] - Initial version

### Added
- Clean Architecture solution (Core, Application, Infrastructure, Api) on .NET 10.
- PostgreSQL persistence with EF Core migrations.
- JWT authentication and Swagger.
- Configurable workflows with ordered steps and amount-based routing rules.
- Approve, reject and escalate actions with audit logging.
- In-app notifications and SMTP email notifications.
- Users, roles, departments and role assignment.
- Dashboard and report endpoints.
