using ApprovalSystem.Core.Entities;
using ApprovalSystem.Core.Interfaces;
using ApprovalSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApprovalSystem.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApprovalDbContext _context;

    public UserRepository(
        ApprovalDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users
            .ToListAsync();
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
    }
    public async Task<User?> GetByEmailAsync(string email)
{
    return await _context.Users
        .FirstOrDefaultAsync(x => x.Email == email);
}
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}