using EFApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EFApp.Services
{
    /// <summary>
    /// Implements repository operations for data access.
    /// </summary>
    public class Repository : IRepository
    {
        private readonly DbContext _context;

        /// <summary>
        /// Initializes a new instance of the Repository class.
        /// </summary>
        /// <param name="context">The database context to be used for data operations.</param>
        public Repository(DbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task AddManufacturerAsync(Manufacturer manufacturer)
        {
            _context.Set<Manufacturer>().Add(manufacturer);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<Manufacturer> GetManufacturerByIdAsync(int id)
        {
            return await _context.Set<Manufacturer>().FindAsync(id);
        }

        /// <inheritdoc/>
        public async Task<List<Manufacturer>> GetAllManufacturersAsync()
        {
            return await _context.Set<Manufacturer>().AsNoTracking().ToListAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateManufacturerAsync(Manufacturer manufacturer)
        {
            _context.Set<Manufacturer>().Update(manufacturer);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteManufacturerAsync(int id)
        {
            var manufacturer = await GetManufacturerByIdAsync(id);
            if (manufacturer != null)
            {
                _context.Set<Manufacturer>().Remove(manufacturer);
                await _context.SaveChangesAsync();
            }
        }

        /// <inheritdoc/>
        public async Task AddShipAsync(Ship ship)
        {
            _context.Set<Ship>().Add(ship);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<List<Ship>> GetAllShipsAsync()
        {
            return await _context.Set<Ship>().AsNoTracking().Include(s => s.Manufacturer).ToListAsync();
        }
    }
}
