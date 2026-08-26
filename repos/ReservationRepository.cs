using HotelManagement.DTOs;
using HotelManagement.Entities;
using Microsoft.EntityFrameworkCore;

public class ReservationRepository(IGeneric<Reservation> reservationInterface, ApplicationDbContext Context) : IReservationService
{
    public async Task<ServicesResponse> CancelAsync(int id)
    {
        var reservation = await Context.Reservations.FindAsync(id);
        if (reservation == null)
        {
            return new ServicesResponse(false, "Reservation not found");
        }
        reservation.Status = ReservationStatus.Cancelled;
        var result = await reservationInterface.UpdateAsync(reservation);
        return result > 0 ? new ServicesResponse(true, "The Reservation Cancelled Successfully")
           : new ServicesResponse(false, "The Reservation Cancellation Failed");
    }

    public async Task<ServicesResponse> CheckInAsync(int id)
    {
        var reservation = await Context.Reservations.FindAsync(id);
        if (reservation == null)
        {
            return new ServicesResponse(false, "Reservation not found");
        }
        reservation.Status = ReservationStatus.CheckedIn;
        var result = await reservationInterface.UpdateAsync(reservation);
        return result > 0 ? new ServicesResponse(true, "The Reservation Checked In Successfully")
           : new ServicesResponse(false, "The Reservation Check In Failed");
    }

    public async Task<ServicesResponse> CheckOutAsync(int id)
    {
        var reservation = await Context.Reservations
            .Include(r => r.Room)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (reservation == null)
        {
            return new ServicesResponse(false, "Reservation not found");
        }

        var nights = (reservation.CheckOutDate.Date - reservation.CheckInDate.Date).Days;
        reservation.TotalAmount = nights * reservation.Room.PricePerNight;
        reservation.Status = ReservationStatus.CheckedOut;

        var result = await reservationInterface.UpdateAsync(reservation);
        return result > 0 ? new ServicesResponse(true, "The Reservation Checked Out Successfully")
           : new ServicesResponse(false, "The Reservation Check Out Failed");
    }

    public async Task<ServicesResponse> ConfirmAsync(int id)
    {
        var reservation = await Context.Reservations.FindAsync(id);
        if (reservation == null)
        {
            return new ServicesResponse(false, "Reservation not found");
        }
        reservation.Status = ReservationStatus.Confirmed;
        var result = await reservationInterface.UpdateAsync(reservation);
        return result > 0 ? new ServicesResponse(true, "The Reservation Confirmed Successfully")
           : new ServicesResponse(false, "The Reservation Confirmation Failed");
    }

    public async Task<ServicesResponse> CreateAsync(CreateReservationRequestDTO request)
    {
        var reservation = request.ReservationToEntityMapper();
        var result = await reservationInterface.CreateAsync(reservation);
        return result > 0 ? new ServicesResponse(true, "The Reservation Created Successfully")
           : new ServicesResponse(false, "The Reservation Creation Failed");
    }

    public async Task<ServicesResponse> DeleteAsync(int id)
    {
        var reservation = await Context.Reservations.FindAsync(id);
        if (reservation == null)
        {
            return new ServicesResponse(false, "Reservation not found");
        }
        var result = await reservationInterface.DeleteAsync(id);
        return result > 0 ? new ServicesResponse(true, "The Reservation Deleted Successfully")
           : new ServicesResponse(false, "The Reservation Deletion Failed");
    }

    public async Task<IEnumerable<ReservationDTO>> GetAllAsync()
    {
        var result = await reservationInterface.GetAllAsync(r => r.Guest, r => r.Room, r => r.Payment!);
        if (!result.Any())
        {
            return [];
        }

        var reservations = result.Select(reservation => reservation.ReservationToDtoMapper());
        return reservations;
    }

    public async Task<IEnumerable<ReservationDTO>> GetByGuestAsync(int guestId)
    {
        var query = Context.Reservations
                .Include(r => r.Guest)
                .Include(r => r.Room)
                .Include(r => r.Payment)
                .Where(r => r.GuestId == guestId);
        if (!query.Any())
        {
            return [];
        }
        return await query.Select(r => r.ReservationToDtoMapper()).ToListAsync();
    }

    public async Task<ReservationDTO?> GetByIdAsync(int id)
    {
        var result = await reservationInterface.GetByIdAsync(id, r => r.Guest, r => r.Room, r => r.Payment!);
        if (result == null)
        {
            throw new ItemNotFoundException($"Reservation with id {id} not found");
        }
        return result.ReservationToDtoMapper();
    }
}