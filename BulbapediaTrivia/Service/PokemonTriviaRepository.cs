using System.Text.Json;
using BulbapediaTrivia.Const;
using BulbapediaTrivia.Model;

namespace BulbapediaTrivia.Service
{
    public class PokemonTriviaRepository
    {
        public async Task<List<Trivia>> GetTriviaFromJson()
        {
            List<Trivia> trivias = new();
            string filePath = GetTriviaDataPath();

            try
            {
                // 3. Ensure the file actually exists
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"Error: File not found at {filePath}");
                }

                // 4. Open the file stream and deserialize asynchronously
                using FileStream openStream = File.OpenRead(filePath);
                List<Trivia>? users = await JsonSerializer.DeserializeAsync<List<Trivia>>(openStream);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Parsing Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return trivias;
        }

        public string GetTriviaDataPath()
        {
            string relativeDirectory = Path.Combine(@"..\..\..\..\", Constants.TRIVIA_PATH);
            string fullPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), relativeDirectory));
            return fullPath;
        }
    }
}
