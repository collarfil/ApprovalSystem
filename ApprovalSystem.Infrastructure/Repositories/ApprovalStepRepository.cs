using ApprovalSystem.Core.Entities;
using ApprovalSystem.Core.Enums;
using ApprovalSystem.Core.Interfaces;
using ApprovalSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace ApprovalSystem.Infrastructure.Repositories;


public class ApprovalStepRepository : IApprovalStepRepository
{
    private readonly ApprovalDbContext _context;


    public ApprovalStepRepository(
        ApprovalDbContext context)
    {
        _context = context;
    }


    public async Task<ApprovalStep?> GetByIdAsync(Guid id)
    {
        return await _context.ApprovalSteps
            .Include(x => x.Request)
            .Include(x => x.Approver)
            .FirstOrDefaultAsync(x => x.Id == id);
    }


    public async Task<ApprovalStep?> GetCurrentStepAsync(
        Guid requestId)
    {
        return await _context.ApprovalSteps
            .Where(x =>
                x.RequestId == requestId &&
                x.Status == ApprovalStatus.Pending)
            .OrderBy(x => x.StepOrder)
            .FirstOrDefaultAsync();
    }


    public async Task<List<ApprovalStep>> GetStepsByRequestIdAsync(
        Guid requestId)
    {
        return await _context.ApprovalSteps
            .Where(x => x.RequestId == requestId)
            .Include(x => x.Approver)
            .OrderBy(x => x.StepOrder)
            .ToListAsync();
    }


    public async Task<List<ApprovalStep>> GetStepsByApproverIdAsync(
        Guid approverId)
    {
        return await _context.ApprovalSteps
            .Where(x => x.ApproverId == approverId)
            .Include(x => x.Request)
            .OrderBy(x => x.StepOrder)
            .ToListAsync();
    }


    public async Task AddAsync(
        ApprovalStep step)
    {
        await _context.ApprovalSteps.AddAsync(step);
    }


    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(
    ApprovalStep step)
{
    _context.ApprovalSteps.Update(step);

    await Task.CompletedTask;
}


public async Task<List<ApprovalStep>>
    GetPendingApprovalsAsync(Guid approverId)
{
    return await _context.ApprovalSteps

        .Include(x => x.Request)
            .ThenInclude(r => r.RequestedBy)

        .Include(x => x.AssignedToUser)
        
        .Include(x => x.Approver)  // ← ADD THIS LINE

        .Where(x =>
            (x.ApproverId == approverId || x.AssignedToUserId == approverId) &&  // ← MODIFY THIS LINE
            x.Status == ApprovalStatus.Pending)

        .OrderBy(x => x.Request.CreatedAt)

        .ToListAsync();
}
public async Task<List<ApprovalStep>> GetAllApprovalsAsync()
{
    return await _context.ApprovalSteps
        .Include(x => x.Request)
        .Include(x => x.Approver)
        .ToListAsync();
}
}