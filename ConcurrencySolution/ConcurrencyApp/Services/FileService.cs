using ConcurrencyApp.Models;
using System.Collections.Concurrent;
using System.Xml.Serialization;

namespace ConcurrencyApp.Services
{
    /// <summary>
    /// Provides thread-safe file operations for ship data.
    /// </summary>
    public class FileService
    {
        private readonly ConcurrentDictionary<string, ConcurrentBag<Ship>> _shipsDictionary = new();
        private readonly SemaphoreSlim _fileLock = new(1, 1);

        /// <summary>
        /// Generates ship instances and saves them to XML files.
        /// </summary>
        /// <param name="progress">Progress reporter for tracking operation completion.</param>
        /// <param name="cancellationToken">Cancellation token for operation.</param>
        public async Task GenerateAndSaveShipsAsync(IProgress<int> progress = null, CancellationToken cancellationToken = default)
        {
            const int totalShips = 50;
            const int shipsPerFile = 10;
            int totalFiles = totalShips / shipsPerFile;

            var ships = GenerateShips(totalShips);

            for (int i = 0; i < totalFiles; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var fileName = $"ships_{i + 1}.xml";
                var chunk = ships.Skip(i * shipsPerFile).Take(shipsPerFile)
                    .ToList();

                await SaveShipsToFileAsync(fileName, chunk, progress, i + 1, totalFiles);
                progress?.Report((i + 1) * 100 / totalFiles);
            }
        }

        /// <summary>
        /// Reads ship data from files and populates the dictionary.
        /// </summary>
        /// <param name="progress">Progress reporter for tracking operation completion.</param>
        /// <param name="cancellationToken">Cancellation token for operation.</param>
        public async Task ReadFilesAndPopulateDictionaryAsync(IProgress<int> progress = null, CancellationToken cancellationToken = default)
        {
            const int totalFiles = 5;

            for (int i = 1; i <= totalFiles; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var fileName = $"ships_{i}.xml";
                await ReadFileAndAddToDictionaryAsync(fileName, progress, i, totalFiles);
                progress?.Report(i * 100 / totalFiles);
            }
        }

        /// <summary>
        /// Gets the thread-safe dictionary containing ship data.
        /// </summary>
        /// <returns>Dictionary with ship collections.</returns>
        public ConcurrentDictionary<string, ConcurrentBag<Ship>> GetShipsDictionary()
        {
            return _shipsDictionary;
        }

        /// <summary>
        /// Saves all ships to a combined file.
        /// </summary>
        /// <param name="outputFileName">Name of the output file.</param>
        public async Task SaveCombinedFileAsync(string outputFileName)
        {
            var allShips = _shipsDictionary.Values.SelectMany(bag => bag)
                .ToList();
            await SaveShipsToFileAsync(outputFileName, allShips);
        }

        /// <summary>
        /// Prints dictionary contents to console.
        /// </summary>
        public void PrintDictionaryContents()
        {
            foreach (var kvp in _shipsDictionary)
            {
                Console.WriteLine($"\nFile: {kvp.Key}");
                foreach (var ship in kvp.Value)
                {
                    Console.WriteLine(ship);
                }
            }
        }

        /// <summary>
        /// Generates a list of ship instances.
        /// </summary>
        /// <param name="count">Number of ships to generate.</param>
        /// <returns>List of generated ships.</returns>
        private List<Ship> GenerateShips(int count)
        {
            var random = new Random();
            var ships = new List<Ship>();

            for (int i = 0; i < count; i++)
            {
                var shipType = (ShipType)random.Next(0, 3);
                var ship = Ship.Create(
                    i,
                    $"Model_{i}",
                    $"SN_{i}",
                    shipType);

                ships.Add(ship);
            }

            return ships;
        }

        /// <summary>
        /// Saves ships to an XML file.
        /// </summary>
        /// <param name="fileName">Name of the file to save.</param>
        /// <param name="ships">List of ships to save.</param>
        /// <param name="progress">Progress reporter.</param>
        /// <param name="currentFile">Current file index for progress tracking.</param>
        /// <param name="totalFiles">Total number of files.</param>
        private async Task SaveShipsToFileAsync(string fileName, List<Ship> ships, IProgress<int> progress = null, int currentFile = 1, int totalFiles = 1)
        {
            await _fileLock.WaitAsync();
            try
            {
                var serializer = new XmlSerializer(typeof(List<Ship>));
                using var writer = new StreamWriter(fileName);

                for (int i = 0; i < ships.Count; i++)
                {
                    await Task.Delay(50);
                    progress?.Report((currentFile - 1) * 100 / totalFiles + (i + 1) * 100 / (ships.Count * totalFiles));
                }

                serializer.Serialize(writer, ships);
            }
            finally
            {
                _fileLock.Release();
            }
        }

        /// <summary>
        /// Reads ships from an XML file and adds them to the dictionary.
        /// </summary>
        /// <param name="fileName">Name of the file to read from.</param>
        /// <param name="progress">Progress reporter for UI updates.</param>
        /// <param name="currentFile">Current file index for progress tracking.</param>
        /// <param name="totalFiles">Total number of files.</param>
        /// <exception cref="FileNotFoundException">Thrown if file does not exist.</exception>
        private async Task ReadFileAndAddToDictionaryAsync(string fileName, IProgress<int> progress = null, int currentFile = 1, int totalFiles = 1)
        {
            await _fileLock.WaitAsync();
            try
            {
                if (!File.Exists(fileName))
                {
                    throw new FileNotFoundException("File not found", fileName);
                }

                var serializer = new XmlSerializer(typeof(List<Ship>));
                using var reader = new StreamReader(fileName);
                var ships = (List<Ship>)serializer.Deserialize(reader);

                for (int i = 0; i < ships.Count; i++)
                {
                    await Task.Delay(50);
                    progress?.Report((currentFile - 1) * 100 / totalFiles + (i + 1) * 100 / (ships.Count * totalFiles));
                }

                _shipsDictionary.TryAdd(fileName, new ConcurrentBag<Ship>(ships));
            }
            finally
            {
                _fileLock.Release();
            }
        }
    }
}
