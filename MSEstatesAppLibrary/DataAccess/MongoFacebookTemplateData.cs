using MSEstatesAppLibrary.Models;

namespace MSEstatesAppLibrary.DataAccess;

public class MongoFacebookTemplateData : IFacebookTemplateData
{
    private readonly IMongoCollection<FacebookTemplateModel> _facebookTemplates;

    public MongoFacebookTemplateData(IDbConnection db)
    {
        _facebookTemplates = db.FacebookTemplateCollection;
    }

    public async Task<List<FacebookTemplateModel>> GetAllTemplates()
    {
        var results = await _facebookTemplates.FindAsync(_ => true);
        return results.ToList();
    }

    public async Task CreateTemplate(FacebookTemplateModel facebookTemplate)
    {
        facebookTemplate.Id = null;
        await _facebookTemplates.InsertOneAsync(facebookTemplate);
    }

    public async Task UpdateTemplate(FacebookTemplateModel facebookTemplate)
    {
        facebookTemplate.DateCreated = DateTime.Now;
        var filter = Builders<FacebookTemplateModel>.Filter.Eq(template => template.Id, facebookTemplate.Id);
        var existingPost = await _facebookTemplates.Find(filter).FirstOrDefaultAsync();

        if (existingPost != null)
        {
            await _facebookTemplates.ReplaceOneAsync(filter, facebookTemplate);
        }
        else
        {
            facebookTemplate.Id = null;
            await _facebookTemplates.InsertOneAsync(facebookTemplate);
        }
    }

    public async Task<FacebookTemplateModel> GetTemplateById(string? templateId)
    {
        if (string.IsNullOrEmpty(templateId))
        {
            throw new ArgumentNullException(nameof(templateId));
        }
        var filter = Builders<FacebookTemplateModel>.Filter.Eq(f => f.Id, templateId);
        var template = await _facebookTemplates.Find(filter).FirstOrDefaultAsync();
        return template;
    }

    public async Task DeleteTemplate(string templateId)
    {
        if (string.IsNullOrEmpty(templateId))
        {
            throw new ArgumentNullException(nameof(templateId));
        }
        var filter = Builders<FacebookTemplateModel>.Filter.Eq(f => f.Id, templateId);
        await _facebookTemplates.DeleteOneAsync(filter);
    }
}