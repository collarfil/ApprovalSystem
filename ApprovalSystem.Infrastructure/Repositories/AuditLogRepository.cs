using ApprovalSystem.Core.Entities;
using ApprovalSystem.Core.Interfaces;
using ApprovalSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApprovalSystem.Infrastructure.Repositories;

public class AuditLogRepository
    : IAuditLogRepository
{
    private readonly ApprovalDbContext _context;

    public AuditLogRepository(
        ApprovalDbContext context)
    {
        _context = context;
    }

    public async Task<AuditLog?> GetByIdAsync(
        Guid id)
    {
        return await _context.AuditLogs
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<AuditLog>>
        GetAllAsync()
    {
        return await _context.AuditLogs
            .ToListAsync();
    }

    public async Task AddAsync(
        AuditLog auditLog)
    {
        await _context.AuditLogs
            .AddAsync(auditLog);
    }

    public Task UpdateAsync(
        AuditLog auditLog)
    {
        _context.AuditLogs
            .Update(auditLog);

        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}