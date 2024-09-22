using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MSEstatesAppLibrary.DataAccess;
using MSEstatesAppLibrary.Models;
using MSEstatesAppLibrary.Services;
using MSEstatesAppLibrary.Services.MarketPlace;

namespace MSEstatesWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FacebookPostController : Controller
{
    private readonly IFacebookGroupData _facebookGroupData;
    private readonly FacebookPostingService _facebookPostingService;
    private readonly MarketPlacePostingService _marketPlacePostingService;
    private readonly IFacebookPostData _facebookPostData;


    public FacebookPostController(FacebookPostingService facebookPostingService,
        MarketPlacePostingService marketPlacePostingService, IFacebookGroupData facebookGroupData, IFacebookPostData facebookPostData)
    {
        _facebookGroupData = facebookGroupData;
        _facebookPostingService = facebookPostingService;
        _marketPlacePostingService = marketPlacePostingService;
        _facebookPostData = facebookPostData;
    }

    [Authorize] 
    [RequiredScope("Files.ReadWrite")]
    [HttpPost]
    [Route("CreateFacebookPost")]
    public async Task CreateFacebookPost(FacebookPostModel facebookPost)
    {
        await _facebookPostingService.CreateFacebookPost(facebookPost);
    }
    
    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpPost]
    [Route("CreateMarketPlacePost")]
    public async Task CreateMarketPlacePost(FacebookPostModel facebookPost)
    { 
        var facebookGroups = await _facebookGroupData.GetAllGroups();
        await _marketPlacePostingService.PublishListing(facebookPost, facebookGroups)!;
    }
    
    [HttpGet("PostAllPreviousListings/{startIndex}")]
    public async Task PostAllPreviousListings(int startIndex)
    {
        var facebookGroups = await _facebookGroupData.GetAllGroups();
        var facebookPosts = await _facebookPostData.GetAllFacebookPosts();
    
        for (int i = startIndex; i < facebookPosts.Count; i++)
        {
            FacebookPostModel post = new FacebookPostModel()
            {
                ListingId = facebookPosts[i].ListingId,
                Template = facebookPosts[i]
            };
            Console.WriteLine($"Posting listing {i + 1} of {facebookPosts.Count}");
            Console.WriteLine($"Posting listingId {post.ListingId}");
            await _marketPlacePostingService.PublishListing(post, facebookGroups)!;
        }
    }
}