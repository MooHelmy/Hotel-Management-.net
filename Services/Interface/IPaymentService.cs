using HotelManagement.DTOs;
using HotelManagement.Entities;

public interface IPaymentService
{
    Task<ServicesResponse<PaymentDTO>> GetByReservationAsync(int reservationId);
    Task<ServicesResponse<PaymentDTO>> PayAsync(int reservationId, decimal amount, PaymentMethod method, string? transactionReference);
    Task<ServicesResponse> RefundAsync(int reservationId);
}