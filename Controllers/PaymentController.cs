using HotelManagement.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin, Manager")]
public class PaymentController(IPaymentService paymentService) : ControllerBase
{
    [HttpGet("ByReservation/{reservationId}")]
    public async Task<IActionResult> GetByReservation(int reservationId)
    {
        var payment = await paymentService.GetByReservationAsync(reservationId);
        if (payment.Success)
        {
            return Ok(payment.Data);
        }
        return BadRequest(payment.Message);
    }
    [HttpPost("Pay")]
    public async Task<IActionResult> Pay(int reservationId, decimal amount, PaymentMethod method, string? transactionReference)
    {
        var payment = await paymentService.PayAsync(reservationId, amount, method, transactionReference);
        if (payment.Success)
        {
            return Ok(payment.Data);
        }
        return BadRequest(payment.Message);
    }
    [HttpPost("Refund")]
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