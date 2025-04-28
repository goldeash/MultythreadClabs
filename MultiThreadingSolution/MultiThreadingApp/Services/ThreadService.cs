using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using MultiThreadingApp.Models;

namespace MultiThreadingApp.Services
{
    public class ThreadService
    {
        private readonly FileService _fileService = new FileService();
        private readonly SemaphoreSlim _semaphore;

        public ThreadService(int maxConcurrentThreads = 5)
        {
            _semaphore = new SemaphoreSlim(maxConcurrentThreads, maxConcurrentThreads);
        }

        public void ExecuteTask1()
        {
            var manufacturers = GenerateManufacturers(10);
            var ships = GenerateShips(10);

            var thread1 = new Thread(() =>
                _fileService.SerializeToFile("manufacturers.xml", manufacturers));
            var thread2 = new Thread(() =>
                _fileService.SerializeToFile("ships.xml", ships));

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            Console.WriteLine("Task 1 completed. Files created: manufacturers.xml and ships.xml");
        }

        public void ExecuteTask2()
        {
            var thread1 = new Thread(() =>
            {
                var manufacturers = _fileService.DeserializeFromFile<Manufacturer>("manufacturers.xml");
                foreach (var m in manufacturers)
                {
                    _fileService.WriteToFile("combined.txt", m.ToString());
                    Thread.Sleep(100); // добавил для имитации работы
                }
            });

            var thread2 = new Thread(() =>
            {
                var ships = _fileService.DeserializeFromFile<Ship>("ships.xml");
                foreach (var s in ships)
                {
                    _fileService.WriteToFile("combined.txt", s.ToString());
                    Thread.Sleep(100);
                }
            });

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            Console.WriteLine("Task 2 completed. Combined file created: combined.txt");
        }

        public void ExecuteTask3_1()
        {
            var stopwatch = Stopwatch.StartNew();
            var content = _fileService.ReadFile("combined.txt");
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
                var lines = _fileService.ReadFileLines("combined.txt", 0, 5);
                firstHalf = string.Join(Environment.NewLine, lines);
            });

            var thread2 = new Thread(() =>
            {
                var lines = _fileService.ReadFileLines("combined.txt", 5, 10);
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
                        var content = _fileService.ReadFile("combined.txt");
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
            var random = new Random();
            for (int i = 0; i < count; i++)
            {
                var shipType = (ShipType)random.Next(0, 3);
                ships.Add(Ship.Create(i, $"Model_{i}", $"SN_{i}", shipType));
            }
            return ships;
        }
    }
}