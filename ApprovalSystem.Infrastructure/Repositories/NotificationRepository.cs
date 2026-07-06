using ApprovalSystem.Core.Entities;
using ApprovalSystem.Core.Interfaces;
using ApprovalSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApprovalSystem.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly ApprovalDbContext _context;

    public NotificationRepository(
        ApprovalDbContext context)
    {
        _context = context;
    }

    public async Task<Notification?> GetByIdAsync(
        Guid id)
    {
        return await _context.Notifications
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Notification>> GetAllAsync()
    {
        return await _context.Notifications
            .ToListAsync();
    }

    public async Task<IEnumerable<Notification>> GetByUserIdAsync(
        Guid userId)
    {
        return await _context.Notifications
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(
        Guid userId)
    {
        return await _context.Notifications
            .Where(x =>
                x.UserId == userId &&
                !x.IsRead)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<int> GetUnreadCountAsync(
        Guid userId)
    {
        return await _context.Notifications
            .CountAsync(x =>
                x.UserId == userId &&
                !x.IsRead);
    }

    public async Task AddAsync(
        Notification notification)
    {
        await _context.Notifications.AddAsync(notification);
    }

    public Task UpdateAsync(
        Notification notification)
    {
        _context.Notifications.Update(notification);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(
        Notification notification)
    {
        _context.Notifications.Remove(notification);

        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}