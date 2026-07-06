using System.Threading.Tasks;
using ApprovalSystem.Application.DTOs.Dashboard;

namespace ApprovalSystem.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardSummaryDto> GetSummaryAsync();
}