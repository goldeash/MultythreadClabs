using XmlSerializationDemo.Models;

namespace XmlSerializationDemo.Services
{
    public interface IVehicleXmlSerializer
    {
        void SerializeToFile(string filePath, IEnumerable<Vehicle> vehicles);
        List<Vehicle> DeserializeFromFile(string filePath);
        void PrintFileContent(string filePath);
        void PrintAllModelAttributesWithXDocument(string filePath);
        void PrintAllModelAttributesWithXmlDocument(string filePath);
        void UpdateAttributeWithXDocument(string filePath, string attributeName, int elementIndex, string newValue);
        void UpdateAttributeWithXmlDocument(string filePath, string attributeName, int elementIndex, string newValue);
    }
}