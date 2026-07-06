using ApprovalSystem.Application.DTOs.Notifications;
using ApprovalSystem.Application.Interfaces;
using ApprovalSystem.Core.Entities;
using ApprovalSystem.Core.Interfaces;

namespace ApprovalSystem.Application.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repository;

    public NotificationService(
        INotificationRepository repository)
    {
        _repository = repository;
    }

    public async Task<NotificationResponseDto> CreateAsync(
        CreateNotificationDto dto)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = dto.UserId,
            Title = dto.Title,
            Message = dto.Message,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(notification);

        await _repository.SaveChangesAsync();

        return new NotificationResponseDto
        {
            Id = notification.Id,
            UserId = notification.UserId,
            Title = notification.Title,
            Message = notification.Message,
            IsRead = notification.IsRead,
            CreatedAt = notification.CreatedAt
        };
    }

    public async Task<IEnumerable<NotificationResponseDto>>
        GetAllAsync()
    {
        var notifications =
            await _repository.GetAllAsync();

        return notifications.Select(x => new NotificationResponseDto
        {
            Id = x.Id,
            UserId = x.UserId,
            Title = x.Title,
            Message = x.Message,
            IsRead = x.IsRead,
            CreatedAt = x.CreatedAt
        });
    }

    public async Task<NotificationResponseDto?>
        GetByIdAsync(Guid id)
    {
        var notification =
            await _repository.GetByIdAsync(id);

        if (notification == null)
            return null;

        return new NotificationResponseDto
        {
            Id = notification.Id,
            UserId = notification.UserId,
            Title = notification.Title,
            Message = notification.Message,
            IsRead = notification.IsRead,
            CreatedAt = notification.CreatedAt
        };
    }

    public async Task<IEnumerable<NotificationResponseDto>>
        GetByUserIdAsync(Guid userId)
    {
        var notifications =
            await _repository.GetByUserIdAsync(userId);

        return notifications.Select(x => new NotificationResponseDto
        {
            Id = x.Id,
            UserId = x.UserId,
            Title = x.Title,
            Message = x.Message,
            IsRead = x.IsRead,
            CreatedAt = x.CreatedAt
        });
    }

    public async Task<IEnumerable<NotificationResponseDto>>
        GetUnreadByUserIdAsync(Guid userId)
    {
        var notifications =
            await _repository.GetUnreadByUserIdAsync(userId);

        return notifications.Select(x => new NotificationResponseDto
        {
            Id = x.Id,
            UserId = x.UserId,
            Title = x.Title,
            Message = x.Message,
            IsRead = x.IsRead,
            CreatedAt = x.CreatedAt
        });
    }

    public async Task<int>
        GetUnreadCountAsync(Guid userId)
    {
        return await _repository.GetUnreadCountAsync(userId);
    }

    public async Task MarkAsReadAsync(Guid id)
    {
        var notification =
            await _repository.GetByIdAsync(id);

        if (notification == null)
            return;

        notification.IsRead = true;

        await _repository.UpdateAsync(notification);

        await _repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var notification =
            await _repository.GetByIdAsync(id);

        if (notification == null)
            return;

        notification.IsRead = true;

        await _repository.UpdateAsync(notification);

        await _repository.SaveChangesAsync();
    }
}