using EFApp.Models;


namespace EFApp.Services
{
    /// <summary>
    /// In-memory implementation of IRepository for testing purposes
    /// </summary>
    public class InMemoryRepository : IRepository
    {
        private readonly List<Manufacturer> _manufacturers = new();
        private readonly List<Ship> _ships = new();
        private int _manufacturerIdCounter = 1;
        private int _shipIdCounter = 1;

        public Task<int> AddManufacturerAsync(Manufacturer manufacturer)
        {
            manufacturer.Id = _manufacturerIdCounter++;
            _manufacturers.Add(manufacturer);
            return Task.FromResult(manufacturer.Id);
        }

        public Task AddShipAsync(Ship ship)
        {
            ship.Id = _shipIdCounter++;
            _ships.Add(ship);
            return Task.CompletedTask;
        }

        public Task<List<Manufacturer>> GetAllManufacturersAsync()
        {
            return Task.FromResult(new List<Manufacturer>(_manufacturers));
        }

        public Task<List<Ship>> GetShipsByManufacturerAsync(int manufacturerId)
        {
            var ships = _ships.Where(s => s.ManufacturerId == manufacturerId)
                .ToList();
            return Task.FromResult(ships);
        }

        public Task<Manufacturer> GetManufacturerByIdAsync(int id)
        {
            var manufacturer = _manufacturers.FirstOrDefault(m => m.Id == id);
            return Task.FromResult(manufacturer);
        }

        public Task UpdateManufacturerAsync(Manufacturer manufacturer)
        {
            var existing = _manufacturers.FirstOrDefault(m => m.Id == manufacturer.Id);
            if (existing != null)
            {
                existing.Name = manufacturer.Name;
                existing.Address = manufacturer.Address;
            }
            return Task.CompletedTask;
        }

        public Task DeleteManufacturerAsync(int id)
        {
            var manufacturer = _manufacturers.FirstOrDefault(m => m.Id == id);
            if (manufacturer != null)
            {
                _manufacturers.Remove(manufacturer);
            }
            return Task.CompletedTask;
        }

        public Task<Ship> GetShipByIdAsync(int id)
        {
            var ship = _ships.FirstOrDefault(s => s.Id == id);
            return Task.FromResult(ship);
        }

        public Task UpdateShipAsync(Ship ship)
        {
            var existing = _ships.FirstOrDefault(s => s.Id == ship.Id);
            if (existing != null)
            {
                existing.Model = ship.Model;
                existing.SerialNumber = ship.SerialNumber;
                existing.ShipType = ship.ShipType;
                existing.ManufacturerId = ship.ManufacturerId;
            }
            return Task.CompletedTask;
        }

        public Task DeleteShipAsync(int id)
        {
            var ship = _ships.FirstOrDefault(s => s.Id == id);
            if (ship != null)
            {
                _ships.Remove(ship);
            }
            return Task.CompletedTask;
        }

        public async Task<bool> AddProductForNewManufacturerAsync(Manufacturer manufacturer, Ship ship)
        {
            if (string.IsNullOrEmpty(manufacturer?.Name) ||
                string.IsNullOrEmpty(manufacturer.Address) ||
                string.IsNullOrEmpty(ship?.Model) ||
                string.IsNullOrEmpty(ship.SerialNumber))
            {
                return false;
            }

            try
            {
                var manufacturerId = await AddManufacturerAsync(manufacturer);
                ship.ManufacturerId = manufacturerId;
                await AddShipAsync(ship);
                return true;
            }
            catch
            {
                if (manufacturer.Id != 0)
                {
                    _manufacturers.RemoveAll(m => m.Id == manufacturer.Id);
                }
                return false;
            }
        }
    }
}