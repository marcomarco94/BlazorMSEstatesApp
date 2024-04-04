namespace MSEstatesAppLibrary.DataAccess;

public interface ICompanyData
{
    Task<CompanyModel> GetCompany();
    Task UpdateCompany(CompanyModel? updatedCompany);
    Task CreateCompany(CompanyModel company);
}