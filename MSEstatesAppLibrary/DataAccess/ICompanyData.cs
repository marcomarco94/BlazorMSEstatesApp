using MSEstatesAppLibrary.Models;

namespace MSEstatesAppLibrary.DataAccess;

public interface ICompanyData
{
    Task<CompanyModel?> GetCompany();
    Task UpdateCompany(string id, CompanyModel? updatedCompany);
    Task CreateCompany(CompanyModel company);
}