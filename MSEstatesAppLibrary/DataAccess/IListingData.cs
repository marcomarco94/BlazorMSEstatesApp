using MSEstatesAppLibrary.Models;

namespace MSEstatesAppLibrary.DataAccess;

public interface IListingData
{
    Task<List<ListingModel>?> GetAllListings();
    Task<List<ListingModel>?> NoCache();
    Task<ListingModel> CreateListing(ListingModel listing);
    Task<ListingModel?> GetListingById(string? id);
    Task UpdateListing(ListingModel? updatedListing);
    Task<long> GetTotalListingsCount();
}