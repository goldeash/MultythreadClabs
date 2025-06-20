namespace EFApp.Models
{
    /// <summary>
    /// Represents the abstract base class for all ship types.
    /// </summary>
    public abstract class Ship
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public int ManufacturerId { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
    }
}
