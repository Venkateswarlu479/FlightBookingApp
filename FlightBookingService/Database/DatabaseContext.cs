using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightBookingService.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<FlightDetails> FlightDetails { get; set; }
        public DbSet<BookingDetails> BookingDetails { get; set; }
        public DbSet<PassengerList> PassengerList { get; set; }
        public DbSet<DiscountDetails> DiscountDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PassengerList>()
                .HasOne<BookingDetails>(b => b.BookingDetails)
                .WithMany(p => p.PassengerList)
                .HasForeignKey(b => b.BookingId);
        }
    }
}
