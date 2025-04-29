using XmlSerializationDemo.Models;

namespace XmlSerializationDemo.Services
{
    /// <summary>
    /// Provides methods to print Vehicle information.
    /// </summary>
    public static class VehiclePrinter
    {
        /// <summary>
        /// Prints a list of vehicles to the console.
        /// </summary>
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
