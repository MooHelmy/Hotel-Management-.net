
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.DTOs
{
    public class CreateReservationRequestDTO
    {
        [Required]
        public int GuestId { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [Range(1, 20)]
        public int? NumberOfGuests { get; set; }

        [MaxLength(500)]
        public string? SpecialRequests { get; set; }
    }
}
