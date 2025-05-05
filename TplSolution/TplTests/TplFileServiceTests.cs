using TplApp.Models;
using TplApp.Services;

namespace TplTests.Services
{
    public class TplFileServiceTests
    {
        private readonly TplFileService _service = new TplFileService();
        private const string TestFile = "test.xml";

        [Fact]
        public async Task SerializeAndDeserialize_ShouldWork()
        {
            var testData = new List<Manufacturer>
            {
                Manufacturer.Create("Test1", "Address1"),
                Manufacturer.Create("Test2", "Address2")
            };

            await _service.SerializeToFileAsync(TestFile, testData);
            var result = await _service.DeserializeFromFileAsync<Manufacturer>(TestFile);

            Assert.Equal(2, result.Count);
            Assert.Equal("Test1", result[0].Name);
            Assert.Equal("Address2", result[1].Address);

            File.Delete(TestFile);
        }

        [Fact]
        public async Task MergeFiles_ShouldCombineContent()
        {
            await File.WriteAllTextAsync("file1.txt", "Content1");
            await File.WriteAllTextAsync("file2.txt", "Content2");

            await _service.MergeFilesAsync("file1.txt", "file2.txt", "merged.txt");
            var result = await File.ReadAllTextAsync("merged.txt");

            Assert.Equal("Content1Content2", result);

            File.Delete("file1.txt");
            File.Delete("file2.txt");
            File.Delete("merged.txt");
        }
    }
}