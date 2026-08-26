using HotelManagement.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin, Manager")]
public class EmployeeController(IEmployeeService employeeService) : ControllerBase
{
    [HttpGet("All")]
    public async Task<ActionResult> GetAllAsync()
    {
        var employees = await employeeService.GetAllAsync();
        return employees.Any() ? Ok(employees) : NotFound(employees);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var employee = await employeeService.GetByIdAsync(id);
        return employee == null ? NotFound(employee) : Ok(employee);
    }
    [HttpPost("Add")]
    public async Task<ActionResult> CreateAsync(EmployeeDTO employee)
    {
        var result = await employeeService.CreateAsync(employee);
        return result.Success ? Ok(result) : BadRequest(result);
    }
    [HttpPut("Update")]
    public async Task<ActionResult> UpdateAsync(int id, EmployeeDTO employee)
    {
        var result = await employeeService.UpdateAsync(id, employee);
        return result!.Success ? Ok(result) : BadRequest(result);
    }
    [HttpDelete("Delete/{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        var result = await employeeService.DeleteAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }
    [HttpGet("SearchByHotel/{hotelId}")]
    public async Task<ActionResult> SearchByHotelAsync(int hotelId)
    {
        var employees = await employeeService.GetByHotelAsync(hotelId);
        return employees.Any() ? Ok(employees) : NotFound(employees);
    }
}