namespace HotelManagement.DTOs
{
    public class RoomDTO
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;   // e.g. "Deluxe"
        public string Status { get; set; } = string.Empty;     // e.g. "Available"
        public decimal PricePerNight { get; set; }
        public int? Floor { get; set; }
        public int? Capacity { get; set; }
        public int HotelId { get; set; }
        public string HotelName { get; set; } = string.Empty;
        public List<string> Amenities { get; set; } = new();
    }
}