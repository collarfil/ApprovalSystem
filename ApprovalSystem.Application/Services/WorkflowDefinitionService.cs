using ApprovalSystem.Application.DTOs.Workflow;
using ApprovalSystem.Application.Interfaces;
using ApprovalSystem.Core.Entities;
using ApprovalSystem.Core.Interfaces;

namespace ApprovalSystem.Application.Services;

public class WorkflowDefinitionService 
    : IWorkflowDefinitionService
{
    private readonly IWorkflowDefinitionRepository _repository;


    public WorkflowDefinitionService(
        IWorkflowDefinitionRepository repository)
    {
        _repository = repository;
    }


    public async Task<WorkflowDefinitionResponseDto> CreateAsync(
        CreateWorkflowDefinitionDto dto)
    {
        var workflow = new WorkflowDefinition
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            IsActive = true
        };


        await _repository.AddAsync(workflow);

        await _repository.SaveChangesAsync();


        return new WorkflowDefinitionResponseDto
        {
            Id = workflow.Id,
            Name = workflow.Name,
            Description = workflow.Description,
            IsActive = workflow.IsActive
        };
    }



    public async Task<IEnumerable<WorkflowDefinitionResponseDto>> GetAllAsync()
    {
        var workflows = await _repository.GetAllAsync();


        return workflows.Select(x => new WorkflowDefinitionResponseDto
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            IsActive = x.IsActive

        });
    }



    public async Task<WorkflowDefinitionResponseDto?> GetByIdAsync(
        Guid id)
    {
        var workflow =
            await _repository.GetByIdAsync(id);


        if(workflow == null)
            return null;


        return new WorkflowDefinitionResponseDto
        {
            Id = workflow.Id,
            Name = workflow.Name,
            Description = workflow.Description,
            IsActive = workflow.IsActive
        };
    }



    public async Task DeleteAsync(Guid id)
    {
        var workflow =
            await _repository.GetByIdAsync(id);


        if(workflow == null)
            return;


        workflow.IsActive = false;


        await _repository.UpdateAsync(workflow);

        await _repository.SaveChangesAsync();
    }
}