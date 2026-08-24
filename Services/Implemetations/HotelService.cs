using HotelManagement.DTOs;
using HotelManagement.Entities;
using Microsoft.EntityFrameworkCore;

public class HotelService(IGeneric<Hotel> hotelInterface, ApplicationDbContext Context, IAmenityService amenityRepo) : IHotelService
{
    public async Task<ServicesResponse> CreateAsync(HotelDTO dto)
    {
        var hotel = dto.HotelToEntityMapper();

        if (dto.Amenities is { Count: > 0 })
        {
            hotel.Amenities = await amenityRepo.FindOrCreateAsync(dto.Amenities);
        }

        var result = await hotelInterface.CreateAsync(hotel);
        return result > 0 ? new ServicesResponse(true, "The Hotel Created Successfully")
           : new ServicesResponse(false, "The Hotel Creation Failed");
    }
    public async Task<ServicesResponse> DeleteAsync(int id)
    {
        var result = await hotelInterface.DeleteAsync(id);
        if (result == 0)
        {
            return new ServicesResponse(false, "Hotel not found");
        }
        return result > 0 ? new ServicesResponse(true, "The Hotel Deleted Successfully")
           : new ServicesResponse(false, "The Hotel Deletion Failed");

    }

    public async Task<IEnumerable<HotelDTO>> GetAllAsync()
    {
        var hotels = await hotelInterface.GetAllAsync(h => h.Amenities);
        if (!hotels.Any())
        {
            return [];
        }
        return hotels.Select(hotel => hotel.HotelToDtoMapper());
    }

    public async Task<HotelDTO?> GetByIdAsync(int id)
    {
        var hotel = await hotelInterface.GetByIdAsync(id, h => h.Amenities);

        return hotel!.HotelToDtoMapper();
    }

    public async Task<ServicesResponse?> UpdateAsync(int id, HotelDTO dto)
    {
        var existinghotel = await hotelInterface.GetByIdAsync(id, h => h.Amenities);
        if (existinghotel == null)
        {
            return new ServicesResponse(false, "Hotel not found");
        }

        var updatedHotel = dto.ApplyUpdateTo(existinghotel);

        if (dto.Amenities is { Count: > 0 })
        {
            updatedHotel.Amenities = await amenityRepo.FindOrCreateAsync(dto.Amenities);
        }

        var result = await hotelInterface.UpdateAsync(updatedHotel);
        return result > 0 ? new ServicesResponse(true, "Hotel updated successfully ")
        : new ServicesResponse(false, "Hotel not updated  ");
    }
}