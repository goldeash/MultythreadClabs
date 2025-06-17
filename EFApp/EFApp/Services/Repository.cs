using EFApp.Data;
using EFApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFApp.Services
{
    /// <summary>
    /// Repository for performing CRUD operations
    /// </summary>
    public class Repository : IRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of Repository
        /// </summary>
        /// <param name="context">AppDbContext</param>
        public Repository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a manufacturer to the database
        /// </summary>
        /// <param name="manufacturer">Manufacturer</param>
        /// <returns>Task<int></returns>
        public async Task<int> AddManufacturerAsync(Manufacturer manufacturer)
        {
            _context.Manufacturers.Add(manufacturer);
            await _context.SaveChangesAsync();
            return manufacturer.Id;
        }

        /// <summary>
        /// Adds a ship to the database
        /// </summary>
        /// <param name="ship">Ship</param>
        /// <returns>Task</returns>
        public async Task AddShipAsync(Ship ship)
        {
            _context.Ships.Add(ship);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets all manufacturers from the database
        /// </summary>
        /// <returns>Task<List<Manufacturer>></returns>
        public async Task<List<Manufacturer>> GetAllManufacturersAsync()
        {
            return await _context.Manufacturers.ToListAsync();
        }

        /// <summary>
        /// Gets all ships for a specific manufacturer
        /// </summary>
        /// <param name="manufacturerId">Manufacturer ID</param>
        /// <returns>Task<List<Ship>></returns>
        public async Task<List<Ship>> GetShipsByManufacturerAsync(int manufacturerId)
        {
            return await _context.Ships
                .Where(s => s.ManufacturerId == manufacturerId)
                .ToListAsync();
        }

        /// <summary>
        /// Gets a manufacturer by ID
        /// </summary>
        /// <param name="id">Manufacturer ID</param>
        /// <returns>Task<Manufacturer></returns>
        public async Task<Manufacturer> GetManufacturerByIdAsync(int id)
        {
            return await _context.Manufacturers.FindAsync(id);
        }

        /// <summary>
        /// Updates a manufacturer
        /// </summary>
        /// <param name="manufacturer">Manufacturer</param>
        /// <returns>Task</returns>
        public async Task UpdateManufacturerAsync(Manufacturer manufacturer)
        {
            _context.Manufacturers.Update(manufacturer);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a manufacturer
        /// </summary>
        /// <param name="id">Manufacturer ID</param>
        /// <returns>Task</returns>
        public async Task DeleteManufacturerAsync(int id)
        {
            var manufacturer = await _context.Manufacturers.FindAsync(id);
            if (manufacturer != null)
            {
                _context.Manufacturers.Remove(manufacturer);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Gets a ship by ID
        /// </summary>
        /// <param name="id">Ship ID</param>
        /// <returns>Task<Ship></returns>
        public async Task<Ship> GetShipByIdAsync(int id)
        {
            return await _context.Ships.FindAsync(id);
        }

        /// <summary>
        /// Updates a ship
        /// </summary>
        /// <param name="ship">Ship</param>
        /// <returns>Task</returns>
        public async Task UpdateShipAsync(Ship ship)
        {
            _context.Ships.Update(ship);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a ship
        /// </summary>
        /// <param name="id">Ship ID</param>
        /// <returns>Task</returns>
        public async Task DeleteShipAsync(int id)
        {
            var ship = await _context.Ships.FindAsync(id);
            if (ship != null)
            {
                _context.Ships.Remove(ship);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Adds a new product for a new manufacturer with transaction
        /// </summary>
        /// <param name="manufacturer">Manufacturer</param>
        /// <param name="ship">Ship</param>
        /// <returns>Task<bool></returns>
        public async Task<bool> AddProductForNewManufacturerAsync(Manufacturer manufacturer, Ship ship)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Manufacturers.Add(manufacturer);
                await _context.SaveChangesAsync();

                ship.ManufacturerId = manufacturer.Id;
                _context.Ships.Add(ship);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
    }
}