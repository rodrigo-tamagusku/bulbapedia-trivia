using BulbapediaTrivia.Const;
using BulbapediaTrivia.Model;
using BulbapediaTrivia.Service;
using Xunit;

namespace BulbapediaTrivia.Tests
{
    public class TextProcessorServiceTest
    {
        private TextProcessorService textProcessorService;
        private const string METAGROSS_EXTRACT =
            "=== Pokémon UNITE ===\r\nMain article: Metagross (UNITE)\r\nMetagross is playable through obtaining a Unite License. It is a melee all-rounder that starts as Beldum and evolves into Metang at level 5, which evolves into Metagross at level 9.\r\n\r\n\r\n== Trivia ==\r\n\r\nThe Metagross family are the only Pokémon with a catch rate of 3 that are not Legendary or Mythical Pokémon.\r\nDue to the way damage is calculated for Grass Knot and Low Kick, Metagross will take the same amount of damage from these moves with or without its Hidden Ability, Light Metal.\r\nMetagross's number in the Hoenn Pokédex in Generation III and the Fiore Browser are the same: 192.\r\nDespite being a gender unknown species, Metagross is referred to as a male in PokéPark Wii: Pikachu's Adventure.\r\nMetagross and its pre-evolved forms all share both their standard and Hidden Abilities with Registeel.\r\nMega Metagross is the heaviest Mega Evolved Pokémon.\r\nMetagross is also the only pseudo-Legendary Pokémon that, in its debut generation, had no members of its evolutionary line able to be encountered in the wild.\r\n\r\n\r\n=== Origin ===\r\nMetagross appears to be based on a robot. It also appears to be based on a supercomputer, given its mechanical structure and sheer intelligence, and a spider. Additionally, it is reminiscent of a UFO and a lander. Metagross's face may draw inspiration from the largest gear of the Antikythera mechanism, an ancient analog computer.";

        public TextProcessorServiceTest()
        {
            this.textProcessorService = new TextProcessorService();
        }

        [Theory]
        [InlineData(METAGROSS_EXTRACT)]
        public async Task FullPagePlainTextQuery(string extract)
        {
            List<Trivia> result = this.textProcessorService.GetHeaderFromPageContent("Metagross", extract, Constants.TRIVIA_HEADER);
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}
