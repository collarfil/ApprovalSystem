using ApprovalSystem.Application.DTOs.Approvals;
using ApprovalSystem.Application.Interfaces;
using ApprovalSystem.Core.Entities;
using ApprovalSystem.Core.Interfaces;
using ApprovalSystem.Core.Enums;
using ApprovalSystem.Application.DTOs.Requests;

namespace ApprovalSystem.Application.Services;
public class ApprovalService : IApprovalService
{
    private readonly IWorkflowEngine _workflowEngine;
    private readonly IRequestRepository _requestRepository;

    public ApprovalService(
        IWorkflowEngine workflowEngine,
        IRequestRepository requestRepository)
    {
        _workflowEngine = workflowEngine;
        _requestRepository = requestRepository;
    }

   
    public async Task ApproveRequestAsync(
        Guid requestId,
        Guid approverId,
        string comment)
    {
        await _workflowEngine.ApproveStepAsync(requestId, approverId, comment);

        var request = await _requestRepository.GetByIdAsync(requestId);
        if (request != null)
        {
            request.Status = RequestStatus.Approved;
            await _requestRepository.UpdateAsync(request);
            await _requestRepository.SaveChangesAsync();
        }
    }

    public async Task RejectRequestAsync(
        Guid requestId,
        Guid approverId,
        string reason)
    {
        await _workflowEngine.RejectStepAsync(requestId, approverId, reason);

        var request = await _requestRepository.GetByIdAsync(requestId);
        if (request != null)
        {
            request.Status = RequestStatus.Rejected;
            await _requestRepository.UpdateAsync(request);
            await _requestRepository.SaveChangesAsync();
        }
    }

    public async Task EscalateRequestAsync(
        Guid requestId,
        Guid approverId,
        string reason)
    {
        await _workflowEngine.EscalateStepAsync(requestId, approverId, reason);

        var request = await _requestRepository.GetByIdAsync(requestId);
        if (request != null)
        {
            request.Status = RequestStatus.Escalated;
            await _requestRepository.UpdateAsync(request);
            await _requestRepository.SaveChangesAsync();
        }
    }
}