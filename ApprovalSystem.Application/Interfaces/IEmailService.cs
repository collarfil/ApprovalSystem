using ApprovalSystem.Application.DTOs.Emails;

namespace ApprovalSystem.Application.Interfaces;

public interface IEmailService
{
    Task SendAsync(
        SendEmailDto dto);

    Task SendRequestSubmittedAsync(
        string email,
        string requesterName,
        string requestTitle);

    Task SendApprovalAssignedAsync(
        string email,
        string approverName,
        string requestTitle);

    Task SendRequestApprovedAsync(
        string email,
        string requesterName,
        string requestTitle);

    Task SendRequestRejectedAsync(
        string email,
        string requesterName,
        string requestTitle,
        string reason);

    Task SendPasswordResetAsync(
        string email,
        string resetLink);

    Task SendEmailVerificationAsync(
        string email,
        string verificationLink);
}