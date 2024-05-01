using Microsoft.Extensions.Caching.Memory;

public class CacheBackgroundSerivce : BackgroundService
{
    private readonly ApiService _apiService;
    private readonly TimeSpan _cacheDuration;
    private readonly CacheService _cacheService;

    public CacheBackgroundSerivce(ApiService apiService, CacheService cacheService)
    {
        _apiService = apiService;
        _cacheDuration = TimeSpan.FromMinutes(10);
        _cacheService = cacheService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var listings = await _apiService.GetListings();
            _cacheService.Set(nameof(listings), listings);
            var categories = await _apiService.GetCategories();
            _cacheService.Set(nameof(categories), categories);
            var company = await _apiService.GetCompany();
            _cacheService.Set(nameof(company), company);
            var locations = await _apiService.GetLocations();
            _cacheService.Set(nameof(locations), locations);
            
            await Task.Delay(_cacheDuration, stoppingToken);
        }
    }
}