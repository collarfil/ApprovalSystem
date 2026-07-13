using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApprovalSystem.Infrastructure.Persistence;
using ApprovalSystem.Core.Entities;

namespace ApprovalSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkflowSetupController : ControllerBase
{
    private readonly ApprovalDbContext _context;

    public WorkflowSetupController(ApprovalDbContext context)
    {
        _context = context;
    }

    [HttpPost("create-template")]
    public async Task<IActionResult> CreateWorkflowTemplate([FromBody] CreateWorkflowTemplateDto dto)
    {
        var workflowId = Guid.NewGuid();

        // 1. Create the parent Blueprint record
        var definition = new WorkflowDefinition
        {
            Id = workflowId,
            Name = dto.Name,
            Description = dto.Description,
            IsActive = true
        };

        await _context.WorkflowDefinitions.AddAsync(definition);

        // 2. Loop through and create the tracking steps automatically
        foreach (var stepDto in dto.Steps)
        {
            var step = new WorkflowStepDefinition
            {
                Id = Guid.NewGuid(),
                WorkflowDefinitionId = workflowId,
                StepOrder = stepDto.StepOrder,
                StepName = stepDto.StepName,
                RoleId = stepDto.RoleId
            };
            await _context.WorkflowStepDefinitions.AddAsync(step);
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = "Professional workflow blueprint created successfully!", workflowDefinitionId = workflowId });
    }
}

public class CreateWorkflowTemplateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<WorkflowStepDto> Steps { get; set; } = new();
}

public class WorkflowStepDto
{
    public int StepOrder { get; set; }
    public string StepName { get; set; } = string.Empty;
    public Guid RoleId { get; set; }
}