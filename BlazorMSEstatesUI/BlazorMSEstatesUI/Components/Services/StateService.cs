
namespace BlazorMSEstatesUI.Client.Services;

public class StateService
{
    public List<ListingModel>? Listings { get; set; }
    public List<CategoryModel>? Categories { get; set; }
    public CompanyModel? Company { get; set; }
    public List<LocationModel>? Locations { get; set; }
    public ListingModel? Listing { get; set; }
    
    private readonly Timer _timer;
    
    public StateService( )
    {
        _timer = new Timer(_ => ResetData(), null, 0, 1000*60);
        Listings = new List<ListingModel>();
        Categories = new List<CategoryModel>();
        Locations = new List<LocationModel>();
    }
    
    private void ResetData()
    {
        Listings = null;
        Categories = null;
        Company = null;
        Locations = null;
        Listing = null;
    }
}