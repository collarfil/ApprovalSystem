using ApprovalSystem.Application.DTOs.WorkflowRules;


namespace ApprovalSystem.Application.Interfaces;

public interface IWorkflowRuleService
{
    Task<WorkflowRuleResponseDto> CreateAsync(
        CreateWorkflowRuleDto dto);

    Task<IEnumerable<WorkflowRuleResponseDto>> GetAllAsync();

    Task<WorkflowRuleResponseDto?> GetByIdAsync(
        Guid id);

    Task DeleteAsync(Guid id);
}