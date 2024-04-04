using Microsoft.Extensions.Caching.Memory;

namespace MSEstatesAppLibrary.DataAccess;

public class MongoAgentData : IAgentData
{
    private const string CacheName = "Agent";
    private readonly IMongoCollection<AgentModel> _agents;
    private readonly IMemoryCache _cache;

    public MongoAgentData(IDbConnection db, IMemoryCache cache)
    {
        _cache = cache;
        _agents = db.AgentCollection;
    }

    public async Task<AgentModel> GetAgentById(string id)
    {
        var cacheKey = $"{CacheName}:{id}";
        var output = _cache.Get<AgentModel>(cacheKey);
        if (output is null)
        {
            var result = await _agents.FindAsync(a => a.AgentId == id);
            output = result.FirstOrDefault();
            _cache.Set(cacheKey, output, TimeSpan.FromMinutes(60));
        }

        return output;
    }

    public async Task UpdateAgent(string id, AgentModel? updatedAgent)
    {
        var filter = Builders<AgentModel>.Filter.Eq(a => a.AgentId, id);
        var updateBuilder = Builders<AgentModel>.Update;

        var update = Builders<AgentModel>.Update.Combine(
            typeof(AgentModel)
                .GetProperties()
                .Where(prop => prop.GetValue(updatedAgent) != null)
                .Select(prop => updateBuilder.Set(prop.Name, prop.GetValue(updatedAgent)))
                .ToList()
        );

        await _agents.UpdateOneAsync(filter, update);
    }


    public async Task<List<AgentModel>> GetAllAgents()
    {
        var output = _cache.Get<List<AgentModel>>(CacheName);
        if (output is null)
        {
            var results = await _agents.Find(agent => true).ToListAsync();
            output = results.ToList();

            _cache.Set(CacheName, output, TimeSpan.FromMinutes(60));
        }

        return output;
    }

    public async Task CreateAgent(AgentModel agent)
    {
        await _agents.InsertOneAsync(agent);
    }
}