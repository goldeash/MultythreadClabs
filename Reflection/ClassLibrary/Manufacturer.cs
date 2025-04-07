namespace Laba1Task2and3
{
    public class Manufacturer
    {
        public string Name;
        public string Address;

        public static Manufacturer Create(string name, string address)
        {
            return new Manufacturer { Name = name, Address = address };
        }

        public void PrintObject()
        {
            Console.WriteLine($"Manufacturer: Name = {Name}, Address = {Address}");
        }
    }
}