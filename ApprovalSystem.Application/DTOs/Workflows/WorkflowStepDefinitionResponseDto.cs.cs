namespace ApprovalSystem.Application.DTOs.WorkflowStepDefinitions;

public class WorkflowStepDefinitionResponseDto
{
    public Guid Id { get; set; }

    public Guid WorkflowDefinitionId { get; set; }

    public string WorkflowDefinitionName { get; set; } = string.Empty;

    public int StepOrder { get; set; }

    public string StepName { get; set; } = string.Empty;

    public Guid RoleId { get; set; }

    public string RoleName { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}