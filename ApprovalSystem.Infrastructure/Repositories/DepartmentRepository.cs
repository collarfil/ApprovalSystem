using ApprovalSystem.Core.Entities;
using ApprovalSystem.Core.Enums;
using ApprovalSystem.Core.Interfaces;
using ApprovalSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApprovalSystem.Infrastructure.Repositories;



public class DepartmentRepository
    : IDepartmentRepository
{
    private readonly ApprovalDbContext _context;

    public DepartmentRepository(
        ApprovalDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Department department)
    {
        await _context.Departments
            .AddAsync(department);
    }

    public async Task<Department?> GetByIdAsync(
        Guid id)
    {
        return await _context.Departments
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Department>> GetAllAsync()
    {
        return await _context.Departments
            .ToListAsync();
    }

    public async Task UpdateAsync(
        Department department)
    {
        _context.Departments.Update(department);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}