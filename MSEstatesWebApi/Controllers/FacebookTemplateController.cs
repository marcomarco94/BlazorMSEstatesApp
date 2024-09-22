using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MSEstatesAppLibrary.DataAccess;
using MSEstatesAppLibrary.Models;
using MSEstatesAppLibrary.Services;

namespace MSEstatesWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FacebookTemplateController : Controller
{
    private readonly IFacebookTemplateData _facebookTemplateData;
    private readonly IListingData _listingData;
    private readonly FacebookTemplateService _facebookTemplateService;
    private readonly IFacebookPostData _facebookPostData;

    public FacebookTemplateController(IFacebookTemplateData facebookTemplateData, IFacebookPostData facebookPostData,
        IListingData listingData, FacebookTemplateService facebookTemplateService)
    {
        _facebookTemplateData = facebookTemplateData;
        _listingData = listingData;
        _facebookTemplateService = facebookTemplateService;
        _facebookPostData = facebookPostData;
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpGet("GetFacebookTemplates")]
    public async Task<List<FacebookTemplateModel>> GetFacebookTemplates()
    {
        var templates = await _facebookTemplateData.GetAllTemplates();
        return templates;
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
			  
    [HttpPost("CreateFacebookTemplate")]
    public async Task CreateTemplate(FacebookTemplateModel facebookTemplate)
    {
        await _facebookTemplateData.CreateTemplate(facebookTemplate);
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpPost("UpdateTemplate")]
    public async Task<IActionResult> UpdateTemplate([FromBody] FacebookTemplateModel? updatetTemplate)
    {
        if (updatetTemplate == null) return BadRequest();
        await _facebookTemplateData.UpdateTemplate(updatetTemplate);
        return Ok();
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpDelete("DeleteTemplate/{templateId}")]
    public async Task<IActionResult> DeleteTemplate(string templateId)
    {
        if (string.IsNullOrEmpty(templateId)) return BadRequest();
        await _facebookTemplateData.DeleteTemplate(templateId);
        return Ok();
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpGet("GetFilledTemplate/{templateId}/{listingId}")]
    public async Task<ActionResult<FacebookTemplateModel>> GetFilledTemplate(string? templateId, string? listingId)
    {
        var template = await _facebookTemplateData.GetTemplateById(templateId);
        var listing = await _listingData.GetListingById(listingId);

        var filledTemplate = _facebookTemplateService.FillTemplateWithListingData(template.Template, listing);
        template.Template = filledTemplate;
        return template;
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpGet("GetPostedTemplates")]
    public async Task<List<FacebookTemplateModel>> GetFacebookPosts()
    {
        var posts = await _facebookPostData.GetAllFacebookPosts();
        return posts.ToList();
    }
    
    [Authorize]
    [RequiredScope("Files.ReadWrite")]	  
    [HttpPost("UpdatePostedTemplate")]
    public async Task<IActionResult> UpdatePostedTemplate([FromBody] FacebookTemplateModel? updatetTemplate)
    {
        if (updatetTemplate == null) return BadRequest();
        await _facebookPostData.CreateFacebookPost(updatetTemplate);
        return Ok();
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpGet("GetFacebookPostsByListingId/{id}")]
    public async Task<List<FacebookTemplateModel>> GetFacebookPostsByListingId(string id)
    {
        var posts = await _facebookPostData.GetFacebookPostsByListingId(id);
        return posts.ToList();
    }
	
}