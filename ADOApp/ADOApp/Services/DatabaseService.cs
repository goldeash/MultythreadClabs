using ADOApp.Constants;
using ADOApp.Models;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ADOApp.Services
{
    public class DatabaseService : IDatabaseService
    {
        private const string ManufacturerPrefix = "Manufacturer";
        private const string AddressPrefix = "Address";
        private const string ModelPrefix = "Model";
        private const string SerialPrefix = "SN";
        private const int DefaultNumberOfEntries = 30;
        private const int ShipTypeVariants = 5;

        private const string NameParam = "@Name";
        private const string AddressParam = "@Address";

        private const string ModelParam = "@Model";
        private const string SerialNumberParam = "@SerialNumber";
        private const string ShipTypeParam = "@ShipType";
        private const string ManufacturerIdParam = "@ManufacturerId";

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
                VALUES ({NameParam}, {AddressParam});
                SELECT SCOPE_IDENTITY();", connection);

            command.Parameters.AddWithValue(NameParam, manufacturer.Name);
            command.Parameters.AddWithValue(AddressParam, manufacturer.Address);

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
                VALUES ({ModelParam}, {SerialNumberParam}, {ShipTypeParam}, {ManufacturerIdParam})", connection);

            command.Parameters.AddWithValue(ModelParam, ship.Model);
            command.Parameters.AddWithValue(SerialNumberParam, ship.SerialNumber);
            command.Parameters.AddWithValue(ShipTypeParam, (int)ship.ShipType);
            command.Parameters.AddWithValue(ManufacturerIdParam, ship.ManufacturerId);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<Ship>> GetShipsByManufacturerAsync(int manufacturerId)
        {
            var ships = new List<Ship>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                $@"SELECT * FROM {DatabaseConstants.SHIPS_TABLE_NAME} 
                WHERE ManufacturerId = {ManufacturerIdParam}", connection);

            command.Parameters.AddWithValue(ManufacturerIdParam, manufacturerId);

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