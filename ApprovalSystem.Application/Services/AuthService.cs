using ApprovalSystem.Application.DTOs.Auth;
using ApprovalSystem.Application.Interfaces;
using ApprovalSystem.Core.Entities;
using ApprovalSystem.Core.Interfaces;

namespace ApprovalSystem.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public AuthService(
        IUserRepository userRepository,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }


   public async Task<AuthResponseDto> LoginAsync(
    LoginDto dto)
{
    var user =
        await _userRepository
            .GetByEmailAsync(dto.Email);

    if (user == null)
    {
        throw new Exception(
            "Invalid email or password");
    }

    if (user.Password != dto.Password)
    {
        throw new Exception(
            "Invalid email or password");
    }

    var token =
        _tokenService.GenerateToken(
            user.Id,
            user.Email);

    return new AuthResponseDto
    {
        Id = user.Id,
        Email = user.Email,
        Token = token
    };
}

    public async Task RegisterAsync(RegisterUserDto dto)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),

            FirstName = dto.FirstName,

            LastName = dto.LastName,

            Email = dto.Email,
            
            DepartmentId = dto.DepartmentId,

            Password = dto.Password
        };


        await _userRepository.AddAsync(user);

        await _userRepository.SaveChangesAsync();
    }
}