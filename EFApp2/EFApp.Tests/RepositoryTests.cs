using EFApp.Data.Contexts;
using EFApp.Models;
using EFApp.Services;
using Moq;
using Moq.EntityFrameworkCore;

namespace EFApp.Tests
{
    /// <summary>
    /// Contains unit tests for the Repository class.
    /// </summary>
    public class RepositoryTests
    {
        private readonly Mock<TphDbContext> _mockContext;
        private readonly Repository _repository;
        private readonly List<Manufacturer> _manufacturers;
        private readonly List<Ship> _ships;

        /// <summary>
        /// Initializes a new instance of the RepositoryTests class.
        /// Sets up the mock context and test data before each test.
        /// </summary>
        public RepositoryTests()
        {
            _mockContext = new Mock<TphDbContext>();

            _manufacturers = new List<Manufacturer>
            {
                Manufacturer.Create("Test Manufacturer 1", "Address 1"),
                Manufacturer.Create("Test Manufacturer 2", "Address 2")
            };
            _manufacturers[0].Id = 1;
            _manufacturers[1].Id = 2;

            _ships = new List<Ship>
            {
                new Battleship { Id = 1, Model = "BS-01", SerialNumber = "SN001", ManufacturerId = 1, MainCaliberGuns = 8 },
                new Aircarrier { Id = 2, Model = "AC-01", SerialNumber = "SN002", ManufacturerId = 2, AircraftCapacity = 75 }
            };

            _ships.ForEach(s => s.Manufacturer = _manufacturers.First(m => m.Id == s.ManufacturerId));

            _mockContext.Setup(c => c.Set<Manufacturer>()).ReturnsDbSet(_manufacturers);
            _mockContext.Setup(c => c.Set<Ship>()).ReturnsDbSet(_ships);

            _repository = new Repository(_mockContext.Object);
        }

        /// <summary>
        /// Tests if GetAllManufacturersAsync returns all manufacturers from the mock context.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [Fact]
        public async Task GetAllManufacturersAsync_ShouldReturnAllManufacturers()
        {
            // Arrange
            // Common arrangement is done in the constructor.

            // Act
            var result = await _repository.GetAllManufacturersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Test Manufacturer 1", result[0].Name);
        }

        /// <summary>
        /// Tests if GetManufacturerByIdAsync returns the correct manufacturer.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [Fact]
        public async Task GetManufacturerByIdAsync_ShouldReturnCorrectManufacturer()
        {
            // Arrange
            var expectedId = 1;
            _mockContext.Setup(c => c.Set<Manufacturer>().FindAsync(expectedId))
                .ReturnsAsync(_manufacturers.First(m => m.Id == expectedId));

            // Act
            var result = await _repository.GetManufacturerByIdAsync(expectedId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedId, result.Id);
        }

        /// <summary>
        /// Tests if AddManufacturerAsync adds a manufacturer to the context.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [Fact]
        public async Task AddManufacturerAsync_ShouldAddManufacturer()
        {
            // Arrange
            var newManufacturer = Manufacturer.Create("New Manufacturer", "New Address");

            // Act
            await _repository.AddManufacturerAsync(newManufacturer);

            // Assert
            _mockContext.Verify(c => c.Set<Manufacturer>().Add(newManufacturer), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<System.Threading.CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Tests if GetAllShipsAsync returns all ships from the mock context.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [Fact]
        public async Task GetAllShipsAsync_ShouldReturnAllShips()
        {
            // Arrange
            // Common arrangement is done in the constructor.

            // Act
            var result = await _repository.GetAllShipsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.IsType<Battleship>(result[0]);
            Assert.IsType<Aircarrier>(result[1]);
        }

        /// <summary>
        /// Tests if AddShipAsync adds a ship to the context.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [Fact]
        public async Task AddShipAsync_ShouldAddShip()
        {
            // Arrange
            var newShip = new Cruiser { Id = 3, Model = "CR-01", SerialNumber = "SN003", ManufacturerId = 1, MissileCount = 24 };

            // Act
            await _repository.AddShipAsync(newShip);

            // Assert
            _mockContext.Verify(c => c.Set<Ship>().Add(newShip), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<System.Threading.CancellationToken>()), Times.Once);
        }
    }
}