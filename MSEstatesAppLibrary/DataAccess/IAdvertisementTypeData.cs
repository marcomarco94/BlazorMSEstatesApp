using MSEstatesAppLibrary.Models;

namespace MSEstatesAppLibrary.DataAccess;

public interface IAdvertisementTypeData
{
    Task<List<AdvertisementTypeModel>?> GetAllAdvertisementTypes();
    Task CreateAdvertisementType(AdvertisementTypeModel advertisementType);
}