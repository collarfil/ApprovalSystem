using System;
using System.Collections.Generic;
using ApprovalSystem.Core.Enums;

namespace ApprovalSystem.Core.Entities
{
public class ApprovalStep
{
    public Guid Id { get; set; }

    public Guid RequestId { get; set; }

    public Request Request { get; set; } = null!;

    public int StepOrder { get; set; }

    public Guid AssignedToUserId { get; set; }

    public User AssignedToUser { get; set; } = null!;

    public ApprovalStatus Status { get; set; }

    public DateTime? ActionDate { get; set; }
    public Guid ApproverId { get; set; }

    public User Approver { get; set; } = null!;
    public string? Comment { get; set; }
}
}
