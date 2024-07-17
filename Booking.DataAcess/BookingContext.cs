using Microsoft.EntityFrameworkCore;

namespace Booking.DataAcess
{
    public class BookingContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<EventRegistration> EventRegistrations { get; set; }

        public BookingContext(DbContextOptions<BookingContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("BookingDb");
            }
        }
    }
}
