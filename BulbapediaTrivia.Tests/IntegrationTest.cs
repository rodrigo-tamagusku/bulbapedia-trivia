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
            object triviaList = this.textProcessorService.GetHeaderFromPageContent(pageTitle, pageContent, Constants.TRIVIA_HEADER);
            JsonHelper.SaveObject(pageTitle, triviaList, Constants.TRIVIA_PATH);
        }

        [Fact]
        public async Task SaveAllPokemonTriviaAsJson()
        {
            List<Trivia> triviaList = new();
            foreach (var pokemonName in PokemonNames.Gen1)
            {
                string pageTitle = $"{pokemonName}_(Pokémon)";
                WikipediaResponse? result = await this.mediaWikiService.FullPagePlainTextQuery(pageTitle);
                string? pageContent = this.mediaWikiService.GetPlainText(result);
                if (pageContent == null) { Assert.Fail(); }
                triviaList.AddRange(this.textProcessorService.GetHeaderFromPageContent(pokemonName, pageContent, Constants.TRIVIA_HEADER));
            }
            JsonHelper.SaveObject(nameof(PokemonNames.Gen1), triviaList, Constants.TRIVIA_PATH);
        }
        [Fact]
        public async Task SaveAllPokemonTriviaAsJson_Individual()
        {
            foreach (var pokemonName in PokemonNames.Gen1)
            {
                string pageTitle = $"{pokemonName}_(Pokémon)";
                WikipediaResponse? result = await this.mediaWikiService.FullPagePlainTextQuery(pageTitle);
                string? pageContent = this.mediaWikiService.GetPlainText(result);
                if (pageContent == null) { Assert.Fail(); }
                object triviaList = this.textProcessorService.GetHeaderFromPageContent(pokemonName, pageContent, Constants.TRIVIA_HEADER);
                JsonHelper.SaveObject(pokemonName, triviaList, Constants.TRIVIA_PATH);

            }
        }
    }
}
