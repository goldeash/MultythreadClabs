using ConcurrencyApp.Models;
using System.Collections.Concurrent;

namespace ConcurrencyApp.Services
{
    /// <summary>
    /// Continuously sorts ship collections in the dictionary by ID.
    /// </summary>
    public class SorterService
    {
        private readonly ConcurrentDictionary<string, ConcurrentBag<Ship>> _dictionary;
        private bool _isRunning;
        private readonly object _sortLock = new();

        /// <summary>
        /// Initializes a new instance of the SorterService.
        /// </summary>
        /// <param name="dictionary">Dictionary to be sorted.</param>
        public SorterService(ConcurrentDictionary<string, ConcurrentBag<Ship>> dictionary)
        {
            _dictionary = dictionary;
        }

        /// <summary>
        /// Starts the continuous sorting process.
        /// </summary>
        public void StartSorting()
        {
            if (_isRunning) return;
            _isRunning = true;
            Task.Run(() => SortContinuously());
        }

        /// <summary>
        /// Stops the continuous sorting process.
        /// </summary>
        public void StopSorting()
        {
            _isRunning = false;
        }

        private async Task SortContinuously()
        {
            while (_isRunning)
            {
                lock (_sortLock)
                {
                    foreach (var kvp in _dictionary)
                    {
                        var sortedList = kvp.Value.OrderBy(ship => ship.ID).ToList();
                        _dictionary[kvp.Key] = new ConcurrentBag<Ship>(sortedList);
                    }
                }
                await Task.Delay(1000);
            }
        }
    }
}