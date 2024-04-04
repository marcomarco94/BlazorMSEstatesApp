namespace MSEstatesAppLibrary.DataAccess;

public interface IRealtorContactData
{
    Task<List<RealtorContactModel>> GetAllRealtorContacts();
    Task CreateRealtorContact(RealtorContactModel realtorContact);
    Task ArchiveRealtorContact(string id);
    Task<RealtorContactModel> GetRealtorContactById(string id);
}