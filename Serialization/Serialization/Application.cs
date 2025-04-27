using XmlSerializationDemo.Models;
using XmlSerializationDemo.Services;

namespace XmlSerializationDemo
{
    public class Application
    {
        private readonly VehicleCollection _vehicleCollection = new();
        private readonly IVehicleXmlSerializer _serializer = new VehicleXmlSerializer();
        private const string DefaultFilePath = "vehicles.xml";

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
            Console.WriteLine("0. Exit");
            Console.Write("Enter your choice: ");
        }

        private void GenerateAndDisplayVehicles()
        {
            _vehicleCollection.Clear();
            var vehicles = VehicleGenerator.GenerateVehicles(10);
            _vehicleCollection.AddRange(vehicles);
            VehiclePrinter.Print(vehicles);
        }

        private void SerializeToXml()
        {
            if (_vehicleCollection.GetAll().Any())
            {
                _serializer.SerializeToFile(DefaultFilePath, _vehicleCollection.GetAll());
                Console.WriteLine($"Vehicles serialized to {DefaultFilePath}");
            }
            else
            {
                Console.WriteLine("No vehicles to serialize. Generate some vehicles first.");
            }
        }

        private void PrintXmlFileContent()
        {
            _serializer.PrintFileContent(DefaultFilePath);
        }

        private void DeserializeAndDisplay()
        {
            try
            {
                var vehicles = _serializer.DeserializeFromFile(DefaultFilePath);
                _vehicleCollection.Clear();
                _vehicleCollection.AddRange(vehicles);
                VehiclePrinter.Print(vehicles);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during deserialization: {ex.Message}");
            }
        }

        private void PrintAllModelsWithXDocument()
        {
            _serializer.PrintAllModelElementsWithXDocument(DefaultFilePath);
        }

        private void PrintAllModelsWithXmlDocument()
        {
            _serializer.PrintAllModelElementsWithXmlDocument(DefaultFilePath);
        }

        private void UpdateElementWithXDocument()
        {
            UpdateElement(true);
        }

        private void UpdateElementWithXmlDocument()
        {
            UpdateElement(false);
        }

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
    }
}