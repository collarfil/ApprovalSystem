    namespace ApprovalSystem.Core.Entities
    {
public class WorkflowRule
{
    public Guid Id { get; set; }

    public Guid WorkflowDefinitionId { get; set; }
    public WorkflowDefinition WorkflowDefinition { get; set; } = null!;
    public string FieldName { get; set; } = string.Empty;

    public string Operator { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public Guid TargetRoleId { get; set; }
    public Role TargetRole { get; set; } = null!;
    public bool IsActive { get; set; } = true;
}
    }