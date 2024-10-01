using BlazorMSEstatesUIAdmin.Models;

namespace BlazorMSEstatesUIAdmin.Services;

public class ListingStateService
{
    public List<ListingModel>? FilteredItems;
    public List<ListingModel>? Listings;
    public ListingModel? SelectedListing;
}