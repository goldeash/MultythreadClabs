using ADOApp.Services;

namespace ADOApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var dbService = new DatabaseService();
                await dbService.InitializeDatabaseAsync();

                var menuService = new MenuService(dbService);
                await menuService.ShowMenuAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}