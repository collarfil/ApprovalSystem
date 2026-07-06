namespace ApprovalSystem.Application.DTOs.WorkflowStepDefinitions;

public class CreateWorkflowStepDefinitionDto
{
    public Guid WorkflowDefinitionId { get; set; }

    public int StepOrder { get; set; }

    public string StepName { get; set; } = string.Empty;

    public Guid RoleId { get; set; }

    public bool IsActive { get; set; } = true;
}