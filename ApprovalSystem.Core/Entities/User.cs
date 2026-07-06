
namespace ApprovalSystem.Core.Entities
{
    public class User
    {
        public Guid Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public Guid DepartmentId { get; set; }

    public Department Department { get; set; } = null!;

    public ICollection<UserRole> UserRoles { get; set; }
        = new List<UserRole>();
}
}