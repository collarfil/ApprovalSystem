using ApprovalSystem.Application.DTOs.Notifications;

namespace ApprovalSystem.Application.Interfaces;

public interface INotificationService
{
    Task<NotificationResponseDto> CreateAsync(
        CreateNotificationDto dto);

    Task<IEnumerable<NotificationResponseDto>>
        GetAllAsync();

    Task<NotificationResponseDto?>
        GetByIdAsync(Guid id);

    Task<IEnumerable<NotificationResponseDto>>
        GetByUserIdAsync(Guid userId);

    Task<IEnumerable<NotificationResponseDto>>
        GetUnreadByUserIdAsync(Guid userId);

    Task<int>
        GetUnreadCountAsync(Guid userId);

    Task MarkAsReadAsync(Guid id);

    Task DeleteAsync(Guid id);
}