using System;

namespace ApprovalSystem.Application.DTOs.Dashboard;

public class DashboardSummaryDto
{
    public int TotalUsers { get; set; }

    public int TotalDepartments { get; set; }

    public int TotalRoles { get; set; }

    public int TotalWorkflowDefinitions { get; set; }

    public int ActiveWorkflowDefinitions { get; set; }

    public int TotalRequests { get; set; }

    public int PendingRequests { get; set; }

    public int ApprovedRequests { get; set; }

    public int RejectedRequests { get; set; }

    public int EscalatedRequests { get; set; }

    public int PendingApprovalSteps { get; set; }

    public int TotalNotifications { get; set; }

    public int UnreadNotifications { get; set; }

    public int TotalAuditLogs { get; set; }

    public int TotalUploadedFiles { get; set; }
}