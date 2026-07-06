using ApprovalSystem.Application.DTOs.Department;

namespace ApprovalSystem.Application.Interfaces;

public interface IDepartmentService
{
    Task<DepartmentResponseDto> CreateAsync(
        CreateDepartmentDto dto);

    Task<IEnumerable<DepartmentResponseDto>>
        GetAllAsync();

    Task<DepartmentResponseDto?>
        GetByIdAsync(Guid id);

    Task DeleteAsync(Guid id);
}