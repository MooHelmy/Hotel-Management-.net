using HotelManagement.DTOs;
using HotelManagement.Entities;
using Microsoft.EntityFrameworkCore;

public class EmployeeRepository(IGeneric<Employee> employeeInterface, ApplicationDbContext Context) : IEmployeeService
{
    public async Task<ServicesResponse> CreateAsync(EmployeeDTO dto)
    {
        var employee = dto.EmployeeToEntityMapper();
        var result = await employeeInterface.CreateAsync(employee);
        return result > 0 ? new ServicesResponse(true, "The Employee Created Successfully")
           : new ServicesResponse(false, "The Employee Creation Failed");
    }

    public async Task<ServicesResponse> DeleteAsync(int id)
    {
        var employee = await Context.Employees.FindAsync(id);
        if (employee == null)
        {
            return new ServicesResponse(false, "Employee not found");
        }
        var result = await employeeInterface.DeleteAsync(id);
        return result > 0 ? new ServicesResponse(true, "The Employee Deleted Successfully")
           : new ServicesResponse(false, "The Employee Deletion Failed");
    }

    public async Task<IEnumerable<EmployeeDTO>> GetAllAsync()
    {
        var employees = await employeeInterface.GetAllAsync(e => e.Hotel);
        if (!employees.Any())
        {
            return [];
        }
        var employeesData = employees.Select(employee => employee.EmployeeToDtoMapper());
        return employeesData;
    }

    public async Task<IEnumerable<EmployeeDTO>> GetByHotelAsync(int hotelId)

    {

        var query = Context.Employees
                .Include(e => e.Hotel)
                .Where(e => e.HotelId == hotelId);
        if (!query.Any())
        {
            return [];
        }
        return await query.Select(e => e.EmployeeToDtoMapper()).ToListAsync();
    }

    public async Task<EmployeeDTO?> GetByIdAsync(int id)
    {
        var employee = await employeeInterface.GetByIdAsync(id, e => e.Hotel);
        if (employee == null)
        {
            throw new ItemNotFoundException($"Employee with id {id} not found");
        }
        return employee.EmployeeToDtoMapper();
    }

    public async Task<ServicesResponse?> UpdateAsync(int id, EmployeeDTO dto)
    {
        var employee = await employeeInterface.GetByIdAsync(id, e => e.Hotel);
        if (employee == null)
        {
            throw new ItemNotFoundException($"Employee with id {id} not found");
        }
        employee = dto.ApplyUpdateTo(employee);

        var result = await employeeInterface.UpdateAsync(employee);
        return result > 0 ? new ServicesResponse(true, "Employee updated successfully ")
        : new ServicesResponse(true, "Employee not updated  ");
    }
}