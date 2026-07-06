using ApprovalSystem.Core.Entities;

namespace ApprovalSystem.Core.Interfaces;

public interface INotificationRepository
{
    Task<Notification?> GetByIdAsync(Guid id);

    Task<IEnumerable<Notification>> GetAllAsync();

    Task<IEnumerable<Notification>> GetByUserIdAsync(
        Guid userId);

    Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(
        Guid userId);

    Task<int> GetUnreadCountAsync(
        Guid userId);

    Task AddAsync(Notification notification);

    Task UpdateAsync(Notification notification);
    Task DeleteAsync(Notification notification);

    Task SaveChangesAsync();
}