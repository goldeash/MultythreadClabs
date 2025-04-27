namespace XmlSerializationDemo.Models
{
    public class VehicleCollection
    {
        private readonly List<Vehicle> _vehicles = new();

        public void Add(Vehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle);
            _vehicles.Add(vehicle);
        }

        public void AddRange(IEnumerable<Vehicle> vehicles)
        {
            ArgumentNullException.ThrowIfNull(vehicles);
            _vehicles.AddRange(vehicles);
        }

        public void Clear() => _vehicles.Clear();

        public IEnumerable<Vehicle> GetAll() => _vehicles;
    }
}