using XmlSerializationDemo.Models;

namespace XmlSerializationDemo.Services
{
    public static class VehicleGenerator
    {
        private static readonly Random Random = new Random();
        private static readonly string[] Manufacturers = { "Toyota", "Ford", "Honda", "BMW", "Mercedes", "Audi", "Volkswagen", "Tesla" };
        private static readonly string[] Models = { "Camry", "Focus", "Civic", "X5", "E-Class", "A4", "Golf", "Model S" };
        private static readonly string[] Colors = { "Red", "Blue", "Black", "White", "Silver", "Green", "Yellow" };

        public static List<Vehicle> GenerateVehicles(int count)
        {
            var vehicles = new List<Vehicle>();

            for (int i = 0; i < count; i++)
            {
                vehicles.Add(new Vehicle
                {
                    Manufacturer = Manufacturers[Random.Next(Manufacturers.Length)],
                    Model = Models[Random.Next(Models.Length)],
                    Year = Random.Next(2000, 2023),
                    Color = Colors[Random.Next(Colors.Length)]
                });
            }

            return vehicles;
        }
    }
}