namespace XmlSerializationDemo.Models
{
    /// <summary>
    /// Manages a collection of Vehicle objects.
    /// </summary>
    public class VehicleCollection
    {
        private readonly List<Vehicle> _vehicles = new();

        /// <summary>
        /// Adds a single vehicle to the collection.
        /// </summary>
        public void Add(Vehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle);
            _vehicles.Add(vehicle);
        }

        /// <summary>
        /// Adds multiple vehicles to the collection.
        /// </summary>
        public void AddRange(IEnumerable<Vehicle> vehicles)
        {
            ArgumentNullException.ThrowIfNull(vehicles);
            _vehicles.AddRange(vehicles);
        }

        /// <summary>
        /// Clears all vehicles from the collection.
        /// </summary>
        public void Clear() => _vehicles.Clear();

        /// <summary>
        /// Retrieves all vehicles in the collection.
        /// </summary>
        public IEnumerable<Vehicle> GetAll() => _vehicles;
    }
}
