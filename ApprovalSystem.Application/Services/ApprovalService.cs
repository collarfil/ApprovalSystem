using ApprovalSystem.Application.DTOs.Approvals;
using ApprovalSystem.Application.Interfaces;
using ApprovalSystem.Core.Interfaces;

namespace ApprovalSystem.Application.Services;

public class ApprovalService : IApprovalService
{
    private readonly IWorkflowEngine _workflowEngine;
    private readonly IRequestRepository _requestRepository;
    private readonly IApprovalStepRepository _approvalStepRepository;

    public ApprovalService(
        IWorkflowEngine workflowEngine,
        IRequestRepository requestRepository,
        IApprovalStepRepository approvalStepRepository)
    {
        _workflowEngine = workflowEngine;
        _requestRepository = requestRepository;
        _approvalStepRepository = approvalStepRepository;
    }

    public async Task ApproveRequestAsync(Guid requestId, Guid approverId, string comment)
    {
        await _workflowEngine.ApproveStepAsync(requestId, approverId, comment);
    }

    public async Task RejectRequestAsync(Guid requestId, Guid approverId, string reason)
    {
        await _workflowEngine.RejectStepAsync(requestId, approverId, reason);
    }

    public async Task EscalateRequestAsync(Guid requestId, Guid approverId, string reason)
    {
        await _workflowEngine.EscalateStepAsync(requestId, approverId, reason);
    }

    public async Task<List<PendingApprovalDto>> GetPendingApprovalsAsync(Guid approverId)
{
    var approvals = await _approvalStepRepository.GetPendingApprovalsAsync(approverId);

    if (!approvals.Any())
        return new List<PendingApprovalDto>();  // ← ADD THIS LINE

    return approvals.Select(a => new PendingApprovalDto
    {
        ApprovalStepId = a.Id,
        RequestId = a.RequestId,
        Title = a.Request.Title,
        Description = a.Request.Description,
        Amount = a.Request.Amount,
        Status = a.Request.Status.ToString(),
        StepOrder = a.StepOrder,
        RequestedById = a.Request.RequestedById,
        RequestedByName = $"{a.Request.RequestedBy.FirstName} {a.Request.RequestedBy.LastName}",
        AssignedToUserId = a.AssignedToUserId,
        AssignedToName = $"{a.AssignedToUser.FirstName} {a.AssignedToUser.LastName}",
        CreatedAt = a.Request.CreatedAt
    }).ToList();
}
}