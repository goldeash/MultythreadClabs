using XmlSerializationDemo.Models;
using XmlSerializationDemo.Services;

namespace XmlSerializationDemo
{
    /// <summary>
    /// Main application class handling user interaction and orchestration.
    /// </summary>
    public class Application
    {
        private readonly VehicleCollection _vehicleCollection = new();
        private readonly IVehicleXmlSerializer _serializer = new VehicleXmlSerializer();
        private const string DefaultFilePath = "vehicles.xml";

        /// <summary>
        /// Starts the application loop.
        /// </summary>
        public void Run()
        {
            while (true)
            {
                DisplayMenu();
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        GenerateAndDisplayVehicles();
                        break;
                    case "2":
                        SerializeToXml();
                        break;
                    case "3":
                        PrintXmlFileContent();
                        break;
                    case "4":
                        DeserializeAndDisplay();
                        break;
                    case "5":
                        PrintAllModelsWithXDocument();
                        break;
                    case "6":
                        PrintAllModelsWithXmlDocument();
                        break;
                    case "7":
                        UpdateElementWithXDocument();
                        break;
                    case "8":
                        UpdateElementWithXmlDocument();
                        break;
                    case "9":
                        ClearVehicleList();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Displays the main menu options.
        /// </summary>
        private static void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("XML Serialization Demo");
            Console.WriteLine("1. Generate and display 10 vehicles");
            Console.WriteLine("2. Serialize vehicles to XML file");
            Console.WriteLine("3. Print XML file content");
            Console.WriteLine("4. Deserialize from XML and display");
            Console.WriteLine("5. Print all Model attributes (XDocument)");
            Console.WriteLine("6. Print all Model attributes (XmlDocument)");
            Console.WriteLine("7. Update attribute (XDocument)");
            Console.WriteLine("8. Update attribute (XmlDocument)");
            Console.WriteLine("9. Clear current vehicle list");
            Console.WriteLine("0. Exit");
            Console.Write("Enter your choice: ");
        }

        /// <summary>
        /// Generates and displays a list of 10 random vehicles.
        /// </summary>
        private void GenerateAndDisplayVehicles()
        {
            var vehicles = VehicleGenerator.GenerateVehicles(10);
            _vehicleCollection.AddRange(vehicles);
            VehiclePrinter.Print(vehicles);
        }

        /// <summary>
        /// Serializes the current vehicle list to an XML file.
        /// </summary>
        private void SerializeToXml()
        {
            var vehicles = _vehicleCollection.GetAll();

            if (vehicles.Any())
            {
                _serializer.SerializeToFile(DefaultFilePath, vehicles);
                Console.WriteLine($"Vehicles serialized to {DefaultFilePath}");
            }
            else
            {
                Console.WriteLine("No vehicles to serialize. Generate or load vehicles first.");
            }
        }

        /// <summary>
        /// Prints the contents of the XML file.
        /// </summary>
        private void PrintXmlFileContent()
        {
            _serializer.PrintFileContent(DefaultFilePath);
        }

        /// <summary>
        /// Deserializes vehicles from an XML file and displays them.
        /// </summary>
        private void DeserializeAndDisplay()
        {
            try
            {
                var vehicles = _serializer.DeserializeFromFile(DefaultFilePath);
                _vehicleCollection.AddRange(vehicles);
                VehiclePrinter.Print(vehicles);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during deserialization: {ex.Message}");
            }
        }

        /// <summary>
        /// Prints all model values using XDocument.
        /// </summary>
        private void PrintAllModelsWithXDocument()
        {
            _serializer.PrintAllModelElementsWithXDocument(DefaultFilePath);
        }

        /// <summary>
        /// Prints all model values using XmlDocument.
        /// </summary>
        private void PrintAllModelsWithXmlDocument()
        {
            _serializer.PrintAllModelElementsWithXmlDocument(DefaultFilePath);
        }

        /// <summary>
        /// Updates an XML attribute using XDocument.
        /// </summary>
        private void UpdateElementWithXDocument()
        {
            UpdateElement(useXDocument: true);
        }

        /// <summary>
        /// Updates an XML attribute using XmlDocument.
        /// </summary>
        private void UpdateElementWithXmlDocument()
        {
            UpdateElement(useXDocument: false);
        }

        /// <summary>
        /// Shared method to update XML attribute using specified approach.
        /// </summary>
        private void UpdateElement(bool useXDocument)
        {
            try
            {
                Console.Write("Enter attribute name (Model/Manufacturer/Year/Color): ");
                var attributeName = Console.ReadLine();

                Console.Write("Enter element index (0-based): ");
                if (!int.TryParse(Console.ReadLine(), out int index))
                {
                    Console.WriteLine("Invalid index.");
                    return;
                }

                Console.Write("Enter new value: ");
                var newValue = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(attributeName) || newValue == null)
                {
                    Console.WriteLine("Invalid input.");
                    return;
                }

                if (useXDocument)
                {
                    _serializer.UpdateElementWithXDocument(DefaultFilePath, attributeName, index, newValue);
                }
                else
                {
                    _serializer.UpdateElementWithXmlDocument(DefaultFilePath, attributeName, index, newValue);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Allows the user to clear the current vehicle list manually.
        /// </summary>
        private void ClearVehicleList()
        {
            Console.Write("Are you sure you want to clear the vehicle list? (y/n): ");
            var confirmation = Console.ReadLine();

            if (confirmation?.Trim().ToLower() == "y")
            {
                _vehicleCollection.Clear();
                Console.WriteLine("Vehicle list cleared.");
            }
            else
            {
                Console.WriteLine("Clear operation cancelled.");
            }
        }
    }
}
