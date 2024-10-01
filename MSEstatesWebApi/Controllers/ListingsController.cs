using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MSEstatesAppLibrary.DataAccess;
using MSEstatesAppLibrary.Models;
using MSEstatesAppLibrary.Services;

namespace MSEstatesWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ListingsController : ControllerBase
{
    private readonly IListingData _listingData;
    private readonly ListingService _listingService;
    private readonly ILogger<ListingsController> _logger;

    public ListingsController(IListingData listingData,
        ListingService listingService, ILogger<ListingsController> logger)
    {
        _listingData = listingData;
        _listingService = listingService;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpGet]
    [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<ActionResult<List<ListingModel>>> Get()
    {
        try
        {
            var listings = await _listingData.GetAllListings();
            if (listings == null)
            {
                _logger.LogWarning("Locations not found");
                return NotFound();
            }

            return Ok(listings.OrderBy(l => l.DateCreated).ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving locations");
            return StatusCode(500, "Internal server error");
        }
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpGet]
    [Route("no-cache")]
    public async Task<ActionResult<List<ListingModel>>> GetNoCache()
    {
        try
        {
            var listings = await _listingData.NoCache();
            if (listings == null)
            {
                _logger.LogWarning("No listings found");
                return NotFound();
            }
            return Ok(listings.OrderByDescending(l => l.Id).ToList());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error retrieving listings");
            return StatusCode(500, "Internal server error");
        }
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ListingModel>> Get(string id)
    {
        try
        {
            var listing = await _listingData.GetListingById(id);
            if (listing == null)
            {
                _logger.LogWarning("Listing not found");
                return NotFound();
            }

            return Ok(listing);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error retrieving listing");
            return StatusCode(500, "Internal server error");
        }
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpPost]
    public async Task<ActionResult<ListingModel>> Post([FromBody] ListingModel listing)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for ListingModel");
            return BadRequest(ModelState);
        }

        try
        {
            await _listingData.CreateListing(listing);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating Listing");
            return StatusCode(500, "Internal server error");
        }
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(string id, [FromBody] ListingModel updatedListing)
    {
        if (!ModelState.IsValid || string.IsNullOrEmpty(id))
        {
            _logger.LogWarning("Invalid input for ListingModel");
            return BadRequest("Invalid input");
        }

        try
        {
            var oldListing = await _listingData.GetListingById(updatedListing.Id);
            if (oldListing == null)
            {
                _logger.LogWarning("Listing not found");
                return NotFound();
            }

            await _listingService.UpdateImagesOnCloud(oldListing.ImageUrls, updatedListing.ImageUrls);
            await _listingData.UpdateListing(updatedListing);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error updating listing");
            return StatusCode(500, "Internal server error");
        }
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpGet("count")]
    public async Task<ActionResult<long>> GetCount()
    {
        try
        {
            var count = await _listingData.GetTotalListingsCount();
            return Ok(count);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error retrieving listing count");
            return StatusCode(500, "Internal server error");
        }
    }

    [Authorize] 
    [RequiredScope("Files.ReadWrite")]
    [HttpPatch]
    [Route("{id}/images")]
    public async Task<ActionResult> PatchImages(string id, IFormFileCollection files)
    {
        if (files.Count == 0 || string.IsNullOrEmpty(id)) return BadRequest(new { message = "No files uploaded" });

        try
        {
            var listing = await _listingData.GetListingById(id);
            if (listing == null)
            {
                _logger.LogWarning("Listing not found");
                return NotFound();
            }

            var imageUrls = await _listingService.ProcessAndUploadImages(files, listing);
            listing.ImageUrls = imageUrls;
            await _listingData.UpdateListing(listing);
            return Ok(new { message = "Upload successful" });
        }
        catch (InvalidOperationException e)
        {
            _logger.LogWarning(e, "Invalid file upload");
            return BadRequest(new { message = e.Message });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error uploading images");
            return StatusCode(500, "Internal server error");
        }
    }
}