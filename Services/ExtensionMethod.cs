using HotelManagement.DTOs;
using HotelManagement.Entities;

public static class ExtensionMethod
{
    public static Hotel HotelToEntityMapper(this HotelDTO dto)
    {
        return new Hotel
        {
            Name = dto.Name,
            Address = dto.Address,
            City = dto.City,
            Country = dto.Country,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            StarRating = dto.StarRating
        };
    }
    public static HotelDTO HotelToDtoMapper(this Hotel hotel)
    {
        return new HotelDTO
        {
            Id = hotel.Id,
            Name = hotel.Name,
            Address = hotel.Address,
            City = hotel.City,
            Country = hotel.Country,
            PhoneNumber = hotel.PhoneNumber,
            Email = hotel.Email,
            StarRating = hotel.StarRating,
            Amenities = hotel.Amenities.Select(a => a.Name).ToList()
        };
    }

}