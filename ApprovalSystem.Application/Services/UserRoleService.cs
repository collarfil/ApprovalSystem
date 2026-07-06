using ApprovalSystem.Application.DTOs.UserRoles;
using ApprovalSystem.Application.Interfaces;
using ApprovalSystem.Core.Entities;
using ApprovalSystem.Core.Interfaces;

namespace ApprovalSystem.Application.Services;

public class UserRoleService
    : IUserRoleService
{
    private readonly IUserRoleRepository _repository;

    public UserRoleService(
        IUserRoleRepository repository)
    {
        _repository = repository;
    }

    public async Task<UserRoleResponseDto> CreateAsync(
        CreateUserRoleDto dto)
    {
        var userRole = new UserRole
        {
            UserId = dto.UserId,
            RoleId = dto.RoleId
        };

        await _repository.AddAsync(userRole);

        await _repository.SaveChangesAsync();

        return new UserRoleResponseDto
        {
            UserId = userRole.UserId,
            RoleId = userRole.RoleId
        };
    }

    public async Task<IEnumerable<UserRoleResponseDto>>
        GetAllAsync()
    {
        var data =
            await _repository.GetAllAsync();

        return data.Select(x =>
            new UserRoleResponseDto
            {
                UserId = x.UserId,
                RoleId = x.RoleId
            });
    }

    public async Task DeleteAsync(
        Guid userId,
        Guid roleId)
    {
        var userRole =
            await _repository.GetAsync(
                userId,
                roleId);

        if (userRole == null)
            return;

        await _repository.DeleteAsync(userRole);

        await _repository.SaveChangesAsync();
    }
}