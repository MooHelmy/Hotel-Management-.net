using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotelManagement.Entities;

public class Payment
{
    [Key]
    public int Id { get; set; }

    [Required, Column(TypeName = "decimal(10,2)")]
    public decimal Amount { get; set; }

    [Required]
    public PaymentMethod PaymentMethod { get; set; }

    [Required]
    public PaymentStatus Status { get; set; }

    [MaxLength(100)]
    public string? TransactionReference { get; set; }

    public DateTime PaidAt { get; set; } = DateTime.UtcNow;

    // Foreign key (one-to-one)
    [ForeignKey(nameof(Reservation))]
    public int ReservationId { get; set; }
    public Reservation Reservation { get; set; } = null!;
}

