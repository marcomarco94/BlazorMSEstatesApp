using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MarketPlaceHelper.Services;
using MSEstatesAppLibrary.DataAccess;
using MSEstatesAppLibrary.Models;

namespace MSEstatesWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FacebookTemplateController : Controller
{
    private readonly IFacebookTemplateData _facebookTemplateData;
    private readonly IListingData _listingData;
    private readonly FacebookTemplateService _facebookTemplateService;

    public FacebookTemplateController(IFacebookTemplateData facebookTemplateData, IListingData listingData, FacebookTemplateService facebookTemplateService)
    {
        _facebookTemplateData = facebookTemplateData;
        _listingData = listingData;
        _facebookTemplateService = facebookTemplateService;
    }
    
    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpGet(Name = "GetFacebookTemplates")]
    public async Task<List<FacebookTemplateModel>> GetFacebookTemplates()
    {
        var templates = await _facebookTemplateData.GetAllTemplates();
        return templates;
    }
    
    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpPost]
    [Route("CreateFacebookTemplate")]
    public async Task CreateTemplate(FacebookTemplateModel facebookTemplate)
    {
         await _facebookTemplateData.CreateTemplate(facebookTemplate);
    }
    
    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpPost]
    [Route("UpdateTemplate")]
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
    
}