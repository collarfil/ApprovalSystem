using ApprovalSystem.Application.DTOs.Auth;
using ApprovalSystem.Application.DTOs.Requests;
using ApprovalSystem.Application.DTOs.Users;
using ApprovalSystem.Application.DTOs.Workflows;
using ApprovalSystem.Core.Entities;
using ApprovalSystem.Core.Enums;
using ApprovalSystem.Core.Interfaces;


namespace ApprovalSystem.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(
        LoginDto dto);

    Task RegisterAsync(
        RegisterUserDto dto);
}