using HotelManagement.DTOs;

public interface IGuestService
{
    Task<IEnumerable<GuestDTO>> GetAllAsync();
    Task<GuestDTO?> GetByIdAsync(int id);
    Task<GuestDTO?> GetByEmailAsync(string email);
    Task<ServicesResponse> CreateAsync(GuestDTO dto);
    Task<ServicesResponse?> UpdateAsync(int id, GuestDTO dto);
    Task<ServicesResponse> DeleteAsync(int id);
}