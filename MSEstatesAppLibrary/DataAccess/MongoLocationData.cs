using Microsoft.Extensions.Caching.Memory;
using MSEstatesAppLibrary.Models;

namespace MSEstatesAppLibrary.DataAccess;

public class MongoLocationData : ILocationData
{
    private const string CacheName = "LocationData";
    private readonly IMemoryCache _cache;
    private readonly IMongoCollection<LocationModel> _locations;

    public MongoLocationData(IDbConnection db, IMemoryCache cache)
    {
        _cache = cache;
        _locations = db.LocationCollection;
    }

    public async Task<List<LocationModel>?> GetAllLocations()
    {
        var output = _cache.Get<List<LocationModel>>(CacheName);
        if (output is null)
        {
            var results = await _locations.FindAsync(_ => true);
            output = results.ToList();
            output = output.OrderBy(location => location.Location).ToList();
            _cache.Set(CacheName, output, TimeSpan.FromMinutes(5));
        }

        return output;
    }

    public Task CreateLocation(LocationModel location)
    {
        if (location.Id is not null) location.Id = null;
        return _locations.InsertOneAsync(location);
    }

    public async Task UpdateLocation(LocationModel? updatedLocation)
    {
        if (updatedLocation == null) throw new ArgumentNullException(nameof(updatedLocation));

        var filter = Builders<LocationModel>.Filter.Eq(l => l.Id, updatedLocation.Id);
        await _locations.ReplaceOneAsync(filter, updatedLocation);
    }
}