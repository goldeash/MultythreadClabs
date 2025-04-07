namespace Laba1Task2and3
{
    public class Ship
    {
        private int ID;
        public string Model;
        public string SerialNumber;
        public ShipType ShipType;

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