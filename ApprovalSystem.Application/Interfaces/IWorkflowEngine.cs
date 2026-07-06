using ApprovalSystem.Core.Entities;

namespace ApprovalSystem.Application.Interfaces;

public interface IWorkflowEngine
{
    Task StartWorkflowAsync(Request request);

    Task ApproveStepAsync(
        Guid requestId,
        Guid approverId,
        string comment);

    Task RejectStepAsync(
        Guid requestId,
        Guid approverId,
        string reason);

    Task EscalateStepAsync(
        Guid requestId,
        Guid approverId,
        string reason);
}