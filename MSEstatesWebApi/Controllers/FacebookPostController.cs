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
    private readonly IFacebookPostData _facebookPostData;
    private readonly IFacebookGroupData _facebookGroupData;
    private readonly FacebookPostingService _facebookPostingService;
    private readonly MarketPlacePostingService _marketPlacePostingService;


    public FacebookPostController(IFacebookPostData facebookPostData, FacebookPostingService facebookPostingService,
        MarketPlacePostingService marketPlacePostingService, IFacebookGroupData facebookGroupData)
    {
        _facebookPostData = facebookPostData;
        _facebookGroupData = facebookGroupData;
        _facebookPostingService = facebookPostingService;
        _marketPlacePostingService = marketPlacePostingService;
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
    [HttpGet(Name = "GetFacebookPosts")]
    public async Task<List<FacebookPostModel>> GetFacebookPosts()
    {
        var posts = await _facebookPostData.GetAllFacebookPosts();
        return posts.ToList();
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
}