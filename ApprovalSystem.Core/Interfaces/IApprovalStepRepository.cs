using ApprovalSystem.Core.Entities;

namespace ApprovalSystem.Core.Interfaces;


public interface IApprovalStepRepository
{
    Task<ApprovalStep?> GetByIdAsync(Guid id);


    Task<ApprovalStep?> GetCurrentStepAsync(
        Guid requestId);


    Task<List<ApprovalStep>> GetStepsByRequestIdAsync(
        Guid requestId);


    Task<List<ApprovalStep>> GetStepsByApproverIdAsync(
        Guid approverId);


    Task AddAsync(
        ApprovalStep step);

    Task UpdateAsync(
    ApprovalStep step);
    
    Task<List<ApprovalStep>>
    GetPendingApprovalsAsync(Guid approverId);
    Task SaveChangesAsync();
}