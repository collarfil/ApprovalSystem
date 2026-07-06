using ApprovalSystem.Application.DTOs.UserRoles;
using ApprovalSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApprovalSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserRoleController : ControllerBase
{
    private readonly IUserRoleService _service;

    public UserRoleController(
        IUserRoleService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateUserRoleDto dto)
    {
        var result =
            await _service.CreateAsync(dto);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result =
            await _service.GetAllAsync();

        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(
        Guid userId,
        Guid roleId)
    {
        await _service.DeleteAsync(
            userId,
            roleId);

        return NoContent();
    }
}