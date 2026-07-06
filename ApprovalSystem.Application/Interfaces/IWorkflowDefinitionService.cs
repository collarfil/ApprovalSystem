using ApprovalSystem.Application.DTOs.Workflow;

namespace ApprovalSystem.Application.Interfaces;

public interface IWorkflowDefinitionService
{
    Task<WorkflowDefinitionResponseDto> CreateAsync(
        CreateWorkflowDefinitionDto dto);


    Task<IEnumerable<WorkflowDefinitionResponseDto>> GetAllAsync();


    Task<WorkflowDefinitionResponseDto?> GetByIdAsync(
        Guid id);


    Task DeleteAsync(
        Guid id);
}