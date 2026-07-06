using ApprovalSystem.Core.Models;

namespace ApprovalSystem.Core.Interfaces;

public interface IDashboardRepository
{
    Task<DashboardSummary> GetSummaryAsync();
        }
