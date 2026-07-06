using ApprovalSystem.Core.Entities;

namespace ApprovalSystem.Core.Interfaces;

public interface IAuditLogRepository
{
    Task<AuditLog?> GetByIdAsync(Guid id);

    Task<IEnumerable<AuditLog>> GetAllAsync();

    Task AddAsync(AuditLog auditLog);

    Task UpdateAsync(AuditLog auditLog);

    Task SaveChangesAsync();
}