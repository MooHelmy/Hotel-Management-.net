namespace HotelManagement.DTOs
{
    public class ReservationDTO
    {
        public int Id { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int? NumberOfGuests { get; set; }
        public string Status { get; set; } = string.Empty; // e.g. "Confirmed"
        public decimal TotalAmount { get; set; }
        public string? SpecialRequests { get; set; }
        public DateTime CreatedAt { get; set; }

        public int GuestId { get; set; }
        public string GuestFullName { get; set; } = string.Empty;

        public int RoomId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;

        public PaymentDTO? Payment { get; set; }
    }
}