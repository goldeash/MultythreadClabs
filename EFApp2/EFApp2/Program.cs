using EFApp.Data;
using EFApp.Data.Contexts;
using EFApp.UI;
using Microsoft.EntityFrameworkCore;

namespace EFApp
{
    /// <summary>
    /// The main class for the application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Applying migrations and seeding data for all strategies...");

                await InitializeAndSeedContextAsync(new TphDbContext());
                await InitializeAndSeedContextAsync(new TptDbContext());
                await InitializeAndSeedContextAsync(new TpcDbContext());

                Console.WriteLine("Database initialization complete.");

                var menuService = new MenuService();
                await menuService.ShowStrategySelectionMenuAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n--- A CRITICAL ERROR OCCURRED ---");
                Console.WriteLine($"Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
            }
            finally
            {
                Console.WriteLine("\nApplication is shutting down. Press any key to exit...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Applies migrations for the given context and seeds the database.
        /// </summary>
        /// <param name="context">The DbContext to initialize.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private static async Task InitializeAndSeedContextAsync(DbContext context)
        {
            await using (context)
            {
                await context.Database.MigrateAsync();

                var initializer = new DataInitializer(context);
                await initializer.SeedAsync();
            }
        }
    }
}
