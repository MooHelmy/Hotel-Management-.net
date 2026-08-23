using HotelManagement.DTOs;

public interface IHotelService
{
    Task<IEnumerable<HotelDTO>> GetAllAsync();
    Task<HotelDTO?> GetByIdAsync(int id);
    Task<ServicesResponse> CreateAsync(HotelDTO dto);
    Task<ServicesResponse?> UpdateAsync(int id, HotelDTO dto);
    Task<ServicesResponse> DeleteAsync(int id);
}
