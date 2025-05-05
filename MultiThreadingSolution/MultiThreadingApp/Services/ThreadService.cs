using System.Diagnostics;
using MultiThreadingApp.Models;

namespace MultiThreadingApp.Services
{
    /// <summary>
    /// Provides multi-threading operations with file operations.
    /// </summary>
    public class ThreadService
    {
        private const string ManufacturersFile = "manufacturers.xml";
        private const string ShipsFile = "ships.xml";
        private const string CombinedFile = "combined.txt";

        private readonly FileService _fileService = new FileService();
        private readonly SemaphoreSlim _semaphore;
        private readonly Random _random = new Random();

        public ThreadService(int maxConcurrentThreads = 5)
        {
            _semaphore = new SemaphoreSlim(maxConcurrentThreads, maxConcurrentThreads);
        }

        private List<Manufacturer> GenerateManufacturers(int count)
        {
            var manufacturers = new List<Manufacturer>();
            for (int i = 0; i < count; i++)
            {
                manufacturers.Add(Manufacturer.Create($"Manufacturer_{i}", $"Address_{i}"));
            }
            return manufacturers;
        }

        private List<Ship> GenerateShips(int count)
        {
            var ships = new List<Ship>();
            for (int i = 0; i < count; i++)
            {
                var shipType = (ShipType)_random.Next(0, 3);
                ships.Add(Ship.Create(i, $"Model_{i}", $"SN_{i}", shipType));
            }
            return ships;
        }

        public void ExecuteTask1()
        {
            var manufacturers = GenerateManufacturers(10);
            var ships = GenerateShips(10);

            var thread1 = new Thread(() =>
                _fileService.SerializeToFile(ManufacturersFile, manufacturers));

            var thread2 = new Thread(() =>
                _fileService.SerializeToFile(ShipsFile, ships));

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            Console.WriteLine($"Task 1 completed. Files created: {ManufacturersFile} and {ShipsFile}");
        }

        public void ExecuteTask2()
        {
            var thread1 = new Thread(() =>
            {
                var manufacturers = _fileService.DeserializeFromFile<Manufacturer>(ManufacturersFile);
                foreach (var m in manufacturers)
                {
                    _fileService.WriteToFile(CombinedFile, m.ToString());
                    Thread.Sleep(100);
                }
            });

            var thread2 = new Thread(() =>
            {
                var ships = _fileService.DeserializeFromFile<Ship>(ShipsFile);
                foreach (var s in ships)
                {
                    _fileService.WriteToFile(CombinedFile, s.ToString());
                    Thread.Sleep(100);
                }
            });

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            Console.WriteLine($"Task 2 completed. Combined file created: {CombinedFile}");
        }

        public void ExecuteTask3_1()
        {
            var stopwatch = Stopwatch.StartNew();
            var content = _fileService.ReadFile(CombinedFile);
            Console.WriteLine(content);
            stopwatch.Stop();
            Console.WriteLine($"Time taken: {stopwatch.ElapsedMilliseconds} ms");
        }

        public void ExecuteTask3_2()
        {
            var stopwatch = Stopwatch.StartNew();

            string firstHalf = null;
            string secondHalf = null;

            var thread1 = new Thread(() =>
            {
                var lines = _fileService.ReadFileLines(CombinedFile, 0, 5);
                firstHalf = string.Join(Environment.NewLine, lines);
            });

            var thread2 = new Thread(() =>
            {
                var lines = _fileService.ReadFileLines(CombinedFile, 5, 10);
                secondHalf = string.Join(Environment.NewLine, lines);
            });

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            Console.WriteLine(firstHalf);
            Console.WriteLine(secondHalf);
            stopwatch.Stop();
            Console.WriteLine($"Time taken: {stopwatch.ElapsedMilliseconds} ms");
        }

        public void ExecuteTask3_3()
        {
            var stopwatch = Stopwatch.StartNew();
            var threads = new List<Thread>();

            for (int i = 0; i < 10; i++)
            {
                var thread = new Thread(() =>
                {
                    _semaphore.Wait();
                    try
                    {
                        var content = _fileService.ReadFile(CombinedFile);
                        Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} read file. Content length: {content.Length}");
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                });
                threads.Add(thread);
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            stopwatch.Stop();
            Console.WriteLine($"Time taken: {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}