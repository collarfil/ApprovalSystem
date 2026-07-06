using ApprovalSystem.Application.DTOs.AuditLogs;
using ApprovalSystem.Application.Interfaces;
using ApprovalSystem.Core.Entities;
using ApprovalSystem.Core.Interfaces;

namespace ApprovalSystem.Application.Services;

public class AuditLogService
    : IAuditLogService
{
    private readonly IAuditLogRepository _repository;

    public AuditLogService(
        IAuditLogRepository repository)
    {
        _repository = repository;
    }

    public async Task<AuditLogResponseDto>
        CreateAsync(
            CreateAuditLogDto dto)
    {
        var auditLog = new AuditLog
        {
            Id = Guid.NewGuid(),
            EntityName = dto.EntityName,
            EntityId = dto.EntityId,
            Action = dto.Action,
            OldValue = dto.OldValue,
            NewValue = dto.NewValue,
            PerformedById = dto.PerformedById,
            Timestamp = DateTime.UtcNow
        };

        await _repository.AddAsync(auditLog);

        await _repository.SaveChangesAsync();

        return new AuditLogResponseDto
        {
            Id = auditLog.Id,
            EntityName = auditLog.EntityName,
            EntityId = auditLog.EntityId,
            Action = auditLog.Action,
            OldValue = auditLog.OldValue,
            NewValue = auditLog.NewValue,
            PerformedById = auditLog.PerformedById,
            Timestamp = auditLog.Timestamp
        };
    }

    public async Task<IEnumerable<AuditLogResponseDto>>
        GetAllAsync()
    {
        var logs =
            await _repository.GetAllAsync();

        return logs.Select(x =>
            new AuditLogResponseDto
            {
                Id = x.Id,
                EntityName = x.EntityName,
                EntityId = x.EntityId,
                Action = x.Action,
                OldValue = x.OldValue,
                NewValue = x.NewValue,
                PerformedById = x.PerformedById,
                Timestamp = x.Timestamp
            });
    }

    public async Task<AuditLogResponseDto?>
        GetByIdAsync(Guid id)
    {
        var log =
            await _repository.GetByIdAsync(id);

        if (log == null)
            return null;

        return new AuditLogResponseDto
        {
            Id = log.Id,
            EntityName = log.EntityName,
            EntityId = log.EntityId,
            Action = log.Action,
            OldValue = log.OldValue,
            NewValue = log.NewValue,
            PerformedById = log.PerformedById,
            Timestamp = log.Timestamp
        };
    }
}