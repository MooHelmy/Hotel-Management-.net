using HotelManagement.DTOs;
using HotelManagement.Entities;
using Microsoft.EntityFrameworkCore;

public class RoomService(IRoomService roomInterface)
{
    public async Task CreateAsync(RoomDTO dto)
    {
        await roomInterface.CreateAsync(dto);


    }

    public async Task DeleteAsync(int id)
    {
        await roomInterface.DeleteAsync(id);
    }

    public async Task<IEnumerable<RoomDTO>> GetAllAsync()
    {
        return await roomInterface.GetAllAsync();

    }

    public async Task<IEnumerable<RoomDTO>> GetByHotelAsync(int hotelId)
    {
        return await roomInterface.GetByHotelAsync(hotelId);
    }

    public async Task<RoomDTO?> GetByIdAsync(int id)
    {
        return await roomInterface.GetByIdAsync(id);
    }

    public async Task<IEnumerable<RoomDTO>> SearchAvailableAsync(RoomAvailabilityRequestDTO request)
    {
        return await roomInterface.SearchAvailableAsync(request);

    }

    public async Task UpdateAsync(int id, RoomDTO dto)
    {
        await roomInterface.UpdateAsync(id, dto);
    }

    public async Task UpdateStatusAsync(int id, RoomStatus status)
    {
        await roomInterface.UpdateStatusAsync(id, status);
    }
}