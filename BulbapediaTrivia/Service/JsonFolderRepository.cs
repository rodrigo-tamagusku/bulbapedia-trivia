namespace BulbapediaTrivia.Service
{
    using System.IO;
    using System.Text.Json;

    public abstract class JsonFolderRepository<T>
    {
        // Derived classes must specify where their data lives
        public abstract string FolderPath { get; }

        // The main entry point to get data
        public Dictionary<string, T> GetAllData()
        {
            return ReadFiles(FolderPath);
        }

        private Dictionary<string, T> ReadFiles(string path)
        {
            var dataStore = new Dictionary<string, T>();

            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException($"Data directory not found: {path}");
            }

            string[] files = Directory.GetFiles(path, "*.json");

            foreach (string file in files)
            {
                try
                {
                    string jsonString = File.ReadAllText(file);
                    T? data = JsonSerializer.Deserialize<T>(jsonString);

                    if (data != null)
                    {
                        dataStore.Add(Path.GetFileName(file), data);
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Error parsing {file}: {ex.Message}");
                }
            }

            return dataStore;
        }
    }
}
