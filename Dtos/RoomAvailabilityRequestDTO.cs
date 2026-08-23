
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.DTOs
{
    public class RoomAvailabilityRequestDTO
    {
        [Required]
        public int HotelId { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        public string? RoomType { get; set; }  // optional filter, e.g. "Deluxe"

        public int? NumberOfGuests { get; set; }
    }
}