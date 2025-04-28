using MultiThreadingApp.Services;

namespace MultiThreadingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var threadService = new ThreadService();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("MultiThreading Application");
                Console.WriteLine("1. Task 1 - Generate and write objects to files");
                Console.WriteLine("2. Task 2 - Read files and combine data");
                Console.WriteLine("3. Task 3.1 - Read combined file (single thread)");
                Console.WriteLine("4. Task 3.2 - Read combined file (two threads)");
                Console.WriteLine("5. Task 3.3 - Read combined file (ten threads with semaphore)");
                Console.WriteLine("0. Exit");
                Console.Write("Enter your choice: ");

                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            threadService.ExecuteTask1();
                            break;
                        case "2":
                            threadService.ExecuteTask2();
                            break;
                        case "3":
                            threadService.ExecuteTask3_1();
                            break;
                        case "4":
                            threadService.ExecuteTask3_2();
                            break;
                        case "5":
                            threadService.ExecuteTask3_3();
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Try again.");
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
}