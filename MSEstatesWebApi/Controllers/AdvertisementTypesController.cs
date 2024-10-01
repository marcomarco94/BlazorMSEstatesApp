using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MSEstatesAppLibrary.DataAccess;
using MSEstatesAppLibrary.Models;

namespace MSEstatesWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdvertisementTypesController : ControllerBase
{
    private readonly IAdvertisementTypeData _advertisementTypeData;
    private readonly ILogger<AdvertisementTypesController> _logger;

    public AdvertisementTypesController(IAdvertisementTypeData advertisementTypeData,
        ILogger<AdvertisementTypesController> logger)
    {
        _advertisementTypeData = advertisementTypeData;
        _logger = logger;
    }

    [HttpGet(Name = "GetAdvertisementTypes")]
    [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<ActionResult<List<AdvertisementTypeModel>>> Get()
    {
        try
        {
            var advertisementTypes = await _advertisementTypeData.GetAllAdvertisementTypes();
            if (advertisementTypes == null)
            {
                _logger.LogWarning("No advertisement types found");
                return NotFound();
            }

            return Ok(advertisementTypes);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error retrieving advertisement types");
            return StatusCode(500, "Internal server error");
        }
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] AdvertisementTypeModel advertisementType)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for AdvertisementTypeModel");
            return BadRequest(ModelState);
        }

        try
        {
            await _advertisementTypeData.CreateAdvertisementType(advertisementType);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error creating advertisement type");
            return StatusCode(500, "Internal server error");
        }
    }
}