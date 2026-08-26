using HotelManagement.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin, Manager")]
public class HotelController(IHotelService hotelService) : ControllerBase

{

    [HttpGet("All")]
    [AllowAnonymous]
    public async Task<ActionResult> GetAllAsync()
    {
        var hotels = await hotelService.GetAllAsync();
        return hotels.Any() ? Ok(hotels) : NotFound(hotels);
    }
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var hotel = await hotelService.GetByIdAsync(id);
        return hotel == null ? NotFound(hotel) : Ok(hotel);
    }
    [HttpPost("Add")]
    public async Task<ActionResult> CreateAsync(HotelDTO hotel)
    {
        var result = await hotelService.CreateAsync(hotel);
        return result.Success ? Ok(result) : BadRequest(result);
    }
    [HttpPut("Update")]
    public async Task<ActionResult> UpdateAsync(int id, HotelDTO hotel)
    {
        var result = await hotelService.UpdateAsync(id, hotel);
        return result!.Success ? Ok(result) : BadRequest(result);
    }
    [HttpDelete("Delete/{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        var result = await hotelService.DeleteAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}