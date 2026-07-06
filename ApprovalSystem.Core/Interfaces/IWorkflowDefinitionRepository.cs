using ApprovalSystem.Core.Entities;

namespace ApprovalSystem.Core.Interfaces;

public interface IWorkflowDefinitionRepository
{
    Task<WorkflowDefinition?> GetByIdAsync(Guid id);

    Task<IEnumerable<WorkflowDefinition>> GetAllAsync();

    Task AddAsync(WorkflowDefinition workflow);

    Task UpdateAsync(WorkflowDefinition workflow);
    Task<WorkflowDefinition?> GetWithStepsAsync(
    Guid id);
    Task SaveChangesAsync();
}