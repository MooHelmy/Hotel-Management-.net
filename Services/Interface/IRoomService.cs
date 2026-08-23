using HotelManagement.DTOs;
using HotelManagement.Entities;

public interface IRoomService
{
    Task<IEnumerable<RoomDTO>> GetAllAsync();
    Task<RoomDTO?> GetByIdAsync(int id);
    Task<IEnumerable<RoomDTO>> GetByHotelAsync(int hotelId);
    Task<IEnumerable<RoomDTO>> SearchAvailableAsync(RoomAvailabilityRequestDTO request);
    Task<ServicesResponse> CreateAsync(RoomDTO dto);
    Task<ServicesResponse?> UpdateAsync(int id, RoomDTO dto);
    Task<ServicesResponse> UpdateStatusAsync(int id, RoomStatus status);
    Task<ServicesResponse> DeleteAsync(int id);
}
