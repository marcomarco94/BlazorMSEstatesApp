using MSEstatesAppLibrary.Models;

namespace MSEstatesAppLibrary.DataAccess;

public interface IFacebookTemplateData
{
    Task<List<FacebookTemplateModel>> GetAllTemplates();
    Task CreateTemplate(FacebookTemplateModel facebookTemplatet);
    Task UpdateTemplate(FacebookTemplateModel facebookTemplate);
    Task<FacebookTemplateModel> GetTemplateById(string? templateId);
    Task DeleteTemplate(string templateId);
}