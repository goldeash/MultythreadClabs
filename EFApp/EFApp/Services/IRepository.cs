using EFApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EFApp.Services
{
    /// <summary>
    /// Interface for the Repository
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Adds a manufacturer to the database
        /// </summary>
        /// <param name="manufacturer">Manufacturer</param>
        /// <returns>Task<int></returns>
        Task<int> AddManufacturerAsync(Manufacturer manufacturer);

        /// <summary>
        /// Adds a ship to the database
        /// </summary>
        /// <param name="ship">Ship</param>
        /// <returns>Task</returns>
        Task AddShipAsync(Ship ship);

        /// <summary>
        /// Gets all manufacturers from the database
        /// </summary>
        /// <returns>Task<List<Manufacturer>></returns>
        Task<List<Manufacturer>> GetAllManufacturersAsync();

        /// <summary>
        /// Gets all ships for a specific manufacturer
        /// </summary>
        /// <param name="manufacturerId">Manufacturer ID</param>
        /// <returns>Task<List<Ship>></returns>
        Task<List<Ship>> GetShipsByManufacturerAsync(int manufacturerId);

        /// <summary>
        /// Gets a manufacturer by ID
        /// </summary>
        /// <param name="id">Manufacturer ID</param>
        /// <returns>Task<Manufacturer></returns>
        Task<Manufacturer> GetManufacturerByIdAsync(int id);

        /// <summary>
        /// Updates a manufacturer
        /// </summary>
        /// <param name="manufacturer">Manufacturer</param>
        /// <returns>Task</returns>
        Task UpdateManufacturerAsync(Manufacturer manufacturer);

        /// <summary>
        /// Deletes a manufacturer
        /// </summary>
        /// <param name="id">Manufacturer ID</param>
        /// <returns>Task</returns>
        Task DeleteManufacturerAsync(int id);

        /// <summary>
        /// Gets a ship by ID
        /// </summary>
        /// <param name="id">Ship ID</param>
        /// <returns>Task<Ship></returns>
        Task<Ship> GetShipByIdAsync(int id);

        /// <summary>
        /// Updates a ship
        /// </summary>
        /// <param name="ship">Ship</param>
        /// <returns>Task</returns>
        Task UpdateShipAsync(Ship ship);

        /// <summary>
        /// Deletes a ship
        /// </summary>
        /// <param name="id">Ship ID</param>
        /// <returns>Task</returns>
        Task DeleteShipAsync(int id);

        /// <summary>
        /// Adds a new product for a new manufacturer with transaction
        /// </summary>
        /// <param name="manufacturer">Manufacturer</param>
        /// <param name="ship">Ship</param>
        /// <returns>Task<bool></returns>
        Task<bool> AddProductForNewManufacturerAsync(Manufacturer manufacturer, Ship ship);
    }
}