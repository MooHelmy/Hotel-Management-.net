using HotelManagement.DTOs;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDTO>> GetAllAsync();
    Task<EmployeeDTO?> GetByIdAsync(int id);
    Task<IEnumerable<EmployeeDTO>> GetByHotelAsync(int hotelId);
    Task<ServicesResponse> CreateAsync(EmployeeDTO dto);
    Task<ServicesResponse?> UpdateAsync(int id, EmployeeDTO dto);
    Task<ServicesResponse> DeleteAsync(int id);
}