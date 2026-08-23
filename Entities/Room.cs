using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagement.Entities
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string RoomNumber { get; set; } = string.Empty;

        [Required]
        public RoomType RoomType { get; set; }

        [Required]
        public RoomStatus Status { get; set; }

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal PricePerNight { get; set; }

        public int? Floor { get; set; }

        public int? Capacity { get; set; }

        // Foreign key
        [ForeignKey(nameof(Hotel))]
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; } = null!;

        // Navigation properties
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public ICollection<Amenity> Amenities { get; set; } = new List<Amenity>();
    }
}
