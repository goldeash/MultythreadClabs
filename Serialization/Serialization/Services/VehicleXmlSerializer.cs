using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using XmlSerializationDemo.Models;

namespace XmlSerializationDemo.Services
{
    /// <summary>
    /// Implements XML serialization and manipulation methods for Vehicle objects.
    /// </summary>
    public class VehicleXmlSerializer : IVehicleXmlSerializer
    {
        private const string VehicleElementName = "Vehicle";
        private const string ModelAttributeName = "Model";

        /// <summary>
        /// Serializes a collection of vehicles to an XML file.
        /// </summary>
        public void SerializeToFile(string filePath, IEnumerable<Vehicle> vehicles)
        {
            ArgumentNullException.ThrowIfNull(filePath);
            ArgumentNullException.ThrowIfNull(vehicles);

            var serializer = new XmlSerializer(typeof(List<Vehicle>));

            using var writer = new StreamWriter(filePath);
            serializer.Serialize(writer, vehicles.ToList());
        }

        /// <summary>
        /// Deserializes a list of vehicles from an XML file.
        /// </summary>
        public List<Vehicle> DeserializeFromFile(string filePath)
        {
            ArgumentNullException.ThrowIfNull(filePath);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found.", filePath);
            }

            var serializer = new XmlSerializer(typeof(List<Vehicle>));

            using var reader = new StreamReader(filePath);
            return (List<Vehicle>)serializer.Deserialize(reader);
        }

        /// <summary>
        /// Prints the contents of an XML file to the console.
        /// </summary>
        public void PrintFileContent(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File does not exist.");
                return;
            }

            Console.WriteLine($"Content of {filePath}:");

            string content = File.ReadAllText(filePath);
            Console.WriteLine(content);
        }

        /// <summary>
        /// Prints all Model attribute values using LINQ to XML (XDocument).
        /// </summary>
        public void PrintAllModelElementsWithXDocument(string filePath)
        {
            var document = XDocument.Load(filePath);

            var models = document
                .Descendants(VehicleElementName)
                .Select(element => (string)element.Attribute(ModelAttributeName));

            Console.WriteLine("All Model attributes (using XDocument):");

            foreach (var model in models)
            {
                Console.WriteLine(model);
            }
        }

        /// <summary>
        /// Prints all Model attribute values using XmlDocument.
        /// </summary>
        public void PrintAllModelElementsWithXmlDocument(string filePath)
        {
            var document = new XmlDocument();
            document.Load(filePath);

            XmlNodeList nodes = document.SelectNodes($"//{VehicleElementName}");

            Console.WriteLine("All Model attributes (using XmlDocument):");

            foreach (XmlNode node in nodes)
            {
                if (node?.Attributes != null)
                {
                    Console.WriteLine(node.Attributes[ModelAttributeName]?.InnerText);
                }
            }
        }

        /// <summary>
        /// Updates a specific attribute of a vehicle using XDocument.
        /// </summary>
        public void UpdateElementWithXDocument(string filePath, string attributeName, int elementIndex, string newValue)
        {
            var document = XDocument.Load(filePath);

            var vehicles = document
                .Descendants(VehicleElementName)
                .ToList();

            if (elementIndex < 0)
            {
                Console.WriteLine("Invalid element index.");
                return;
            }

            var element = vehicles[elementIndex];
            var attribute = element.Attribute(attributeName);

            if (attribute != null)
            {
                attribute.Value = newValue;
                document.Save(filePath);
                Console.WriteLine("Attribute updated successfully using XDocument.");
            }
            else
            {
                Console.WriteLine($"Attribute '{attributeName}' not found.");
            }
        }

        /// <summary>
        /// Updates a specific attribute of a vehicle using XmlDocument.
        /// </summary>
        public void UpdateElementWithXmlDocument(string filePath, string attributeName, int elementIndex, string newValue)
        {
            var document = new XmlDocument();
            document.Load(filePath);

            XmlNodeList nodes = document.SelectNodes($"//{VehicleElementName}");

            if (nodes == null || elementIndex < 0)
            {
                Console.WriteLine("Invalid element index.");
                return;
            }

            var node = nodes[elementIndex];

            if (node?.Attributes?[attributeName] != null)
            {
                node.Attributes[attributeName].Value = newValue;
                document.Save(filePath);
                Console.WriteLine("Attribute updated successfully using XmlDocument.");
            }
            else
            {
                Console.WriteLine($"Attribute '{attributeName}' not found.");
            }
        }
    }
}