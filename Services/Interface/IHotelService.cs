using HotelManagement.DTOs;

public interface IHotelService
{
    Task<IEnumerable<HotelDTO>> GetAllAsync();
    Task<HotelDTO?> GetByIdAsync(int id);
    Task<HotelDTO> CreateAsync(HotelDTO dto);
    Task<HotelDTO?> UpdateAsync(int id, HotelDTO dto);
    Task<bool> DeleteAsync(int id);
}
