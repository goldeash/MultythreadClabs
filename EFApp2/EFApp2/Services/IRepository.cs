using EFApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EFApp.Services
{
    /// <summary>
    /// Defines the contract for repository operations.
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Adds a new manufacturer.
        /// </summary>
        /// <param name="manufacturer">The manufacturer to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddManufacturerAsync(Manufacturer manufacturer);

        /// <summary>
        /// Gets a manufacturer by its ID.
        /// </summary>
        /// <param name="id">The ID of the manufacturer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the manufacturer or null.</returns>
        Task<Manufacturer> GetManufacturerByIdAsync(int id);

        /// <summary>
        /// Gets all manufacturers.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of manufacturers.</returns>
        Task<List<Manufacturer>> GetAllManufacturersAsync();

        /// <summary>
        /// Updates an existing manufacturer.
        /// </summary>
        /// <param name="manufacturer">The manufacturer to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateManufacturerAsync(Manufacturer manufacturer);

        /// <summary>
        /// Deletes a manufacturer by its ID.
        /// </summary>
        /// <param name="id">The ID of the manufacturer to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteManufacturerAsync(int id);

        /// <summary>
        /// Adds a new ship.
        /// </summary>
        /// <param name="ship">The ship to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddShipAsync(Ship ship);

        /// <summary>
        /// Gets all ships with their manufacturer details.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of ships.</returns>
        Task<List<Ship>> GetAllShipsAsync();
    }
}
