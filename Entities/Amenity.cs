using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Entities
{
    public class Amenity
    {
        //Amenity هو عبارة عن مر
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? Description { get; set; }

        [MaxLength(50)]
        public string? Icon { get; set; }

        // Navigation properties (many-to-many)
        public ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();
        public ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}