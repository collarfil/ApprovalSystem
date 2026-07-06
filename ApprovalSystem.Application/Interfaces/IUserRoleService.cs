using ApprovalSystem.Application.DTOs.UserRoles;

namespace ApprovalSystem.Application.Interfaces;

public interface IUserRoleService
{
    Task<UserRoleResponseDto> CreateAsync(
        CreateUserRoleDto dto);

    Task<IEnumerable<UserRoleResponseDto>>
        GetAllAsync();

    Task DeleteAsync(
        Guid userId,
        Guid roleId);
}