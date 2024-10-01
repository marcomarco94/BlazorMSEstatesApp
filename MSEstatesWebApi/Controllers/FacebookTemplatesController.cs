using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MSEstatesAppLibrary.DataAccess;
using MSEstatesAppLibrary.Models;
using MSEstatesAppLibrary.Services;

namespace MSEstatesWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[RequiredScope("Files.ReadWrite")]
public class FacebookTemplatesController : ControllerBase
{
    private readonly IFacebookPostData _facebookPostData;
    private readonly IFacebookTemplateData _facebookTemplateData;
    private readonly FacebookTemplateService _facebookTemplateService;
    private readonly IListingData _listingData;

    public FacebookTemplatesController(IFacebookTemplateData facebookTemplateData, IFacebookPostData facebookPostData,
        IListingData listingData, FacebookTemplateService facebookTemplateService)
    {
        _facebookTemplateData = facebookTemplateData;
        _listingData = listingData;
        _facebookTemplateService = facebookTemplateService;
        _facebookPostData = facebookPostData;
    }

    [HttpGet("GetFacebookTemplates")]
    public async Task<List<FacebookTemplateModel>> GetFacebookTemplates()
    {
        var templates = await _facebookTemplateData.GetAllTemplates();
        return templates;
    }

    [HttpPost("CreateFacebookTemplate")]
    public async Task CreateTemplate([FromBody] FacebookTemplateModel facebookTemplate)
    {
        await _facebookTemplateData.CreateTemplate(facebookTemplate);
    }

    [HttpPost("UpdateTemplate")]
    public async Task<ActionResult> UpdateTemplate([FromBody] FacebookTemplateModel? updatetTemplate)
    {
        if (updatetTemplate == null) return BadRequest();
        await _facebookTemplateData.UpdateTemplate(updatetTemplate);
        return Ok();
    }

    [HttpDelete("DeleteTemplate/{templateId}")]
    public async Task<ActionResult> DeleteTemplate(string templateId)
    {
        if (string.IsNullOrEmpty(templateId)) return BadRequest();
        await _facebookTemplateData.DeleteTemplate(templateId);
        return Ok();
    }

    [HttpGet("GetFilledTemplate/{templateId}/{listingId}")]
    public async Task<ActionResult<FacebookTemplateModel>> GetFilledTemplate(string? templateId, string? listingId)
    {
        var template = await _facebookTemplateData.GetTemplateById(templateId);
        var listing = await _listingData.GetListingById(listingId);

        var filledTemplate = _facebookTemplateService.FillTemplateWithListingData(template.Template, listing);
        template.Template = filledTemplate;
        return template;
    }

    [HttpGet("GetPostedTemplates")]
    public async Task<List<FacebookTemplateModel>> GetFacebookPosts()
    {
        var posts = await _facebookPostData.GetAllFacebookPosts();
        return posts.ToList();
    }

    [HttpPost("UpdatePostedTemplate")]
    public async Task<ActionResult> UpdatePostedTemplate([FromBody] FacebookTemplateModel? updatetTemplate)
    {
        if (updatetTemplate == null) return BadRequest();
        await _facebookPostData.CreateFacebookPost(updatetTemplate);
        return Ok();
    }

    [HttpGet("GetFacebookPostsByListingId/{id}")]
    public async Task<List<FacebookTemplateModel>> GetFacebookPostsByListingId(string id)
    {
        var posts = await _facebookPostData.GetFacebookPostsByListingId(id);
        return posts.ToList();
    }
}