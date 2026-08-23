using HotelManagement.DTOs;
using HotelManagement.Entities;

public interface IRoomService
{
    Task<IEnumerable<RoomDTO>> GetAllAsync();
    Task<RoomDTO?> GetByIdAsync(int id);
    Task<IEnumerable<RoomDTO>> GetByHotelAsync(int hotelId);
    Task<IEnumerable<RoomDTO>> SearchAvailableAsync(RoomAvailabilityRequestDTO request);
    Task<RoomDTO> CreateAsync(RoomDTO dto);
    Task<RoomDTO?> UpdateAsync(int id, RoomDTO dto);
    Task<bool> UpdateStatusAsync(int id, RoomStatus status);
    Task<bool> DeleteAsync(int id);
}
