using HotelManagement.DTOs;
using HotelManagement.Entities;

public interface IPaymentService
{
    Task<PaymentDTO?> GetByReservationAsync(int reservationId);
    Task<PaymentDTO> PayAsync(int reservationId, decimal amount, PaymentMethod method, string? transactionReference);
    Task<bool> RefundAsync(int reservationId);
}