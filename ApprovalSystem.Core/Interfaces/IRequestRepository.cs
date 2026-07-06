using ApprovalSystem.Core.Entities;

namespace ApprovalSystem.Core.Interfaces;

public interface IRequestRepository
{
    Task<Request?> GetByIdAsync(Guid id);

    Task<Request?> GetRequestWithStepsAsync(Guid id);

    Task<IEnumerable<Request>> GetAllAsync();

    Task AddAsync(Request request);

    Task UpdateAsync(Request request);
    Task<List<Request>> GetByRequesterIdAsync(
    Guid requesterId);

    Task SaveChangesAsync();
}