using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
using MultiThreadingApp.Models;

namespace MultiThreadingApp.Services
{
    public class FileService
    {
        private static readonly object _lockObject = new object();

        public void SerializeToFile(string filePath, IEnumerable<object> objects)
        {
            lock (_lockObject)
            {
                var serializer = new XmlSerializer(objects.GetType());
                using var writer = new StreamWriter(filePath);
                serializer.Serialize(writer, objects);
            }
        }

        public List<T> DeserializeFromFile<T>(string filePath)
        {
            lock (_lockObject)
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException("File not found.", filePath);

                var serializer = new XmlSerializer(typeof(List<T>));
                using var reader = new StreamReader(filePath);
                return (List<T>)serializer.Deserialize(reader);
            }
        }

        public void WriteToFile(string filePath, string content)
        {
            lock (_lockObject)
            {
                File.AppendAllText(filePath, content + Environment.NewLine);
            }
        }

        public string ReadFile(string filePath)
        {
            lock (_lockObject)
            {
                return File.ReadAllText(filePath);
            }
        }

        public string[] ReadFileLines(string filePath, int startLine, int endLine)
        {
            lock (_lockObject)
            {
                var allLines = File.ReadAllLines(filePath);
                if (endLine > allLines.Length) endLine = allLines.Length;
                var lines = new string[endLine - startLine];
                Array.Copy(allLines, startLine, lines, 0, endLine - startLine);
                return lines;
            }
        }
    }
}