using HotelManagement.DTOs;
using HotelManagement.Entities;

public static class ExtensionMethod
{
    public static Hotel HotelToEntityMapper(this HotelDTO dto)
    {
        return new Hotel
        {
            Name = dto.Name ?? string.Empty,
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
    public static Room RoomToEntityMapper(this RoomDTO dto)
    {
        return new Room
        {
            RoomNumber = dto.RoomNumber,
            RoomType = Enum.Parse<RoomType>(dto.RoomType, true),
            Status = Enum.TryParse<RoomStatus>(dto.Status, true, out var status)
             ? status : RoomStatus.Available,
            PricePerNight = dto.PricePerNight,
            Floor = dto.Floor,
            Capacity = dto.Capacity,
            HotelId = dto.HotelId,
            Hotel = new Hotel
            {
                Id = dto.HotelId,
                Name = dto.HotelName
            },
            Amenities = dto.Amenities.Select(a => new Amenity
            {
                Name = a
            }).ToList()
        };
    }
    public static RoomDTO RoomToDtoMapper(this Room room)
    {
        return new RoomDTO
        {
            Id = room.Id,
            RoomNumber = room.RoomNumber,
            RoomType = room.RoomType.ToString(),
            Status = room.Status.ToString(),
            PricePerNight = room.PricePerNight,
            Floor = room.Floor,
            Capacity = room.Capacity,
            HotelId = room.HotelId,
            HotelName = room.Hotel.Name,
            Amenities = room.Amenities.Select(a => a.Name).ToList()
        };
    }
    public static RoomDTO MapRoomToDto(Room r) => new()
    {
        Id = r.Id,
        RoomNumber = r.RoomNumber,
        RoomType = r.RoomType.ToString(),
        Status = r.Status.ToString(),
        PricePerNight = r.PricePerNight,
        Floor = r.Floor,
        Capacity = r.Capacity,
        HotelId = r.HotelId,
        HotelName = r.Hotel?.Name ?? string.Empty,
        Amenities = r.Amenities.Select(a => a.Name).ToList()
    };
    public static Guest GuestToEntityMapper(this GuestDTO dto)
    {
        return new Guest
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            NationalIdOrPassport = dto.NationalIdOrPassport,
            DateOfBirth = dto.DateOfBirth,
            Address = dto.Address
        };
    }
    public static GuestDTO GuestToDtoMapper(this Guest guest)
    {
        return new GuestDTO
        {
            Id = guest.Id,
            FirstName = guest.FirstName,
            LastName = guest.LastName,
            Email = guest.Email,
            PhoneNumber = guest.PhoneNumber,
            NationalIdOrPassport = guest.NationalIdOrPassport,
            DateOfBirth = guest.DateOfBirth,
            Address = guest.Address
        };
    }
    public static Hotel ApplyUpdateTo(this HotelDTO hotel, Hotel existinghotel)
    {
        if (hotel.Name != null) existinghotel.Name = hotel.Name;
        if (hotel.Address != null) existinghotel.Address = hotel.Address;
        if (hotel.City != null) existinghotel.City = hotel.City;
        if (hotel.Country != null) existinghotel.Country = hotel.Country;
        if (hotel.PhoneNumber != null) existinghotel.PhoneNumber = hotel.PhoneNumber;
        if (hotel.Email != null) existinghotel.Email = hotel.Email;
        if (hotel.StarRating.HasValue) existinghotel.StarRating = hotel.StarRating.Value;
        if (hotel.Amenities != null) existinghotel.Amenities = hotel.Amenities.Select(a => new Amenity
        {
            Name = a
        }).ToList();

        return existinghotel;

    }
}