using System;
using System.Collections.Generic;
using ApprovalSystem.Core.Enums;

namespace ApprovalSystem.Core.Entities
{

public class Request
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public RequestStatus Status { get; set; }
    public Guid? CurrentApprovalStepId { get; set; }

    public Guid WorkflowDefinitionId { get; set; }

    public WorkflowDefinition WorkflowDefinition { get; set; }
        = null!;

    public Guid RequestedById { get; set; }

    public User RequestedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public ICollection<ApprovalStep> ApprovalSteps
        = new List<ApprovalStep>();
}
}