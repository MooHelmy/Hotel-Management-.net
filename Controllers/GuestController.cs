using HotelManagement.DTOs;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class GuestController(IGuestService guestService) : ControllerBase
{
    [HttpGet("All")]
    public async Task<ActionResult> GetAllAsync()
    {
        var guests = await guestService.GetAllAsync();
        return guests.Any() ? Ok(guests) : NotFound(guests);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var guest = await guestService.GetByIdAsync(id);
        return guest == null ? NotFound(guest) : Ok(guest);
    }
    [HttpPost("Add")]
    public async Task<ActionResult> CreateAsync(GuestDTO guest)
    {
        var result = await guestService.CreateAsync(guest);
        return result.Success ? Ok(result) : BadRequest(result);
    }
    [HttpPut("Update")]
    public async Task<ActionResult> UpdateAsync(int id, GuestDTO guest)
    {
        var result = await guestService.UpdateAsync(id, guest);
        return result!.Success ? Ok(result) : BadRequest(result);
    }
    [HttpDelete("Delete/{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        var result = await guestService.DeleteAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }
    [HttpGet("SearchByEmail/{email}")]
    public async Task<ActionResult> SearchByEmailAsync(string email)
    {
        var guest = await guestService.GetByEmailAsync(email);
        return guest == null ? NotFound(guest) : Ok(guest);
    }
}