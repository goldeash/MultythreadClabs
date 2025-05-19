namespace ConcurrencyApp.Models
{
    /// <summary>
    /// Represents a manufacturer with basic properties.
    /// </summary>
    public class Manufacturer
    {
        public string Name { get; set; }
        public string Address { get; set; }

        /// <summary>
        /// Creates a new Manufacturer instance with specified parameters.
        /// </summary>
        public static Manufacturer Create(string name, string address)
        {
            return new Manufacturer
            {
                Name = name,
                Address = address
            };
        }

        public override string ToString()
        {
            return $"Manufacturer: {Name}, Address: {Address}";
        }
    }
}