using ApprovalSystem.Core.Entities;
using ApprovalSystem.Core.Interfaces;
using ApprovalSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApprovalSystem.Infrastructure.Repositories;

public class RequestRepository : IRequestRepository
{
    private readonly ApprovalDbContext _context;


    public RequestRepository(
        ApprovalDbContext context)
    {
        _context = context;
    }


    public async Task<Request?> GetByIdAsync(Guid id)
    {
        return await _context.Requests
            .Include(x => x.ApprovalSteps)
            .FirstOrDefaultAsync(x => x.Id == id);
    }


    public async Task<Request?> GetRequestWithStepsAsync(Guid id)
    {
        return await _context.Requests
            .Include(x => x.ApprovalSteps)
            .Include(x => x.WorkflowDefinition)
            .Include(x => x.RequestedBy)
            .FirstOrDefaultAsync(x => x.Id == id);
    }


    public async Task AddAsync(Request request)
    {
        await _context.Requests.AddAsync(request);
    }

    public async Task UpdateAsync(Request request)
    {
        _context.Requests.Update(request);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Request>> GetAllAsync()
{
    return await _context.Requests
        .Include(x => x.WorkflowDefinition)
        .Include(x => x.RequestedBy)
        .OrderByDescending(x => x.CreatedAt)
        .ToListAsync();
}
public async Task<List<Request>> GetByRequesterIdAsync(
    Guid requesterId)
{
    return await _context.Requests
        .Include(x => x.WorkflowDefinition)
        .Include(x => x.RequestedBy)
        .Where(x => x.RequestedById == requesterId)
        .OrderByDescending(x => x.CreatedAt)
        .ToListAsync();
}
}