using HotelManagement.DTOs;

public interface IGuestService
{
    Task<IEnumerable<GuestDTO>> GetAllAsync();
    Task<GuestDTO?> GetByIdAsync(int id);
    Task<GuestDTO?> GetByEmailAsync(string email);
    Task<GuestDTO> CreateAsync(GuestDTO dto);
    Task<GuestDTO?> UpdateAsync(int id, GuestDTO dto);
    Task<bool> DeleteAsync(int id);
}