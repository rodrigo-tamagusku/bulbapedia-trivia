using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using BulbapediaTrivia.Const;
using BulbapediaTrivia.Model;
using BulbapediaTrivia.Service;
using Xunit;

namespace BulbapediaTrivia.Tests
{
    public class IntegrationTest
    {
        private HttpClient httpClient;
        private MediaWikiService mediaWikiService;
        private TextProcessorService textProcessorService;

        public IntegrationTest()
        {
            this.httpClient = new HttpClient();
            this.mediaWikiService = new MediaWikiService(Constants.BULBAPEDIA_WIKI, httpClient);
            this.textProcessorService = new TextProcessorService();
        }

        [Theory]
        [InlineData("Ditto_(Pokémon)")]
        [InlineData("Metagross_(Pokémon)")]
        [InlineData("orthworm_(Pokémon)")]
        public async Task SaveTriviaAsJson(string pageTitle)
        {
            WikipediaResponse? result = await this.mediaWikiService.FullPagePlainTextQuery(pageTitle);
            string? pageContent = this.mediaWikiService.GetPlainText(result);
            if (pageContent == null) { Assert.Fail(); }
            object linksPokedex = this.textProcessorService.GetTriviaFromPageContent(pageTitle, pageContent);
            SaveObject(pageTitle, linksPokedex);
        }

        private static void SaveObject(string name, object someJsonCompatibleObject)
        {
            string jsonString = JsonSerializer.Serialize(someJsonCompatibleObject,
                new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                    WriteIndented = true
                });
            string filename = $"{name}.json";
            string current = Directory.GetCurrentDirectory();
            string directory = Path.Combine(current, Constants.FOLDER_TRIVIA);
            string fullPath = Path.Combine(directory, filename);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            File.WriteAllText(fullPath, jsonString);
        }
    }
}
