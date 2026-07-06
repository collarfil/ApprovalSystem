using ApprovalSystem.Core.Entities;
using ApprovalSystem.Core.Interfaces;
using ApprovalSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApprovalSystem.Infrastructure.Repositories;

public class UserRoleRepository
    : IUserRoleRepository
{
    private readonly ApprovalDbContext _context;

    public UserRoleRepository(
        ApprovalDbContext context)
    {
        _context = context;
    }

    public async Task<UserRole?> GetAsync(
        Guid userId,
        Guid roleId)
    {
        return await _context.UserRoles
            .Include(x => x.User)
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x =>
                x.UserId == userId &&
                x.RoleId == roleId);
    }

    public async Task<List<UserRole>> GetByRoleIdAsync(
        Guid roleId)
    {
        return await _context.UserRoles
            .Include(x => x.User)
            .Where(x => x.RoleId == roleId)
            .ToListAsync();
    }

    public async Task<IEnumerable<UserRole>> GetAllAsync()
    {
        return await _context.UserRoles
            .Include(x => x.User)
            .Include(x => x.Role)
            .ToListAsync();
    }

    public async Task AddAsync(
        UserRole userRole)
    {
        await _context.UserRoles.AddAsync(userRole);
    }

    public async Task DeleteAsync(
        UserRole userRole)
    {
        _context.UserRoles.Remove(userRole);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}