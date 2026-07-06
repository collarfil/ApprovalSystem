using ApprovalSystem.Core.Entities;

namespace ApprovalSystem.Core.Interfaces;

public interface IWorkflowStepDefinitionRepository
{
    Task<WorkflowStepDefinition?> GetByIdAsync(Guid id);

    Task<IEnumerable<WorkflowStepDefinition>> GetAllAsync();

    Task AddAsync(WorkflowStepDefinition workflowStep);

    Task UpdateAsync(WorkflowStepDefinition workflowStep);
    Task<List<WorkflowStepDefinition>>
    GetByWorkflowDefinitionIdAsync(
    Guid workflowDefinitionId);
    Task SaveChangesAsync();
}