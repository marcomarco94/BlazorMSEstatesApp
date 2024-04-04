using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MSEstatesAppLibrary.DataAccess;
using MSEstatesAppLibrary.Models;

namespace MSEstatesWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdvertisementTypeController : Controller
{
    private readonly IAdvertisementTypeData _advertisementTypeData;

    public AdvertisementTypeController(IAdvertisementTypeData advertisementTypeData)
    {
        _advertisementTypeData = advertisementTypeData;
    }

    [Authorize(Roles = "Task.Write")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    [HttpPost]
    [Route("CreateAdvertisementType")]
    public async Task AddAdvertisementType(AdvertisementTypeModel advertisementType)
    {
        if (ModelState.IsValid) await _advertisementTypeData.CreateAdvertisementType(advertisementType);
    }

    [HttpGet(Name = "GetAdvertisementTypes")]
    [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<List<AdvertisementTypeModel>> Get()
    {
        var advertisementTypes = await _advertisementTypeData.GetAllAdvertisementTypes();
        return advertisementTypes;
    }
}