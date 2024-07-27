
using BlazorMSEstatesUI.Components.Pages;
using Microsoft.Extensions.Caching.Memory;
namespace BlazorMSEstatesUI.Components.Services;

public class ApiService
{
    private IConfiguration _config;
    private readonly HttpClient _http;
    private readonly string? _basePath;
    private readonly CacheService _cacheService;

    public ApiService(IConfiguration config, HttpClient http, CacheService cacheService)
    {
        _config = config;
        _basePath = _config.GetValue<string>("ImgBasePath");
        _http = http;
        _cacheService = cacheService;
       
    }

    public async Task<List<ListingModel>> GetListings()
    {
        List<ListingModel> listings;
         listings = _cacheService.Get<List<ListingModel>>(nameof(listings));
        if (listings == null)
        {
            listings = await _http.GetFromJsonAsync<List<ListingModel>>("api/Listing");
            foreach (var listing in listings)
            {
                listing.ImageUrls = listing.ImageUrls.Select(url => Path.Combine(_basePath, url)).ToList();
                listing.ImageUrlsOrg = listing.ImageUrls.Select(url =>
                {
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(url);
                    var extension = Path.GetExtension(url);
                    var newFileName = $"{fileNameWithoutExtension}_org{extension}";
                    return Path.Combine(_basePath, newFileName);
                }).ToList();

            }
            _cacheService.Set(nameof(listings), listings);
        }

        return listings;
    }
    
    

    public async Task<List<CategoryModel>> GetCategories()
    {
        List <CategoryModel> categories;
        categories = _cacheService.Get<List<CategoryModel>>(nameof(categories));
            if (categories == null)
            {
                categories = await _http.GetFromJsonAsync<List<CategoryModel>>("api/Category");
                _cacheService.Set(nameof(categories), categories);
            }
        return categories;
    }
    
    public async Task<CompanyModel> GetCompany()
    {
        CompanyModel company;
        company = _cacheService.Get<CompanyModel>(nameof(company));
        if (company == null)
        {
            company = await _http.GetFromJsonAsync<CompanyModel>("api/Company");
            _cacheService.Set(nameof(company), company);
        }
        return company;
    }
    
    public async Task<List<LocationModel>> GetLocations()
    {
        List<LocationModel> locations;
        locations = _cacheService.Get<List<LocationModel>>(nameof(locations));
        if (locations == null)
        {
                locations = await _http.GetFromJsonAsync<List<LocationModel>>("api/Location");
                _cacheService.Set(nameof(locations), locations);
        }
        return locations;
    }


    public async Task<ListingModel> GetListingById(string id)
    {
        List<ListingModel> listings;
        ListingModel listing = null;

        listings = _cacheService.Get<List<ListingModel>>(nameof(listings));
        if (listings.Any())
        {
            listing = listings.FirstOrDefault(l => l.Id == id);
        }

        if (listing == null)
        {
            listing = await _http.GetFromJsonAsync<ListingModel>($"api/Listing/{id}");
        }
        return listing;
    }
}