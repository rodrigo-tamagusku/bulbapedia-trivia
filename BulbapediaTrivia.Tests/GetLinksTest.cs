using System.Text.Json;
using BulbapediaTrivia.Service;

namespace BulbapediaTrivia.Tests
{
    public class GetLinksTest
    {
        [Test]
        public void GetAllImageLinks()
        {
            BulbapediaHtmlService httpService = new BulbapediaHtmlService();
            Dictionary<int, string> linksPokedex = httpService.GetAllPokemonThumbLinks();

            string jsonString = JsonSerializer.Serialize(linksPokedex, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("PokedexImageLinks.json", jsonString);
            ///https://archives.bulbagarden.net/wiki/Category:HOME_menu_sprites
        }
    }
}
