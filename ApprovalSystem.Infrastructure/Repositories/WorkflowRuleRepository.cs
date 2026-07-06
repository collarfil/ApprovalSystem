using ApprovalSystem.Core.Entities;
using ApprovalSystem.Core.Interfaces;
using ApprovalSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApprovalSystem.Infrastructure.Repositories;

public class WorkflowRuleRepository
    : IWorkflowRuleRepository
{
    private readonly ApprovalDbContext _context;

    public WorkflowRuleRepository(
        ApprovalDbContext context)
    {
        _context = context;
    }

    public async Task<WorkflowRule?> GetByIdAsync(Guid id)
    {
        return await _context.WorkflowRules
            .Include(x => x.WorkflowDefinition)
            .Include(x => x.TargetRole)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    
    public async Task<List<WorkflowRule>>
    GetByWorkflowDefinitionIdAsync(
        Guid workflowDefinitionId)
{
    return await _context.WorkflowRules
        .Where(x =>
            x.WorkflowDefinitionId == workflowDefinitionId &&
            x.IsActive)
        .ToListAsync();
}
    public async Task<IEnumerable<WorkflowRule>> GetAllAsync()
    {
        return await _context.WorkflowRules
            .Include(x => x.WorkflowDefinition)
            .Include(x => x.TargetRole)
            .ToListAsync();
    }

    public async Task AddAsync(WorkflowRule workflowRule)
    {
        await _context.WorkflowRules.AddAsync(workflowRule);
    }

    public async Task UpdateAsync(WorkflowRule workflowRule)
    {
        _context.WorkflowRules.Update(workflowRule);
    }
 
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}