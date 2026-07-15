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
        try
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
            
            Console.WriteLine($"[EMAIL] Email sent successfully to {dto.To}");
        }
        catch (Exception ex)
        {
            // Log the error but don't throw - email is optional
            Console.WriteLine($"[EMAIL ERROR] Failed to send email to {dto.To}: {ex.Message}");
            // Don't re-throw - let the workflow continue
        }
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
            IsHtml = true,
            Body =
            $"""
            <html>
            <body>
                <h2>Request Submitted</h2>
                <p>Hello {requesterName},</p>
                <p>Your request <strong>"{requestTitle}"</strong> has been submitted successfully.</p>
                <p>Status: <strong>Pending Approval</strong></p>
                <br/>
                <p>Thank you,<br/>Approval System</p>
            </body>
            </html>
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
            IsHtml = true,
            Body =
            $"""
            <html>
            <body>
                <h2>Approval Required</h2>
                <p>Hello {approverName},</p>
                <p>A request requires your approval.</p>
                <p><strong>Request:</strong> {requestTitle}</p>
                <p>Please log in to review it.</p>
                <br/>
                <p>Thank you,<br/>Approval System</p>
            </body>
            </html>
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
            IsHtml = true,
            Body =
            $"""
            <html>
            <body>
                <h2>Request Approved</h2>
                <p>Hello {requesterName},</p>
                <p>Your request <strong>"{requestTitle}"</strong> has been <strong style="color:green;">approved</strong>.</p>
                <br/>
                <p>Thank you,<br/>Approval System</p>
            </body>
            </html>
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
            IsHtml = true,
            Body =
            $"""
            <html>
            <body>
                <h2>Request Rejected</h2>
                <p>Hello {requesterName},</p>
                <p>Your request <strong>"{requestTitle}"</strong> has been <strong style="color:red;">rejected</strong>.</p>
                <p><strong>Reason:</strong> {reason}</p>
                <br/>
                <p>Thank you,<br/>Approval System</p>
            </body>
            </html>
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
            IsHtml = true,
            Body =
            $"""
            <html>
            <body>
                <h2>Password Reset</h2>
                <p>Click the link below to reset your password:</p>
                <p><a href="{resetLink}">Reset Password</a></p>
                <p>This link will expire in 24 hours.</p>
                <br/>
                <p>Thank you,<br/>Approval System</p>
            </body>
            </html>
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
            IsHtml = true,
            Body =
            $"""
            <html>
            <body>
                <h2>Verify Your Email</h2>
                <p>Click below to verify your email:</p>
                <p><a href="{verificationLink}">Verify Email</a></p>
                <br/>
                <p>Thank you,<br/>Approval System</p>
            </body>
            </html>
            """
        });
    }
}