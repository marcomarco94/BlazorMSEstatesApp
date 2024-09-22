using BlazorMSEstatesUIAdmin.Models;

namespace BlazorMSEstatesUIAdmin.Services;

public class ListingStateService
{
    public ListingModel? SelectedListing;
    public List<ListingModel>? Listings;
    public List<ListingModel>? FilteredItems;
}