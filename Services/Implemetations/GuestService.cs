using HotelManagement.DTOs;

public class GuestService(IGuestService guestInterface)
{
    public async Task CreateAsync(GuestDTO dto)
    {
        await guestInterface.CreateAsync(dto);
    }

    public async Task DeleteAsync(int id)
    {
        await guestInterface.DeleteAsync(id);
    }

    public async Task<IEnumerable<GuestDTO>> GetAllAsync()
    {
        return await guestInterface.GetAllAsync();
    }

    public async Task<GuestDTO?> GetByEmailAsync(string email)
    {
        return await guestInterface.GetByEmailAsync(email);
    }

    public async Task<GuestDTO?> GetByIdAsync(int id)
    {
        return await guestInterface.GetByIdAsync(id);
    }

    public async Task UpdateAsync(int id, GuestDTO dto)
    {
        await guestInterface.UpdateAsync(id, dto);
    }
}