namespace ApprovalSystem.Core.Entities
{
public class WorkflowStepDefinition
{
    public Guid Id { get; set; }

    public Guid WorkflowDefinitionId { get; set; }

    public WorkflowDefinition WorkflowDefinition { get; set; }
        = null!;

    public int StepOrder { get; set; }

    public string StepName { get; set; } = string.Empty;

    public Guid RoleId { get; set; }
    public bool IsActive { get; set; } = true;
    public Role Role { get; set; } = null!;
}
}