using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Plugins.Demo.ObjectAdapter.LocalStorage
{
    public class LocalFileStorage
    {
        public string SaveDirectory { get; }

        public LocalFileStorage(string savePath = "C:\\Users\\Public\\SaveFiles")
        {
            Directory.CreateDirectory(savePath);
            SaveDirectory = savePath;
        }

        ~LocalFileStorage()
        {
            Directory.Delete(SaveDirectory, true);
        }

        public Task<string> ReadFile(DateTime timestamp)
        {
            var fileName = ConvertToFileName(timestamp);
            var directoryInfo = new DirectoryInfo(SaveDirectory);
            var saveFile = directoryInfo.GetFiles()
                .FirstOrDefault(fileInfo => fileInfo.Name == fileName);

            if (saveFile == null) return Task.FromResult<string>(null);
            return File.ReadAllTextAsync(saveFile.FullName);
        }

        public Task WriteFile(DateTime timestamp, string text)
        {
            var fileName = ConvertToFileName(timestamp);
            var path = Path.Combine(SaveDirectory, fileName);
            return File.WriteAllTextAsync(path, text);
        }

        private string ConvertToFileName(DateTime timestamp) => $"{timestamp.ToFileTime()}.txt";
    }
}