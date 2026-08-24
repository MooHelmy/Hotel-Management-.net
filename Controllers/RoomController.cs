using HotelManagement.DTOs;
using HotelManagement.Entities;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class RoomController(IRoomService roomService) : ControllerBase
{
    [HttpGet("All")]
    public async Task<ActionResult> GetAllAsync()
    {
        var rooms = await roomService.GetAllAsync();
        return rooms.Any() ? Ok(rooms) : NotFound(rooms);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var room = await roomService.GetByIdAsync(id);
        return room == null ? NotFound(room) : Ok(room);
    }
    [HttpPost("Add")]
    public async Task<ActionResult> CreateAsync(RoomDTO room)
    {
        var result = await roomService.CreateAsync(room);
        return result.Success ? Ok(result) : BadRequest(result);
    }
    [HttpPut("Update")]
    public async Task<ActionResult> UpdateAsync(int id, RoomDTO room)
    {
        var result = await roomService.UpdateAsync(id, room);
        return result!.Success ? Ok(result) : BadRequest(result);
    }
    [HttpDelete("Delete/{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        var result = await roomService.DeleteAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }
    [HttpGet("SearchAvailable")]
    public async Task<ActionResult> SearchAvailableAsync(RoomAvailabilityRequestDTO request)
    {
        var result = await roomService.SearchAvailableAsync(request);
        return result.Any() ? Ok(result) : NotFound(result);
    }
    [HttpPut("UpdateStatus/{id}")]
    public async Task<ActionResult> UpdateStatusAsync(int id, RoomStatus status)
    {
        var result = await roomService.UpdateStatusAsync(id, status);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}