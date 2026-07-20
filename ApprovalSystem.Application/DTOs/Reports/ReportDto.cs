namespace ApprovalSystem.Application.DTOs.Reports;

public class DashboardStatsDto
{
    public int TotalUsers { get; set; }
    public int TotalDepartments { get; set; }
    public int TotalRoles { get; set; }
    public int TotalWorkflows { get; set; }
    public int ActiveWorkflows { get; set; }
    public int TotalRequests { get; set; }
    public int PendingRequests { get; set; }
    public int ApprovedRequests { get; set; }
    public int RejectedRequests { get; set; }
    public int EscalatedRequests { get; set; }
    public int PendingApprovals { get; set; }
    public int TotalNotifications { get; set; }
    public int UnreadNotifications { get; set; }
    public int TotalAuditLogs { get; set; }
    public int TotalUploadedFiles { get; set; }
}

public class RequestTrendDto
{
    public string Date { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class RequestByStatusDto
{
    public string Status { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class RequestByDepartmentDto
{
    public string Department { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class ApprovalTimeDto
{
    public double AverageHours { get; set; }
    public double MinHours { get; set; }
    public double MaxHours { get; set; }
}

public class UserActivityDto
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int RequestCount { get; set; }
    public int ApprovalCount { get; set; }
}