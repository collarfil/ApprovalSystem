using ApprovalSystem.Application.DTOs.AuditLogs;

namespace ApprovalSystem.Application.Interfaces;

public interface IAuditLogService
{
    Task<AuditLogResponseDto> CreateAsync(
        CreateAuditLogDto dto);

    Task<IEnumerable<AuditLogResponseDto>>
        GetAllAsync();

    Task<AuditLogResponseDto?>
        GetByIdAsync(Guid id);
}