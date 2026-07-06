using ApprovalSystem.Core.Entities;
using ApprovalSystem.Core.Interfaces;
using ApprovalSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApprovalSystem.Infrastructure.Repositories;

public class WorkflowStepDefinitionRepository
    : IWorkflowStepDefinitionRepository
{
    private readonly ApprovalDbContext _context;

    public WorkflowStepDefinitionRepository(
        ApprovalDbContext context)
    {
        _context = context;
    }

    public async Task<WorkflowStepDefinition?> GetByIdAsync(Guid id)
    {
        return await _context.WorkflowStepDefinitions
            .Include(x => x.Role)
            .Include(x => x.WorkflowDefinition)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<WorkflowStepDefinition>> GetAllAsync()
    {
        return await _context.WorkflowStepDefinitions
            .Include(x => x.Role)
            .Include(x => x.WorkflowDefinition)
            .ToListAsync();
    }
  public async Task<List<WorkflowStepDefinition>>
        GetByWorkflowDefinitionIdAsync(
            Guid workflowDefinitionId)
        {
            return await _context.WorkflowStepDefinitions
                .Include(x => x.Role)
                .Include(x => x.WorkflowDefinition)
                .Where(x =>
                    x.WorkflowDefinitionId == workflowDefinitionId &&
                    x.IsActive)
                .OrderBy(x => x.StepOrder)
                .ToListAsync();
        }
    public async Task AddAsync(
        WorkflowStepDefinition workflowStep)
    {
        await _context.WorkflowStepDefinitions.AddAsync(workflowStep);
    }

    public async Task UpdateAsync(
        WorkflowStepDefinition workflowStep)
    {
        _context.WorkflowStepDefinitions.Update(workflowStep);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}