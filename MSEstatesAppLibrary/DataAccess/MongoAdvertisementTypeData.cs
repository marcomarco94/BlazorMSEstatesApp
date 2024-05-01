using Microsoft.Extensions.Caching.Memory;

namespace MSEstatesAppLibrary.DataAccess;

public class MongoAdvertisementTypeData : IAdvertisementTypeData
{
    private const string CacheName = "AdvertisementTypeData";
    private readonly IMongoCollection<AdvertisementTypeModel> _advertisementTypes;
    private readonly IMemoryCache _cache;

    public MongoAdvertisementTypeData(IDbConnection db, IMemoryCache cache)
    {
        _cache = cache;
        _advertisementTypes = db.AdvertisementTypeCollection;
    }

    public async Task<List<AdvertisementTypeModel>> GetAllAdvertisementTypes()
    {
        var output = _cache.Get<List<AdvertisementTypeModel>>(CacheName);
        if (output is null)
        {
            var results = await _advertisementTypes.FindAsync(_ => true);
            output = results.ToList();

            _cache.Set(CacheName, output, TimeSpan.FromMinutes(5));
        }

        return output;
    }

    public Task CreateAdvertisementType(AdvertisementTypeModel advertisementType)
    {
        return _advertisementTypes.InsertOneAsync(advertisementType);
    }
}