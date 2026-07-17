using ApprovalSystem.Application.DTOs.Reports;
using ApprovalSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApprovalSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboardStats()
    {
        var stats = await _reportService.GetDashboardStatsAsync();
        return Ok(stats);
    }

    [HttpGet("trend")]
    public async Task<IActionResult> GetRequestTrend([FromQuery] int days = 30)
    {
        var trend = await _reportService.GetRequestTrendAsync(days);
        return Ok(trend);
    }

    [HttpGet("by-status")]
    public async Task<IActionResult> GetRequestsByStatus()
    {
        var data = await _reportService.GetRequestsByStatusAsync();
        return Ok(data);
    }

    [HttpGet("by-department")]
    public async Task<IActionResult> GetRequestsByDepartment()
    {
        var data = await _reportService.GetRequestsByDepartmentAsync();
        return Ok(data);
    }

[HttpGet("approval-time")]
    public async Task<IActionResult> GetApprovalTime()
    {
        var data = await _reportService.GetApprovalTimeAsync();
        return Ok(data);
    }

    [HttpGet("user-activity")]
    public async Task<IActionResult> GetUserActivity()
    {
        var data = await _reportService.GetUserActivityAsync();
        return Ok(data);
    }
    }