using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MSEstatesAppLibrary.DataAccess;
using MSEstatesAppLibrary.Models;
using MSEstatesAppLibrary.Services;

namespace MSEstatesWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ListingController : Controller
{
    private readonly IListingData _listingData;
    private readonly ListingService _listingService;

    public ListingController(IListingData listingData, ListingService listingService)
    {
        _listingData = listingData;
        _listingService = listingService;
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpPost]
    [Route("CreateListing")]
    public async Task<ActionResult<ListingModel>> AddListing(ListingModel listing)
    {
        if (ModelState.IsValid)
        {
            listing.ImageUrls?.Clear();
            var newListing = await _listingData.CreateListing(listing);
            return Ok(newListing);
        }
        return BadRequest(ModelState);
    }

    [HttpGet(Name = "GetListings")]
    [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<List<ListingModel>> GetListings()
    {
        var listings = await _listingData.GetAllListings();
        return listings.OrderByDescending(l => l.DateCreated).ToList();
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpGet]
    [Route("NoCache")]
    public async Task<List<ListingModel>> GetListingsNoCache()
    {
        var listings = await _listingData.NoCache();
        return listings;
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpGet("{id}", Name = "GetListingById")]
    public async Task<ActionResult<ListingModel>> Get(string? id)
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

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpPost]
    [Route("UpdateListing")]
    public async Task<IActionResult> Update([FromBody] ListingModel? updatedListing)
    {
        if (updatedListing == null) return BadRequest();
        var oldListing = await _listingData.GetListingById(updatedListing.Id);
        await _listingService.UpdateImagesOnCloud(oldListing.ImageUrls, updatedListing.ImageUrls);
        
        await _listingData.UpdateListing(updatedListing);
        return NoContent();
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpGet ("GetCount")]
    public async Task<long> GetCount()
    {
        var count = await _listingData.GetTotalListingsCount();
        return count;
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpPost]
    [Route("UploadImage")]
    public async Task<IActionResult> UploadImage()
    {
        var formCollection = await Request.ReadFormAsync();
        string? listingId = formCollection["listingId"].FirstOrDefault();
        var files = formCollection.Files;

        if (files.Count == 0 || listingId == null) return BadRequest(new { message = "No files uploaded" });

        var listing = await _listingData.GetListingById(listingId);
        foreach (var file in files)
        {
            if (file.Length <= 0) continue;
            await using var stream = file.OpenReadStream();
            string? imageUrl = await _listingService.ProcessAndUploadImage(stream);
            if (imageUrl != null)
            {
                listing.ImageUrls?.Add(imageUrl);
                await _listingData.UpdateListing(listing);

            }
        }

        return Ok(new { message = "Upload successed" });
    }
}