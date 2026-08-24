using HotelManagement.DTOs;
using HotelManagement.Entities;

public static class ExtensionMethod
{
    // ============================================================
    // Hotel
    // ============================================================

    /// <summary>
    /// Maps a HotelDTO to a new Hotel entity.
    /// Amenities are NOT resolved here because this is a static mapper
    /// with no DbContext access. If the hotel has amenities, resolve the
    /// existing Amenity rows in the repository first (find-or-create),
    /// then pass them in via <paramref name="resolvedAmenities"/>.
    /// Passing raw DTO strings straight into `new Amenity { Name = ... }`
    /// causes EF Core to insert duplicate rows for names that already exist.
    /// </summary>
    public static Hotel HotelToEntityMapper(this HotelDTO dto, IEnumerable<Amenity>? resolvedAmenities = null)
    {
        var hotel = new Hotel
        {
            Name = dto.Name ?? string.Empty,
            Address = dto.Address,
            City = dto.City,
            Country = dto.Country,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            StarRating = dto.StarRating
        };

        if (resolvedAmenities != null)
        {
            hotel.Amenities = resolvedAmenities.ToList();
        }

        return hotel;
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
            // Safe-guard: if the caller forgot .Include(h => h.Amenities),
            // hotel.Amenities may be null instead of an empty collection.
            Amenities = hotel.Amenities?.Select(a => a.Name).ToList() ?? []
        };
    }

    /// <summary>
    /// Applies partial updates from a HotelDTO onto an existing tracked Hotel.
    /// Amenities are only replaced if resolvedAmenities is supplied — resolve
    /// them in the repository first, same reasoning as HotelToEntityMapper.
    /// </summary>
    public static Hotel ApplyUpdateTo(this HotelDTO dto, Hotel existingHotel, IEnumerable<Amenity>? resolvedAmenities = null)
    {
        if (dto.Name != null) existingHotel.Name = dto.Name;
        if (dto.Address != null) existingHotel.Address = dto.Address;
        if (dto.City != null) existingHotel.City = dto.City;
        if (dto.Country != null) existingHotel.Country = dto.Country;
        if (dto.PhoneNumber != null) existingHotel.PhoneNumber = dto.PhoneNumber;
        if (dto.Email != null) existingHotel.Email = dto.Email;
        if (dto.StarRating.HasValue) existingHotel.StarRating = dto.StarRating.Value;

        if (resolvedAmenities != null)
        {
            existingHotel.Amenities = resolvedAmenities.ToList();
        }

        return existingHotel;
    }

    // ============================================================
    // Room
    // ============================================================

    /// <summary>
    /// Maps a RoomDTO to a new Room entity. Only the HotelId foreign key is set —
    /// no new Hotel navigation object is fabricated, so EF Core resolves the
    /// relationship purely through the FK without trying to insert a duplicate Hotel.
    /// Same reasoning applies to Amenities: pass already-resolved Amenity rows in via
    /// <paramref name="resolvedAmenities"/> instead of letting this mapper invent new ones.
    /// </summary>
    public static Room RoomToEntityMapper(this RoomDTO dto, IEnumerable<Amenity>? resolvedAmenities = null)
    {
        if (!Enum.TryParse<RoomType>(dto.RoomType, true, out var roomType))
        {
            throw new ArgumentException(
                $"Invalid RoomType: '{dto.RoomType}'. Valid values are: {string.Join(", ", Enum.GetNames<RoomType>())}");
        }

        if (!Enum.TryParse<RoomStatus>(dto.Status, true, out var status))
        {
            status = RoomStatus.Available;
        }

        var room = new Room
        {
            RoomNumber = dto.RoomNumber,
            RoomType = roomType,
            Status = status,
            PricePerNight = dto.PricePerNight,
            Floor = dto.Floor,
            Capacity = dto.Capacity,
            HotelId = dto.HotelId
        };

        if (resolvedAmenities != null)
        {
            room.Amenities = resolvedAmenities.ToList();
        }

        return room;
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
            HotelName = room.Hotel?.Name ?? string.Empty,
            // Safe-guard against missing .Include(r => r.Amenities).
            Amenities = room.Amenities?.Select(a => a.Name).ToList() ?? []
        };
    }

    /// <summary>
    /// Kept only for backward compatibility with existing call sites (e.g. LINQ
    /// projections like `.Select(r => ExtensionMethod.MapRoomToDto(r))`).
    /// Delegates to RoomToDtoMapper so there is a single source of truth for the mapping.
    /// </summary>
    public static RoomDTO MapRoomToDto(Room room) => room.RoomToDtoMapper();

    /// <summary>
    /// Applies partial updates from a RoomDTO onto an existing tracked Room.
    /// RoomType/Status are validated and throw a clear ArgumentException on bad input
    /// instead of letting an unhandled Enum.Parse exception crash the request.
    /// Amenities are only replaced if resolvedAmenities is supplied (resolve them
    /// in the repository — see notes on RoomToEntityMapper).
    /// </summary>
    public static Room ApplyUpdateTo(this RoomDTO dto, Room existingRoom, IEnumerable<Amenity>? resolvedAmenities = null)
    {
        if (dto.RoomNumber != null) existingRoom.RoomNumber = dto.RoomNumber;

        if (dto.RoomType != null)
        {
            if (!Enum.TryParse<RoomType>(dto.RoomType, true, out var roomType))
            {
                throw new ArgumentException(
                    $"Invalid RoomType: '{dto.RoomType}'. Valid values are: {string.Join(", ", Enum.GetNames<RoomType>())}");
            }
            existingRoom.RoomType = roomType;
        }

        if (dto.Status != null)
        {
            existingRoom.Status = Enum.TryParse<RoomStatus>(dto.Status, true, out var status)
                ? status
                : RoomStatus.Available;
        }

        if (dto.PricePerNight != 0) existingRoom.PricePerNight = dto.PricePerNight;
        if (dto.Floor != null) existingRoom.Floor = dto.Floor;
        if (dto.Capacity != null) existingRoom.Capacity = dto.Capacity;
        if (dto.HotelId != 0) existingRoom.HotelId = dto.HotelId;

        if (resolvedAmenities != null)
        {
            existingRoom.Amenities = resolvedAmenities.ToList();
        }

        return existingRoom;
    }

    // ============================================================
    // Guest
    // ============================================================

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

    public static Guest ApplyUpdateTo(this GuestDTO dto, Guest existingGuest)
    {
        if (dto.FirstName != null) existingGuest.FirstName = dto.FirstName;
        if (dto.LastName != null) existingGuest.LastName = dto.LastName;
        if (dto.Email != null) existingGuest.Email = dto.Email;
        if (dto.PhoneNumber != null) existingGuest.PhoneNumber = dto.PhoneNumber;
        if (dto.NationalIdOrPassport != null) existingGuest.NationalIdOrPassport = dto.NationalIdOrPassport;
        if (dto.DateOfBirth != null) existingGuest.DateOfBirth = dto.DateOfBirth;
        if (dto.Address != null) existingGuest.Address = dto.Address;

        return existingGuest;
    }
}