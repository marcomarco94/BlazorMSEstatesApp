using MSEstatesAppLibrary.Models;

namespace MSEstatesAppLibrary.DataAccess;

public interface IAgentData
{
    Task<AgentModel?> GetAgentById(string id);
    Task UpdateAgent(string id, AgentModel? updatedAgent);
    Task<List<AgentModel>> GetAllAgents();
    Task CreateAgent(AgentModel agent);
}