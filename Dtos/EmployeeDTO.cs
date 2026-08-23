namespace HotelManagement.DTOs
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Position { get; set; }
        public string? Department { get; set; }
        public decimal? Salary { get; set; }
        public DateTime? HireDate { get; set; }
        public int HotelId { get; set; }
    }
}