using ApprovalSystem.Application.DTOs.Requests;
using ApprovalSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApprovalSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RequestController : ControllerBase
{
    private readonly IRequestService _requestService;

    public RequestController(IRequestService requestService)
    {
        _requestService = requestService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRequestDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var requestedById))
        {
            return Unauthorized(new { message = "User identification claim missing or invalid." });
        }

        try
        {
            var result = await _requestService.CreateAsync(dto, requestedById);
            
            // Standard REST specification for object creation returns 201 + path header
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            // Catches any underlying DB exceptions (like foreign key mismatches) safely
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _requestService.GetAllAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _requestService.GetByIdAsync(id);

        if (result == null)
            return NotFound(new { message = $"Request with ID {id} was not found." });

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRequestDto dto)
    {
        try
        {
            var result = await _requestService.UpdateAsync(id, dto);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _requestService.DeleteAsync(id);
        return Ok(new { message = "Request successfully marked as Cancelled." });
    }
}