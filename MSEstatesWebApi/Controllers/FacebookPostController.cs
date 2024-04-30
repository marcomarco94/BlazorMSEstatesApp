using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MSEstatesAppLibrary.DataAccess;
using MSEstatesAppLibrary.Models;

namespace MSEstatesWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FacebookPostController : Controller
{
    private readonly IFacebookData _facebookData;

    public FacebookPostController(IFacebookData facebookData)
    {
        _facebookData = facebookData;
    }
    
    [Authorize(Roles = "Task.Write")]
    [HttpGet(Name = "GetFacebookPosts")]
    public async Task<List<FacebookPostModel>> GetFacebookPosts()
    {
        var listings = await _facebookData.GetAllFacebookPosts();
        return listings.ToList();
    }
    
    [Authorize(Roles = "Task.Write")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    [HttpPost]
    [Route("CreateFacebookPost")]
    public async Task CreatePost(FacebookPostModel facebookPost)
    {
         await _facebookData.CreateFacebookPost(facebookPost);
    }
}