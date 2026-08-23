using HotelManagement.Entities;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Unique room number per hotel
        modelBuilder.Entity<Room>()
            .HasIndex(r => new { r.HotelId, r.RoomNumber })
            .IsUnique();

        // One-to-one Reservation <-> Payment
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Reservation)
            .WithOne(r => r.Payment)
            .HasForeignKey<Payment>(p => p.ReservationId);

        // Unique guest / employee email
        modelBuilder.Entity<Guest>().HasIndex(g => g.Email).IsUnique();
        modelBuilder.Entity<Employee>().HasIndex(e => e.Email).IsUnique();
    }
    public DbSet<Hotel> Hotels => Set<Hotel>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Guest> Guests => Set<Guest>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Amenity> Amenities => Set<Amenity>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<Payment> Payments => Set<Payment>();
}