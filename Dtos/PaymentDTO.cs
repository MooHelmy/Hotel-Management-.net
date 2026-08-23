namespace HotelManagement.DTOs
{
    public class PaymentDTO
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty; // e.g. "CreditCard"
        public string Status { get; set; } = string.Empty;        // e.g. "Paid"
        public string? TransactionReference { get; set; }
        public DateTime PaidAt { get; set; }
        public int ReservationId { get; set; }
    }
}