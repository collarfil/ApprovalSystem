using ApprovalSystem.Core.Entities;

namespace ApprovalSystem.Core.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);

    Task<IEnumerable<User>> GetAllAsync();

    Task AddAsync(User user);

    Task UpdateAsync(User user);
    Task<User?> GetByEmailAsync(string email);
    Task SaveChangesAsync();
}