using HotelManagement.DTOs;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDTO>> GetAllAsync();
    Task<EmployeeDTO?> GetByIdAsync(int id);
    Task<IEnumerable<EmployeeDTO>> GetByHotelAsync(int hotelId);
    Task<EmployeeDTO> CreateAsync(EmployeeDTO dto);
    Task<EmployeeDTO?> UpdateAsync(int id, EmployeeDTO dto);
    Task<bool> DeleteAsync(int id);
}