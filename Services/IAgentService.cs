using SecurityApi.Contracts;

namespace SecurityApi.Services;

public interface IAgentService
{
    Task<IReadOnlyList<AgentResponse>> GetAllAsync(CancellationToken cancellationToken);

    Task<AgentResponse?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<AgentResponse> CreateAsync(
        CreateAgentRequest request,
        CancellationToken cancellationToken);

    Task<AgentResponse?> UpdateAsync(
        int id,
        UpdateAgentRequest request,
        CancellationToken cancellationToken);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}
