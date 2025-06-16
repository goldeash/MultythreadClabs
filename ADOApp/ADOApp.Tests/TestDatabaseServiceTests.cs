using ADOApp.Models;
using ADOApp.Tests;

public class TestDatabaseServiceTests
{
    private readonly TestDatabaseService _service;

    public TestDatabaseServiceTests()
    {
        _service = new TestDatabaseService();
    }

    [Fact]
    public async Task InitializeDatabaseAsync_ShouldClearExistingData()
    {
        await _service.AddManufacturerAsync(new Manufacturer { Name = "Test", Address = "Test" });
        await _service.AddShipAsync(new Ship { Model = "Test", SerialNumber = "Test", ShipType = ShipType.Battleship, ManufacturerId = 1 });

        await _service.InitializeDatabaseAsync();

        var manufacturers = await _service.GetAllManufacturersAsync();
        var ships = await _service.GetShipsByManufacturerAsync(1);

        Assert.Empty(manufacturers);
        Assert.Empty(ships);
    }

    [Fact]
    public async Task AddManufacturerAsync_ShouldAddManufacturerAndReturnId()
    {
        var manufacturer = new Manufacturer { Name = "Test", Address = "Test" };

        var id = await _service.AddManufacturerAsync(manufacturer);

        Assert.Equal(1, id);
        Assert.Equal(1, manufacturer.Id);

        var manufacturers = await _service.GetAllManufacturersAsync();
        Assert.Single(manufacturers);
        Assert.Equal("Test", manufacturers[0].Name);
    }

    [Fact]
    public async Task AddShipAsync_ShouldAddShipWithAutoIncrementedId()
    {
        var manufacturerId = await _service.AddManufacturerAsync(new Manufacturer { Name = "Test", Address = "Test" });
        var ship = new Ship { Model = "Model1", SerialNumber = "SN1", ShipType = ShipType.Cruiser, ManufacturerId = manufacturerId };

        await _service.AddShipAsync(ship);

        Assert.Equal(1, ship.Id);

        var ships = await _service.GetShipsByManufacturerAsync(manufacturerId);
        Assert.Single(ships);
        Assert.Equal("Model1", ships[0].Model);
    }

    [Fact]
    public async Task GetShipsByManufacturerAsync_ShouldReturnOnlyShipsForSpecifiedManufacturer()
    {
        var m1 = await _service.AddManufacturerAsync(new Manufacturer { Name = "M1", Address = "A1" });
        var m2 = await _service.AddManufacturerAsync(new Manufacturer { Name = "M2", Address = "A2" });

        await _service.AddShipAsync(new Ship { Model = "S1", SerialNumber = "SN1", ShipType = ShipType.Destroyer, ManufacturerId = m1 });
        await _service.AddShipAsync(new Ship { Model = "S2", SerialNumber = "SN2", ShipType = ShipType.Submarine, ManufacturerId = m1 });
        await _service.AddShipAsync(new Ship { Model = "S3", SerialNumber = "SN3", ShipType = ShipType.Aircarrier, ManufacturerId = m2 });

        var shipsForM1 = await _service.GetShipsByManufacturerAsync(m1);
        var shipsForM2 = await _service.GetShipsByManufacturerAsync(m2);

        Assert.Equal(2, shipsForM1.Count);
        Assert.All(shipsForM1, s => Assert.Equal(m1, s.ManufacturerId));

        Assert.Single(shipsForM2);
        Assert.Equal(m2, shipsForM2[0].ManufacturerId);
    }

    [Fact]
    public async Task GetAllManufacturersAsync_ShouldReturnAllAddedManufacturers()
    {
        await _service.AddManufacturerAsync(new Manufacturer { Name = "M1", Address = "A1" });
        await _service.AddManufacturerAsync(new Manufacturer { Name = "M2", Address = "A2" });

        var manufacturers = await _service.GetAllManufacturersAsync();

        Assert.Equal(2, manufacturers.Count);
        Assert.Contains(manufacturers, m => m.Name == "M1");
        Assert.Contains(manufacturers, m => m.Name == "M2");
    }

    [Fact]
    public async Task PopulateDatabaseAsync_ShouldAdd30ManufacturersAnd30Ships()
    {
        await _service.PopulateDatabaseAsync();

        var manufacturers = await _service.GetAllManufacturersAsync();
        var ships = new List<Ship>();

        foreach (var m in manufacturers)
        {
            ships.AddRange(await _service.GetShipsByManufacturerAsync(m.Id));
        }

        Assert.Equal(30, manufacturers.Count);
        Assert.Equal(30, ships.Count);

        foreach (var m in manufacturers)
        {
            var manufacturerShips = await _service.GetShipsByManufacturerAsync(m.Id);
            Assert.NotEmpty(manufacturerShips);
        }
    }
}