using ApprovalSystem.Application.DTOs.FileUploads;
using ApprovalSystem.Application.Interfaces;
using ApprovalSystem.Core.Entities;
using ApprovalSystem.Core.Interfaces;

namespace ApprovalSystem.Application.Services;

public class FileUploadService
    : IFileUploadService
{
    private readonly IFileUploadRepository _repository;

    public FileUploadService(
        IFileUploadRepository repository)
    {
        _repository = repository;
    }

    public async Task<FileUploadResponseDto>
        CreateAsync(
            CreateFileUploadDto dto)
    {
        var file = new FileUpload
        {
            Id = Guid.NewGuid(),
            FileName = dto.FileName,
            StoredFileName = dto.StoredFileName,
            FilePath = dto.FilePath,
            ContentType = dto.ContentType,
            FileSize = dto.FileSize,
            UploadedBy = dto.UploadedBy,
            UploadedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(file);

        await _repository.SaveChangesAsync();

        return new FileUploadResponseDto
        {
            Id = file.Id,
            FileName = file.FileName,
            StoredFileName = file.StoredFileName,
            FilePath = file.FilePath,
            ContentType = file.ContentType,
            FileSize = file.FileSize,
            UploadedBy = file.UploadedBy,
            UploadedAt = file.UploadedAt
        };
    }

    public async Task<IEnumerable<FileUploadResponseDto>>
        GetAllAsync()
    {
        var files =
            await _repository.GetAllAsync();

        return files.Select(x =>
            new FileUploadResponseDto
            {
                Id = x.Id,
                FileName = x.FileName,
                StoredFileName = x.StoredFileName,
                FilePath = x.FilePath,
                ContentType = x.ContentType,
                FileSize = x.FileSize,
                UploadedBy = x.UploadedBy,
                UploadedAt = x.UploadedAt
            });
    }

    public async Task<FileUploadResponseDto?>
        GetByIdAsync(Guid id)
    {
        var file =
            await _repository.GetByIdAsync(id);

        if (file == null)
            return null;

        return new FileUploadResponseDto
        {
            Id = file.Id,
            FileName = file.FileName,
            StoredFileName = file.StoredFileName,
            FilePath = file.FilePath,
            ContentType = file.ContentType,
            FileSize = file.FileSize,
            UploadedBy = file.UploadedBy,
            UploadedAt = file.UploadedAt
        };
    }

   public async Task DeleteAsync(Guid id)
{
    var file = await _repository.GetByIdAsync(id);

    if (file == null)
        throw new Exception("File not found.");

    await _repository.DeleteAsync(file);

    await _repository.SaveChangesAsync();
}
}