using System.Collections.Generic;
using MSEstatesAppLibrary.Models;

namespace BlazorMSEstatesUIAdmin.Services;

public class ListingStateService
{
    public ListingModel? SelectedListing;
    public List<ListingModel>? Listings;
}