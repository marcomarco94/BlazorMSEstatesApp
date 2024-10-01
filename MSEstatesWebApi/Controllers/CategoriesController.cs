using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MSEstatesAppLibrary.DataAccess;
using MSEstatesAppLibrary.Models;

namespace MSEstatesWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryData _categoryData;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(ICategoryData categoryData,
        ILogger<CategoriesController> logger)
    {
        _categoryData = categoryData;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<ActionResult<List<CategoryModel>>> Get()
    {
        try
        {
            var categories = await _categoryData.GetAllCategories();
            if (categories == null)
            {
                _logger.LogWarning("No categories found");
                return NotFound();
            }

            return Ok(categories);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error retrieving categories");
            return StatusCode(500, "Internal server error");
        }
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CategoryModel category)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for CategoryModel");
            return BadRequest(ModelState);
        }

        try
        {
            await _categoryData.CreateCategory(category);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating Category");
            return StatusCode(500, "Internal server error");
        }
    }
}