using ApprovalSystem.Application.DTOs.WorkflowStepDefinitions;
using ApprovalSystem.Application.Interfaces;
using ApprovalSystem.Core.Entities;
using ApprovalSystem.Core.Interfaces;

namespace ApprovalSystem.Application.Services;

public class WorkflowStepDefinitionService
    : IWorkflowStepDefinitionService
{
    private readonly IWorkflowStepDefinitionRepository _repository;

    public WorkflowStepDefinitionService(
        IWorkflowStepDefinitionRepository repository)
    {
        _repository = repository;
    }

    public async Task<WorkflowStepDefinitionResponseDto> CreateAsync(
        CreateWorkflowStepDefinitionDto dto)
    {
        var step = new WorkflowStepDefinition
        {
            Id = Guid.NewGuid(),
            WorkflowDefinitionId = dto.WorkflowDefinitionId,
            StepOrder = dto.StepOrder,
            StepName = dto.StepName,
            RoleId = dto.RoleId,
            IsActive = dto.IsActive
        };

        await _repository.AddAsync(step);

        await _repository.SaveChangesAsync();

        return new WorkflowStepDefinitionResponseDto
        {
            Id = step.Id,
            WorkflowDefinitionId = step.WorkflowDefinitionId,
            StepOrder = step.StepOrder,
            StepName = step.StepName,
            RoleId = step.RoleId,
            IsActive = step.IsActive
        };
    }

    public async Task<IEnumerable<WorkflowStepDefinitionResponseDto>> GetAllAsync()
    {
        var steps = await _repository.GetAllAsync();

        return steps.Select(x => new WorkflowStepDefinitionResponseDto
        {
            Id = x.Id,
            WorkflowDefinitionId = x.WorkflowDefinitionId,
            WorkflowDefinitionName = x.WorkflowDefinition.Name,
            StepOrder = x.StepOrder,
            StepName = x.StepName,
            RoleId = x.RoleId,
            RoleName = x.Role.Name,
            IsActive = x.IsActive
        });
    }

    public async Task<WorkflowStepDefinitionResponseDto?> GetByIdAsync(Guid id)
    {
        var step = await _repository.GetByIdAsync(id);

        if (step == null)
            return null;

        return new WorkflowStepDefinitionResponseDto
        {
            Id = step.Id,
            WorkflowDefinitionId = step.WorkflowDefinitionId,
            WorkflowDefinitionName = step.WorkflowDefinition.Name,
            StepOrder = step.StepOrder,
            StepName = step.StepName,
            RoleId = step.RoleId,
            RoleName = step.Role.Name,
            IsActive = step.IsActive
        };
    }

    public async Task DeleteAsync(Guid id)
    {
        var step = await _repository.GetByIdAsync(id);

        if (step == null)
            return;

        step.IsActive = false;

        await _repository.UpdateAsync(step);

        await _repository.SaveChangesAsync();
    }
}