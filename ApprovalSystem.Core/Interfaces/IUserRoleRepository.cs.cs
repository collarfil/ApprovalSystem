using ApprovalSystem.Core.Entities;

namespace ApprovalSystem.Core.Interfaces;

public interface IUserRoleRepository
{
    Task<UserRole?> GetAsync(
        Guid userId,
        Guid roleId);

    Task<List<UserRole>> GetByRoleIdAsync(
        Guid roleId);

    Task<IEnumerable<UserRole>> GetAllAsync();

    Task AddAsync(
        UserRole userRole);

    Task DeleteAsync(
        UserRole userRole);

    Task SaveChangesAsync();
}