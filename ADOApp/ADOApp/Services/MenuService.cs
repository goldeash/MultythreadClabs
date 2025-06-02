using ADOApp.Models;
using ADOApp.Services;

namespace ADOApp
{
    /// <summary>
    /// Service for handling console menu operations
    /// </summary>
    public class MenuService
    {
        private readonly DatabaseService _databaseService;

        public MenuService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        /// <summary>
        /// Displays the main menu and handles user input
        /// </summary>
        /// <returns>Task</returns>
        public async Task ShowMenuAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. Add Manufacturer");
                Console.WriteLine("2. Add Ship");
                Console.WriteLine("3. Get Ships by Manufacturer");
                Console.WriteLine("4. Populate Database with Sample Data");
                Console.WriteLine("5. Exit");
                Console.Write("Select an option: ");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        await AddManufacturerAsync();
                        break;
                    case "2":
                        await AddShipAsync();
                        break;
                    case "3":
                        await GetShipsByManufacturerAsync();
                        break;
                    case "4":
                        await PopulateDatabaseAsync();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        /// <summary>
        /// Add Manufacturer to db
        /// </summary>
        /// <returns>Task</returns>
        public async Task AddManufacturerAsync()
        {
            Console.Clear();
            Console.WriteLine("Add New Manufacturer");

            Console.Write("Name: ");
            var name = Console.ReadLine();

            Console.Write("Address: ");
            var address = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(address))
            {
                Console.WriteLine("Name and Address cannot be empty. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            var manufacturer = Manufacturer.Create(name, address);
            var id = await _databaseService.AddManufacturerAsync(manufacturer);

            Console.WriteLine($"Manufacturer added with ID: {id}. Press any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Add ship to db
        /// </summary>
        /// <returns>Task</returns>
        public async Task AddShipAsync()
        {
            Console.Clear();
            Console.WriteLine("Add New Ship");

            var manufacturers = await _databaseService.GetAllManufacturersAsync();
            if (manufacturers.Count == 0)
            {
                Console.WriteLine("No manufacturers found. Please add a manufacturer first. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Available Manufacturers:");
            foreach (var m in manufacturers)
            {
                Console.WriteLine($"{m.Id}. {m.Name}");
            }

            Console.Write("Manufacturer ID: ");
            if (!int.TryParse(Console.ReadLine(), out int manufacturerId) ||
                !manufacturers.Any(m => m.Id == manufacturerId))
            {
                Console.WriteLine("Invalid Manufacturer ID. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.Write("Model: ");
            var model = Console.ReadLine();

            Console.Write("Serial Number: ");
            var serialNumber = Console.ReadLine();

            Console.WriteLine("Available Ship Types:");
            foreach (ShipType type in Enum.GetValues(typeof(ShipType)))
            {
                Console.WriteLine($"{(int)type}. {type}");
            }

            Console.Write("Ship Type (number): ");
            if (!int.TryParse(Console.ReadLine(), out int shipType) ||
                !Enum.IsDefined(typeof(ShipType), shipType))
            {
                Console.WriteLine("Invalid Ship Type. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            if (string.IsNullOrWhiteSpace(model) || string.IsNullOrWhiteSpace(serialNumber))
            {
                Console.WriteLine("Model and Serial Number cannot be empty. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            var ship = Ship.Create(model, serialNumber, (ShipType)shipType, manufacturerId);
            await _databaseService.AddShipAsync(ship);

            Console.WriteLine("Ship added successfully. Press any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Get Ship from db
        /// </summary>
        /// <returns>Task</returns>
        public async Task GetShipsByManufacturerAsync()
        {
            Console.Clear();
            Console.WriteLine("Get Ships by Manufacturer");

            var manufacturers = await _databaseService.GetAllManufacturersAsync();
            if (manufacturers.Count == 0)
            {
                Console.WriteLine("No manufacturers found. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Available Manufacturers:");
            foreach (var m in manufacturers)
            {
                Console.WriteLine($"{m.Id}. {m.Name}");
            }

            Console.Write("Manufacturer ID: ");
            if (!int.TryParse(
                Console.ReadLine(), out int manufacturerId) ||
                !manufacturers.Any(m => m.Id == manufacturerId))
            {
                Console.WriteLine("Invalid Manufacturer ID. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            var ships = await _databaseService.GetShipsByManufacturerAsync(manufacturerId);

            Console.WriteLine($"\nShips for Manufacturer ID {manufacturerId}:");
            if (ships.Count == 0)
            {
                Console.WriteLine("No ships found for this manufacturer.");
            }
            else
            {
                foreach (var ship in ships)
                {
                    Console.WriteLine(ship);
                }
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Create many manufcaturer and ships for test
        /// </summary>
        /// <returns>Task</returns>
        public async Task PopulateDatabaseAsync()
        {
            Console.Clear();
            Console.WriteLine("Populating database with sample data...");

            await _databaseService.PopulateDatabaseAsync();

            Console.WriteLine("Database populated with 30 manufacturers and 30 ships. Press any key to continue...");
            Console.ReadKey();
        }
    }
}