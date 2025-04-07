namespace XmlSerializationDemo.Models
{
    public class Vehicle
    {
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }

        public override string ToString()
        {
            return $"{Manufacturer} {Model} ({Year}), Color: {Color}";
        }
    }
}