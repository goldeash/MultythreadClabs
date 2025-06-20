using EFApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EFApp.Data
{
    /// <summary>
    /// Handles seeding the database with initial data.
    /// </summary>
    public class DataInitializer
    {
        private readonly DbContext _context;
        private readonly Random _random = new();

        /// <summary>
        /// Initializes a new instance of the DataInitializer class.
        /// </summary>
        /// <param name="context">The database context to seed.</param>
        public DataInitializer(DbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Seeds the database if it is empty.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SeedAsync()
        {
            if (!_context.Set<Manufacturer>().Any())
            {
                await SeedDataAsync();
            }
        }

        private async Task SeedDataAsync()
        {
            const int manufacturersCount = 2;
            const int shipsPerManufacturer = 3;

            for (int i = 1; i <= manufacturersCount; i++)
            {
                string name = $"Warship Dynamics #{i}";
                string address = $"Classified Sector {i}";
                var manufacturer = Manufacturer.Create(name, address);

                _context.Set<Manufacturer>().Add(manufacturer);
                await _context.SaveChangesAsync();

                for (int j = 1; j <= shipsPerManufacturer; j++)
                {
                    int manufacturerId = manufacturer.Id;
                    Ship ship = CreateRandomShip(manufacturerId, i, j);
                    _context.Set<Ship>().Add(ship);
                }
            }
            await _context.SaveChangesAsync();
        }

        private Ship CreateRandomShip(int manufacturerId, int manufIndex, int shipIndex)
        {
            int shipType = _random.Next(0, 5);
            string model = $"M{manufIndex}-S{shipIndex}";
            string serialNumber = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

            switch (shipType)
            {
                case 0:
                    return new Battleship { Model = model, SerialNumber = serialNumber, ManufacturerId = manufacturerId, MainCaliberGuns = _random.Next(8, 12) };
                case 1:
                    return new Aircarrier { Model = model, SerialNumber = serialNumber, ManufacturerId = manufacturerId, AircraftCapacity = _random.Next(40, 90) };
                case 2:
                    return new Cruiser { Model = model, SerialNumber = serialNumber, ManufacturerId = manufacturerId, MissileCount = _random.Next(20, 50) };
                case 3:
                    return new Destroyer { Model = model, SerialNumber = serialNumber, ManufacturerId = manufacturerId, TorpedoTubes = _random.Next(4, 10) };
                default:
                    return new Submarine { Model = model, SerialNumber = serialNumber, ManufacturerId = manufacturerId, MaxDepth = _random.Next(300, 700) };
            }
        }
    }
}
