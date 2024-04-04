using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MSEstatesAppLibrary.DataAccess;
using MSEstatesAppLibrary.Models;

namespace MSEstatesWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ListingController : Controller
{
    private readonly IListingData _listingData;

    public ListingController(IListingData listingData)
    {
        _listingData = listingData;
    }

    [Authorize(Roles = "Task.Write")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    [HttpPost]
    [Route("CreateListing")]
    public async Task AddListing(ListingModel listing)
    {
        if (ModelState.IsValid) await _listingData.CreateListing(listing);
    }
    
    [HttpGet(Name = "GetListings")]
    [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<List<ListingModel>> GetListings()
    {
        var listings = await _listingData.GetAllListings();
        return listings.OrderByDescending(l => l.DateCreated).ToList();
    }
    
     [Authorize(Roles = "Task.Write")]
     [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
     [HttpGet]
     [Route("NoCache")]
     public async Task<List<ListingModel>> GetListingsNoCache()
     {
         var listings = await _listingData.NoCache();
         return listings;
     }

    [HttpGet("{id}", Name = "GetListingById")]
    [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<ActionResult<ListingModel>> Get(string id)
    {
        var listing = await _listingData.GetListingById(id);
        return listing;
    }
    
    [HttpGet("GetByToken{token}", Name = "GetListingByToken")]
    [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<ActionResult<ListingModel>> GetByToken(string token)
    {
        var listing = await _listingData.GetListingByToken(token);
        return listing;
    }

    [Authorize(Roles = "Task.Write")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    [HttpPut]
    [Route("UpdateListing")]
    public async Task<IActionResult> Update([FromBody] ListingModel? updatedListing)
    {
        if (updatedListing == null) return BadRequest();

        await _listingData.UpdateListing(updatedListing);
        return NoContent();
    }
    
    [HttpGet]
    [Route("GetCount")]
    public async Task <long> GetCount()
    {
        var count = await _listingData.GetTotalListingsCount();
        return count;
    }
}