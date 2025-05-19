using ConcurrencyApp.Services;
using ShellProgressBar;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencyApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var fileService = new FileService();
            var sorterService = new SorterService(fileService.GetShipsDictionary());
            sorterService.StartSorting();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Concurrency Application Menu");
                Console.WriteLine("1. Generate and save 50 ships to 5 files");
                Console.WriteLine("2. Read files and populate dictionary");
                Console.WriteLine("3. Print dictionary contents");
                Console.WriteLine("4. Save combined file");
                Console.WriteLine("0. Exit");
                Console.Write("Select option: ");

                var input = Console.ReadLine();
                var cts = new CancellationTokenSource();

                try
                {
                    switch (input)
                    {
                        case "1":
                            await ExecuteWithProgressBar(
                                "Generating 50 ships to 5 files...",
                                100,
                                (progress) => fileService.GenerateAndSaveShipsAsync(progress, cts.Token));
                            break;
                        case "2":
                            await ExecuteWithProgressBar(
                                "Reading 5 files with ship data...",
                                100,
                                (progress) => fileService.ReadFilesAndPopulateDictionaryAsync(progress, cts.Token));
                            break;
                        case "3":
                            fileService.PrintDictionaryContents();
                            break;
                        case "4":
                            await fileService.SaveCombinedFileAsync("combined_ships.xml");
                            Console.WriteLine("Combined file saved successfully");
                            break;
                        case "0":
                            sorterService.StopSorting();
                            return;
                        default:
                            Console.WriteLine("Invalid option");
                            break;
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("\nOperation was canceled!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                finally
                {
                    cts.Dispose();
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private static async Task ExecuteWithProgressBar(
            string message,
            int maxTicks,
            Func<IProgress<int>, Task> action)
        {
            var options = new ProgressBarOptions
            {
                ForegroundColor = ConsoleColor.Yellow,
                BackgroundColor = ConsoleColor.DarkGray,
                ProgressCharacter = '─',
                ProgressBarOnBottom = true,
                ShowEstimatedDuration = true,
                DisplayTimeInRealTime = false
            };

            using var pbar = new ProgressBar(maxTicks, message, options);

            try
            {
                var progress = new Progress<int>(percent =>
                {
                    pbar.Tick(percent, $"Progress: {percent}%");
                });

                await action(progress);
                pbar.Tick(maxTicks, $"{message} Complete!");
            }
            catch
            {
                pbar.Dispose();
                throw;
            }
        }
    }
}