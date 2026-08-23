using System.ComponentModel.DataAnnotations;

public class Guest
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(80)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(80)]
    public string LastName { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    [MaxLength(50)]
    public string? NationalIdOrPassport { get; set; }

    public DateTime? DateOfBirth { get; set; }

    [MaxLength(250)]
    public string? Address { get; set; }

    // Navigation property
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
}