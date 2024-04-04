using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MSEstatesAppLibrary.DataAccess;
using MSEstatesAppLibrary.Models;

namespace MSEstatesWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : Controller
{
    private readonly ICategoryData _categoryData;

    public CategoryController(ICategoryData categoryData)
    {
        _categoryData = categoryData;
    }

    [Authorize(Roles = "Task.Write")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    [HttpPost]
    [Route("CreateCategory")]
    public async Task AddCategory(CategoryModel category)
    {
        if (ModelState.IsValid) await _categoryData.CreateCategory(category);
    }

    [HttpGet(Name = "GetCategories")]
    [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<List<CategoryModel>> Get()
    {
        var categories = await _categoryData.GetAllCategories();
        return categories;
    }
}