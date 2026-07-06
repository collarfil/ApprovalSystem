using ApprovalSystem.Application.DTOs.Users;

namespace ApprovalSystem.Application.Interfaces;

public interface IUserService
{
    Task<UserResponseDto> CreateAsync(
        CreateUserDto dto);

    Task<IEnumerable<UserResponseDto>>
        GetAllAsync();

    Task<UserResponseDto?>
        GetByIdAsync(Guid id);

    Task DeleteAsync(Guid id);
}