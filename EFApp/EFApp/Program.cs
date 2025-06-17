using EFApp.Data;
using EFApp.Services;
using Microsoft.EntityFrameworkCore;
using System;

namespace EFApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Starting application...");

                using var context = new AppDbContext();
                Console.WriteLine("Database connection testing...");

                try
                {
                    var canConnect = await context.Database.CanConnectAsync();
                    Console.WriteLine(canConnect ? "Database connection successful" : "Could not connect to database");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Database connection failed: {ex.Message}");
                    throw;
                }

                var initializer = new DataInitializer(context);
                await initializer.InitializeAsync();

                var repository = new Repository(context);
                var menuService = new MenuService(repository);

                await menuService.ShowMenuAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fatal error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}