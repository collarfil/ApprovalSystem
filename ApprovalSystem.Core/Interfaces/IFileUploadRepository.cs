using ApprovalSystem.Core.Entities;

namespace ApprovalSystem.Core.Interfaces;

public interface IFileUploadRepository
{
    Task<FileUpload?> GetByIdAsync(Guid id);

    Task<IEnumerable<FileUpload>> GetAllAsync();

    Task AddAsync(FileUpload fileUpload);

    Task UpdateAsync(FileUpload fileUpload);
    Task DeleteAsync(FileUpload fileUpload);

    Task SaveChangesAsync();
}