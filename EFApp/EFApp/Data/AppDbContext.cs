using EFApp.Constants;
using EFApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EFApp.Data
{
    /// <summary>
    /// DbContext for the application
    /// </summary>
    public class AppDbContext : DbContext
    {
        public DbSet<Manufacturer> Manufacturers { get; set; }

        public DbSet<Ship> Ships { get; set; }

        /// <summary>
        /// Configures the database connection
        /// </summary>
        /// <param name="optionsBuilder">DbContextOptionsBuilder</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DatabaseConstants.CONNECTION_STRING);
        }

        /// <summary>
        /// Configures the model using Fluent API
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.ToTable(DatabaseConstants.MANUFACTURERS_TABLE_NAME);
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(100);
                entity.Property(m => m.Address)
                .IsRequired()
                .HasMaxLength(200);
            });

            modelBuilder.Entity<Ship>(entity =>
            {
                entity.ToTable(DatabaseConstants.SHIPS_TABLE_NAME);
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Model)
                .IsRequired()
                .HasMaxLength(100);
                entity.Property(s => s.SerialNumber)
                .IsRequired()
                .HasMaxLength(100);
                entity.Property(s => s.ShipType)
                .IsRequired();
                entity.HasOne<Manufacturer>()
                    .WithMany()
                    .HasForeignKey(s => s.ManufacturerId);
            });
        }
    }
}