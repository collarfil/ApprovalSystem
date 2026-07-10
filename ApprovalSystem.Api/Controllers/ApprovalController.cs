using ApprovalSystem.Application.DTOs.Approvals;
using ApprovalSystem.Application.DTOs.Requests;
using ApprovalSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApprovalSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ApprovalController : ControllerBase
{
    private readonly IApprovalService _approvalService;

    public ApprovalController(
        IApprovalService approvalService)
    {
        _approvalService = approvalService;
    }

  

    [HttpPost("approve")]
    public async Task<IActionResult> Approve(
        ApproveRequestDto dto)
    {
        await _approvalService.ApproveRequestAsync(
            dto.RequestId,
            dto.ApproverId,
            dto.Comment);

        return Ok(new
        {
            message = "Request approved successfully."
        });
    }

[HttpGet("pending/{approverId}")]
public async Task<IActionResult> GetPending(
    Guid approverId)
{
    return Ok(
        await _approvalService.GetPendingApprovalsAsync(
            approverId));
}

    [HttpPost("reject")]
    public async Task<IActionResult> Reject(
        RejectRequestDto dto)
    {
        await _approvalService.RejectRequestAsync(
            dto.RequestId,
            dto.ApproverId,
            dto.Reason);

        return Ok(new
        {
            message = "Request rejected successfully."
        });
    }

    [HttpPost("escalate")]
    public async Task<IActionResult> Escalate(
        EscalateRequestDto dto)
    {
        await _approvalService.EscalateRequestAsync(
            dto.RequestId,
            dto.ApproverId,
            dto.Reason);

        return Ok(new
        {
            message = "Request escalated successfully."
        });
    }
    
}