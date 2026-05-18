using BulbapediaTrivia.Const;
using BulbapediaTrivia.Service;
using Xunit;

namespace BulbapediaTrivia.Tests
{
    public class GameSimulatorTest
    {
        private PokemonTriviaRepository pokemonTriviaRepository;

        public GameSimulatorTest()
        {
            this.pokemonTriviaRepository = new PokemonTriviaRepository();
        }
        [Fact]
        public async Task GetTriviaFromJson()
        {
            var result = this.pokemonTriviaRepository.GetTriviaFromJson();
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Single(result); //Change when more
            Assert.True(result.Values.First().Count > 1000); 
        }
        [Fact]
        public async Task GetTriviaDataPath()
        {
            string path = this.pokemonTriviaRepository.GetTriviaDataPath();
            Assert.EndsWith(Constants.TRIVIA_PATH, path);
        }
    }
}
