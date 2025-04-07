namespace XmlSerializationDemo.Models
{
    public class VehicleCollection
    {
        private readonly List<Vehicle> _vehicles = new List<Vehicle>();

        public void Add(Vehicle vehicle) => _vehicles.Add(vehicle);
        public void AddRange(IEnumerable<Vehicle> vehicles) => _vehicles.AddRange(vehicles);
        public void Clear() => _vehicles.Clear();
        public IEnumerable<Vehicle> GetAll() => _vehicles;

        public void PrintAll()
        {
            if (_vehicles.Count == 0)
            {
                Console.WriteLine("No vehicles in collection.");
                return;
            }

            Console.WriteLine("Vehicle list:");
            for (int i = 0; i < _vehicles.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_vehicles[i]}");
            }
        }
    }
}