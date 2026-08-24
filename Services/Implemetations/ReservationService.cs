using HotelManagement.DTOs;

public class ReservationService(IReservationService reservationInterface)
{
    public async Task<IEnumerable<ReservationDTO>> GetAllAsync()
    {
        return await reservationInterface.GetAllAsync();
    }

    public async Task<ReservationDTO?> GetByIdAsync(int id)
    {
        return await reservationInterface.GetByIdAsync(id);
    }

    public async Task<IEnumerable<ReservationDTO>> GetByGuestAsync(int guestId)
    {
        return await reservationInterface.GetByGuestAsync(guestId);
    }

    public async Task<ServicesResponse> CreateAsync(CreateReservationRequestDTO request)
    {
        return await reservationInterface.CreateAsync(request);
    }

    public async Task<ServicesResponse> ConfirmAsync(int id)
    {
        return await reservationInterface.ConfirmAsync(id);
    }

    public async Task<ServicesResponse> CheckInAsync(int id)
    {
        return await reservationInterface.CheckInAsync(id);
    }

    public async Task<ServicesResponse> CheckOutAsync(int id)
    {
        return await reservationInterface.CheckOutAsync(id);
    }

    public async Task<ServicesResponse> CancelAsync(int id)
    {
        return await reservationInterface.CancelAsync(id);
    }

    public async Task<ServicesResponse> DeleteAsync(int id)
    {
        return await reservationInterface.DeleteAsync(id);
    }
}