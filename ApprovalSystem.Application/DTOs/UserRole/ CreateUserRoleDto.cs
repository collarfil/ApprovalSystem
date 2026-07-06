namespace ApprovalSystem.Application.DTOs.UserRoles;

public class CreateUserRoleDto
{
    public Guid UserId { get; set; }

    public Guid RoleId { get; set; }
}