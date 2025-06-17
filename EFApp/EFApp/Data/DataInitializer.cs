using EFApp.Constants;
using EFApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace EFApp.Data
{
    public class DataInitializer
    {
        private readonly AppDbContext _context;
        private readonly Random _random = new();

        public DataInitializer(AppDbContext context)
        {
            _context = context;
        }

        public async Task InitializeAsync()
        {
            try
            {
                Console.WriteLine("Checking database...");
                bool created = await _context.Database.EnsureCreatedAsync();
                Console.WriteLine(created ? "Database created" : "Database already exists");

                if (created)
                {
                    Console.WriteLine("Seeding data...");
                    await SeedDataAsync();
                    Console.WriteLine("Data seeded successfully");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Initialization failed: {ex.Message}");
                throw;
            }
        }

        private async Task SeedDataAsync()
        {
            for (int i = 0; i < DatabaseConstants.DEFAULT_NUMBER_OF_ENTRIES; i++)
            {
                var manufacturer = new Manufacturer
                {
                    Name = $"EF_Manufacturer_{i + 1}",
                    Address = $"EF_Address_{i + 1}"
                };

                _context.Manufacturers.Add(manufacturer);
                await _context.SaveChangesAsync();

                var ship = new Ship
                {
                    Model = $"EF_Model_{i + 1}",
                    SerialNumber = $"EF_SN_{i + 1}",
                    ShipType = (ShipType)_random.Next(0, 5),
                    ManufacturerId = manufacturer.Id
                };

                _context.Ships.Add(ship);
                await _context.SaveChangesAsync();
            }
        }
    }
}