namespace EFApp.Models
{
    /// <summary>
    /// Represents a Battleship, inheriting from Ship.
    /// </summary>
    public class Battleship : Ship
    {
        public int MainCaliberGuns { get; set; }
    }

    /// <summary>
    /// Represents an Aircraft Carrier, inheriting from Ship.
    /// </summary>
    public class Aircarrier : Ship
    {
        public int AircraftCapacity { get; set; }
    }

    /// <summary>
    /// Represents a Cruiser, inheriting from Ship.
    /// </summary>
    public class Cruiser : Ship
    {
        public int MissileCount { get; set; }
    }

    /// <summary>
    /// Represents a Destroyer, inheriting from Ship.
    /// </summary>
    public class Destroyer : Ship
    {
        public int TorpedoTubes { get; set; }
    }

    /// <summary>
    /// Represents a Submarine, inheriting from Ship.
    /// </summary>
    public class Submarine : Ship
    {
        public int MaxDepth { get; set; }
    }
}
