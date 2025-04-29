using XmlSerializationDemo.Models;

namespace XmlSerializationDemo.Services
{
    /// <summary>
    /// Defines methods for serializing and deserializing vehicle data to and from XML.
    /// </summary>
    public interface IVehicleXmlSerializer
    {
        /// <summary>
        /// Serializes the given vehicle list to an XML file.
        /// </summary>
        /// <param name="filePath">The file path to save the XML.</param>
        /// <param name="vehicles">The vehicles to serialize.</param>
        void SerializeToFile(string filePath, IEnumerable<Vehicle> vehicles);

        /// <summary>
        /// Deserializes vehicles from the specified XML file.
        /// </summary>
        /// <param name="filePath">The file path of the XML file.</param>
        /// <returns>List of deserialized vehicles.</returns>
        List<Vehicle> DeserializeFromFile(string filePath);

        /// <summary>
        /// Prints the raw XML content of the file.
        /// </summary>
        /// <param name="filePath">The file path of the XML file.</param>
        void PrintFileContent(string filePath);

        /// <summary>
        /// Prints all "Model" attributes using LINQ to XML (XDocument).
        /// </summary>
        /// <param name="filePath">The file path of the XML file.</param>
        void PrintAllModelElementsWithXDocument(string filePath);

        /// <summary>
        /// Prints all "Model" attributes using XmlDocument.
        /// </summary>
        /// <param name="filePath">The file path of the XML file.</param>
        void PrintAllModelElementsWithXmlDocument(string filePath);

        /// <summary>
        /// Updates an attribute in a specific element using XDocument.
        /// </summary>
        /// <param name="filePath">The file path of the XML file.</param>
        /// <param name="elementName">The attribute name to update.</param>
        /// <param name="elementIndex">The index of the element.</param>
        /// <param name="newValue">The new value to assign.</param>
        void UpdateElementWithXDocument(string filePath, string elementName, int elementIndex, string newValue);

        /// <summary>
        /// Updates an attribute in a specific element using XmlDocument.
        /// </summary>
        /// <param name="filePath">The file path of the XML file.</param>
        /// <param name="elementName">The attribute name to update.</param>
        /// <param name="elementIndex">The index of the element.</param>
        /// <param name="newValue">The new value to assign.</param>
        void UpdateElementWithXmlDocument(string filePath, string elementName, int elementIndex, string newValue);
    }
}
