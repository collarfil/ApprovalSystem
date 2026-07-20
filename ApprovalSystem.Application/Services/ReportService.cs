using ApprovalSystem.Application.DTOs.Reports;
using ApprovalSystem.Application.Interfaces;
using ApprovalSystem.Core.Enums;
using ApprovalSystem.Core.Interfaces;

namespace ApprovalSystem.Application.Services;

public class ReportService : IReportService
{
    private readonly IUserRepository _userRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IWorkflowDefinitionRepository _workflowDefinitionRepository;
    private readonly IRequestRepository _requestRepository;
    private readonly IApprovalStepRepository _approvalStepRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IAuditLogRepository _auditLogRepository;
    private readonly IFileUploadRepository _fileUploadRepository;

    public ReportService(
        IUserRepository userRepository,
        IDepartmentRepository departmentRepository,
        IRoleRepository roleRepository,
        IWorkflowDefinitionRepository workflowDefinitionRepository,
        IRequestRepository requestRepository,
        IApprovalStepRepository approvalStepRepository,
        INotificationRepository notificationRepository,
        IAuditLogRepository auditLogRepository,
        IFileUploadRepository fileUploadRepository)
    {
        _userRepository = userRepository;
        _departmentRepository = departmentRepository;
        _roleRepository = roleRepository;
        _workflowDefinitionRepository = workflowDefinitionRepository;
        _requestRepository = requestRepository;
        _approvalStepRepository = approvalStepRepository;
        _notificationRepository = notificationRepository;
        _auditLogRepository = auditLogRepository;
        _fileUploadRepository = fileUploadRepository;
    }

    public async Task<DashboardStatsDto> GetDashboardStatsAsync()
    {
        var users = await _userRepository.GetAllAsync();
        var departments = await _departmentRepository.GetAllAsync();
        var roles = await _roleRepository.GetAllAsync();
        var workflows = await _workflowDefinitionRepository.GetAllAsync();
        var requests = await _requestRepository.GetAllAsync();
        var notifications = await _notificationRepository.GetAllAsync();
        var auditLogs = await _auditLogRepository.GetAllAsync();
        var files = await _fileUploadRepository.GetAllAsync();
        var pendingApprovals = await _approvalStepRepository.GetPendingApprovalsAsync(Guid.Empty);

        return new DashboardStatsDto
        {
            TotalUsers = users.Count(),
            TotalDepartments = departments.Count(),
            TotalRoles = roles.Count(),
            TotalWorkflows = workflows.Count(),
            ActiveWorkflows = workflows.Count(w => w.IsActive),
            TotalRequests = requests.Count(),
            PendingRequests = requests.Count(r => r.Status == RequestStatus.Pending),
            ApprovedRequests = requests.Count(r => r.Status == RequestStatus.Approved),
            RejectedRequests = requests.Count(r => r.Status == RequestStatus.Rejected),
            EscalatedRequests = requests.Count(r => r.Status == RequestStatus.Escalated),
            PendingApprovals = pendingApprovals.Count(),
            TotalNotifications = notifications.Count(),
            UnreadNotifications = notifications.Count(n => !n.IsRead),
            TotalAuditLogs = auditLogs.Count(),
            TotalUploadedFiles = files.Count(),
        };
    }

    public async Task<List<RequestTrendDto>> GetRequestTrendAsync(int days = 30)
    {
        var requests = await _requestRepository.GetAllAsync();
        var startDate = DateTime.UtcNow.AddDays(-days);

        return requests
            .Where(r => r.CreatedAt >= startDate)
            .GroupBy(r => r.CreatedAt.Date)
            .Select(g => new RequestTrendDto
            {
                Date = g.Key.ToString("yyyy-MM-dd"),
                Count = g.Count()
            })
            .OrderBy(d => d.Date)
            .ToList();
    }

    public async Task<List<RequestByStatusDto>> GetRequestsByStatusAsync()
    {
        var requests = await _requestRepository.GetAllAsync();

        return requests
            .GroupBy(r => r.Status)
            .Select(g => new RequestByStatusDto
            {
                Status = g.Key.ToString(),
                Count = g.Count()
            })
            .ToList();
    }

    public async Task<List<RequestByDepartmentDto>> GetRequestsByDepartmentAsync()
    {
        var requests = await _requestRepository.GetAllAsync();
        
        return requests
            .Where(r => r.RequestedBy?.Department != null)
            .GroupBy(r => r.RequestedBy.Department.Name)
            .Select(g => new RequestByDepartmentDto
            {
                Department = g.Key ?? "Unknown",
                Count = g.Count()
            })
            .ToList();
    }

    public async Task<ApprovalTimeDto> GetApprovalTimeAsync()
    {
        var allApprovals = await _approvalStepRepository.GetAllApprovalsAsync();
        
        var completedApprovals = allApprovals
            .Where(a => a.Status == ApprovalStatus.Approved && a.ActionDate.HasValue)
            .ToList();

        if (!completedApprovals.Any())
        {
            return new ApprovalTimeDto
            {
                AverageHours = 0,
                MinHours = 0,
                MaxHours = 0
            };
        }

        var hours = completedApprovals
            .Select(a => (a.ActionDate!.Value - a.Request.CreatedAt).TotalHours)
            .ToList();

        return new ApprovalTimeDto
        {
            AverageHours = Math.Round(hours.Average(), 1),
            MinHours = Math.Round(hours.Min(), 1),
            MaxHours = Math.Round(hours.Max(), 1)
        };
    }

    public async Task<List<UserActivityDto>> GetUserActivityAsync()
    {
        var users = await _userRepository.GetAllAsync();
        var requests = await _requestRepository.GetAllAsync();
        var allApprovals = await _approvalStepRepository.GetAllApprovalsAsync();

        var result = new List<UserActivityDto>();

        foreach (var user in users)
        {
            var requestCount = requests.Count(r => r.RequestedById == user.Id);
            var approvalCount = allApprovals.Count(a => a.ApproverId == user.Id);

            if (requestCount > 0 || approvalCount > 0)
            {
                result.Add(new UserActivityDto
                {
                    UserId = user.Id,
                    UserName = $"{user.FirstName} {user.LastName}",
                    RequestCount = requestCount,
                    ApprovalCount = approvalCount
                });
            }
        }

        return result.OrderByDescending(u => u.RequestCount + u.ApprovalCount).ToList();
    }
}