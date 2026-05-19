using System.Text.Json;
using BulbapediaTrivia.Const;
using BulbapediaTrivia.Model;
using BulbapediaTrivia.Service;
using Xunit;
using static BulbapediaTrivia.Tests.IntegrationTest;

namespace BulbapediaTrivia.Tests
{
    public class IntegrationTest : IClassFixture<TestStateFixture>
    {
        private TestStateFixture shared;
        private HttpClient httpClient;
        private MediaWikiService mediaWikiService;
        private TextProcessorService textProcessorService;

        public IntegrationTest(TestStateFixture shared)
        {
            this.shared = shared;
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
        public async Task SaveAllPokemonTrivia_Gen1_AsJson()
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

        [Theory]
        [MemberData(nameof(AllPokemonNames))]
        public async Task CheckAllPages(string pokemonName)
        {
            List<Trivia> triviaList = new();
            string pageTitle = $"{pokemonName}_(Pokémon)";
            WikipediaResponse? result = await this.mediaWikiService.FullPagePlainTextQuery(pageTitle);
            string? pageContent = this.mediaWikiService.GetPlainText(result);
            if (pageContent == null) { Assert.Fail("Page not found: " + pageTitle); }
            var list = this.textProcessorService.GetHeaderFromPageContent(pokemonName, pageContent, Constants.TRIVIA_HEADER);
            if (list == null || !list.Any()) 
            { 
                Assert.Skip("Trivia not found for: " + pageTitle); 
            }
        }

        [Theory]
        [MemberData(nameof(AllPokemonNames))]
        public async Task SaveAllPages(string pokemonName)
        {
            string pageTitle = $"{pokemonName}_(Pokémon)";
            WikipediaResponse? result = await this.mediaWikiService.FullPagePlainTextQuery(pageTitle);
            string? pageContent = this.mediaWikiService.GetPlainText(result);
            if (pageContent == null) { Assert.Fail("Page not found: " + pageTitle); }
            this.shared.Trivias.AddRange(this.textProcessorService.GetHeaderFromPageContent(pokemonName, pageContent, Constants.TRIVIA_HEADER));
        }

        public static IEnumerable<string[]> AllPokemonNames()
        {
            var pokemonImagesRepository = new PokemonImagesRepository();
            var items = pokemonImagesRepository.GetAllData().Values;
            foreach (var item in items.First().Keys)
            {
                yield return new string[] { item };
            }
        }

        ~IntegrationTest()
        {
            JsonHelper.SaveObject(nameof(AllPokemonNames), this.shared.Trivias, Constants.TRIVIA_PATH);            
        }
        public class TestStateFixture
        {
            public List<Trivia> Trivias { get; set; } = new();
        }

    }
}
