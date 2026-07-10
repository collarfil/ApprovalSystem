using ApprovalSystem.Application.DTOs.Approvals;

namespace ApprovalSystem.Application.Interfaces;

public interface IApprovalService
{
    Task ApproveRequestAsync(Guid requestId, Guid approverId, string comment);
    Task RejectRequestAsync(Guid requestId, Guid approverId, string reason);
    Task EscalateRequestAsync(Guid requestId, Guid approverId, string reason);
    Task<List<PendingApprovalDto>> GetPendingApprovalsAsync(Guid approverId);
}