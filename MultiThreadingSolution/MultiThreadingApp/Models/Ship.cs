namespace MultiThreadingApp.Models
{
    public class Ship
    {
        public int ID { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public ShipType ShipType { get; set; }

        public static Ship Create(int id, string model, string serialNumber, ShipType shipType)
        {
            return new Ship { ID = id, Model = model, SerialNumber = serialNumber, ShipType = shipType };
        }

        public void PrintObject()
        {
            Console.WriteLine($"Ship: ID = {ID}, Model = {Model}, SerialNumber = {SerialNumber}, ShipType = {ShipType}");
        }
    }
}