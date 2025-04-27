using XmlSerializationDemo.Models;

namespace XmlSerializationDemo.Services
{
    public static class VehiclePrinter
    {
        public static void Print(IEnumerable<Vehicle> vehicles)
        {
            if (!vehicles.Any())
            {
                Console.WriteLine("No vehicles to display.");
                return;
            }

            Console.WriteLine("Vehicle list:");
            int index = 1;
            foreach (var vehicle in vehicles)
            {
                Console.WriteLine($"{index++}. {vehicle}");
            }
        }
    }
}