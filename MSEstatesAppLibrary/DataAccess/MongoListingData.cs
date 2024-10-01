using Microsoft.Extensions.Caching.Memory;
using MSEstatesAppLibrary.Models;

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

    public async Task<List<ListingModel>?> GetAllListings()
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

    public async Task<List<ListingModel>?> NoCache()
    {
        var results = await _listings.FindAsync(l => l.Archived == false);
        return results.ToList();
    }

    public async Task<ListingModel> CreateListing(ListingModel listing)
    {
        listing.Id = null;
        listing.ImageUrls?.Clear();
        listing.DateCreated = DateTime.Now;
        var count = await GetTotalListingsCount() + 1301;
        var newToken = listing.Token?.Replace("XXXX", count.ToString());
        listing.Token = newToken;
        await _listings.InsertOneAsync(listing);
        return listing;
    }

    public async Task<ListingModel?> GetListingById(string? id)
    {
        var result = await _listings.FindAsync(l => l.Id == id);
        var output = result.FirstOrDefault();
        return output;
    }

    public async Task UpdateListing(ListingModel? updatedListing)
    {
        if (updatedListing == null) throw new ArgumentNullException(nameof(updatedListing));
        var filter = Builders<ListingModel>.Filter.Eq(l => l.Id, updatedListing.Id);
        await _listings.ReplaceOneAsync(filter, updatedListing);
    }

    public async Task<long> GetTotalListingsCount()
    {
        var count = await _listings.CountDocumentsAsync(_ => true);
        return count;
    }
}