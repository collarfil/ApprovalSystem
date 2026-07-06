using ApprovalSystem.Core.Entities;
using ApprovalSystem.Core.Interfaces;
using ApprovalSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApprovalSystem.Infrastructure.Repositories;

public class FileUploadRepository
    : IFileUploadRepository
{
    private readonly ApprovalDbContext _context;

    public FileUploadRepository(
        ApprovalDbContext context)
    {
        _context = context;
    }

    public async Task<FileUpload?> GetByIdAsync(
        Guid id)
    {
        return await _context.FileUploads
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<FileUpload>>
        GetAllAsync()
    {
        return await _context.FileUploads
            .ToListAsync();
    }

    public async Task AddAsync(
        FileUpload fileUpload)
    {
        await _context.FileUploads
            .AddAsync(fileUpload);
    }

    public Task UpdateAsync(
        FileUpload fileUpload)
    {
        _context.FileUploads
            .Update(fileUpload);

        return Task.CompletedTask;
    }
    public Task DeleteAsync(FileUpload fileUpload)
{
    _context.FileUploads.Remove(fileUpload);

    return Task.CompletedTask;
}
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}