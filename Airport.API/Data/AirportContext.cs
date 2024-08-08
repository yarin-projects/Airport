using Airport.API.Data.ValueConverter;
using Airport.API.Models;
using Airport.API.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Airport.API.Data
{
    public class AirportContext(DbContextOptions<AirportContext> options) : DbContext(options)
    {
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Leg> Legs { get; set; }
        public DbSet<LegConnection> LegConnections { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<WaitingFlight> WaitingFlights { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Flight-Leg relationship
            modelBuilder.Entity<Flight>()
                .HasOne(f => f.Leg)
                .WithOne(l => l.Flight)
                .HasForeignKey<Flight>(f => f.LegId)
                .IsRequired(false);

            modelBuilder.Entity<Leg>()
                .HasOne(l => l.Flight)
                .WithOne(f => f.Leg)
                .HasForeignKey<Leg>(l => l.FlightId)
                .IsRequired(false);

            modelBuilder.Entity<Leg>()
                .Property(l => l.RowVersion)
                .IsRowVersion()
                .HasConversion(new SqliteTimeStampConverter())
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Configure Leg-NextLegs relationship
            modelBuilder.Entity<LegConnection>()
                .HasKey(lc => new { lc.LegId, lc.NextLegId });

            modelBuilder.Entity<LegConnection>()
                .HasOne(lc => lc.Leg)
                .WithMany(l => l.NextLegs)
                .HasForeignKey(lc => lc.LegId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LegConnection>()
                .HasOne(lc => lc.NextLeg)
                .WithMany()
                .HasForeignKey(lc => lc.NextLegId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed initial legs
            modelBuilder.Entity<Leg>().HasData(
                new Leg { LegId = LegIds.Leg1, LegType = LegType.Landing, CrossingTime = TimeSpan.FromSeconds(Random.Shared.Next(2, 13)) },
                new Leg { LegId = LegIds.Leg2, LegType = LegType.Landing, CrossingTime = TimeSpan.FromSeconds(Random.Shared.Next(2, 13)) },
                new Leg { LegId = LegIds.Leg3, LegType = LegType.Landing, CrossingTime = TimeSpan.FromSeconds(Random.Shared.Next(2, 13)) },
                new Leg { LegId = LegIds.Leg4, LegType = LegType.Landing | LegType.Departure, CrossingTime = TimeSpan.FromSeconds(Random.Shared.Next(5, 13)) },
                new Leg { LegId = LegIds.Leg5, LegType = LegType.Landing, CrossingTime = TimeSpan.FromSeconds(Random.Shared.Next(2, 13)) },
                new Leg { LegId = LegIds.Leg6, LegType = LegType.Landing | LegType.Departure, CrossingTime = TimeSpan.FromSeconds(Random.Shared.Next(3, 13)) },
                new Leg { LegId = LegIds.Leg7, LegType = LegType.Landing | LegType.Departure, CrossingTime = TimeSpan.FromSeconds(Random.Shared.Next(3, 13)) },
                new Leg { LegId = LegIds.Leg8, LegType = LegType.Departure, CrossingTime = TimeSpan.FromSeconds(Random.Shared.Next(2, 13)) },
                new Leg { LegId = LegIds.Leg9, LegType = LegType.Departure, CrossingTime = TimeSpan.FromSeconds(Random.Shared.Next(2, 13)) }
            );

            // Seed LegConnection relationships
            modelBuilder.Entity<LegConnection>().HasData(
                new LegConnection { LegId = LegIds.Leg1, NextLegId = LegIds.Leg2 },
                new LegConnection { LegId = LegIds.Leg2, NextLegId = LegIds.Leg3 },
                new LegConnection { LegId = LegIds.Leg3, NextLegId = LegIds.Leg4 },
                new LegConnection { LegId = LegIds.Leg4, NextLegId = LegIds.Leg5 },
                new LegConnection { LegId = LegIds.Leg4, NextLegId = LegIds.Leg9 },
                new LegConnection { LegId = LegIds.Leg5, NextLegId = LegIds.Leg6 },
                new LegConnection { LegId = LegIds.Leg5, NextLegId = LegIds.Leg7 },
                new LegConnection { LegId = LegIds.Leg6, NextLegId = LegIds.Leg8 },
                new LegConnection { LegId = LegIds.Leg7, NextLegId = LegIds.Leg8 },
                new LegConnection { LegId = LegIds.Leg8, NextLegId = LegIds.Leg4 }
            );
        }
    }
}
