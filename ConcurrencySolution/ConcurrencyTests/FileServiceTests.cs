using ConcurrencyApp.Models;
using ConcurrencyApp.Services;
using Xunit;

namespace ConcurrencyTests
{
    /// <summary>
    /// Contains unit tests for FileService class.
    /// </summary>
    public class FileServiceTests
    {
        private readonly FileService _fileService = new();

        /// <summary>
        /// Tests that GenerateAndSaveShipsAsync creates exactly five files.
        /// </summary>
        [Fact]
        public async Task GenerateAndSaveShipsAsync_CreatesFiveFiles()
        {
            await _fileService.GenerateAndSaveShipsAsync();

            for (int i = 1; i <= 5; i++)
            {
                Assert.True(
                    File.Exists($"ships_{i}.xml"));
                File.Delete($"ships_{i}.xml");
            }
        }

        /// <summary>
        /// Tests that ReadFilesAndPopulateDictionaryAsync correctly populates the dictionary.
        /// </summary>
        [Fact]
        public async Task ReadFilesAndPopulateDictionaryAsync_PopulatesDictionary()
        {
            await _fileService.GenerateAndSaveShipsAsync();
            await _fileService.ReadFilesAndPopulateDictionaryAsync();

            var dictionary = _fileService.GetShipsDictionary();
            Assert.Equal(5, dictionary.Count);
            Assert.All(dictionary.Values, bag =>
            Assert.Equal(10, bag.Count));

            for (int i = 1; i <= 5; i++)
            {
                File.Delete($"ships_{i}.xml");
            }
        }
    }
}