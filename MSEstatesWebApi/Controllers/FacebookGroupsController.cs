using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MSEstatesAppLibrary.DataAccess;
using MSEstatesAppLibrary.Models;

namespace MSEstatesWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[RequiredScope("Files.ReadWrite")]
public class FacebookGroupsController : ControllerBase
{
    private readonly IFacebookGroupData _facebookGroupData;
    private readonly ILogger<FacebookGroupsController> _logger;

    public FacebookGroupsController(IFacebookGroupData facebookGroupData, ILogger<FacebookGroupsController> logger)
    {
        _facebookGroupData = facebookGroupData;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<FacebookGroupModel>>> Get()
    {
        try
        {
            var groups = await _facebookGroupData.GetAllGroups();
            return Ok(groups);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving Facebook groups");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] FacebookGroupModel facebookGroup)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for FacebookGroupModel");
            return BadRequest("Invalid input");
        }

        try
        {
            await _facebookGroupData.CreateGroup(facebookGroup);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating Facebook group");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> Patch(string id, [FromBody] FacebookGroupModel? updatedGroup)
    {
        if (updatedGroup == null || string.IsNullOrEmpty(id))
        {
            _logger.LogWarning("Invalid input for FacebookGroupModel");
            return BadRequest("Invalid input");
        }

        try
        {
            await _facebookGroupData.UpdateGroup(updatedGroup);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating Facebook group");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            _logger.LogWarning("Invalid input for FacebookGroupModel");
            return BadRequest("Invalid input");
        }

        try
        {
            await _facebookGroupData.DeleteGroup(id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting Facebook group");
            return StatusCode(500, "Internal server error");
        }
    }
}