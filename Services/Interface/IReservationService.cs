using HotelManagement.DTOs;

public interface IReservationService
{
    Task<IEnumerable<ReservationDTO>> GetAllAsync();
    Task<ReservationDTO?> GetByIdAsync(int id);
    Task<IEnumerable<ReservationDTO>> GetByGuestAsync(int guestId);
    Task<ServicesResponse> CreateAsync(CreateReservationRequestDTO request);
    Task<ServicesResponse> ConfirmAsync(int id);
    Task<ServicesResponse> CheckInAsync(int id);
    Task<ServicesResponse> CheckOutAsync(int id);
    Task<ServicesResponse> CancelAsync(int id);
    Task<ServicesResponse> DeleteAsync(int id);
}