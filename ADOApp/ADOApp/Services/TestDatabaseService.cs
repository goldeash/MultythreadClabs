using ADOApp.Models;
using ADOApp.Services;
using System.Collections.Concurrent;

namespace ADOApp.Tests
{
    public class TestDatabaseService : IDatabaseService
    {
        private readonly ConcurrentDictionary<int, Manufacturer> _manufacturers = new();
        private readonly ConcurrentDictionary<int, Ship> _ships = new();
        private int _manufacturerIdCounter = 1;
        private int _shipIdCounter = 1;

        public Task InitializeDatabaseAsync()
        {
            _manufacturers.Clear();
            _ships.Clear();
            _manufacturerIdCounter = 1;
            _shipIdCounter = 1;
            return Task.CompletedTask;
        }

        public async Task<int> AddManufacturerAsync(Manufacturer manufacturer)
        {
            await Task.Delay(1);

            var id = _manufacturerIdCounter++;
            manufacturer.Id = id;
            _manufacturers.TryAdd(id, manufacturer);
            return id;
        }

        public async Task AddShipAsync(Ship ship)
        {
            await Task.Delay(1);

            var id = _shipIdCounter++;
            ship.Id = id;
            _ships.TryAdd(id, ship);
        }

        public async Task<List<Ship>> GetShipsByManufacturerAsync(int manufacturerId)
        {
            await Task.Delay(1);

            return _ships.Values
                .Where(s => s.ManufacturerId == manufacturerId)
                .ToList();
        }

        public async Task<List<Manufacturer>> GetAllManufacturersAsync()
        {
            await Task.Delay(1);

            return _manufacturers.Values.ToList();
        }

        public async Task PopulateDatabaseAsync()
        {
            var random = new Random();

            for (int i = 0; i < 30; i++)
            {
                var manufacturer = Manufacturer.Create(
                    $"Manufacturer_{i + 1}",
                    $"Address_{i + 1}");

                var manufacturerId = await AddManufacturerAsync(manufacturer);

                var ship = Ship.Create(
                    $"Model_{i + 1}",
                    $"SN_{i + 1}",
                    (ShipType)random.Next(0, 5),
                    manufacturerId);

                await AddShipAsync(ship);
            }
        }
    }
}