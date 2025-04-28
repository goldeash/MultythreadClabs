using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using XmlSerializationDemo.Models;

namespace XmlSerializationDemo.Services
{
    public class VehicleXmlSerializer : IVehicleXmlSerializer
    {
        public void SerializeToFile(string filePath, IEnumerable<Vehicle> vehicles)
        {
            ArgumentNullException.ThrowIfNull(filePath);
            ArgumentNullException.ThrowIfNull(vehicles);

            var serializer = new XmlSerializer(typeof(List<Vehicle>));
            using var writer = new StreamWriter(filePath);
            serializer.Serialize(writer, vehicles.ToList());
        }

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

        public void PrintFileContent(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File does not exist.");
                return;
            }

            Console.WriteLine($"Content of {filePath}:");
            Console.WriteLine(File.ReadAllText(filePath));
        }

        public void PrintAllModelElementsWithXDocument(string filePath)
        {
            var doc = XDocument.Load(filePath);
            var models = doc.Descendants("Vehicle").Select(v => (string)v.Attribute("Model"));

            Console.WriteLine("All Model attributes (using XDocument):");
            foreach (var model in models)
            {
                Console.WriteLine(model);
            }
        }

        public void PrintAllModelElementsWithXmlDocument(string filePath)
        {
            var doc = new XmlDocument();
            doc.Load(filePath);
            var nodes = doc.SelectNodes("//Vehicle");

            Console.WriteLine("All Model attributes (using XmlDocument):");
            foreach (XmlNode node in nodes)
            {
                if (node.Attributes != null)
                {
                    Console.WriteLine(node.Attributes["Model"]?.InnerText);
                }
            }
        }

        public void UpdateElementWithXDocument(string filePath, string elementName, int elementIndex, string newValue)
        {
            var doc = XDocument.Load(filePath);
            var vehicles = doc.Descendants("Vehicle").ToList();

            if (elementIndex < 0 || elementIndex >= vehicles.Count)
            {
                Console.WriteLine("Invalid element index.");
                return;
            }

            var element = vehicles[elementIndex];
            var attribute = element.Attribute(elementName);

            if (attribute != null)
            {
                attribute.Value = newValue;
                doc.Save(filePath);
                Console.WriteLine("Attribute updated successfully using XDocument.");
            }
            else
            {
                Console.WriteLine($"Attribute {elementName} not found.");
            }
        }

        public void UpdateElementWithXmlDocument(string filePath, string elementName, int elementIndex, string newValue)
        {
            var doc = new XmlDocument();
            doc.Load(filePath);
            var nodes = doc.SelectNodes("//Vehicle");

            if (nodes == null || elementIndex < 0 || elementIndex >= nodes.Count)
            {
                Console.WriteLine("Invalid element index.");
                return;
            }

            var node = nodes[elementIndex];
            if (node.Attributes?[elementName] != null)
            {
                node.Attributes[elementName].Value = newValue;
                doc.Save(filePath);
                Console.WriteLine("Attribute updated successfully using XmlDocument.");
            }
            else
            {
                Console.WriteLine($"Attribute {elementName} not found.");
            }
        }
    }
}

// вынести стринги. убрать несколько методов в 1 строке. убрать лишние проверки elementIndex >= vehicles.Count так как этого никогда не произойдёт. добавить коменты xml формата. убрать клир. добавить для пользователя вариант выбора где он сам очищает