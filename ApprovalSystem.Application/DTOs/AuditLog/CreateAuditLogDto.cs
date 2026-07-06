namespace ApprovalSystem.Application.DTOs.AuditLogs;

public class CreateAuditLogDto
{
    public string EntityName { get; set; } = string.Empty;

    public Guid EntityId { get; set; }

    public string Action { get; set; } = string.Empty;

    public string OldValue { get; set; } = string.Empty;

    public string NewValue { get; set; } = string.Empty;

    public Guid PerformedById { get; set; }
}