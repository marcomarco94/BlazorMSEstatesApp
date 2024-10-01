using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MSEstatesAppLibrary.DataAccess;
using MSEstatesAppLibrary.Models;

namespace MSEstatesWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationsController : ControllerBase
{
    private readonly ILocationData _locationData;
    private readonly ILogger<LocationsController> _logger;

    public LocationsController(ILocationData locationData,
        ILogger<LocationsController> logger)
    {
        _locationData = locationData;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpGet]
    [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<ActionResult<List<LocationModel>>> Get()
    {
        try
        {
            var locations = await _locationData.GetAllLocations();
            if (locations == null)
            {
                _logger.LogWarning("Locations not found");
                return NotFound();
            }

            return Ok(locations);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving agent");
            return StatusCode(500, "Internal server error");
        }
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] LocationModel location)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for LocationModel");
            BadRequest(ModelState);
        }

        try
        {
            await _locationData.CreateLocation(location);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating location");
            return StatusCode(500, "Internal server error");
        }
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(string id, [FromBody] LocationModel? updatedLocation)
    {
        if (!ModelState.IsValid || string.IsNullOrEmpty(id))
        {
            _logger.LogWarning("Invalid input for LocationModel");
            return BadRequest("Invalid input");
        }

        try
        {
            await _locationData.UpdateLocation(updatedLocation);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating location");
            return StatusCode(500, "Internal server error");
        }
    }
}