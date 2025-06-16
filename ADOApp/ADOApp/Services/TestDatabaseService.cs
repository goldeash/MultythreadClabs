using ADOApp.Models;
using ADOApp.Services;
using System.Collections.Concurrent;

namespace ADOApp.Tests
{
    /// <summary>
    /// Test implementation of IDatabaseService for testing purposes
    /// </summary>
    public class TestDatabaseService : IDatabaseService
    {
        private const string ManufacturerPrefix = "Manufacturer";
        private const string AddressPrefix = "Address";
        private const string ModelPrefix = "Model";
        private const string SerialPrefix = "SN";
        private const int DefaultNumberOfEntries = 30;
        private const int ShipTypeVariants = 5;

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
            return _ships.Values.Where(s => s.ManufacturerId == manufacturerId).ToList();
        }

        public async Task<List<Manufacturer>> GetAllManufacturersAsync()
        {
            await Task.Delay(1);
            return _manufacturers.Values.ToList();
        }

        public async Task PopulateDatabaseAsync()
        {
            var random = new Random();

            for (int i = 0; i < DefaultNumberOfEntries; i++)
            {
                var currentNumber = i + 1;

                var manufacturer = Manufacturer.Create(
                    $"{ManufacturerPrefix}_{currentNumber}",
                    $"{AddressPrefix}_{currentNumber}");

                var manufacturerId = await AddManufacturerAsync(manufacturer);

                var ship = Ship.Create(
                    $"{ModelPrefix}_{currentNumber}",
                    $"{SerialPrefix}_{currentNumber}",
                    (ShipType)random.Next(0, ShipTypeVariants),
                    manufacturerId);

                await AddShipAsync(ship);
            }
        }
    }
}