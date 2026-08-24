using HotelManagement.Entities;
using Microsoft.EntityFrameworkCore;



public class AmenityRepository(ApplicationDbContext Context)
    : GenericRepo<Amenity>(Context), IAmenityService
{
    public async Task<List<Amenity>> FindOrCreateAsync(IEnumerable<string> names)
    {
        var distinctNames = names.Distinct(StringComparer.OrdinalIgnoreCase).ToList();

        var existing = await Context.Amenities
            .Where(a => distinctNames.Contains(a.Name))
            .ToListAsync();

        var existingNames = existing.Select(a => a.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);

        var newOnes = distinctNames
            .Where(name => !existingNames.Contains(name))
            .Select(name => new Amenity { Name = name });

        return existing.Concat(newOnes).ToList();
    }
}