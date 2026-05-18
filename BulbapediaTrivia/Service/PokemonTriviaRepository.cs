using System.Text.Json;
using BulbapediaTrivia.Const;
using BulbapediaTrivia.Model;

namespace BulbapediaTrivia.Service
{
    public class PokemonTriviaRepository
    {
        public Dictionary<string, List<Trivia>> GetTriviaFromJson()
        {
            string filePath = GetTriviaDataPath();
            return ReadFiles(filePath);
        }
        public Dictionary<string, List<Trivia>> ReadFiles(string folderPath)
        {
            Dictionary<string, List<Trivia>> dict = new();
            // 2. Check if the directory exists to avoid exceptions
            if (!Directory.Exists(folderPath)) throw new Exception("NotFound");

            // 3. Get all files with the .json extension
            string[] files = Directory.GetFiles(folderPath, "*.json");

            foreach (string file in files)
            {
                try
                {
                    // 4. Read the raw text
                    string fileName = Path.GetFileName(file);
                    string jsonString = File.ReadAllText(file);

                    // 5. Deserialize into your object
                    List<Trivia>? list = JsonSerializer.Deserialize<List<Trivia>>(jsonString);
                    if (list != null)
                    {
                        dict.Add(fileName, list);
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Error parsing {file}: {ex.Message}");
                }
            }
            return dict;
        }

        public string GetTriviaDataPath()
        {
            string relativeDirectory = Path.Combine(@"..\..\..\..\", Constants.TRIVIA_PATH);
            string fullPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), relativeDirectory));
            return fullPath;
        }
    }
}
