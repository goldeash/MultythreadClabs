using ADOApp.Models;

namespace ADOApp.Services
{
    public interface IDatabaseService
    {
        /// <summary>
        /// Creates database tables if they don't exist
        /// </summary>
        /// <returns>Task</returns>
        Task InitializeDatabaseAsync();

        /// <summary>
        /// Adds a manufacturer to the database
        /// </summary>
        /// <param name="manufacturer">manufacturer</param>
        /// <returns>Task<int></returns>
        Task<int> AddManufacturerAsync(Manufacturer manufacturer);

        /// <summary>
        /// Adds a ship to the database
        /// </summary>
        /// <param name="ship">ship</param>
        /// <returns>Task</returns>
        Task AddShipAsync(Ship ship);

        /// <summary>
        /// Gets all ships for a specific manufacturer
        /// </summary>
        /// <param name="manufacturerId">manufacturerId</param>
        /// <returns>Task<List<Ship>></returns>
        Task<List<Ship>> GetShipsByManufacturerAsync(int manufacturerId);

        /// <summary>
        /// Gets all manufacturers from the database
        /// </summary>
        /// <returns>Task<List<Manufacturer>></returns>
        Task<List<Manufacturer>> GetAllManufacturersAsync();

        /// <summary>
        /// Populates database with sample data (30 manufacturers and 30 ships)
        /// </summary>
        /// <returns>Task</returns>
        Task PopulateDatabaseAsync();
    }
}
