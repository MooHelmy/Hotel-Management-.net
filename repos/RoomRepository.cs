using HotelManagement.DTOs;
using HotelManagement.Entities;
using Microsoft.EntityFrameworkCore;

public class RoomRepository(IGeneric<Room> roomInterface, ApplicationDbContext Context) : IRoomService
{
    public async Task<ServicesResponse> CreateAsync(RoomDTO dto)
    {
        var room = dto.RoomToEntityMapper();
        var result = await roomInterface.CreateAsync(room);
        return result > 0 ? new ServicesResponse(true, "The Room Created Successfully")
           : new ServicesResponse(false, "The Room Creation Failed");

    }

    public async Task<ServicesResponse> DeleteAsync(int id)
    {
        var result = await roomInterface.DeleteAsync(id);
        return result > 0 ? new ServicesResponse(true, "The Room Deleted Successfully")
           : new ServicesResponse(false, "The Room Deletion Failed");
    }

    public async Task<IEnumerable<RoomDTO>> GetAllAsync()
    {
        var rooms = await roomInterface.GetAllAsync(r => r.Hotel, r => r.Amenities);
        if (!rooms.Any())
        {
            return [];
        }
        return rooms.Select(room => room.RoomToDtoMapper());
    }

    public async Task<IEnumerable<RoomDTO>> GetByHotelAsync(int hotelId)
    {
        return await Context.Rooms
                 .Include(r => r.Hotel)
                 .Include(r => r.Amenities)
                 .Where(r => r.HotelId == hotelId)
                 .Select(r => ExtensionMethod.MapRoomToDto(r))
                 .ToListAsync();
    }

    public async Task<RoomDTO?> GetByIdAsync(int id)
    {
        var room = await roomInterface.GetByIdAsync(id, r => r.Hotel, r => r.Amenities);
        if (room == null)
        {
            return null;
        }
        return room.RoomToDtoMapper();
    }

    public async Task<IEnumerable<RoomDTO>> SearchAvailableAsync(RoomAvailabilityRequestDTO request)
    {
        var query = Context.Rooms
                 .Include(r => r.Hotel)
                 .Include(r => r.Amenities)
                 .Where(r => r.HotelId == request.HotelId && r.Status != RoomStatus.OutOfService);
        if (!string.IsNullOrWhiteSpace(request.RoomType) &&
          Enum.TryParse<RoomType>(request.RoomType, true, out var roomType))
        {
            query = query.Where(r => r.RoomType == roomType);
        }

        if (request.NumberOfGuests.HasValue)
        {
            query = query.Where(r => r.Capacity == null || r.Capacity >= request.NumberOfGuests);
        }
        query = query.Where(r => !r.Reservations.Any(res =>
            res.Status != ReservationStatus.Cancelled &&
            res.Status != ReservationStatus.NoShow &&
            res.CheckInDate < request.CheckOutDate &&
            res.CheckOutDate > request.CheckInDate));

        return await query.Select(r => ExtensionMethod.MapRoomToDto(r)).ToListAsync();

    }

    public async Task<ServicesResponse?> UpdateAsync(int id, RoomDTO dto)
    {
        var existingRoom = await roomInterface.GetByIdAsync(id);
        if (existingRoom == null)
        {
            return new ServicesResponse(false, "Room not Found");
        }
        var result = await roomInterface.UpdateAsync(dto.RoomToEntityMapper());
        return result > 0 ? new ServicesResponse(true, "Room updated successfully ")
        : new ServicesResponse(true, "Room not updated  ");
    }

    public async Task<ServicesResponse> UpdateStatusAsync(int id, RoomStatus status)
    {
        var room = await Context.Rooms.FindAsync(id);
        if (room is null)
        {
            return new ServicesResponse(false, $"id {id} not found");
        }
        room.Status = status;
        var result = await Context.SaveChangesAsync();
        return result > 0 ? new ServicesResponse(true, "Room Status updated successfully ")
               : new ServicesResponse(true, "Room Status not updated  ");



    }
}