using ApprovalSystem.Application.DTOs.WorkflowRules;
using ApprovalSystem.Application.Interfaces;
using ApprovalSystem.Core.Entities;
using ApprovalSystem.Core.Interfaces;

namespace ApprovalSystem.Application.Services;

public class WorkflowRuleService
    : IWorkflowRuleService
{
    private readonly IWorkflowRuleRepository _repository;

    public WorkflowRuleService(
        IWorkflowRuleRepository repository)
    {
        _repository = repository;
    }

    public async Task<WorkflowRuleResponseDto> CreateAsync(
        CreateWorkflowRuleDto dto)
    {
        var rule = new WorkflowRule
        {
            Id = Guid.NewGuid(),
            WorkflowDefinitionId = dto.WorkflowDefinitionId,
            FieldName = dto.FieldName,
            Operator = dto.Operator,
            Value = dto.Value,
            TargetRoleId = dto.TargetRoleId,
            IsActive = dto.IsActive
        };

        await _repository.AddAsync(rule);

        await _repository.SaveChangesAsync();

        return new WorkflowRuleResponseDto
        {
            Id = rule.Id,
            WorkflowDefinitionId = rule.WorkflowDefinitionId,
            FieldName = rule.FieldName,
            Operator = rule.Operator,
            Value = rule.Value,
            TargetRoleId = rule.TargetRoleId,
            IsActive = rule.IsActive
        };
    }

    public async Task<IEnumerable<WorkflowRuleResponseDto>> GetAllAsync()
    {
        var rules = await _repository.GetAllAsync();

        return rules.Select(x => new WorkflowRuleResponseDto
        {
            Id = x.Id,
            WorkflowDefinitionId = x.WorkflowDefinitionId,
            WorkflowDefinitionName = x.WorkflowDefinition.Name,
            FieldName = x.FieldName,
            Operator = x.Operator,
            Value = x.Value,
            TargetRoleId = x.TargetRoleId,
            TargetRoleName = x.TargetRole.Name,
            IsActive = x.IsActive
        });
    }

    public async Task<WorkflowRuleResponseDto?> GetByIdAsync(Guid id)
    {
        var rule = await _repository.GetByIdAsync(id);

        if (rule == null)
            return null;

        return new WorkflowRuleResponseDto
        {
            Id = rule.Id,
            WorkflowDefinitionId = rule.WorkflowDefinitionId,
            WorkflowDefinitionName = rule.WorkflowDefinition.Name,
            FieldName = rule.FieldName,
            Operator = rule.Operator,
            Value = rule.Value,
            TargetRoleId = rule.TargetRoleId,
            TargetRoleName = rule.TargetRole.Name,
            IsActive = rule.IsActive
        };
    }

    public async Task DeleteAsync(Guid id)
    {
        var rule = await _repository.GetByIdAsync(id);

        if (rule == null)
            return;

        rule.IsActive = false;

        await _repository.UpdateAsync(rule);

        await _repository.SaveChangesAsync();
    }
}