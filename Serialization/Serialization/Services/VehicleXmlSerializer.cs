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
            var serializer = new XmlSerializer(typeof(List<Vehicle>));
            using var writer = new StreamWriter(filePath);
            serializer.Serialize(writer, new List<Vehicle>(vehicles));
        }

        public List<Vehicle> DeserializeFromFile(string filePath)
        {
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

        public void PrintAllModelAttributesWithXDocument(string filePath)
        {
            var doc = XDocument.Load(filePath);
            var models = doc.Descendants("Vehicle").Select(v => (string)v.Element("Model"));

            Console.WriteLine("All Model attributes (using XDocument):");
            foreach (var model in models)
            {
                Console.WriteLine(model);
            }
        }

        public void PrintAllModelAttributesWithXmlDocument(string filePath)
        {
            var doc = new XmlDocument();
            doc.Load(filePath);
            var nodes = doc.SelectNodes("//Vehicle/Model");

            Console.WriteLine("All Model attributes (using XmlDocument):");
            foreach (XmlNode node in nodes)
            {
                Console.WriteLine(node.InnerText);
            }
        }

        public void UpdateAttributeWithXDocument(string filePath, string attributeName, int elementIndex, string newValue)
        {
            var doc = XDocument.Load(filePath);
            var element = doc.Descendants("Vehicle").ElementAt(elementIndex);
            var attributeElement = element.Element(attributeName);

            if (attributeElement != null)
            {
                attributeElement.Value = newValue;
                doc.Save(filePath);
                Console.WriteLine("Attribute updated successfully using XDocument.");
            }
            else
            {
                Console.WriteLine($"Attribute {attributeName} not found in element {elementIndex}.");
            }
        }

        public void UpdateAttributeWithXmlDocument(string filePath, string attributeName, int elementIndex, string newValue)
        {
            var doc = new XmlDocument();
            doc.Load(filePath);
            var nodes = doc.SelectNodes("//Vehicle");

            if (elementIndex >= 0 && elementIndex < nodes.Count)
            {
                var element = nodes[elementIndex];
                var attributeNode = element.SelectSingleNode(attributeName);

                if (attributeNode != null)
                {
                    attributeNode.InnerText = newValue;
                    doc.Save(filePath);
                    Console.WriteLine("Attribute updated successfully using XmlDocument.");
                }
                else
                {
                    Console.WriteLine($"Attribute {attributeName} not found in element {elementIndex}.");
                }
            }
            else
            {
                Console.WriteLine($"Element index {elementIndex} is out of range.");
            }
        }
    }
}