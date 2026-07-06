using ApprovalSystem.Core.Entities;
using ApprovalSystem.Core.Interfaces;
using ApprovalSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApprovalSystem.Infrastructure.Repositories;

public class WorkflowDefinitionRepository 
    : IWorkflowDefinitionRepository
{
    private readonly ApprovalDbContext _context;

    public WorkflowDefinitionRepository(
        ApprovalDbContext context)
    {
        _context = context;
    }


    public async Task<WorkflowDefinition?> GetByIdAsync(Guid id)
    {
        return await _context.WorkflowDefinitions
            .FirstOrDefaultAsync(x => x.Id == id);
    }


    public async Task<IEnumerable<WorkflowDefinition>> GetAllAsync()
    {
        return await _context.WorkflowDefinitions
            .ToListAsync();
    }


    public async Task AddAsync(
        WorkflowDefinition workflow)
    {
        await _context.WorkflowDefinitions.AddAsync(workflow);
    }


    public async Task UpdateAsync(
        WorkflowDefinition workflow)
    {
        _context.WorkflowDefinitions.Update(workflow);
    }


    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    public async Task<WorkflowDefinition?> GetWithStepsAsync(
    Guid id)
{
    return await _context.WorkflowDefinitions
        .Include(x => x.StepDefinitions)
            .ThenInclude(x => x.Role)
        .FirstOrDefaultAsync(x => x.Id == id);
}
}