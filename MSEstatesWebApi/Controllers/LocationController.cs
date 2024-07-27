using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MSEstatesAppLibrary.DataAccess;
using MSEstatesAppLibrary.Models;

namespace MSEstatesWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationController : Controller
{
    private readonly ILocationData _locationData;

    public LocationController(ILocationData locationData)
    {
        _locationData = locationData;
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpPost]
    [Route("CreateLocation")]
    public async Task AddLocation(LocationModel location)
    {
        if (ModelState.IsValid) await _locationData.CreateLocation(location);
    }

    [HttpGet(Name = "GetLocations")]
    [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<List<LocationModel>> Get()
    {
        var locations = await _locationData.GetAllLocations();
        return locations;
    }
    
    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpPut]
    [Route("UpdateLocation")]
    public async Task<IActionResult> Update([FromBody] LocationModel? updatedLocation)
    {
        if (updatedLocation == null) return BadRequest();

        await _locationData.UpdateLocation(updatedLocation);
        return NoContent();
    }
}