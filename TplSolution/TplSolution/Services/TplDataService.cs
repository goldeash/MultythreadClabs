using TplApp.Models;
using System.Threading.Tasks;

namespace TplApp.Services
{
    /// <summary>
    /// Service for generating and processing data using TPL
    /// Handles the business logic for the application tasks
    /// </summary>
    public class TplDataService
    {
        private const int DefaultGenerationCount = 20;
        private const int SplitCount = 10;

        /// <summary>
        /// Generates a list of Manufacturer objects
        /// </summary>
        /// <param name="count">Number of manufacturers to generate</param>
        /// <returns>List of generated manufacturers</returns>
        public List<Manufacturer> GenerateManufacturers(int count)
        {
            var result = new List<Manufacturer>();

            for (int i = 0; i < count; i++)
            {
                var manufacturer = Manufacturer.Create(
                    $"Manuf_{i}",
                    $"Address_{i}");

                result.Add(manufacturer);
            }

            return result;
        }

        /// <summary>
        /// Generates a list of Ship objects
        /// </summary>
        /// <param name="count">Number of ships to generate</param>
        /// <returns>List of generated ships</returns>
        public List<Ship> GenerateShips(int count)
        {
            var result = new List<Ship>();
            var random = new Random();

            for (int i = 0; i < count; i++)
            {
                var type = (ShipType)random.Next(0, 3);

                var ship = Ship.Create(
                    i,
                    $"Model_{i}",
                    $"SN_{i}",
                    type);

                result.Add(ship);
            }

            return result;
        }

        /// <summary>
        /// Executes Task 1 - Generates data and saves it to files
        /// </summary>
        /// <param name="fileService">File service instance</param>
        public async Task ExecuteTask1Async(TplFileService fileService)
        {
            var manufacturers = GenerateManufacturers(DefaultGenerationCount);
            var ships = GenerateShips(DefaultGenerationCount);

            var firstManufacturers = manufacturers.Take(SplitCount);
            var firstManufacturersList = firstManufacturers.ToList();
            var task1 = fileService.SerializeToFileAsync(
                TplFileService.ManufacturersFirstPartFile,
                firstManufacturersList);

            var secondManufacturers = manufacturers.Skip(SplitCount);
            var secondManufacturersList = secondManufacturers.ToList();
            var task2 = fileService.SerializeToFileAsync(
                TplFileService.ManufacturersSecondPartFile,
                secondManufacturersList);

            var firstShips = ships.Take(SplitCount);
            var firstShipsList = firstShips.ToList();
            var task3 = fileService.SerializeToFileAsync(
                TplFileService.ShipsFirstPartFile,
                firstShipsList);

            var secondShips = ships.Skip(SplitCount);
            var secondShipsList = secondShips.ToList();
            var task4 = fileService.SerializeToFileAsync(
                TplFileService.ShipsSecondPartFile,
                secondShipsList);

            await Task.WhenAll(task1, task2, task3, task4);
        }

        /// <summary>
        /// Executes Task 2 - Combines data from multiple files
        /// </summary>
        /// <param name="fileService">File service instance</param>
        public async Task ExecuteTask2Async(TplFileService fileService)
        {
            Task<List<Manufacturer>> task1 =
                fileService.DeserializeFromFileAsync<Manufacturer>(
                    TplFileService.ManufacturersFirstPartFile);

            Task<List<Manufacturer>> task2 =
                fileService.DeserializeFromFileAsync<Manufacturer>(
                    TplFileService.ManufacturersSecondPartFile);

            Task<List<Ship>> task3 =
                fileService.DeserializeFromFileAsync<Ship>(
                    TplFileService.ShipsFirstPartFile);

            Task<List<Ship>> task4 =
                fileService.DeserializeFromFileAsync<Ship>(
                    TplFileService.ShipsSecondPartFile);

            await Task.WhenAll(task1, task2, task3, task4);

            var manufacturersPart1 = await task1;
            var manufacturersPart2 = await task2;
            var shipsPart1 = await task3;
            var shipsPart2 = await task4;

            var allManufacturers = manufacturersPart1.Concat(manufacturersPart2);
            var allManufacturersList = allManufacturers.ToList();

            var allShips = shipsPart1.Concat(shipsPart2);
            var allShipsList = allShips.ToList();

            await fileService.SerializeToFileAsync(
                TplFileService.CombinedManufacturersFile,
                allManufacturersList);

            await fileService.SerializeToFileAsync(
                TplFileService.CombinedShipsFile,
                allShipsList);
        }
    }
}