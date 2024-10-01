using Microsoft.Extensions.Caching.Memory;
using MSEstatesAppLibrary.Models;

namespace MSEstatesAppLibrary.DataAccess;

public class MongoRealtorContactData : IRealtorContactData
{
    private const string CacheName = "RealtorContactData";
    private readonly IMemoryCache _cache;
    private readonly IMongoCollection<RealtorContactModel> _realtorContacts;

    public MongoRealtorContactData(IDbConnection db, IMemoryCache cache)
    {
        _cache = cache;
        _realtorContacts = db.RealtorContactCollection;
    }

    public async Task<List<RealtorContactModel>?> GetAllRealtorContacts()
    {
        var output = _cache.Get<List<RealtorContactModel>>(CacheName);
        if (output is null)
        {
            var results = await _realtorContacts.FindAsync(_ => true);
            output = results.ToList();

            _cache.Set(CacheName, output, TimeSpan.FromMinutes(5));
        }

        return output;
    }

    public Task CreateRealtorContact(RealtorContactModel realtorContact)
    {
        return _realtorContacts.InsertOneAsync(realtorContact);
    }

    public Task ArchiveRealtorContact(string id)
    {
        return _realtorContacts.UpdateOneAsync(r => r.Id == id,
            Builders<RealtorContactModel>.Update.Set(r => r.Archived, true));
    }

    public async Task<RealtorContactModel?> GetRealtorContactById(string id)
    {
        var cacheKey = $"{CacheName}:id:{id}";
        var output = _cache.Get<RealtorContactModel>(cacheKey);
        if (output is null)
        {
            var result = await _realtorContacts.FindAsync(r => r.Id == id);
            output = result.FirstOrDefault();
            _cache.Set(cacheKey, output, TimeSpan.FromMinutes(5));
        }

        return output;
    }
}