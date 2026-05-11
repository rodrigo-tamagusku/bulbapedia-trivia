using System.Text.Json;
using BulbapediaTrivia.Service;

namespace BulbapediaTrivia.Tests
{
    public class GetLinksTest
    {
        [Test]
        public void GetAllImageLinks()
        {
            HtmlService httpService = new HtmlService();
            Dictionary<int, string> linksPokedex = httpService.GetAllImageLinks();

            string jsonString = JsonSerializer.Serialize(linksPokedex, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("PokedexImageLinks.json", jsonString);
        }
    }
}
