using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MarketPlaceHelper.Services;
using MSEstatesAppLibrary.DataAccess;
using MSEstatesAppLibrary.Models;

namespace MSEstatesWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FacebookGroupController : Controller
{
    private readonly IFacebookGroupData _facebookGroupData;

    public FacebookGroupController(IFacebookGroupData facebookGroupData)
    {
        _facebookGroupData = facebookGroupData;
    }
    
    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpGet(Name = "GetFacebookGroups")]
    public async Task<List<FacebookGroupModel>> GetFacebookGroups()
    {
        var groups = await _facebookGroupData.GetAllGroups();
        return groups;
    }
    
    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpPost]
    [Route("CreateFacebookGroup")]
    public async Task CreateGroup(FacebookGroupModel facebookGroup)
    {
         await _facebookGroupData.CreateGroup(facebookGroup);
    }
    
    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpPost]
    [Route("UpdateGroup")]
    public async Task<IActionResult> UpdateGroup([FromBody] FacebookGroupModel? updatetGroup)
    {
        if (updatetGroup == null) return BadRequest();
        await _facebookGroupData.UpdateGroup(updatetGroup);
        return Ok();
    }
    
    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpDelete("DeleteGroup/{groupId}")]
    public async Task<IActionResult> DeleteGroup(string groupId)
    {
        if (string.IsNullOrEmpty(groupId)) return BadRequest();
        await _facebookGroupData.DeleteGroup(groupId);
        return Ok();
    }
}