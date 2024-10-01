using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MSEstatesAppLibrary.DataAccess;
using MSEstatesAppLibrary.Models;

namespace MSEstatesWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompanyController : ControllerBase
{
    private readonly ICompanyData _companyData;
    private readonly ILogger<CompanyController> _logger;

    public CompanyController(ICompanyData companyData, ILogger<CompanyController> logger)
    {
        _companyData = companyData;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    [ResponseCache(Duration = 7 * 24 * 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<ActionResult<CompanyModel>> Get()
    {
        try
        {
            var company = await _companyData.GetCompany();
            if (company == null)
            {
                _logger.LogWarning("No company found");
                return NotFound();
            }

            return Ok(company);
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
    public async Task<ActionResult> Post([FromBody] CompanyModel company)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for CompanyModel");
            return BadRequest(ModelState);
        }

        try
        {
            await _companyData.CreateCompany(company);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating Company");
            return StatusCode(500, "Internal server error");
        }
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(string id, [FromBody] CompanyModel? updatedCompany)
    {
        if (!ModelState.IsValid || string.IsNullOrEmpty(id))
        {
            _logger.LogWarning("Invalid input for CompanyModel");
            return BadRequest("Invalid input");
        }

        try
        {
            await _companyData.UpdateCompany(id, updatedCompany);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating Company");
            return StatusCode(500, "Internal server error");
        }
    }
}