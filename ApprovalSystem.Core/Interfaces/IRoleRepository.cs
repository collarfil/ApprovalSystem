using ApprovalSystem.Core.Entities;

namespace ApprovalSystem.Core.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(Guid id);

    Task<IEnumerable<Role>> GetAllAsync();

    Task AddAsync(Role role);

    Task UpdateAsync(Role role);

    Task SaveChangesAsync();
}