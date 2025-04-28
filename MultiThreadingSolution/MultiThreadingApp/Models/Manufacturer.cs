using System;

namespace MultiThreadingApp.Models
{
    public class Manufacturer
    {
        public string Name { get; set; }
        public string Address { get; set; }

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