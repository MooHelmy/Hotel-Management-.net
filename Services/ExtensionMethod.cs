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
        if (!string.IsNullOrEmpty(dto.Name)) existingHotel.Name = dto.Name;
        if (!string.IsNullOrEmpty(dto.Address)) existingHotel.Address = dto.Address;
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


    public static RoomDTO MapRoomToDto(Room room) => room.RoomToDtoMapper();


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
        if (!string.IsNullOrEmpty(dto.FirstName)) existingGuest.FirstName = dto.FirstName;
        if (!string.IsNullOrEmpty(dto.LastName)) existingGuest.LastName = dto.LastName;
        if (!string.IsNullOrEmpty(dto.Email)) existingGuest.Email = dto.Email;
        if (!string.IsNullOrEmpty(dto.PhoneNumber)) existingGuest.PhoneNumber = dto.PhoneNumber;
        if (!string.IsNullOrEmpty(dto.NationalIdOrPassport)) existingGuest.NationalIdOrPassport = dto.NationalIdOrPassport;
        if (dto.DateOfBirth != null) existingGuest.DateOfBirth = dto.DateOfBirth.Value;
        if (!string.IsNullOrEmpty(dto.Address)) existingGuest.Address = dto.Address;

        return existingGuest;
    }
    // ============================================================
    // Employee
    // ============================================================

    public static Employee EmployeeToEntityMapper(this EmployeeDTO dto)
    {
        return new Employee
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Position = dto.Position,
            Department = dto.Department,
            Salary = dto.Salary,
            HireDate = dto.HireDate,
            HotelId = dto.HotelId
        };
    }

    public static EmployeeDTO EmployeeToDtoMapper(this Employee employee)
    {
        return new EmployeeDTO
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber,
            Position = employee.Position,
            Department = employee.Department,
            Salary = employee.Salary,
            HireDate = employee.HireDate,
            HotelId = employee.HotelId,

        };
    }

    public static Employee ApplyUpdateTo(this EmployeeDTO dto, Employee existingEmployee)
    {
        if (!string.IsNullOrEmpty(dto.FirstName)) existingEmployee.FirstName = dto.FirstName;
        if (!string.IsNullOrEmpty(dto.LastName)) existingEmployee.LastName = dto.LastName;
        if (!string.IsNullOrEmpty(dto.Email)) existingEmployee.Email = dto.Email;
        if (!string.IsNullOrEmpty(dto.PhoneNumber)) existingEmployee.PhoneNumber = dto.PhoneNumber;
        if (!string.IsNullOrEmpty(dto.Position)) existingEmployee.Position = dto.Position;
        if (!string.IsNullOrEmpty(dto.Department)) existingEmployee.Department = dto.Department;
        if (dto.Salary != null) existingEmployee.Salary = dto.Salary;
        if (dto.HireDate != null) existingEmployee.HireDate = dto.HireDate;
        if (dto.HotelId != 0) existingEmployee.HotelId = dto.HotelId;

        return existingEmployee;
    }
    // ============================================================
    // Reservation
    // ============================================================

    public static Reservation ReservationToEntityMapper(this CreateReservationRequestDTO dto)
    {
        return new Reservation
        {
            CheckInDate = dto.CheckInDate,
            CheckOutDate = dto.CheckOutDate,
            NumberOfGuests = dto.NumberOfGuests,
            SpecialRequests = dto.SpecialRequests,
            GuestId = dto.GuestId,
            RoomId = dto.RoomId
        };
    }

    public static CreateReservationRequestDTO ReservationToRequestDTOMapper(this Reservation reservation)
    {
        return new CreateReservationRequestDTO
        {

            CheckInDate = reservation.CheckInDate,
            CheckOutDate = reservation.CheckOutDate,
            NumberOfGuests = reservation.NumberOfGuests,
            SpecialRequests = reservation.SpecialRequests,

            GuestId = reservation.GuestId,
            RoomId = reservation.RoomId,
            // Safe-guard: if the caller forgot .Include(r => r.Guest),
            // reservation.Guest may be null instead of an empty collection.
        };
    }
    public static ReservationDTO ReservationToDtoMapper(this Reservation reservation)
    {
        return new ReservationDTO
        {
            Id = reservation.Id,
            CheckInDate = reservation.CheckInDate,
            CheckOutDate = reservation.CheckOutDate,
            NumberOfGuests = reservation.NumberOfGuests,
            Status = reservation.Status.ToString(),
            TotalAmount = reservation.TotalAmount,
            SpecialRequests = reservation.SpecialRequests,
            CreatedAt = reservation.CreatedAt,
            GuestId = reservation.GuestId,
            RoomId = reservation.RoomId,
            // Safe-guard: if the caller forgot .Include(r => r.Guest),
            // reservation.Guest may be null instead of an empty collection.
        };
    }

    public static Reservation ApplyUpdateTo(this CreateReservationRequestDTO dto, Reservation existingReservation)
    {

        if (dto.SpecialRequests != null) existingReservation.SpecialRequests = dto.SpecialRequests;
        if (dto.NumberOfGuests != null) existingReservation.NumberOfGuests = dto.NumberOfGuests;
        if (!string.IsNullOrEmpty(dto.SpecialRequests)) existingReservation.SpecialRequests = dto.SpecialRequests;
        if (dto.GuestId != 0) existingReservation.GuestId = dto.GuestId;
        if (dto.RoomId != 0) existingReservation.RoomId = dto.RoomId;

        return existingReservation;
    }
    // ============================================================
    // Payment
    // ============================================================

    public static Payment PaymentToEntityMapper(this PaymentDTO dto)
    {
        return new Payment
        {
            ReservationId = dto.ReservationId,
            Amount = dto.Amount,
            PaymentMethod = Enum.TryParse<PaymentMethod>(dto.PaymentMethod, true, out var method)
                ? method
                : throw new ArgumentException($"Invalid PaymentMethod value: {dto.PaymentMethod}"),
            Status = Enum.TryParse<PaymentStatus>(dto.Status, true, out var status)
                ? status
                : throw new ArgumentException($"Invalid Status value: {dto.Status}"),
            TransactionReference = dto.TransactionReference,
            PaidAt = dto.PaidAt
        };
    }

    public static PaymentDTO PaymentToDtoMapper(this Payment payment)
    {
        return new PaymentDTO
        {
            Id = payment.Id,
            ReservationId = payment.ReservationId,
            Amount = payment.Amount,
            PaymentMethod = payment.PaymentMethod.ToString(),
            Status = payment.Status.ToString(),
            TransactionReference = payment.TransactionReference,
            PaidAt = payment.PaidAt
        };
    }

    public static Payment ApplyUpdateTo(this PaymentDTO dto, Payment existingPayment)
    {
        if (dto.ReservationId != 0) existingPayment.ReservationId = dto.ReservationId;
        if (dto.Amount != 0) existingPayment.Amount = dto.Amount;

        if (!string.IsNullOrEmpty(dto.PaymentMethod))
            existingPayment.PaymentMethod = Enum.Parse<PaymentMethod>(dto.PaymentMethod, ignoreCase: true);

        if (!string.IsNullOrEmpty(dto.Status))
            existingPayment.Status = Enum.Parse<PaymentStatus>(dto.Status, ignoreCase: true);

        if (!string.IsNullOrEmpty(dto.TransactionReference))
            existingPayment.TransactionReference = dto.TransactionReference;

        return existingPayment;
    }

}