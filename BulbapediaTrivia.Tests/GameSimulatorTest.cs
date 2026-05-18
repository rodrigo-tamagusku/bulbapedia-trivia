using BulbapediaTrivia.Const;
using BulbapediaTrivia.Service;
using Xunit;

namespace BulbapediaTrivia.Tests
{
    public class GameSimulatorTest
    {
        private PokemonTriviaRepository pokemonTriviaRepository;
        private PokemonImagesRepository pokemonImagesRepository;

        public GameSimulatorTest()
        {
            this.pokemonTriviaRepository = new PokemonTriviaRepository();
            this.pokemonImagesRepository = new PokemonImagesRepository();
        }
        [Fact]
        public async Task PokemonTriviaRepository_GetAllData()
        {
            var result = this.pokemonTriviaRepository.GetAllData();
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Single(result); //Change when more
            Assert.True(result.Values.First().Count > 1000);
        }
        [Fact]
        public async Task PokemonImagesRepository_GetAllData()
        {
            var result = this.pokemonImagesRepository.GetAllData();
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Single(result);
            Assert.Equal(Constants.MAX_POKEDEX_NUMBER, result.Values.First().Count);
        }
        [Fact]
        public async Task GetTriviaDataPath()
        {
            string path = this.pokemonTriviaRepository.FolderPath;
            Assert.EndsWith(Constants.TRIVIA_PATH, path);
        }
    }
}
