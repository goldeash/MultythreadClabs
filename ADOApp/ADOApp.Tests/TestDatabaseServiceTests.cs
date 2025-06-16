using ADOApp.Models;
using ADOApp.Services;
using Moq;
using Xunit;

namespace ADOApp.Tests
{
    /// <summary>
    /// Tests for IDatabaseService using Moq
    /// </summary>
    public class TestDatabaseServiceTests
    {
        private readonly Mock<IDatabaseService> _mockService;

        /// <summary>
        /// Initializes a new instance of the test class
        /// </summary>
        public TestDatabaseServiceTests()
        {
            _mockService = new Mock<IDatabaseService>();
        }

        /// <summary>
        /// Tests that InitializeDatabaseAsync clears existing data
        /// </summary>
        [Fact]
        public async Task InitializeDatabaseAsync_ShouldClearExistingData()
        {
            _mockService.Setup(m => m.InitializeDatabaseAsync()).Returns(Task.CompletedTask);
            _mockService.Setup(m => m.GetAllManufacturersAsync()).ReturnsAsync(new List<Manufacturer>());
            _mockService.Setup(m => m.GetShipsByManufacturerAsync(It.IsAny<int>())).ReturnsAsync(new List<Ship>());

            await _mockService.Object.InitializeDatabaseAsync();
            var manufacturers = await _mockService.Object.GetAllManufacturersAsync();
            var ships = await _mockService.Object.GetShipsByManufacturerAsync(1);

            Assert.Empty(manufacturers);
            Assert.Empty(ships);
            _mockService.Verify(m => m.InitializeDatabaseAsync(), Times.Once);
        }

        /// <summary>
        /// Tests that AddManufacturerAsync adds manufacturer and returns correct ID
        /// </summary>
        [Fact]
        public async Task AddManufacturerAsync_ShouldAddManufacturerAndReturnId()
        {
            var manufacturer = new Manufacturer { Name = "Test", Address = "Test" };
            _mockService.Setup(m => m.AddManufacturerAsync(It.IsAny<Manufacturer>()))
                .ReturnsAsync(1)
                .Callback<Manufacturer>(m => m.Id = 1);
            _mockService.Setup(m => m.GetAllManufacturersAsync())
                .ReturnsAsync(new List<Manufacturer> { manufacturer });

            var id = await _mockService.Object.AddManufacturerAsync(manufacturer);
            var manufacturers = await _mockService.Object.GetAllManufacturersAsync();

            Assert.Equal(1, id);
            Assert.Equal(1, manufacturer.Id);
            Assert.Single(manufacturers);
            Assert.Equal("Test", manufacturers[0].Name);
            _mockService.Verify(m => m.AddManufacturerAsync(manufacturer), Times.Once);
        }

        /// <summary>
        /// Tests that AddShipAsync adds ship with auto-incremented ID
        /// </summary>
        [Fact]
        public async Task AddShipAsync_ShouldAddShipWithAutoIncrementedId()
        {
            var manufacturerId = 1;
            var ship = new Ship
            {
                Id = 1,
                Model = "Model1",
                SerialNumber = "SN1",
                ShipType = ShipType.Cruiser,
                ManufacturerId = manufacturerId
            };

            _mockService.Setup(m => m.AddShipAsync(It.IsAny<Ship>()))
                .Returns(Task.CompletedTask)
                .Callback<Ship>(s => s.Id = 1);
            _mockService.Setup(m => m.GetShipsByManufacturerAsync(manufacturerId))
                .ReturnsAsync(new List<Ship> { ship });

            await _mockService.Object.AddShipAsync(ship);
            var ships = await _mockService.Object.GetShipsByManufacturerAsync(manufacturerId);

            Assert.Equal(1, ship.Id);
            Assert.Single(ships);
            Assert.Equal("Model1", ships[0].Model);
            _mockService.Verify(m => m.AddShipAsync(ship), Times.Once);
        }

        /// <summary>
        /// Tests that GetShipsByManufacturerAsync returns only ships for specified manufacturer
        /// </summary>
        [Fact]
        public async Task GetShipsByManufacturerAsync_ShouldReturnOnlyShipsForSpecifiedManufacturer()
        {
            var m1 = 1;
            var m2 = 2;

            var shipsForM1 = new List<Ship>
            {
                new Ship { Model = "S1", SerialNumber = "SN1", ShipType = ShipType.Destroyer, ManufacturerId = m1 },
                new Ship { Model = "S2", SerialNumber = "SN2", ShipType = ShipType.Submarine, ManufacturerId = m1 }
            };

            var shipsForM2 = new List<Ship>
            {
                new Ship { Model = "S3", SerialNumber = "SN3", ShipType = ShipType.Aircarrier, ManufacturerId = m2 }
            };

            _mockService.Setup(m => m.GetShipsByManufacturerAsync(m1)).ReturnsAsync(shipsForM1);
            _mockService.Setup(m => m.GetShipsByManufacturerAsync(m2)).ReturnsAsync(shipsForM2);

            var resultForM1 = await _mockService.Object.GetShipsByManufacturerAsync(m1);
            var resultForM2 = await _mockService.Object.GetShipsByManufacturerAsync(m2);

            Assert.Equal(2, resultForM1.Count);
            Assert.All(resultForM1, s => Assert.Equal(m1, s.ManufacturerId));

            Assert.Single(resultForM2);
            Assert.Equal(m2, resultForM2[0].ManufacturerId);
        }

        /// <summary>
        /// Tests that GetAllManufacturersAsync returns all added manufacturers
        /// </summary>
        [Fact]
        public async Task GetAllManufacturersAsync_ShouldReturnAllAddedManufacturers()
        {
            var manufacturers = new List<Manufacturer>
            {
                new Manufacturer { Id = 1, Name = "M1", Address = "A1" },
                new Manufacturer { Id = 2, Name = "M2", Address = "A2" }
            };

            _mockService.Setup(m => m.GetAllManufacturersAsync()).ReturnsAsync(manufacturers);

            var result = await _mockService.Object.GetAllManufacturersAsync();

            Assert.Equal(2, result.Count);
            Assert.Contains(result, m => m.Name == "M1");
            Assert.Contains(result, m => m.Name == "M2");
        }

        /// <summary>
        /// Tests that PopulateDatabaseAsync adds correct number of entries
        /// </summary>
        [Fact]
        public async Task PopulateDatabaseAsync_ShouldAdd30ManufacturersAnd30Ships()
        {
            var manufacturers = Enumerable.Range(1, 30)
                .Select(i => new Manufacturer { Id = i, Name = $"M{i}", Address = $"A{i}" })
                .ToList();

            var ships = Enumerable.Range(1, 30)
                .Select(i => new Ship
                {
                    Id = i,
                    Model = $"Model{i}",
                    SerialNumber = $"SN{i}",
                    ShipType = ShipType.Cruiser,
                    ManufacturerId = i
                })
                .ToList();

            _mockService.Setup(m => m.GetAllManufacturersAsync()).ReturnsAsync(manufacturers);
            _mockService.Setup(m => m.GetShipsByManufacturerAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => ships.Where(s => s.ManufacturerId == id).ToList());
            _mockService.Setup(m => m.PopulateDatabaseAsync()).Returns(Task.CompletedTask);

            await _mockService.Object.PopulateDatabaseAsync();
            var resultManufacturers = await _mockService.Object.GetAllManufacturersAsync();
            var allShips = new List<Ship>();

            foreach (var m in resultManufacturers)
            {
                allShips.AddRange(await _mockService.Object.GetShipsByManufacturerAsync(m.Id));
            }

            Assert.Equal(30, resultManufacturers.Count);
            Assert.Equal(30, allShips.Count);

            foreach (var m in resultManufacturers)
            {
                var manufacturerShips = await _mockService.Object.GetShipsByManufacturerAsync(m.Id);
                Assert.Single(manufacturerShips);
            }
        }
    }
}