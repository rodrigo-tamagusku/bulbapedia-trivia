using BulbapediaTrivia.Const;
using BulbapediaTrivia.Service;
using Xunit;

namespace BulbapediaTrivia.Tests
{
    public class PokemonDbNetHtmlServiceTests
    {
        private PokemonDbNetHtmlService pokemonDbNetHtmlService;

        public PokemonDbNetHtmlServiceTests()
        {
            this.pokemonDbNetHtmlService = new PokemonDbNetHtmlService();
        }

        [Fact]
        public async Task GetImages()
        {
            var links = this.pokemonDbNetHtmlService.GetAllPokemonThumbLinks();
            JsonHelper.SaveObject("PokemonIcons", links, Constants.TRIVIA_PATH);
        }
    }
}
