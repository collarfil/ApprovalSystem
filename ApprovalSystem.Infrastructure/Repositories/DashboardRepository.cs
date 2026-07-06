using ApprovalSystem.Core.Enums;
using ApprovalSystem.Core.Interfaces;
using ApprovalSystem.Core.Models;
using ApprovalSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApprovalSystem.Infrastructure.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly ApprovalDbContext _context;

    public DashboardRepository(ApprovalDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardSummary> GetSummaryAsync()
    {
        return new DashboardSummary
        {
            TotalUsers =
                await _context.Users.CountAsync(),

            TotalDepartments =
                await _context.Departments.CountAsync(),

            TotalRoles =
                await _context.Roles.CountAsync(),

            TotalWorkflowDefinitions =
                await _context.WorkflowDefinitions.CountAsync(),

            ActiveWorkflowDefinitions =
                await _context.WorkflowDefinitions.CountAsync(x => x.IsActive),

            TotalRequests =
                await _context.Requests.CountAsync(),

            PendingRequests =
                await _context.Requests.CountAsync(x => x.Status == RequestStatus.Pending),

            ApprovedRequests =
                await _context.Requests.CountAsync(x => x.Status == RequestStatus.Approved),

            RejectedRequests =
                await _context.Requests.CountAsync(x => x.Status == RequestStatus.Rejected),

            EscalatedRequests =
                await _context.Requests.CountAsync(x => x.Status == RequestStatus.Escalated),

            PendingApprovalSteps =
                await _context.ApprovalSteps.CountAsync(x => x.Status == ApprovalStatus.Pending),

            TotalNotifications =
                await _context.Notifications.CountAsync(),

            UnreadNotifications =
                await _context.Notifications.CountAsync(x => !x.IsRead),

            TotalAuditLogs =
                await _context.AuditLogs.CountAsync(),

            TotalUploadedFiles =
                await _context.FileUploads.CountAsync()
        };
    }
}