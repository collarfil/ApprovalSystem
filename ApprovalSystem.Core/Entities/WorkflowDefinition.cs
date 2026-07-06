namespace ApprovalSystem.Core.Entities
{
    public class WorkflowDefinition
    {
        public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public ICollection<WorkflowStepDefinition> StepDefinitions
        = new List<WorkflowStepDefinition>();
}
}