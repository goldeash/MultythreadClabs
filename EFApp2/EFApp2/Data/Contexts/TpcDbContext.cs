using EFApp.Constants;
using EFApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EFApp.Data.Contexts
{
    /// <summary>
    /// DbContext for the Table-Per-Concrete-Type (TPC) strategy.
    /// </summary>
    public class TpcDbContext : DbContext
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
        /// Configures the model for the TPC strategy.
        /// </summary>
        /// <param name="modelBuilder">Provides an API for configuring the model.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manufacturer>().ToTable(AppConstants.TPC_MANUFACTURERS_TABLE);

            modelBuilder.Entity<Ship>().UseTpcMappingStrategy();
            modelBuilder.Entity<Battleship>().ToTable(AppConstants.TPC_BATTLESHIPS_TABLE);
            modelBuilder.Entity<Aircarrier>().ToTable(AppConstants.TPC_AIRCARRIERS_TABLE);
            modelBuilder.Entity<Cruiser>().ToTable(AppConstants.TPC_CRUISERS_TABLE);
            modelBuilder.Entity<Destroyer>().ToTable(AppConstants.TPC_DESTROYERS_TABLE);
            modelBuilder.Entity<Submarine>().ToTable(AppConstants.TPC_SUBMARINES_TABLE);
        }
    }
}
