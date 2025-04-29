using System.Xml.Serialization;

namespace XmlSerializationDemo.Models
{
    /// <summary>
    /// Represents a vehicle with basic properties.
    /// </summary>
    public class Vehicle
    {
        [XmlAttribute]
        public string Model { get; set; }

        [XmlAttribute]
        public string Manufacturer { get; set; }

        [XmlAttribute]
        public int Year { get; set; }

        [XmlAttribute]
        public string Color { get; set; }

        public override string ToString()
        {
            return $"{Manufacturer} {Model} ({Year}), Color: {Color}";
        }
    }
}