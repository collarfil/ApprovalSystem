using ApprovalSystem.Application.DTOs.Notifications;
using ApprovalSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApprovalSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _service;

    public NotificationController(
        INotificationService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateNotificationDto dto)
    {
        var result =
            await _service.CreateAsync(dto);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(
            await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        Guid id)
    {
        var result =
            await _service.GetByIdAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(
        Guid userId)
    {
        return Ok(
            await _service.GetByUserIdAsync(userId));
    }

    [HttpGet("user/{userId}/unread")]
    public async Task<IActionResult> GetUnread(
        Guid userId)
    {
        return Ok(
            await _service.GetUnreadByUserIdAsync(userId));
    }

    [HttpGet("user/{userId}/count")]
    public async Task<IActionResult> GetUnreadCount(
        Guid userId)
    {
        return Ok(
            await _service.GetUnreadCountAsync(userId));
    }

    [HttpPatch("{id}/read")]
    public async Task<IActionResult> MarkAsRead(
        Guid id)
    {
        await _service.MarkAsReadAsync(id);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        Guid id)
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }
}