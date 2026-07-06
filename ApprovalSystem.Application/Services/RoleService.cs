using ApprovalSystem.Application.DTOs.Role;
using ApprovalSystem.Application.Interfaces;
using ApprovalSystem.Core.Entities;
using ApprovalSystem.Core.Interfaces;

namespace ApprovalSystem.Application.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _repository;

    public RoleService(
        IRoleRepository repository)
    {
        _repository = repository;
    }

    public async Task<RoleResponseDto> CreateAsync(
        CreateRoleDto dto)
    {
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            IsActive = true
        };

        await _repository.AddAsync(role);

        await _repository.SaveChangesAsync();

        return new RoleResponseDto
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            IsActive = role.IsActive
        };
    }

    public async Task<IEnumerable<RoleResponseDto>>
        GetAllAsync()
    {
        var roles =
            await _repository.GetAllAsync();

        return roles.Select(x =>
            new RoleResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsActive = x.IsActive
            });
    }

    public async Task<RoleResponseDto?>
        GetByIdAsync(Guid id)
    {
        var role =
            await _repository.GetByIdAsync(id);

        if (role == null)
            return null;

        return new RoleResponseDto
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            IsActive = role.IsActive
        };
    }

    public async Task DeleteAsync(Guid id)
    {
        var role =
            await _repository.GetByIdAsync(id);

        if (role == null)
            return;

        role.IsActive = false;

        await _repository.UpdateAsync(role);

        await _repository.SaveChangesAsync();
    }
}