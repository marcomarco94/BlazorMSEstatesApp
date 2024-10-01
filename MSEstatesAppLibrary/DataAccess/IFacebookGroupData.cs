using MSEstatesAppLibrary.Models;

namespace MSEstatesAppLibrary.DataAccess;

public interface IFacebookGroupData
{
    Task<List<FacebookGroupModel>> GetAllGroups();
    Task CreateGroup(FacebookGroupModel facebookGroup);
    Task UpdateGroup(FacebookGroupModel facebookGroup);
    Task<FacebookGroupModel> GetTemplateById(string? groupId);
    Task DeleteGroup(string groupId);
}