using HotelManagement.Entities;
using Microsoft.AspNetCore.Mvc;

public class PaymentController(IPaymentService paymentService) : ControllerBase
{
    public async Task<IActionResult> GetByReservation(int reservationId)
    {
        var payment = await paymentService.GetByReservationAsync(reservationId);
        if (payment.Success)
        {
            return Ok(payment.Data);
        }
        return BadRequest(payment.Message);
    }

    public async Task<IActionResult> Pay(int reservationId, decimal amount, PaymentMethod method, string? transactionReference)
    {
        var payment = await paymentService.PayAsync(reservationId, amount, method, transactionReference);
        if (payment.Success)
        {
            return Ok(payment.Data);
        }
        return BadRequest(payment.Message);
    }

    public async Task<IActionResult> Refund(int reservationId)
    {
        try
        {
            var refund = await paymentService.RefundAsync(reservationId);
            if (refund.Success)
            {
                return Ok(refund);
            }
            return BadRequest(refund.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}