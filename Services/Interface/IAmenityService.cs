using HotelManagement.Entities;

public interface IAmenityService : IGeneric<Amenity>
{
    public Task<List<Amenity>> FindOrCreateAsync(IEnumerable<string> names);
}