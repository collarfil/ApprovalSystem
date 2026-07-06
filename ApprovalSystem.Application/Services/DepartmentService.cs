using ApprovalSystem.Application.DTOs.Department;
using ApprovalSystem.Application.Interfaces;
using ApprovalSystem.Core.Entities;
using ApprovalSystem.Core.Interfaces;



namespace ApprovalSystem.Application.Services;
public class DepartmentService
    : IDepartmentService
{
    private readonly IDepartmentRepository _repository;

    public DepartmentService(
        IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<DepartmentResponseDto> CreateAsync(
        CreateDepartmentDto dto)
    {
        var department = new Department
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description
        };

        await _repository.AddAsync(department);

        await _repository.SaveChangesAsync();

        return new DepartmentResponseDto
        {
            Id = department.Id,
            Name = department.Name,
            Description = department.Description,
            IsActive = department.IsActive
        };
    }

    public async Task<IEnumerable<DepartmentResponseDto>>
        GetAllAsync()
    {
        var departments =
            await _repository.GetAllAsync();

        return departments.Select(x =>
            new DepartmentResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsActive = x.IsActive
            });
    }

    public async Task<DepartmentResponseDto?>
        GetByIdAsync(Guid id)
    {
        var department =
            await _repository.GetByIdAsync(id);

        if (department == null)
            return null;

        return new DepartmentResponseDto
        {
            Id = department.Id,
            Name = department.Name,
            Description = department.Description,
            IsActive = department.IsActive
        };
    }

    public async Task DeleteAsync(Guid id)
    {
        var department =
            await _repository.GetByIdAsync(id);

        if (department == null)
            return;

        department.IsActive = false;

        await _repository.UpdateAsync(department);

        await _repository.SaveChangesAsync();
    }
}