using EFApp.Data;
using EFApp.Models;
using EFApp.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EFApp
{
    /// <summary>
    /// Service for handling console menu operations
    /// </summary>
    public class MenuService
    {
        private readonly IRepository _repository;

        /// <summary>
        /// Initializes a new instance of MenuService
        /// </summary>
        /// <param name="repository">IRepository</param>
        public MenuService(IRepository repository)
        {
            _repository = repository;
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
                Console.WriteLine("1. CRUD for Manufacturers");
                Console.WriteLine("2. CRUD for Ships");
                Console.WriteLine("3. Add Product for New Manufacturer (with transaction)");
                Console.WriteLine("4. Get Ships by Manufacturer");
                Console.WriteLine("5. Initialize Database with Sample Data");
                Console.WriteLine("6. Exit");
                Console.Write("Select an option: ");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        await ShowManufacturerCrudMenuAsync();
                        break;
                    case "2":
                        await ShowShipCrudMenuAsync();
                        break;
                    case "3":
                        await AddProductForNewManufacturerAsync();
                        break;
                    case "4":
                        await GetShipsByManufacturerAsync();
                        break;
                    case "5":
                        await InitializeDatabaseAsync();
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        /// <summary>
        /// Shows CRUD menu for manufacturers
        /// </summary>
        /// <returns>Task</returns>
        private async Task ShowManufacturerCrudMenuAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Manufacturer CRUD Menu");
                Console.WriteLine("1. Add Manufacturer");
                Console.WriteLine("2. View All Manufacturers");
                Console.WriteLine("3. View Manufacturer by ID");
                Console.WriteLine("4. Update Manufacturer");
                Console.WriteLine("5. Delete Manufacturer");
                Console.WriteLine("6. Back to Main Menu");
                Console.Write("Select an option: ");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        await AddManufacturerAsync();
                        break;
                    case "2":
                        await ViewAllManufacturersAsync();
                        break;
                    case "3":
                        await ViewManufacturerByIdAsync();
                        break;
                    case "4":
                        await UpdateManufacturerAsync();
                        break;
                    case "5":
                        await DeleteManufacturerAsync();
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        /// <summary>
        /// Shows CRUD menu for ships
        /// </summary>
        /// <returns>Task</returns>
        private async Task ShowShipCrudMenuAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Ship CRUD Menu");
                Console.WriteLine("1. Add Ship");
                Console.WriteLine("2. View All Ships");
                Console.WriteLine("3. View Ship by ID");
                Console.WriteLine("4. Update Ship");
                Console.WriteLine("5. Delete Ship");
                Console.WriteLine("6. Back to Main Menu");
                Console.Write("Select an option: ");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        await AddShipAsync();
                        break;
                    case "2":
                        await ViewAllShipsAsync();
                        break;
                    case "3":
                        await ViewShipByIdAsync();
                        break;
                    case "4":
                        await UpdateShipAsync();
                        break;
                    case "5":
                        await DeleteShipAsync();
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        /// <summary>
        /// Adds a manufacturer to the database
        /// </summary>
        /// <returns>Task</returns>
        private async Task AddManufacturerAsync()
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
            var id = await _repository.AddManufacturerAsync(manufacturer);

            Console.WriteLine($"Manufacturer added with ID: {id}. Press any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Views all manufacturers
        /// </summary>
        /// <returns>Task</returns>
        private async Task ViewAllManufacturersAsync()
        {
            Console.Clear();
            Console.WriteLine("All Manufacturers:");

            var manufacturers = await _repository.GetAllManufacturersAsync();

            if (manufacturers.Count == 0)
            {
                Console.WriteLine("No manufacturers found.");
            }
            else
            {
                foreach (var m in manufacturers)
                {
                    Console.WriteLine($"{m.Id}. {m}");
                }
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Views manufacturer by ID
        /// </summary>
        /// <returns>Task</returns>
        private async Task ViewManufacturerByIdAsync()
        {
            Console.Clear();
            Console.WriteLine("View Manufacturer by ID");

            Console.Write("Enter Manufacturer ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            var manufacturer = await _repository.GetManufacturerByIdAsync(id);

            if (manufacturer == null)
            {
                Console.WriteLine("Manufacturer not found.");
            }
            else
            {
                Console.WriteLine(manufacturer);
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Updates a manufacturer
        /// </summary>
        /// <returns>Task</returns>
        private async Task UpdateManufacturerAsync()
        {
            Console.Clear();
            Console.WriteLine("Update Manufacturer");

            Console.Write("Enter Manufacturer ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            var manufacturer = await _repository.GetManufacturerByIdAsync(id);

            if (manufacturer == null)
            {
                Console.WriteLine("Manufacturer not found. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.Write($"Name ({manufacturer.Name}): ");
            var name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
            {
                manufacturer.Name = name;
            }

            Console.Write($"Address ({manufacturer.Address}): ");
            var address = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(address))
            {
                manufacturer.Address = address;
            }

            await _repository.UpdateManufacturerAsync(manufacturer);

            Console.WriteLine("Manufacturer updated successfully. Press any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Deletes a manufacturer
        /// </summary>
        /// <returns>Task</returns>
        private async Task DeleteManufacturerAsync()
        {
            Console.Clear();
            Console.WriteLine("Delete Manufacturer");

            Console.Write("Enter Manufacturer ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            await _repository.DeleteManufacturerAsync(id);

            Console.WriteLine("Manufacturer deleted (if it existed). Press any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Adds a ship to the database
        /// </summary>
        /// <returns>Task</returns>
        private async Task AddShipAsync()
        {
            Console.Clear();
            Console.WriteLine("Add New Ship");

            var manufacturers = await _repository.GetAllManufacturersAsync();
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
            await _repository.AddShipAsync(ship);

            Console.WriteLine("Ship added successfully. Press any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Views all ships (without manufacturer info)
        /// </summary>
        /// <returns>Task</returns>
        private async Task ViewAllShipsAsync()
        {
            Console.Clear();
            Console.WriteLine("All Ships:");

            var manufacturers = await _repository.GetAllManufacturersAsync();

            foreach (var manufacturer in manufacturers)
            {
                var ships = await _repository.GetShipsByManufacturerAsync(manufacturer.Id);

                if (ships.Count > 0)
                {
                    Console.WriteLine($"\nManufacturer: {manufacturer.Name} (ID: {manufacturer.Id})");
                    foreach (var ship in ships)
                    {
                        Console.WriteLine($"- {ship}");
                    }
                }
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Views ship by ID
        /// </summary>
        /// <returns>Task</returns>
        private async Task ViewShipByIdAsync()
        {
            Console.Clear();
            Console.WriteLine("View Ship by ID");

            Console.Write("Enter Ship ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            var ship = await _repository.GetShipByIdAsync(id);

            if (ship == null)
            {
                Console.WriteLine("Ship not found.");
            }
            else
            {
                Console.WriteLine(ship);
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Updates a ship
        /// </summary>
        /// <returns>Task</returns>
        private async Task UpdateShipAsync()
        {
            Console.Clear();
            Console.WriteLine("Update Ship");

            Console.Write("Enter Ship ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            var ship = await _repository.GetShipByIdAsync(id);

            if (ship == null)
            {
                Console.WriteLine("Ship not found. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.Write($"Model ({ship.Model}): ");
            var model = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(model))
            {
                ship.Model = model;
            }

            Console.Write($"Serial Number ({ship.SerialNumber}): ");
            var serialNumber = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(serialNumber))
            {
                ship.SerialNumber = serialNumber;
            }

            Console.WriteLine($"Current Ship Type: {ship.ShipType} ({(int)ship.ShipType})");
            Console.WriteLine("Available Ship Types:");
            foreach (ShipType type in Enum.GetValues(typeof(ShipType)))
            {
                Console.WriteLine($"{(int)type}. {type}");
            }

            Console.Write("Ship Type (number, leave empty to keep current): ");
            var shipTypeInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(shipTypeInput) &&
                int.TryParse(shipTypeInput, out int shipType) &&
                Enum.IsDefined(typeof(ShipType), shipType))
            {
                ship.ShipType = (ShipType)shipType;
            }

            await _repository.UpdateShipAsync(ship);

            Console.WriteLine("Ship updated successfully. Press any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Deletes a ship
        /// </summary>
        /// <returns>Task</returns>
        private async Task DeleteShipAsync()
        {
            Console.Clear();
            Console.WriteLine("Delete Ship");

            Console.Write("Enter Ship ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            await _repository.DeleteShipAsync(id);

            Console.WriteLine("Ship deleted (if it existed). Press any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Adds a new product for a new manufacturer with transaction
        /// </summary>
        /// <returns>Task</returns>
        private async Task AddProductForNewManufacturerAsync()
        {
            Console.Clear();
            Console.WriteLine("Add Product for New Manufacturer (with transaction)");

            Console.Write("Manufacturer Name: ");
            var name = Console.ReadLine();

            Console.Write("Manufacturer Address: ");
            var address = Console.ReadLine();

            Console.Write("Ship Model: ");
            var model = Console.ReadLine();

            Console.Write("Ship Serial Number: ");
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

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(address) ||
                string.IsNullOrWhiteSpace(model) || string.IsNullOrWhiteSpace(serialNumber))
            {
                Console.WriteLine("All fields are required. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            var manufacturer = Manufacturer.Create(name, address);
            var ship = Ship.Create(model, serialNumber, (ShipType)shipType, 0);

            var success = await _repository.AddProductForNewManufacturerAsync(manufacturer, ship);

            if (success)
            {
                Console.WriteLine("Transaction completed successfully. Both manufacturer and ship were added.");
            }
            else
            {
                Console.WriteLine("Transaction failed. No changes were made to the database.");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Gets ships by manufacturer
        /// </summary>
        /// <returns>Task</returns>
        private async Task GetShipsByManufacturerAsync()
        {
            Console.Clear();
            Console.WriteLine("Get Ships by Manufacturer");

            var manufacturers = await _repository.GetAllManufacturersAsync();
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
            if (!int.TryParse(Console.ReadLine(), out int manufacturerId) ||
                !manufacturers.Any(m => m.Id == manufacturerId))
            {
                Console.WriteLine("Invalid Manufacturer ID. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            var ships = await _repository.GetShipsByManufacturerAsync(manufacturerId);

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
        /// Initializes the database with sample data
        /// </summary>
        /// <returns>Task</returns>
        private async Task InitializeDatabaseAsync()
        {
            Console.Clear();
            Console.WriteLine("Initializing database with sample data...");

            var context = new AppDbContext();
            var initializer = new DataInitializer(context);
            await initializer.InitializeAsync();

            Console.WriteLine("Database initialized with 30 manufacturers and 30 ships. Press any key to continue...");
            Console.ReadKey();
        }
    }
}