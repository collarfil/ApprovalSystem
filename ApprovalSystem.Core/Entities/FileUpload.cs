namespace ApprovalSystem.Core.Entities;

public class FileUpload
{
    public Guid Id { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string StoredFileName { get; set; } = string.Empty;

    public string ContentType { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public string FilePath { get; set; } = string.Empty;

    public Guid UploadedBy { get; set; }

    public DateTime UploadedAt { get; set; }

    public User User { get; set; } = null!;
}