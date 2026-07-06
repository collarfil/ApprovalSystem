using System;

namespace ApprovalSystem.Application.DTOs.Users;

public class UserResponseDto
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public Guid DepartmentId { get; set; }
}