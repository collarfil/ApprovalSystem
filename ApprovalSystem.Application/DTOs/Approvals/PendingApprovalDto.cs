namespace ApprovalSystem.Application.DTOs.Approvals;

public class PendingApprovalDto
{
    public Guid ApprovalStepId { get; set; }

    public Guid RequestId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string Status { get; set; } = string.Empty;

    public int StepOrder { get; set; }

    public Guid RequestedById { get; set; }

    public string RequestedByName { get; set; } = string.Empty;

    public Guid AssignedToUserId { get; set; }

    public string AssignedToName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}