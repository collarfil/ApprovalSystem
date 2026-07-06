using ApprovalSystem.Application.DTOs.Role;

namespace ApprovalSystem.Application.Interfaces;

public interface IRoleService
{
    Task<RoleResponseDto> CreateAsync(
        CreateRoleDto dto);

    Task<IEnumerable<RoleResponseDto>>
        GetAllAsync();

    Task<RoleResponseDto?>
        GetByIdAsync(Guid id);

    Task DeleteAsync(Guid id);
}