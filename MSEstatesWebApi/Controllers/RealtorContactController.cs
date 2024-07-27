using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MSEstatesAppLibrary.DataAccess;
using MSEstatesAppLibrary.Models;

namespace MSEstatesWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RealtorContactController : Controller
{
    private readonly IRealtorContactData _realtorContactData;

    public RealtorContactController(IRealtorContactData realtorContactData)
    {
        _realtorContactData = realtorContactData;
    }
    
    [HttpPost]
    [Route("CreateRealtorContact")]
    public async Task AddLocation(RealtorContactModel realtorContact)
    {
        if (ModelState.IsValid) await _realtorContactData.CreateRealtorContact(realtorContact);
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpGet(Name = "GetRealtorContacts")]
    [ResponseCache(Duration = 5 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<List<RealtorContactModel>> Get()
    {
        var realtorContacts = await _realtorContactData.GetAllRealtorContacts();
        return realtorContacts;
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpGet("{id}", Name = "GetRealtorContactById")]
    [ResponseCache(Duration = 5 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<ActionResult<RealtorContactModel>> Get(string id)
    {
        var realtorContact = await _realtorContactData.GetRealtorContactById(id);
        return realtorContact;
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpPut]
    [Route("ArchiveRealtorContact/{id}")]
    public async Task<IActionResult> Put(string id)
    {
        try
        {
            await _realtorContactData.ArchiveRealtorContact(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}