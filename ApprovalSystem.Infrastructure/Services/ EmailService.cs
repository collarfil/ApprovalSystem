using ApprovalSystem.Application.Common.Settings;
using ApprovalSystem.Application.DTOs.Emails;
using ApprovalSystem.Application.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ApprovalSystem.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;

    public EmailService(
        IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;
    }

    public async Task SendAsync(
        SendEmailDto dto)
    {
        var email = new MimeMessage();

        email.From.Add(
            new MailboxAddress(
                _smtpSettings.FromName,
                _smtpSettings.FromEmail));

        email.To.Add(
            MailboxAddress.Parse(dto.To));

        email.Subject = dto.Subject;

        email.Body = new TextPart(
            dto.IsHtml ? "html" : "plain")
        {
            Text = dto.Body
        };

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync(
            _smtpSettings.Host,
            _smtpSettings.Port,
            _smtpSettings.EnableSsl
                ? SecureSocketOptions.StartTls
                : SecureSocketOptions.None);

        await smtp.AuthenticateAsync(
            _smtpSettings.Username,
            _smtpSettings.Password);

        await smtp.SendAsync(email);

        await smtp.DisconnectAsync(true);
    }

    public async Task SendRequestSubmittedAsync(
        string email,
        string requesterName,
        string requestTitle)
    {
        await SendAsync(new SendEmailDto
        {
            To = email,
            Subject = "Request Submitted",
            Body =
            $"""
            Hello {requesterName},

            Your request "<b>{requestTitle}</b>" has been submitted successfully.

            Status: Pending Approval.
            """
        });
    }

    public async Task SendApprovalAssignedAsync(
        string email,
        string approverName,
        string requestTitle)
    {
        await SendAsync(new SendEmailDto
        {
            To = email,
            Subject = "Approval Required",
            Body =
            $"""
            Hello {approverName},

            A request requires your approval.

            Request:
            <b>{requestTitle}</b>

            Please log in to review it.
            """
        });
    }

    public async Task SendRequestApprovedAsync(
        string email,
        string requesterName,
        string requestTitle)
    {
        await SendAsync(new SendEmailDto
        {
            To = email,
            Subject = "Request Approved",
            Body =
            $"""
            Hello {requesterName},

            Your request

            <b>{requestTitle}</b>

            has been approved.
            """
        });
    }

    public async Task SendRequestRejectedAsync(
        string email,
        string requesterName,
        string requestTitle,
        string reason)
    {
        await SendAsync(new SendEmailDto
        {
            To = email,
            Subject = "Request Rejected",
            Body =
            $"""
            Hello {requesterName},

            Your request

            <b>{requestTitle}</b>

            has been rejected.

            Reason:

            {reason}
            """
        });
    }

    public async Task SendPasswordResetAsync(
        string email,
        string resetLink)
    {
        await SendAsync(new SendEmailDto
        {
            To = email,
            Subject = "Password Reset",
            Body =
            $"""
            Click the link below to reset your password.

            <a href="{resetLink}">Reset Password</a>
            """
        });
    }

    public async Task SendEmailVerificationAsync(
        string email,
        string verificationLink)
    {
        await SendAsync(new SendEmailDto
        {
            To = email,
            Subject = "Verify Your Email",
            Body =
            $"""
            Click below to verify your email.

            <a href="{verificationLink}">
                Verify Email
            </a>
            """
        });
    }
}