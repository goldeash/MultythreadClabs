using TplApp.Services;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var fileService = new TplFileService();
        var dataService = new TplDataService();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("TPL Application Menu");
            Console.WriteLine("1. Execute Task 1 (Generate and save data)");
            Console.WriteLine("2. Execute Task 2 (Combine files)");
            Console.WriteLine("3. Execute Task 3 (Read and print combined data)");
            Console.WriteLine("0. Exit");
            Console.Write("Select option: ");

            var input = Console.ReadLine();

            try
            {
                switch (input)
                {
                    case "1":
                        await dataService.ExecuteTask1Async(fileService);
                        Console.WriteLine("Task 1 completed successfully");
                        break;
                    case "2":
                        await dataService.ExecuteTask2Async(fileService);
                        Console.WriteLine("Task 2 completed successfully");
                        break;
                    case "3":
                        Console.WriteLine("Manufacturers:");
                        await fileService.ReadAndPrintAsync("combined_manufacturers.xml");
                        Console.WriteLine("\nShips:");
                        await fileService.ReadAndPrintAsync("combined_ships.xml");
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid option");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}