namespace TplApp.Models
{
    /// <summary>
    /// Represents a ship with basic properties.
    /// </summary>
    public class Ship
    {
        public int ID { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public ShipType ShipType { get; set; }

        /// <summary>
        /// Creates a new Ship instance with specified parameters.
        /// </summary>
        public static Ship Create(int id, string model, string serialNumber, ShipType shipType)
        {
            return new Ship
            {
                ID = id,
                Model = model,
                SerialNumber = serialNumber,
                ShipType = shipType
            };
        }

        /// <summary>
        /// Prints ship details to the console.
        /// </summary>
        public void PrintObject()
        {
            Console.WriteLine($"Ship: ID = {ID}, Model = {Model}, SerialNumber = {SerialNumber}, ShipType = {ShipType}");
        }

        public override string ToString()
        {
            return $"Ship {ID}: {Model} ({SerialNumber}), Type: {ShipType}";
        }
    }
}