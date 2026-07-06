using ApprovalSystem.Application.DTOs.Approvals;
using ApprovalSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApprovalSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WorkflowController : ControllerBase
{
    private readonly IWorkflowEngine _workflowEngine;

    public WorkflowController(
        IWorkflowEngine workflowEngine)
    {
        _workflowEngine = workflowEngine;
    }


    [HttpPost("approve")]
    public async Task<IActionResult> Approve(
        ApproveRequestDto dto)
    {
        await _workflowEngine.ApproveStepAsync(
            dto.RequestId,
            dto.ApproverId,
            dto.Comment);

        return Ok(new
        {
            Message = "Request approved"
        });
    }



    [HttpPost("reject")]
    public async Task<IActionResult> Reject(
        RejectRequestDto dto)
    {
        await _workflowEngine.RejectStepAsync(
            dto.RequestId,
            dto.ApproverId,
            dto.Reason);

        return Ok(new
        {
            Message = "Request rejected"
        });
    }



    [HttpPost("escalate")]
    public async Task<IActionResult> Escalate(
        EscalateRequestDto dto)
    {
        await _workflowEngine.EscalateStepAsync(
            dto.RequestId,
            dto.ApproverId,
            dto.Reason);

        return Ok(new
        {
            Message = "Request escalated"
        });
    }
}