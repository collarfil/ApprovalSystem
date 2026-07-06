using ApprovalSystem.Application.DTOs.Requests;

namespace ApprovalSystem.Application.Interfaces;

public interface IRequestService
{
    Task<RequestResponseDto> CreateAsync(
        CreateRequestDto dto);

    Task<IEnumerable<RequestResponseDto>> GetAllAsync();

    Task<RequestResponseDto?> GetByIdAsync(
        Guid id);

    Task<RequestResponseDto> UpdateAsync(
        Guid id,
        UpdateRequestDto dto);

    Task DeleteAsync(
        Guid id);
}