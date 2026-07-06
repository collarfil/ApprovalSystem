using ApprovalSystem.Application.DTOs.FileUploads;

namespace ApprovalSystem.Application.Interfaces;

public interface IFileUploadService
{
    Task<FileUploadResponseDto> CreateAsync(
        CreateFileUploadDto dto);

    Task<IEnumerable<FileUploadResponseDto>>
        GetAllAsync();

    Task<FileUploadResponseDto?>
        GetByIdAsync(Guid id);

    Task DeleteAsync(Guid id);
}