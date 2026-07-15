using ApprovalSystem.Application.DTOs.AuditLogs;
using ApprovalSystem.Application.DTOs.Notifications;
using ApprovalSystem.Application.Interfaces;
using ApprovalSystem.Core.Entities;
using ApprovalSystem.Core.Enums;
using ApprovalSystem.Core.Interfaces;

namespace ApprovalSystem.Application.Services;

public class WorkflowEngine : IWorkflowEngine
{
    private readonly IRequestRepository _requestRepository;
    private readonly IWorkflowDefinitionRepository _workflowDefinitionRepository;
    private readonly IWorkflowStepDefinitionRepository _workflowStepRepository;
    private readonly IWorkflowRuleRepository _workflowRuleRepository;
    private readonly IApprovalStepRepository _approvalStepRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly INotificationService _notificationService;
    private readonly IEmailService _emailService;
    private readonly IAuditLogService _auditLogService;

    public WorkflowEngine(
        IRequestRepository requestRepository,
        IWorkflowDefinitionRepository workflowDefinitionRepository,
        IWorkflowStepDefinitionRepository workflowStepRepository,
        IWorkflowRuleRepository workflowRuleRepository,
        IApprovalStepRepository approvalStepRepository,
        IUserRepository userRepository,
        IUserRoleRepository userRoleRepository,
        INotificationService notificationService,
        IEmailService emailService,
        IAuditLogService auditLogService)
    {
        _requestRepository = requestRepository;
        _workflowDefinitionRepository = workflowDefinitionRepository;
        _workflowStepRepository = workflowStepRepository;
        _workflowRuleRepository = workflowRuleRepository;
        _approvalStepRepository = approvalStepRepository;
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _notificationService = notificationService;
        _emailService = emailService;
        _auditLogService = auditLogService;
    }

    public async Task StartWorkflowAsync(Request request)
    {
        var workflow =
            await _workflowDefinitionRepository
                .GetWithStepsAsync(request.WorkflowDefinitionId);

        if (workflow == null)
            throw new Exception("Workflow definition not found.");

        if (!workflow.IsActive)
            throw new Exception("Workflow is inactive.");

        var steps =
            await LoadWorkflowStepsAsync(workflow.Id);

        if (!steps.Any())
            throw new Exception("Workflow contains no active steps.");

        var firstStep = steps.First();

        var approverId =
            await DetermineApproverAsync(firstStep, request);

        await CreateApprovalStepAsync(
            request,
            firstStep,
            approverId);

        request.Status = RequestStatus.Pending;

        await _requestRepository.UpdateAsync(request);
        await _requestRepository.SaveChangesAsync();

        await NotifyApproverAsync(
            approverId,
            request);

        await WriteAuditAsync(
            request,
            "Workflow Started",
            request.RequestedById,
            "Workflow initiated.");
    }

    public async Task ApproveStepAsync(
        Guid requestId,
        Guid approverId,
        string comment)
    {
        var request =
            await _requestRepository
                .GetRequestWithStepsAsync(requestId);

        if (request == null)
            throw new Exception("Request not found.");

        var currentStep =
            await _approvalStepRepository
                .GetCurrentStepAsync(request.Id);

        if (currentStep == null)
            throw new Exception("No active approval step.");

        if (currentStep.ApproverId != approverId)
            throw new Exception("Unauthorized approver.");

        currentStep.Status = ApprovalStatus.Approved;
        currentStep.ActionDate = DateTime.UtcNow;
        currentStep.Comment = comment;

        await _approvalStepRepository.UpdateAsync(currentStep);
        await _approvalStepRepository.SaveChangesAsync();

        await WriteAuditAsync(
            request,
            "Approval",
            approverId,
            comment);

        await MoveToNextStepAsync(request);
    }

     public async Task RejectStepAsync(
    Guid requestId,
    Guid approverId,
    string reason)
        {
            var request =
                await _requestRepository
                    .GetRequestWithStepsAsync(requestId);

            if (request == null)
                throw new Exception("Request not found.");

            var currentStep =
                await _approvalStepRepository
                    .GetCurrentStepAsync(request.Id);

            if (currentStep == null)
                throw new Exception("No pending approval step.");

            if (currentStep.ApproverId != approverId)
                throw new Exception("Unauthorized approver.");

            currentStep.Status = ApprovalStatus.Rejected;
            currentStep.ActionDate = DateTime.UtcNow;
            currentStep.Comment = reason;

            await _approvalStepRepository.UpdateAsync(currentStep);
            await _approvalStepRepository.SaveChangesAsync();

            request.Status = RequestStatus.Rejected;
            request.CurrentApprovalStepId = null;

            await _requestRepository.UpdateAsync(request);
            await _requestRepository.SaveChangesAsync();

            await NotifyRequesterAsync(
                request,
                $"Your request \"{request.Title}\" has been rejected.");

            await WriteAuditAsync(
                request,
                "Rejected",
                approverId,
                reason);
        }

   public async Task EscalateStepAsync(
    Guid requestId,
    Guid approverId,
    string reason)
{
    var request =
        await _requestRepository
            .GetRequestWithStepsAsync(requestId);

    if (request == null)
        throw new Exception("Request not found.");

    request.Status = RequestStatus.Escalated;

    await _requestRepository.UpdateAsync(request);
    await _requestRepository.SaveChangesAsync();

    await WriteAuditAsync(
        request,
        "Escalated",
        approverId,
        reason);

    await NotifyRequesterAsync(
        request,
        $"Your request \"{request.Title}\" has been escalated.");
}

    private async Task<List<WorkflowStepDefinition>>
        LoadWorkflowStepsAsync(Guid workflowDefinitionId)
    {
        return await _workflowStepRepository
            .GetByWorkflowDefinitionIdAsync(workflowDefinitionId);
    }

    private async Task<List<WorkflowRule>>
        LoadWorkflowRulesAsync(Guid workflowDefinitionId)
    {
        return await _workflowRuleRepository
            .GetByWorkflowDefinitionIdAsync(workflowDefinitionId);
    }

    private async Task<Guid> DetermineApproverAsync(
        WorkflowStepDefinition step,
        Request request)
    {
        var rules =
            await LoadWorkflowRulesAsync(
                step.WorkflowDefinitionId);

        foreach (var rule in rules)
        {
            if (rule.FieldName.Equals("Amount",
                StringComparison.OrdinalIgnoreCase))
            {
                decimal ruleValue =
                    decimal.Parse(rule.Value);

                bool matched = false;

                switch (rule.Operator)
                {
                    case ">":
                        matched = request.Amount > ruleValue;
                        break;

                    case "<":
                        matched = request.Amount < ruleValue;
                        break;

                    case ">=":
                        matched = request.Amount >= ruleValue;
                        break;

                    case "<=":
                        matched = request.Amount <= ruleValue;
                        break;

                    case "=":
                        matched = request.Amount == ruleValue;
                        break;
                }

                if (matched)
                {
                    var users =
                        await _userRoleRepository
                            .GetByRoleIdAsync(rule.TargetRoleId);

                    var approver =
                        users.FirstOrDefault();

                    if (approver != null)
                        return approver.UserId;
                }
            }
        }

        var defaultApprovers =
            await _userRoleRepository
                .GetByRoleIdAsync(step.RoleId);

        var user =
            defaultApprovers.FirstOrDefault();

        if (user == null)
            throw new Exception(
                $"No user assigned to role {step.Role.Name}");

        return user.UserId;
    }

    private async Task CreateApprovalStepAsync(
        Request request,
        WorkflowStepDefinition step,
        Guid approverId)
    {
        var approval = new ApprovalStep
        {
            Id = Guid.NewGuid(),
            RequestId = request.Id,
            StepOrder = step.StepOrder,
            ApproverId = approverId,
            AssignedToUserId = approverId,
            Status = ApprovalStatus.Pending
        };

        await _approvalStepRepository.AddAsync(approval);
        await _approvalStepRepository.SaveChangesAsync();

        request.CurrentApprovalStepId = approval.Id;
    }

    private async Task NotifyApproverAsync(
        Guid approverId,
        Request request)
    {
        var approver =
            await _userRepository.GetByIdAsync(approverId);

        if (approver == null)
            return;

        // Always create notification in database
        await _notificationService.CreateAsync(
            new CreateNotificationDto
            {
                UserId = approver.Id,
                Title = "Approval Required",
                Message =
                    $"Request \"{request.Title}\" requires your approval."
            });

        // Try to send email but don't fail if it doesn't work
        try
        {
            await _emailService.SendApprovalAssignedAsync(
                approver.Email,
                $"{approver.FirstName} {approver.LastName}",
                request.Title);
        }
        catch (Exception ex)
        {
            // Log the error but don't throw - email is optional
            Console.WriteLine($"[EMAIL WARNING] Could not send approval email to {approver.Email}: {ex.Message}");
        }
    }

    private async Task NotifyRequesterAsync(
        Request request,
        string message)
    {
        var requester =
            await _userRepository
                .GetByIdAsync(request.RequestedById);

        if (requester == null)
            return;

        // Always create notification in database
        await _notificationService.CreateAsync(
            new CreateNotificationDto
            {
                UserId = requester.Id,
                Title = "Request Update",
                Message = message
            });

        // Try to send email but don't fail if it doesn't work
        try
        {
            if (request.Status == RequestStatus.Approved)
            {
                await _emailService.SendRequestApprovedAsync(
                    requester.Email,
                    requester.FirstName,
                    request.Title);
            }
            else if (request.Status == RequestStatus.Rejected)
            {
                await _emailService.SendRequestRejectedAsync(
                    requester.Email,
                    requester.FirstName,
                    request.Title,
                    message);
            }
        }
        catch (Exception ex)
        {
            // Log the error but don't throw - email is optional
            Console.WriteLine($"[EMAIL WARNING] Could not send notification email to {requester.Email}: {ex.Message}");
        }
    }

    private async Task WriteAuditAsync(
        Request request,
        string action,
        Guid userId,
        string comment)
    {
        await _auditLogService.CreateAsync(
            new CreateAuditLogDto
            {
                EntityName = nameof(Request),
                EntityId = request.Id,
                Action = action,
                OldValue = string.Empty,
                NewValue = comment,
                PerformedById = userId
            });
    }

    private async Task MoveToNextStepAsync(Request request)
    {
        try
        {
            Console.WriteLine($"[MOVE] Moving to next step for request {request.Id}");
            
            var allSteps = await LoadWorkflowStepsAsync(request.WorkflowDefinitionId);
            Console.WriteLine($"[MOVE] Found {allSteps.Count} total steps");

            var currentApproval = await _approvalStepRepository.GetCurrentStepAsync(request.Id);

            int completedOrder = 0;
            if (currentApproval != null)
                completedOrder = currentApproval.StepOrder;

            var nextStep = allSteps
                .OrderBy(x => x.StepOrder)
                .FirstOrDefault(x => x.StepOrder > completedOrder);

            if (nextStep == null)
            {
                Console.WriteLine($"[MOVE] No more steps. Completing workflow.");
                await CompleteWorkflowAsync(request);
                return;
            }

            Console.WriteLine($"[MOVE] Next step: {nextStep.StepName} (Order: {nextStep.StepOrder})");

            try
            {
                var approverId = await DetermineApproverAsync(nextStep, request);
                Console.WriteLine($"[MOVE] Found approver: {approverId}");

                await CreateApprovalStepAsync(request, nextStep, approverId);

                await _requestRepository.UpdateAsync(request);
                await _requestRepository.SaveChangesAsync();

                await NotifyApproverAsync(approverId, request);

                await WriteAuditAsync(request, "Moved To Next Step", approverId, $"Workflow moved to step {nextStep.StepOrder}");
                
                Console.WriteLine($"[MOVE] Successfully moved to step {nextStep.StepOrder}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MOVE ERROR] Could not find approver for step {nextStep.StepName}: {ex.Message}");
                // Don't fail the approval, just log it
                // The request stays in the current step until an approver is assigned
                throw new Exception($"Cannot proceed to next step: No approver found for role '{nextStep.Role?.Name ?? "Unknown"}'");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[MOVE ERROR] {ex.Message}");
            throw;
        }
    }

    private async Task CompleteWorkflowAsync(
        Request request)
    {
        request.Status = RequestStatus.Approved;
        request.CurrentApprovalStepId = null;

        await _requestRepository.UpdateAsync(request);
        await _requestRepository.SaveChangesAsync();

        await NotifyRequesterAsync(
            request,
            $"Your request \"{request.Title}\" has been approved.");

        await WriteAuditAsync(
            request,
            "Workflow Completed",
            request.RequestedById,
            "Workflow completed successfully.");
    }
}