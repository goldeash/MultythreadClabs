using System.Xml.Serialization;

namespace MultiThreadingApp.Services
{
    /// <summary>
    /// Provides thread-safe file operations including serialization and deserialization.
    /// </summary>
    public class FileService
    {
        private static readonly object _lockObject = new object();

        /// <summary>
        /// Serializes a collection of objects to an XML file.
        /// </summary>
        public void SerializeToFile(string filePath, IEnumerable<object> objects)
        {
            lock (_lockObject)
            {
                var serializer = new XmlSerializer(objects.GetType());
                using var writer = new StreamWriter(filePath);
                serializer.Serialize(writer, objects);
            }
        }

        /// <summary>
        /// Deserializes a list of objects from an XML file.
        /// </summary>
        public List<T> DeserializeFromFile<T>(string filePath)
        {
            lock (_lockObject)
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("File not found.", filePath);
                }

                var serializer = new XmlSerializer(typeof(List<T>));
                using var reader = new StreamReader(filePath);
                return (List<T>)serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Appends text content to a file.
        /// </summary>
        public void WriteToFile(string filePath, string content)
        {
            lock (_lockObject)
            {
                File.AppendAllText(filePath, content + Environment.NewLine);
            }
        }

        /// <summary>
        /// Reads all text from a file.
        /// </summary>
        public string ReadFile(string filePath)
        {
            lock (_lockObject)
            {
                return File.ReadAllText(filePath);
            }
        }

        /// <summary>
        /// Reads specific lines from a file.
        /// </summary>
        public string[] ReadFileLines(string filePath, int startLine, int endLine)
        {
            lock (_lockObject)
            {
                var allLines = File.ReadAllLines(filePath);
                if (endLine > allLines.Length)
                {
                    endLine = allLines.Length;
                }

                var lines = new string[endLine - startLine];
                Array.Copy(allLines, startLine, lines, 0, endLine - startLine);
                return lines;
            }
        }
    }
}