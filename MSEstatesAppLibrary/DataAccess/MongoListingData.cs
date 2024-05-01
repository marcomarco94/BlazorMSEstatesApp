using Microsoft.Extensions.Caching.Memory;

namespace MSEstatesAppLibrary.DataAccess;

public class MongoListingData : IListingData
{
    private const string CacheName = "ListingData";
    private readonly IMemoryCache _cache;
    private readonly IMongoCollection<ListingModel> _listings;

    public MongoListingData(IDbConnection db, IMemoryCache cache)
    {
        _cache = cache;
        _listings = db.ListingCollection;
    }

    public async Task<List<ListingModel>> GetAllListings()
    {
        var output = _cache.Get<List<ListingModel>>(CacheName);
        if (output is null)
        {
            var results = await _listings.FindAsync(l => l.Archived == false);
            output = results.ToList();

            _cache.Set(CacheName, output, TimeSpan.FromMinutes(5));
        }

        return output;
    }
    
    public async Task<List<ListingModel>> NoCache()
    {
            var results = await _listings.FindAsync(l => l.Archived == false);
            return results.ToList(); 
    }

    public async Task CreateListing(ListingModel listing)
    {
        listing.Id = null;
        await _listings.InsertOneAsync(listing);
    }

    public async Task<ListingModel> GetListingById(string id)
    {
        var cacheKey = $"{CacheName}:{id}";
        var output = _cache.Get<ListingModel>(cacheKey);
        if (output is null)
        {
            var result = await _listings.FindAsync(l => l.Id == id);
            output = result.FirstOrDefault();
            _cache.Set(cacheKey, output, TimeSpan.FromMinutes(5));
        }

        return output;
    }
    
    public async Task<ListingModel> GetListingByToken(string token)
    {
        var cacheKey = $"{CacheName}:{token}";
        var output = _cache.Get<ListingModel>(cacheKey);
        if (output is null)
        {
            var result = await _listings.FindAsync(l => l.Token == token);
            output = result.FirstOrDefault();
            _cache.Set(cacheKey, output, TimeSpan.FromMinutes(5));
        }

        return output;
    }

    public async Task UpdateListing(ListingModel? updatedListing)
    {
        if (updatedListing == null)
        {
            throw new ArgumentNullException(nameof(updatedListing));
        }

        var filter = Builders<ListingModel>.Filter.Eq(l => l.Id, updatedListing.Id);
        await _listings.ReplaceOneAsync(filter, updatedListing);
    }
    
    public async Task<long> GetTotalListingsCount()
    {
        var count = await _listings.CountDocumentsAsync(_ => true);
        return count;
    }
}