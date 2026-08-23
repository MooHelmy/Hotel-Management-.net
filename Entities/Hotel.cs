using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Entities
{
    public class Hotel
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(250)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(100)]
        public string? Country { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(150)]
        public string? Email { get; set; }

        public int? StarRating { get; set; }

        // Navigation properties
        public ICollection<Room> Rooms { get; set; } = new List<Room>();
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public ICollection<Amenity> Amenities { get; set; } = new List<Amenity>();
    }
}


