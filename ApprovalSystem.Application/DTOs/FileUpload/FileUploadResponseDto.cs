namespace ApprovalSystem.Application.DTOs.FileUploads;

public class FileUploadResponseDto
{
    public Guid Id { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string StoredFileName { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty;

    public string ContentType { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public Guid UploadedBy { get; set; }

    public DateTime UploadedAt { get; set; }
}