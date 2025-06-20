using EFApp.Constants;
using EFApp.Data.Contexts;
using EFApp.Models;
using EFApp.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFApp.UI
{
    /// <summary>
    /// Manages the console menu and user interaction logic.
    /// </summary>
    public class MenuService
    {
        /// <summary>
        /// Displays the main menu for selecting an inheritance strategy.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task ShowStrategySelectionMenuAsync()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("--- Select Inheritance Strategy ---");
                Console.WriteLine("1. TPH (Table-Per-Hierarchy)");
                Console.WriteLine("2. TPT (Table-Per-Type)");
                Console.WriteLine("3. TPC (Table-Per-Concrete-Type)");
                Console.WriteLine("4. Exit");
                Console.Write("Select an option: ");

                string choice = Console.ReadLine();
                DbContext context = null;

                switch (choice)
                {
                    case "1":
                        context = new TphDbContext();
                        break;
                    case "2":
                        context = new TptDbContext();
                        break;
                    case "3":
                        context = new TpcDbContext();
                        break;
                    case "4":
                        exit = true;
                        continue;
                    default:
                        ShowMessage(AppConstants.INVALID_OPTION_MSG);
                        continue;
                }

                var repository = new Repository(context);
                await ShowCrudMenuAsync(repository, context.GetType().Name);
                context.Dispose();
            }
        }

        private async Task ShowCrudMenuAsync(IRepository repository, string contextName)
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine($"--- CRUD Menu (Strategy: {contextName.Replace("DbContext", "")}) ---");
                Console.WriteLine("1. Manage Manufacturers");
                Console.WriteLine("2. Manage Ships");
                Console.WriteLine("3. Back to Strategy Selection");
                Console.Write("Select an option: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        await ShowManufacturerMenuAsync(repository);
                        break;
                    case "2":
                        await ShowShipMenuAsync(repository);
                        break;
                    case "3":
                        back = true;
                        break;
                    default:
                        ShowMessage(AppConstants.INVALID_OPTION_MSG);
                        break;
                }
            }
        }

        private async Task ShowManufacturerMenuAsync(IRepository repository)
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("--- Manufacturer Management ---");
                Console.WriteLine("1. View All Manufacturers");
                Console.WriteLine("2. Add Manufacturer");
                Console.WriteLine("3. Update Manufacturer");
                Console.WriteLine("4. Delete Manufacturer");
                Console.WriteLine("5. Back");
                Console.Write("Select an option: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        await ViewAllManufacturersAsync(repository);
                        break;
                    case "2":
                        await AddManufacturerAsync(repository);
                        break;
                    case "3":
                        await UpdateManufacturerAsync(repository);
                        break;
                    case "4":
                        await DeleteManufacturerAsync(repository);
                        break;
                    case "5":
                        back = true;
                        break;
                    default:
                        ShowMessage(AppConstants.INVALID_OPTION_MSG);
                        break;
                }
            }
        }

        private async Task ShowShipMenuAsync(IRepository repository)
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("--- Ship Management ---");
                Console.WriteLine("1. View All Ships");
                Console.WriteLine("2. Add Ship");
                Console.WriteLine("3. Back");
                Console.Write("Select an option: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        await ViewAllShipsAsync(repository);
                        break;
                    case "2":
                        await AddShipAsync(repository);
                        break;
                    case "3":
                        back = true;
                        break;
                    default:
                        ShowMessage(AppConstants.INVALID_OPTION_MSG);
                        break;
                }
            }
        }

        private async Task ViewAllManufacturersAsync(IRepository repository)
        {
            Console.Clear();
            Console.WriteLine("--- All Manufacturers ---");
            var manufacturers = await repository.GetAllManufacturersAsync();
            if (manufacturers.Any())
            {
                manufacturers.ForEach(m => Console.WriteLine($"ID: {m.Id}, Name: {m.Name}, Address: {m.Address}"));
            }
            else
            {
                Console.WriteLine("No manufacturers found.");
            }
            ShowMessage(AppConstants.PRESS_ANY_KEY_MSG);
        }

        private async Task AddManufacturerAsync(IRepository repository)
        {
            Console.Clear();
            Console.WriteLine("--- Add New Manufacturer ---");
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Address: ");
            string address = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(address))
            {
                ShowMessage(AppConstants.ALL_FIELDS_REQUIRED_MSG);
                return;
            }

            var manufacturer = Manufacturer.Create(name, address);
            await repository.AddManufacturerAsync(manufacturer);
            ShowMessage(AppConstants.OPERATION_SUCCESS_MSG);
        }

        private async Task UpdateManufacturerAsync(IRepository repository)
        {
            Console.Clear();
            Console.WriteLine("--- Update Manufacturer ---");
            int id = GetIntFromUser("Enter Manufacturer ID to update: ");
            if (id == -1) return;

            var manufacturer = await repository.GetManufacturerByIdAsync(id);
            if (manufacturer == null)
            {
                ShowMessage(AppConstants.NOT_FOUND_MSG);
                return;
            }

            Console.Write($"Enter new name (current: {manufacturer.Name}): ");
            string newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName))
            {
                manufacturer.Name = newName;
            }

            Console.Write($"Enter new address (current: {manufacturer.Address}): ");
            string newAddress = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newAddress))
            {
                manufacturer.Address = newAddress;
            }

            await repository.UpdateManufacturerAsync(manufacturer);
            ShowMessage(AppConstants.OPERATION_SUCCESS_MSG);
        }

        private async Task DeleteManufacturerAsync(IRepository repository)
        {
            Console.Clear();
            Console.WriteLine("--- Delete Manufacturer ---");
            int id = GetIntFromUser("Enter Manufacturer ID to delete: ");
            if (id == -1) return;

            await repository.DeleteManufacturerAsync(id);
            ShowMessage(AppConstants.OPERATION_SUCCESS_MSG);
        }

        private async Task ViewAllShipsAsync(IRepository repository)
        {
            Console.Clear();
            Console.WriteLine("--- All Ships ---");
            var ships = await repository.GetAllShipsAsync();
            if (ships.Any())
            {
                foreach (var ship in ships)
                {
                    string output = $"ID: {ship.Id}, Model: {ship.Model}, SN: {ship.SerialNumber}, Manufacturer: {ship.Manufacturer?.Name ?? "N/A"}";
                    output += ship switch
                    {
                        Battleship b => $", Type: Battleship, Guns: {b.MainCaliberGuns}",
                        Aircarrier a => $", Type: Aircarrier, Aircrafts: {a.AircraftCapacity}",
                        Cruiser c => $", Type: Cruiser, Missiles: {c.MissileCount}",
                        Destroyer d => $", Type: Destroyer, Torpedoes: {d.TorpedoTubes}",
                        Submarine s => $", Type: Submarine, Max Depth: {s.MaxDepth}m",
                        _ => ", Type: Unknown"
                    };
                    Console.WriteLine(output);
                }
            }
            else
            {
                Console.WriteLine("No ships found.");
            }
            ShowMessage(AppConstants.PRESS_ANY_KEY_MSG);
        }

        private async Task AddShipAsync(IRepository repository)
        {
            Console.Clear();
            Console.WriteLine("--- Add New Ship ---");
            var manufacturers = await repository.GetAllManufacturersAsync();
            if (!manufacturers.Any())
            {
                ShowMessage(AppConstants.ADD_MANUFACTURER_FIRST_MSG);
                return;
            }

            Console.WriteLine("Available Manufacturers:");
            manufacturers.ForEach(m => Console.WriteLine($"ID: {m.Id}, Name: {m.Name}"));
            int manufacturerId = GetIntFromUser("Select Manufacturer ID: ");
            if (manufacturerId == -1 || !manufacturers.Any(m => m.Id == manufacturerId))
            {
                ShowMessage(AppConstants.INVALID_ID_MSG);
                return;
            }

            Console.Write("Model: ");
            string model = Console.ReadLine();
            Console.Write("Serial Number: ");
            string serialNumber = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(model) || string.IsNullOrWhiteSpace(serialNumber))
            {
                ShowMessage(AppConstants.ALL_FIELDS_REQUIRED_MSG);
                return;
            }

            Ship ship = CreateSpecificShipFromInput();
            if (ship == null) return;

            ship.Model = model;
            ship.SerialNumber = serialNumber;
            ship.ManufacturerId = manufacturerId;

            await repository.AddShipAsync(ship);
            ShowMessage(AppConstants.OPERATION_SUCCESS_MSG);
        }

        private Ship CreateSpecificShipFromInput()
        {
            Console.WriteLine("Select Ship Type: 1.Battleship, 2.Aircarrier, 3.Cruiser, 4.Destroyer, 5.Submarine");
            Console.Write("Enter selection: ");
            string choice = Console.ReadLine();

            int spec = GetIntFromUser("Enter specification value (e.g., number of guns): ");
            if (spec == -1) return null;

            switch (choice)
            {
                case "1": return new Battleship { MainCaliberGuns = spec };
                case "2": return new Aircarrier { AircraftCapacity = spec };
                case "3": return new Cruiser { MissileCount = spec };
                case "4": return new Destroyer { TorpedoTubes = spec };
                case "5": return new Submarine { MaxDepth = spec };
                default:
                    ShowMessage(AppConstants.INVALID_OPTION_MSG);
                    return null;
            }
        }

        private int GetIntFromUser(string message)
        {
            Console.Write(message);
            if (int.TryParse(Console.ReadLine(), out int value) && value > 0)
            {
                return value;
            }
            ShowMessage(AppConstants.INVALID_NUMBER_MSG);
            return -1;
        }

        private void ShowMessage(string message)
        {
            Console.WriteLine(message);
            Console.ReadKey();
        }
    }
}
