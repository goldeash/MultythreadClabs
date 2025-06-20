using EFApp.Constants;
using EFApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EFApp.Data.Contexts
{
    /// <summary>
    /// DbContext for the Table-Per-Type (TPT) strategy.
    /// </summary>
    public class TptDbContext : DbContext
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
        /// Configures the model for the TPT strategy.
        /// </summary>
        /// <param name="modelBuilder">Provides an API for configuring the model.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manufacturer>().ToTable(AppConstants.TPT_MANUFACTURERS_TABLE);

            modelBuilder.Entity<Ship>().ToTable(AppConstants.TPT_SHIPS_TABLE);
            modelBuilder.Entity<Battleship>().ToTable(AppConstants.TPT_BATTLESHIPS_TABLE);
            modelBuilder.Entity<Aircarrier>().ToTable(AppConstants.TPT_AIRCARRIERS_TABLE);
            modelBuilder.Entity<Cruiser>().ToTable(AppConstants.TPT_CRUISERS_TABLE);
            modelBuilder.Entity<Destroyer>().ToTable(AppConstants.TPT_DESTROYERS_TABLE);
            modelBuilder.Entity<Submarine>().ToTable(AppConstants.TPT_SUBMARINES_TABLE);
        }
    }
}
