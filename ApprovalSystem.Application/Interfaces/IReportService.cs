using ApprovalSystem.Application.DTOs.Reports;

namespace ApprovalSystem.Application.Interfaces;

public interface IReportService
{
    Task<DashboardStatsDto> GetDashboardStatsAsync();
    Task<List<RequestTrendDto>> GetRequestTrendAsync(int days = 30);
    Task<List<RequestByStatusDto>> GetRequestsByStatusAsync();
    Task<List<RequestByDepartmentDto>> GetRequestsByDepartmentAsync();
    Task<ApprovalTimeDto> GetApprovalTimeAsync();
    Task<List<UserActivityDto>> GetUserActivityAsync();
}