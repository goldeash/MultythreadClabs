using XmlSerializationDemo.Models;

namespace XmlSerializationDemo.Services
{
    public interface IVehicleXmlSerializer
    {
        void SerializeToFile(string filePath, IEnumerable<Vehicle> vehicles);
        List<Vehicle> DeserializeFromFile(string filePath);
        void PrintFileContent(string filePath);
        void PrintAllModelElementsWithXDocument(string filePath);
        void PrintAllModelElementsWithXmlDocument(string filePath);
        void UpdateElementWithXDocument(string filePath, string elementName, int elementIndex, string newValue);
        void UpdateElementWithXmlDocument(string filePath, string elementName, int elementIndex, string newValue);
    }
}