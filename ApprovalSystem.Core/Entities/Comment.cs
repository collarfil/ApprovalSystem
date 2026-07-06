namespace ApprovalSystem.Core.Entities
{
    public class Comment
    {
    public Guid Id { get; set; }

    public Guid RequestId { get; set; }

    public Guid UserId { get; set; }

    public string Message { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
}