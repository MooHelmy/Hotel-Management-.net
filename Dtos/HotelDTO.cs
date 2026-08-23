namespace HotelManagement.DTOs
{
    public class HotelDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public int? StarRating { get; set; }
        public List<string> Amenities { get; set; } = new();
    }
}
