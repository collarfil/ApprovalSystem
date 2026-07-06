using ApprovalSystem.Application.DTOs.Requests;
using ApprovalSystem.Application.DTOs.Users;
using ApprovalSystem.Application.DTOs.Workflows;
using ApprovalSystem.Core.Entities;
using ApprovalSystem.Core.Enums;
using ApprovalSystem.Core.Interfaces;

namespace ApprovalSystem.Application.Interfaces;

public interface IApprovalService
{
   

    Task ApproveRequestAsync(
        Guid requestId,
        Guid approverId,
        string comment);

    Task RejectRequestAsync(
        Guid requestId,
        Guid approverId,
        string reason);

    Task EscalateRequestAsync(
        Guid requestId,
        Guid approverId,
        string reason);
}