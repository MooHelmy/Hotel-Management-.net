using HotelManagement.DTOs;
using HotelManagement.Entities;

public class PaymentService(IPaymentService paymentRepository)
{
    public async Task<ServicesResponse<PaymentDTO>> GetByReservationAsync(int reservationId)
    {
        var payment = await paymentRepository.GetByReservationAsync(reservationId);
        return payment;
    }

    public async Task<ServicesResponse<PaymentDTO>> PayAsync(int reservationId,
     decimal amount, PaymentMethod method, string? transactionReference)
    {
        var payment = await paymentRepository.PayAsync(reservationId, amount, method, transactionReference);
        return payment;
    }

    public async Task<ServicesResponse> RefundAsync(int reservationId)
    {
        var refund = await paymentRepository.RefundAsync(reservationId);
        return refund;
    }
}