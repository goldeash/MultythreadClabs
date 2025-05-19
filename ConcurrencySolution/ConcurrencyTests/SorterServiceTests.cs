using ConcurrencyApp.Models;
using ConcurrencyApp.Services;
using System.Collections.Concurrent;
using Xunit;

namespace ConcurrencyTests
{
    /// <summary>
    /// Contains unit tests for SorterService class.
    /// </summary>
    public class SorterServiceTests
    {
        /// <summary>
        /// Tests that StartSorting correctly sorts dictionary contents by ID.
        /// </summary>
        [Fact]
        public async Task StartSorting_SortsDictionaryContents()
        {
            var dictionary = new ConcurrentDictionary<string, ConcurrentBag<Ship>>();
            var ships = new ConcurrentBag<Ship>
            {
                Ship.Create(3, "Model3", "SN3", ShipType.battleship),
                Ship.Create(1, "Model1", "SN1", ShipType.aircarrier),
                Ship.Create(2, "Model2", "SN2", ShipType.etc)
            };
            dictionary.TryAdd("test.xml", ships);

            var sorterService = new SorterService(dictionary);
            sorterService.StartSorting();

            await Task.Delay(1500);

            var sortedShips = dictionary["test.xml"].OrderBy(s => s.ID).ToList();

            Assert.Equal(1, sortedShips[0].ID);
            Assert.Equal(2, sortedShips[1].ID);
            Assert.Equal(3, sortedShips[2].ID);

            sorterService.StopSorting();
        }
    }
}