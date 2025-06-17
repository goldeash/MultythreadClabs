namespace EFApp.Models
{
    /// <summary>
    /// Represents a ship with basic properties.
    /// </summary>
    public class Ship
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public ShipType ShipType { get; set; }
        public int ManufacturerId { get; set; }

        /// <summary>
        /// Creates a new Ship instance with specified parameters.
        /// </summary>
        public static Ship Create(string model, string serialNumber, ShipType shipType, int manufacturerId)
        {
            return new Ship
            {
                Model = model,
                SerialNumber = serialNumber,
                ShipType = shipType,
                ManufacturerId = manufacturerId
            };
        }

        public override string ToString()
        {
            return $"Ship: {Model} ({SerialNumber}), Type: {ShipType}, Manufacturer ID: {ManufacturerId}";
        }
    }
}