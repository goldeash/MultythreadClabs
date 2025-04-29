using XmlSerializationDemo.Models;

namespace XmlSerializationDemo.Services
{
    /// <summary>
    /// Defines methods for XML serialization and manipulation of Vehicle objects.
    /// </summary>
    public interface IVehicleXmlSerializer
    {
        void SerializeToFile(string filePath, IEnumerable<Vehicle> vehicles);
        List<Vehicle> DeserializeFromFile(string filePath);
        void PrintFileContent(string filePath);
        void PrintAllModelElementsWithXDocument(string filePath);
        void PrintAllModelElementsWithXmlDocument(string filePath);
        void UpdateElementWithXDocument(string filePath, string attributeName, int elementIndex, string newValue);
        void UpdateElementWithXmlDocument(string filePath, string attributeName, int elementIndex, string newValue);
    }
}
