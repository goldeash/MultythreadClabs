using System.Xml.Serialization;
using System.Threading.Tasks;

namespace TplApp.Services
{
    /// <summary>
    /// Service for file operations using TPL (Task Parallel Library)
    /// Provides thread-safe file operations with async/await support
    /// </summary>
    public class TplFileService
    {
        public const string ManufacturersFirstPartFile = "manufacturers1.xml";
        public const string ManufacturersSecondPartFile = "manufacturers2.xml";
        public const string ShipsFirstPartFile = "ships1.xml";
        public const string ShipsSecondPartFile = "ships2.xml";
        public const string CombinedManufacturersFile = "combined_manufacturers.xml";
        public const string CombinedShipsFile = "combined_ships.xml";

        private static readonly SemaphoreSlim _fileLock = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Serializes a collection of objects to an XML file asynchronously
        /// </summary>
        /// <typeparam name="T">Type of objects to serialize</typeparam>
        /// <param name="path">Path to the output file</param>
        /// <param name="data">Collection of objects to serialize</param>
        public async Task SerializeToFileAsync<T>(string path, List<T> data)
        {
            await _fileLock.WaitAsync();

            try
            {
                var serializer = new XmlSerializer(typeof(List<T>));

                using var writer = new StreamWriter(path);

                serializer.Serialize(writer, data);
            }
            finally
            {
                _fileLock.Release();
            }
        }

        /// <summary>
        /// Deserializes a list of objects from an XML file asynchronously
        /// </summary>
        /// <typeparam name="T">Type of objects to deserialize</typeparam>
        /// <param name="path">Path to the input file</param>
        /// <returns>List of deserialized objects</returns>
        public async Task<List<T>> DeserializeFromFileAsync<T>(string path)
        {
            await _fileLock.WaitAsync();

            try
            {
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException("File not found", path);
                }

                var serializer = new XmlSerializer(typeof(List<T>));

                using var reader = new StreamReader(path);

                return (List<T>)serializer.Deserialize(reader);
            }
            finally
            {
                _fileLock.Release();
            }
        }

        /// <summary>
        /// Merges content of two files into a single output file asynchronously
        /// </summary>
        /// <param name="file1">Path to first source file</param>
        /// <param name="file2">Path to second source file</param>
        /// <param name="outputFile">Path to output merged file</param>
        public async Task MergeFilesAsync(string file1, string file2, string outputFile)
        {
            var content1 = await File.ReadAllTextAsync(file1);
            var content2 = await File.ReadAllTextAsync(file2);

            await _fileLock.WaitAsync();

            try
            {
                await File.WriteAllTextAsync(outputFile, content1 + content2);
            }
            finally
            {
                _fileLock.Release();
            }
        }

        /// <summary>
        /// Reads file content and prints it to console line by line asynchronously
        /// </summary>
        /// <param name="filePath">Path to the file to read</param>
        public async Task ReadAndPrintAsync(string filePath)
        {
            var lines = await File.ReadAllLinesAsync(filePath);

            var tasks = lines.Select(line =>
                Task.Run(() => Console.WriteLine(line)));

            await Task.WhenAll(tasks);
        }
    }
}