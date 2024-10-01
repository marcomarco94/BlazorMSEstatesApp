using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MSEstatesAppLibrary.DataAccess;
using MSEstatesAppLibrary.Models;

namespace MSEstatesWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RealtorContactsController : ControllerBase
{
    private readonly ILogger<RealtorContactsController> _logger;
    private readonly IRealtorContactData _realtorContactData;

    public RealtorContactsController(IRealtorContactData realtorContactData,
        ILogger<RealtorContactsController> logger)
    {
        _realtorContactData = realtorContactData;
        _logger = logger;
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpGet]
    public async Task<ActionResult<List<RealtorContactModel>>> Get()
    {
        try
        {
            var realtorContacts = await _realtorContactData.GetAllRealtorContacts();
            if (realtorContacts == null)
            {
                _logger.LogWarning("Realtor contacts not found");
                return NotFound();
            }

            return Ok(realtorContacts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving realtor contacts");
            return StatusCode(500, "Internal server error");
        }
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpGet("{id}")]
    public async Task<ActionResult<RealtorContactModel>> Get(string id)
    {
        try
        {
            var realtorContact = await _realtorContactData.GetRealtorContactById(id);
            if (realtorContact == null)
            {
                _logger.LogWarning("Realtor contact not found: {Id}", id);
                return NotFound();
            }

            return Ok(realtorContact);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving realtor contact");
            return StatusCode(500, "Internal server error");
        }
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] RealtorContactModel realtorContact)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for RealtorContactModel");
            return BadRequest(ModelState);
        }

        try
        {
            await _realtorContactData.CreateRealtorContact(realtorContact);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating RealtorContact");
            return StatusCode(500, "Internal server error");
        }
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpPatch("{id}/Archive")]
    public async Task<ActionResult> PatchArchived(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            _logger.LogWarning("Invalid input for RealtorContactModel");
            return BadRequest("Invalid input");
        }

        try
        {
            await _realtorContactData.ArchiveRealtorContact(id);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error archiving realtor contact");
            return StatusCode(500, "Internal server error");
        }
    }
}