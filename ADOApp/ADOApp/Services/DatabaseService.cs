using ADOApp.Constants;
using ADOApp.Models;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ADOApp.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService()
        {
            _connectionString = DatabaseConstants.CONNECTION_STRING;
        }

        public async Task InitializeDatabaseAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var createManufacturersTableCommand = new SqlCommand(
                $@"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{DatabaseConstants.MANUFACTURERS_TABLE_NAME}' AND xtype='U')
                CREATE TABLE {DatabaseConstants.MANUFACTURERS_TABLE_NAME} (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    Name NVARCHAR(100) NOT NULL,
                    Address NVARCHAR(200) NOT NULL
                )", connection);

            await createManufacturersTableCommand.ExecuteNonQueryAsync();

            var createShipsTableCommand = new SqlCommand(
                $@"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{DatabaseConstants.SHIPS_TABLE_NAME}' AND xtype='U')
                CREATE TABLE {DatabaseConstants.SHIPS_TABLE_NAME} (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    Model NVARCHAR(100) NOT NULL,
                    SerialNumber NVARCHAR(100) NOT NULL,
                    ShipType INT NOT NULL,
                    ManufacturerId INT NOT NULL,
                    FOREIGN KEY (ManufacturerId) REFERENCES {DatabaseConstants.MANUFACTURERS_TABLE_NAME}(Id)
                )", connection);

            await createShipsTableCommand.ExecuteNonQueryAsync();
        }

        public async Task<int> AddManufacturerAsync(Manufacturer manufacturer)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                $@"INSERT INTO {DatabaseConstants.MANUFACTURERS_TABLE_NAME} (Name, Address) 
                VALUES (@Name, @Address);
                SELECT SCOPE_IDENTITY();", connection);

            command.Parameters.AddWithValue("@Name", manufacturer.Name);
            command.Parameters.AddWithValue("@Address", manufacturer.Address);

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task AddShipAsync(Ship ship)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                $@"INSERT INTO {DatabaseConstants.SHIPS_TABLE_NAME} 
                (Model, SerialNumber, ShipType, ManufacturerId) 
                VALUES (@Model, @SerialNumber, @ShipType, @ManufacturerId)", connection);

            command.Parameters.AddWithValue("@Model", ship.Model);
            command.Parameters.AddWithValue("@SerialNumber", ship.SerialNumber);
            command.Parameters.AddWithValue("@ShipType", (int)ship.ShipType);
            command.Parameters.AddWithValue("@ManufacturerId", ship.ManufacturerId);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<Ship>> GetShipsByManufacturerAsync(int manufacturerId)
        {
            var ships = new List<Ship>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                $@"SELECT * FROM {DatabaseConstants.SHIPS_TABLE_NAME} 
                WHERE ManufacturerId = @ManufacturerId", connection);

            command.Parameters.AddWithValue("@ManufacturerId", manufacturerId);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                ships.Add(new Ship
                {
                    Id = reader.GetInt32(0),
                    Model = reader.GetString(1),
                    SerialNumber = reader.GetString(2),
                    ShipType = (ShipType)reader.GetInt32(3),
                    ManufacturerId = reader.GetInt32(4)
                });
            }

            return ships;
        }

        public async Task<List<Manufacturer>> GetAllManufacturersAsync()
        {
            var manufacturers = new List<Manufacturer>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                $@"SELECT * FROM {DatabaseConstants.MANUFACTURERS_TABLE_NAME}", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                manufacturers.Add(new Manufacturer
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Address = reader.GetString(2)
                });
            }

            return manufacturers;
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
