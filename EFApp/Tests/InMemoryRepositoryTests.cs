using EFApp.Models;
using EFApp.Services;

namespace EFApp.Tests
{
    public class InMemoryRepositoryTests
    {
        private readonly InMemoryRepository _repository;

        public InMemoryRepositoryTests()
        {
            _repository = new InMemoryRepository();
        }

        [Fact]
        public async Task AddManufacturerAsync_ShouldAddManufacturer()
        {
            // Arrange
            var manufacturer = Manufacturer.Create("Test", "Address");

            // Act
            var id = await _repository.AddManufacturerAsync(manufacturer);

            // Assert
            Assert.Equal(1, id);
            var result = await _repository.GetManufacturerByIdAsync(id);
            Assert.Equal("Test", result.Name);
        }

        [Fact]
        public async Task AddShipAsync_ShouldAddShip()
        {
            // Arrange
            var manufacturer = Manufacturer.Create("Test", "Address");
            var manufacturerId = await _repository.AddManufacturerAsync(manufacturer);
            var ship = Ship.Create("Model", "SN123", ShipType.Cruiser, manufacturerId);

            // Act
            await _repository.AddShipAsync(ship);

            // Assert
            var ships = await _repository.GetShipsByManufacturerAsync(manufacturerId);
            Assert.Single(ships);
        }

        [Fact]
        public async Task GetManufacturerByIdAsync_ShouldReturnCorrectManufacturer()
        {
            // Arrange
            var manufacturer = Manufacturer.Create("Test", "Address");
            var id = await _repository.AddManufacturerAsync(manufacturer);

            // Act
            var result = await _repository.GetManufacturerByIdAsync(id);

            // Assert
            Assert.Equal(id, result.Id);
            Assert.Equal("Test", result.Name);
        }

        [Fact]
        public async Task UpdateManufacturerAsync_ShouldUpdateManufacturer()
        {
            // Arrange
            var manufacturer = Manufacturer.Create("Test", "Address");
            var id = await _repository.AddManufacturerAsync(manufacturer);
            manufacturer.Name = "Updated";

            // Act
            await _repository.UpdateManufacturerAsync(manufacturer);

            // Assert
            var updated = await _repository.GetManufacturerByIdAsync(id);
            Assert.Equal("Updated", updated.Name);
        }

        [Fact]
        public async Task DeleteManufacturerAsync_ShouldRemoveManufacturer()
        {
            // Arrange
            var manufacturer = Manufacturer.Create("Test", "Address");
            var id = await _repository.AddManufacturerAsync(manufacturer);

            // Act
            await _repository.DeleteManufacturerAsync(id);

            // Assert
            var result = await _repository.GetManufacturerByIdAsync(id);
            Assert.Null(result);
        }

        [Fact]
        public async Task AddProductForNewManufacturerAsync_ShouldAddBoth()
        {
            // Arrange
            var manufacturer = Manufacturer.Create("Test", "Address");
            var ship = Ship.Create("Model", "SN123", ShipType.Cruiser, 0);

            // Act
            var success = await _repository.AddProductForNewManufacturerAsync(manufacturer, ship);

            // Assert
            Assert.True(success);
            var manufacturers = await _repository.GetAllManufacturersAsync();
            var ships = await _repository.GetShipsByManufacturerAsync(manufacturers[0].Id);
            Assert.Single(manufacturers);
            Assert.Single(ships);
        }

        [Fact]
        public async Task AddProductForNewManufacturerAsync_ShouldRollbackOnFailure()
        {
            // Arrange
            var invalidManufacturer = Manufacturer.Create(null, "Address");
            var ship = Ship.Create("Model", "SN123", ShipType.Cruiser, 0);

            // Act
            var success = await _repository.AddProductForNewManufacturerAsync(invalidManufacturer, ship);

            // Assert
            Assert.False(success);
            var manufacturers = await _repository.GetAllManufacturersAsync();
            Assert.Empty(manufacturers);
        }
    }
}