using MSEstatesAppLibrary.Models;

namespace MarketPlaceHelper.Services;

public class FacebookTemplateService
{
    public string FillTemplateWithListingData(string? template, ListingModel? listing)
    {
        var filledTemplate = template
            .Replace("{ListingName}", listing?.ListingName)
            .Replace("{Location}", listing?.Location?.Location)
            .Replace("{Price}", listing?.Price.ToString())
            .Replace("{Size}", listing?.Size.ToString())
            .Replace("{Bedrooms}", listing?.Bedrooms.ToString())
            .Replace("{Bathrooms}", listing?.Bathrooms.ToString())
            .Replace("{Features}", GetFeatureString(listing))
            .Replace("{Description}", listing?.Description)
            .Replace("{Token}", listing?.Token);
        return filledTemplate;
    }
    
    private string GetFeatureString(ListingModel? listing)
    {
        string featureString = "";
        listing?.Features?.ForEach(feature => featureString += $"🔹 {feature}\n");
        return featureString;
    }
}