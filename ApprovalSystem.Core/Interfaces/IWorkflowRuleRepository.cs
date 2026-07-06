using ApprovalSystem.Core.Entities;

namespace ApprovalSystem.Core.Interfaces;

public interface IWorkflowRuleRepository
{
    Task<WorkflowRule?> GetByIdAsync(Guid id);

    Task<IEnumerable<WorkflowRule>> GetAllAsync();

    Task AddAsync(WorkflowRule workflowRule);

    Task UpdateAsync(WorkflowRule workflowRule);
    Task<List<WorkflowRule>> GetByWorkflowDefinitionIdAsync(
    Guid workflowDefinitionId);

    Task SaveChangesAsync();
}