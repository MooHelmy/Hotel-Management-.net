using HotelManagement.DTOs;
using HotelManagement.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin, Manager")]
public class ReservationController(IReservationService reservationService) : ControllerBase
{
    [HttpGet("All")]
    public async Task<ActionResult> GetAllAsync()
    {
        var reservations = await reservationService.GetAllAsync();
        return reservations.Any() ? Ok(reservations) : NotFound(reservations);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var reservation = await reservationService.GetByIdAsync(id);
        return reservation == null ? NotFound(reservation) : Ok(reservation);
    }
    [HttpPost("Add")]
    public async Task<ActionResult> CreateAsync(CreateReservationRequestDTO reservation)
    {
        var result = await reservationService.CreateAsync(reservation);
        return result.Success ? Ok(result) : BadRequest(result);
    }
    [HttpPut("Cancel")]
    public async Task<ActionResult> CancelAsync(int id)
    {
        var result = await reservationService.CancelAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }
    [HttpPut("CheckIn")]
    public async Task<ActionResult> CheckInAsync(int id)
    {
        var result = await reservationService.CheckInAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }
    [HttpPut("CheckOut")]
    public async Task<ActionResult> CheckOutAsync(int id)
    {
        var result = await reservationService.CheckOutAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }
    [HttpPut("Confirm")]
    public async Task<ActionResult> ConfirmAsync(int id)
    {
        var result = await reservationService.ConfirmAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        var result = await reservationService.DeleteAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }
    [HttpGet("SearchByGuest/{guestId}")]
    public async Task<ActionResult> SearchByGuestAsync(int guestId)
    {
        var reservations = await reservationService.GetByGuestAsync(guestId);
        return reservations.Any() ? Ok(reservations) : NotFound(reservations);
    }
}