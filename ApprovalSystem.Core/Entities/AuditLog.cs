namespace ApprovalSystem.Core.Entities
{
public class AuditLog
{
    public Guid Id { get; set; }

    public string EntityName { get; set; } = string.Empty;

    public Guid EntityId { get; set; }

    public string Action { get; set; } = string.Empty;

    public string OldValue { get; set; } = string.Empty;

    public string NewValue { get; set; } = string.Empty;

    public Guid PerformedById { get; set; }

    public DateTime Timestamp { get; set; }
}
}