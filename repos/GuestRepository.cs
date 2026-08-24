using HotelManagement.DTOs;
using Microsoft.EntityFrameworkCore;

public class GuestRepository(IGeneric<Guest> GuestInterface, ApplicationDbContext Context) : IGuestService
{
    public async Task<ServicesResponse> CreateAsync(GuestDTO dto)
    {
        var Guest = dto.GuestToEntityMapper();
        var result = await GuestInterface.CreateAsync(Guest);
        return result > 0 ? new ServicesResponse(true, "The Guest Created Successfully")
           : new ServicesResponse(false, "The Guest Creation Failed");
    }

    public async Task<ServicesResponse> DeleteAsync(int id)
    {
        var result = await GuestInterface.DeleteAsync(id);
        return result > 0 ? new ServicesResponse(true, "The Guest Deleted Successfully")
           : new ServicesResponse(false, "The Guest Deletion Failed");
    }

    public async Task<IEnumerable<GuestDTO>> GetAllAsync()
    {
        var Guests = await GuestInterface.GetAllAsync(g => g.Reservations);
        if (!Guests.Any())
        {
            return [];
        }
        return Guests.Select(guest => guest.GuestToDtoMapper());
    }

    public async Task<GuestDTO?> GetByEmailAsync(string email)
    {

        var guest = await Context.Guests.FirstOrDefaultAsync(g => g.Email == email);
        if (guest == null)
        {
            throw new ItemNotFoundException($"Guest with email {email} not found");
        }
        return guest.GuestToDtoMapper();
    }

    public async Task<GuestDTO?> GetByIdAsync(int id)
    {
        var guest = await GuestInterface.GetByIdAsync(id, g => g.Reservations);
        if (guest == null)
        {
            throw new ItemNotFoundException($"Guest with id {id} not found");
        }
        return guest.GuestToDtoMapper();
    }

    public async Task<ServicesResponse?> UpdateAsync(int id, GuestDTO dto)
    {
        var guest = await Context.Guests.FirstOrDefaultAsync(g => g.Id == id);
        if (guest == null)
        {
            throw new ItemNotFoundException($"Guest with id {id} not found");
        }
        var updatedGuest = dto.ApplyUpdateTo(guest);
        var ret = await GuestInterface.UpdateAsync(updatedGuest);
        return ret > 0 ? new ServicesResponse(true, "Guest updated successfully ")
        : new ServicesResponse(true, "Guest not updated  ");
    }
}