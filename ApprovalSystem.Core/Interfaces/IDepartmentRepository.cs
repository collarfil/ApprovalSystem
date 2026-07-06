using ApprovalSystem.Core.Entities;

namespace ApprovalSystem.Core.Interfaces;

public interface IDepartmentRepository
{
    Task AddAsync(Department department);

    Task<Department?> GetByIdAsync(Guid id);

    Task<IEnumerable<Department>> GetAllAsync();

    Task UpdateAsync(Department department);

    Task SaveChangesAsync();
}
