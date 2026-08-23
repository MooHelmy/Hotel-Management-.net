using HotelManagement.DTOs;

public interface IReservationService
{
    Task<IEnumerable<ReservationDTO>> GetAllAsync();
    Task<ReservationDTO?> GetByIdAsync(int id);
    Task<IEnumerable<ReservationDTO>> GetByGuestAsync(int guestId);
    Task<ReservationDTO> CreateAsync(CreateReservationRequestDTO request);
    Task<bool> ConfirmAsync(int id);
    Task<bool> CheckInAsync(int id);
    Task<bool> CheckOutAsync(int id);
    Task<bool> CancelAsync(int id);
    Task<bool> DeleteAsync(int id);
}