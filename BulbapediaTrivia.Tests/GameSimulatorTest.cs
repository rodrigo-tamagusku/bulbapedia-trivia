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

        }
        [Fact]
        public async Task GetTriviaDataPath()
        {
            string path = this.pokemonTriviaRepository.GetTriviaDataPath();
            Assert.EndsWith(Constants.TRIVIA_PATH, path);
        }
    }
}
