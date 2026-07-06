using ApprovalSystem.Application.DTOs.Users;
using ApprovalSystem.Application.Interfaces;
using ApprovalSystem.Core.Entities;
using ApprovalSystem.Core.Interfaces;

namespace ApprovalSystem.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(
        IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<UserResponseDto> CreateAsync(
        CreateUserDto dto)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Password = dto.Password,
            DepartmentId = dto.DepartmentId,
            IsActive = true
        };

        await _repository.AddAsync(user);

        await _repository.SaveChangesAsync();

        return new UserResponseDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            DepartmentId = user.DepartmentId,
            IsActive = user.IsActive
        };
    }

    public async Task<IEnumerable<UserResponseDto>>
        GetAllAsync()
    {
        var users =
            await _repository.GetAllAsync();

        return users.Select(x =>
            new UserResponseDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                DepartmentId = x.DepartmentId,
                IsActive = x.IsActive
            });
    }

    public async Task<UserResponseDto?>
        GetByIdAsync(Guid id)
    {
        var user =
            await _repository.GetByIdAsync(id);

        if (user == null)
            return null;

        return new UserResponseDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            DepartmentId = user.DepartmentId,
            IsActive = user.IsActive
        };
    }

    public async Task DeleteAsync(Guid id)
    {
        var user =
            await _repository.GetByIdAsync(id);

        if (user == null)
            return;

        user.IsActive = false;

        await _repository.UpdateAsync(user);

        await _repository.SaveChangesAsync();
    }
}