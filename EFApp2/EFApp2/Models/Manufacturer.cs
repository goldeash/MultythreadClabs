namespace EFApp.Models
{
    /// <summary>
    /// Represents a manufacturer entity.
    /// </summary>
    public class Manufacturer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        /// <summary>
        /// Creates a new instance of the Manufacturer class.
        /// </summary>
        /// <param name="name">The name of the manufacturer.</param>
        /// <param name="address">The address of the manufacturer.</param>
        /// <returns>A new Manufacturer object.</returns>
        public static Manufacturer Create(string name, string address)
        {
            return new Manufacturer
            {
                Name = name,
                Address = address
            };
        }
    }
}
