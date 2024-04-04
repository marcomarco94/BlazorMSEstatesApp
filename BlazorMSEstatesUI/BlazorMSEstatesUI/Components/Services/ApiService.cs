
namespace BlazorMSEstatesUI.Client.Services;

public class ApiService
{
    private IConfiguration _config;
    private readonly HttpClient _http;
    private readonly string? _basePath;
    private readonly StateService _stateService;


    public ApiService(IConfiguration config, HttpClient http, StateService stateService)
    {
        _config = config;
        _basePath = _config.GetValue<string>("ImgBasePath");
        _http = http;
        _stateService = stateService;
       
    }
    
    public async Task<List<ListingModel>> GetListings()
    {
         if (_stateService.Listings == null)
         {
            try
            {
                _stateService.Listings = await _http.GetFromJsonAsync<List<ListingModel>>("api/Listing");
                foreach (var listing in _stateService.Listings)
                {
                    listing.ImageUrls = listing.ImageUrls.Select(url => Path.Combine(_basePath, url)).ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
         } 
         return _stateService.Listings;
    }
    
    public async Task<List<CategoryModel>> GetCategories()
    {
        if (_stateService.Categories == null)
        {
            try
            {
                _stateService.Categories = await _http.GetFromJsonAsync<List<CategoryModel>>("api/Category");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        return _stateService.Categories;
    }
    
    public async Task<CompanyModel> GetCompany()
    {
        if (_stateService.Company == null)
        {
            try
            {
                _stateService.Company = await _http.GetFromJsonAsync<CompanyModel>("api/Company");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        return _stateService.Company;
    }
    
    public async Task<List<LocationModel>> GetLocations()
    {
        if (_stateService.Locations == null)
        {
            try
            {
                _stateService.Locations = await _http.GetFromJsonAsync<List<LocationModel>>("api/Location");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        return _stateService.Locations;
    }
    
    public async Task<ListingModel> GetListingById(string id)
    {
        if (_stateService.Listings != null)
        {
            
            var listing = _stateService.Listings.FirstOrDefault(l => l.Id == id);
            if (listing != null)
            {
                _stateService.Listing = listing;
                return _stateService.Listing;
            }
        }
        
        try
        {
            _stateService.Listing = await _http.GetFromJsonAsync<ListingModel>($"api/Listing/{id}");
            _stateService.Listing.ImageUrls = _stateService.Listing.ImageUrls.Select(url => Path.Combine(_basePath, url)).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return _stateService.Listing;
    }
    
}