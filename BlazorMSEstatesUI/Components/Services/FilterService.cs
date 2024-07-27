using BlazorMSEstatesUI.Components.Models;
using Microsoft.AspNetCore.WebUtilities;

namespace BlazorMSEstatesUI.Components.Services;

public class FilterService
{
    private string? _maxPriceInput;
    private string? _minPriceInput;
    private string? _searchInput;
    private string? _selectedAdvertisement;
    private string? _selectedCategory;
    private string? _selectedLocation;
    private string? _sortedBy;


    public string? SelectedAdvertisement
    {
        get => _selectedAdvertisement ?? "All";
        set
        {
            if (_selectedAdvertisement != value)
            {
                _selectedAdvertisement = value;
                NotifySearchFilterChanged();
            }
        }
    }

    public string? SelectedCategory
    {
        get => _selectedCategory ?? "Category";
        set
        {
            if (_selectedCategory != value)
            {
                _selectedCategory = value;
                NotifySearchFilterChanged();
            }
        }
    }

    public string? SelectedLocation
    {
        get => _selectedLocation ?? "Location";
        set
        {
            if (_selectedLocation != value)
            {
                _selectedLocation = value;
                NotifySearchFilterChanged();
            }
        }
    }

    public string? SearchInput
    {
        get => _searchInput ?? "";
        set
        {
            if (_searchInput != value)
            {
                _searchInput = value;
                NotifySearchFilterChanged();
            }
        }
    }

    public string? SortedBy
    {
        get => _sortedBy ?? "Price - Low to High";

        set
        {
            if (_sortedBy != value)
            {
                _sortedBy = value;
                NotifySearchFilterChanged();
            }
        }
    }

    public string? MinPriceInput
    {
        get => _minPriceInput ?? String.Empty;
        set
        {
            if (_minPriceInput != value)
            {
                _minPriceInput = value;
                NotifySearchFilterChanged();
            }
        }
    }

    public string? MaxPriceInput
    {
        get => _maxPriceInput ?? String.Empty;
        set
        {
            if (_maxPriceInput != value)
            {
                _maxPriceInput = value;
                NotifySearchFilterChanged();
            }
        }
    }

    public event Action OnSearchFilterChanged;

    public void GetQueryParameters(Uri uri)
    {
        var query = QueryHelpers.ParseQuery(uri.Query);
        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("advertisement", out var advertisementValue))
        {
            _selectedAdvertisement = advertisementValue.First();
        }

        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("searchInput", out var searchInputValue))
        {
            _searchInput = searchInputValue.First();
        }

        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("minPrice", out var minPriceValue))
        {
            _minPriceInput = minPriceValue.First();
        }

        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("maxPrice", out var maxPriceValue))
        {
            _maxPriceInput = maxPriceValue.First();
        }

        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("category", out var categoryValue))
        {
            _selectedCategory = categoryValue.First();
        }

        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("location", out var locationValue))
        {
            _selectedLocation = locationValue.First();
        }
        
        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("sortedBy", out var sortedByValue))
        {
            _sortedBy = sortedByValue.First();
        }
    }
    
      public async Task<List<ListingModel>> FilterListings(List<ListingModel> listings)
    {
         var filteredListings = new List<ListingModel>(listings);

        if (filteredListings is not null)
        {
            if (SelectedAdvertisement != "All")
            {
                filteredListings = filteredListings.Where(l => l.AdvertisementType?.AdvertisementType == SelectedAdvertisement).ToList();
            }

            if (SelectedCategory != "Category")
            {
                filteredListings = filteredListings.Where(l => l.Category?.Category == SelectedCategory).ToList();
            }

            if (SelectedLocation != "Location")
            {
                filteredListings = filteredListings.Where(l => l.Location?.Location == SelectedLocation).ToList();
            }

            if (string.IsNullOrWhiteSpace(SearchInput) == false)
            {
                filteredListings = filteredListings.Where(l => l.ListingName.Contains(SearchInput, StringComparison.InvariantCultureIgnoreCase) ||
                                                                 l.Location.Location.Contains(SearchInput, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            
            if (string.IsNullOrWhiteSpace(MaxPriceInput) == false)
            {
                bool canParse =  int.TryParse(MaxPriceInput, out var maxPrice);
                if (canParse && maxPrice > 0)
                {
                    filteredListings = filteredListings.Where(l => l.Price <= maxPrice).ToList();
                }
            }
            
            if (string.IsNullOrWhiteSpace(MinPriceInput) == false)
            {
                bool canParse =  int.TryParse(MinPriceInput, out var minPrice);
                if (canParse && minPrice > 0)
                {
                    filteredListings = filteredListings.Where(l => l.Price >= minPrice).ToList();
                }
            }

            if (SortedBy == "Price - Low to High" || SortedBy == null)
            {
                filteredListings = filteredListings.OrderBy(l => l.Price).ToList();
            }
            else if (SortedBy == "Price - High to Low")
            {
                filteredListings = filteredListings.OrderByDescending(l => l.Price).ToList();
            }
        }
        return filteredListings;
    }

    private void NotifySearchFilterChanged()
    {
        OnSearchFilterChanged?.Invoke();
    }
}