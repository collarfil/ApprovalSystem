using ApprovalSystem.Application.DTOs.WorkflowStepDefinitions;

namespace ApprovalSystem.Application.Interfaces;

public interface IWorkflowStepDefinitionService
{
    Task<WorkflowStepDefinitionResponseDto> CreateAsync(
        CreateWorkflowStepDefinitionDto dto);

    Task<IEnumerable<WorkflowStepDefinitionResponseDto>> GetAllAsync();

    Task<WorkflowStepDefinitionResponseDto?> GetByIdAsync(
        Guid id);

    Task DeleteAsync(Guid id);
}