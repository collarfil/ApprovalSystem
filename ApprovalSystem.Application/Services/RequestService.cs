using ApprovalSystem.Application.DTOs.Requests;
using ApprovalSystem.Application.Interfaces;
using ApprovalSystem.Core.Entities;
using ApprovalSystem.Core.Enums;
using ApprovalSystem.Core.Interfaces;

namespace ApprovalSystem.Application.Services;

public class RequestService : IRequestService
{
    private readonly IRequestRepository _requestRepository;
    private readonly IWorkflowEngine _workflowEngine;

    public RequestService(
        IRequestRepository requestRepository,
        IWorkflowEngine workflowEngine)
    {
        _requestRepository = requestRepository;
        _workflowEngine = workflowEngine;
    }

    public async Task<RequestResponseDto> CreateAsync(
        CreateRequestDto dto)
    {
        var request = new Request
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            Amount = dto.Amount,
            WorkflowDefinitionId = dto.WorkflowDefinitionId,
            Status = RequestStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _requestRepository.AddAsync(request);
        await _requestRepository.SaveChangesAsync();

        await _workflowEngine.StartWorkflowAsync(request);

        return Map(request);
    }

    public async Task<IEnumerable<RequestResponseDto>> GetAllAsync()
    {
        var requests = await _requestRepository.GetAllAsync();

        return requests.Select(Map);
    }

    public async Task<RequestResponseDto?> GetByIdAsync(
        Guid id)
    {
        var request =
            await _requestRepository.GetRequestWithStepsAsync(id);

        if (request == null)
            return null;

        return Map(request);
    }

    public async Task<RequestResponseDto> UpdateAsync(
        Guid id,
        UpdateRequestDto dto)
    {
        var request =
            await _requestRepository.GetByIdAsync(id)
            ?? throw new Exception("Request not found");

        request.Title = dto.Title;
        request.Description = dto.Description;
        request.Amount = dto.Amount;

        await _requestRepository.UpdateAsync(request);
        await _requestRepository.SaveChangesAsync();

        return Map(request);
    }

    public async Task DeleteAsync(Guid id)
    {
        var request =
            await _requestRepository.GetByIdAsync(id);

        if (request == null)
            return;

        request.Status = RequestStatus.Cancelled;

        await _requestRepository.UpdateAsync(request);
        await _requestRepository.SaveChangesAsync();
    }

    private static RequestResponseDto Map(Request request)
    {
        return new RequestResponseDto
        {
            Id = request.Id,
            Title = request.Title,
            Description = request.Description,
            Amount = request.Amount,
            WorkflowDefinitionId = request.WorkflowDefinitionId,
            Status = request.Status.ToString(),
            CreatedAt = request.CreatedAt
        };
    }
}