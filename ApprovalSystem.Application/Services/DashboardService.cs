using ApprovalSystem.Application.DTOs.Dashboard;
using ApprovalSystem.Application.Interfaces;
using ApprovalSystem.Core.Interfaces;

namespace ApprovalSystem.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _repository;

    public DashboardService(IDashboardRepository repository)
    {
        _repository = repository;
    }

    public async Task<DashboardSummaryDto> GetSummaryAsync()
    {
        var summary = await _repository.GetSummaryAsync();

        return new DashboardSummaryDto
        {
            TotalUsers = summary.TotalUsers,
            TotalDepartments = summary.TotalDepartments,
            TotalRoles = summary.TotalRoles,
            TotalWorkflowDefinitions = summary.TotalWorkflowDefinitions,
            ActiveWorkflowDefinitions = summary.ActiveWorkflowDefinitions,
            TotalRequests = summary.TotalRequests,
            PendingRequests = summary.PendingRequests,
            ApprovedRequests = summary.ApprovedRequests,
            RejectedRequests = summary.RejectedRequests,
            EscalatedRequests = summary.EscalatedRequests,
            PendingApprovalSteps = summary.PendingApprovalSteps,
            TotalNotifications = summary.TotalNotifications,
            UnreadNotifications = summary.UnreadNotifications,
            TotalAuditLogs = summary.TotalAuditLogs,
            TotalUploadedFiles = summary.TotalUploadedFiles
        };
    }
}