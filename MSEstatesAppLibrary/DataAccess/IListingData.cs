namespace MSEstatesAppLibrary.DataAccess;

public interface IListingData
{
    Task<List<ListingModel>> GetAllListings();
    Task<List<ListingModel>> NoCache();
    Task CreateListing(ListingModel listing);
    Task<ListingModel> GetListingById(string id);
    Task<ListingModel> GetListingByToken(string token);
    Task UpdateListing(ListingModel? updatedListing);
    Task<long> GetTotalListingsCount();
}