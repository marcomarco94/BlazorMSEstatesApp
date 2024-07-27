using MSEstatesAppLibrary.Models;

namespace MSEstatesAppLibrary.DataAccess;

public interface ILocationData
{
    Task<List<LocationModel>> GetAllLocations();
    Task CreateLocation(LocationModel location);
    Task UpdateLocation(LocationModel? updatedLocation);
}