namespace ApprovalSystem.Application.DTOs.WorkflowRules;

public class CreateWorkflowRuleDto
{
    public Guid WorkflowDefinitionId { get; set; }

    public string FieldName { get; set; } = string.Empty;

    public string Operator { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public Guid TargetRoleId { get; set; }

    public bool IsActive { get; set; } = true;
    public string WorkflowDefinitionName { get; set; } = string.Empty;

    public string TargetRoleName { get; set; } = string.Empty;
}