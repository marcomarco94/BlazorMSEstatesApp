using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MSEstatesAppLibrary.DataAccess;
using MSEstatesAppLibrary.Models;

namespace MSEstatesWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompanyController : Controller
{
    private readonly ICompanyData _companyData;

    public CompanyController(ICompanyData companyData)
    {
        _companyData = companyData;
    }
    
    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpPost]
    [Route("CreateCompany")]
    public async Task CreateCompany(CompanyModel company)
    {
        if (ModelState.IsValid) await _companyData.CreateCompany(company);
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpPut]
    [Route("UpdateCompany")]
    public async Task<IActionResult> Update([FromBody] CompanyModel? updatetCompany)
    {
        if (updatetCompany == null) return BadRequest();

        await _companyData.UpdateCompany(updatetCompany);
        return NoContent();
    }

    [HttpGet(Name = "GetCompany")]
    [ResponseCache(Duration = 7 * 24 * 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<ActionResult<CompanyModel>> Get()
    {
        var company = await _companyData.GetCompany();
        return company;
    }
}