using HotelManagement.DTOs;

public class EmployeeService(IEmployeeService employeeInterface)
{
    public async Task<IEnumerable<EmployeeDTO>> GetAllAsync()
    {
        return await employeeInterface.GetAllAsync();
    }

    public async Task<EmployeeDTO?> GetByIdAsync(int id)
    {
        return await employeeInterface.GetByIdAsync(id);
    }

    public async Task<IEnumerable<EmployeeDTO>> GetByHotelAsync(int hotelId)
    {
        return await employeeInterface.GetByHotelAsync(hotelId);
    }

    public async Task<ServicesResponse> CreateAsync(EmployeeDTO dto)
    {
        return await employeeInterface.CreateAsync(dto);
    }

    public async Task<ServicesResponse?> UpdateAsync(int id, EmployeeDTO dto)
    {
        return await employeeInterface.UpdateAsync(id, dto);
    }

    public async Task<ServicesResponse> DeleteAsync(int id)
    {
        return await employeeInterface.DeleteAsync(id);
    }
}