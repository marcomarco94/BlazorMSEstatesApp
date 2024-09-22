using System.Dynamic;
using Facebook;
using Microsoft.Extensions.Configuration;
using MSEstatesAppLibrary.Models;

namespace MSEstatesAppLibrary.Services;

public class FacebookPostingService
{

    private readonly FacebookClient _facebookClient;
    private readonly string? _pageId;
    private readonly ListingService _listingService;
        
    public FacebookPostingService(IConfiguration config, ListingService listingService)
    {
        _facebookClient = new FacebookClient();
        _pageId = config.GetSection("Facebook")["PageId"];
        _facebookClient.AccessToken = config.GetSection("Facebook")["AccessToken"];
        _listingService = listingService;
           
    }

    public async Task CreateFacebookPost(FacebookPostModel postModel)
    {
        dynamic parameters = new ExpandoObject();
        
        List<dynamic> attachedMedia = new List<dynamic>();
        var imageUrls = await _listingService.GetFullUrlsByListingId(postModel.ListingId);
        foreach (var imageUrl in imageUrls)
        {
            dynamic photoParameters = new ExpandoObject();
            photoParameters.url = imageUrl;
            photoParameters.published = false; 
            photoParameters.temporary = true;
            dynamic  photoResult = await _facebookClient.PostTaskAsync($"/{_pageId}/photos", photoParameters);
            dynamic mediaObject = new ExpandoObject();
            mediaObject.media_fbid = photoResult.id;
            attachedMedia.Add(mediaObject);
        }

        parameters.attached_media = attachedMedia.ToArray();
        parameters.message = postModel.Template?.Template;
        parameters.published = true;

     
        var postId = await _facebookClient.PostTaskAsync($"/{_pageId}/feed", parameters);
    }
}