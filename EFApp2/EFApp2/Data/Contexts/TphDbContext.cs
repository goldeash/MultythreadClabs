using EFApp.Constants;
using EFApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EFApp.Data.Contexts
{
    /// <summary>
    /// DbContext for the Table-Per-Hierarchy (TPH) strategy.
    /// </summary>
    public class TphDbContext : DbContext
    {
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Ship> Ships { get; set; }

        /// <summary>
        /// Configures the database connection.
        /// </summary>
        /// <param name="optionsBuilder">Provides an API for configuring DbContextOptions.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(AppConstants.CONNECTION_STRING);
        }

        /// <summary>
        /// Configures the model for the TPH strategy.
        /// </summary>
        /// <param name="modelBuilder">Provides an API for configuring the model.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manufacturer>().ToTable(AppConstants.TPH_MANUFACTURERS_TABLE);

            modelBuilder.Entity<Ship>()
                .ToTable(AppConstants.TPH_SHIPS_TABLE)
                .HasDiscriminator<string>("ShipType")
                .HasValue<Battleship>("Battleship")
                .HasValue<Aircarrier>("Aircarrier")
                .HasValue<Cruiser>("Cruiser")
                .HasValue<Destroyer>("Destroyer")
                .HasValue<Submarine>("Submarine");
        }
    }
}
