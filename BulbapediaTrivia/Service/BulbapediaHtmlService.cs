using System.Xml.Linq;
using HtmlAgilityPack;

namespace BulbapediaTrivia.Service
{
    public class BulbapediaHtmlService
    {
        private HtmlWeb web;
        private const string BULBAPEDIA_SPRITES_PAGED = "https://archives.bulbagarden.net/wiki/Category:HOME_menu_sprites";
        private const string BULBAPEDIA_BASE_URL = "https://archives.bulbagarden.net/";

        public BulbapediaHtmlService()
        {
            this.web = new HtmlWeb();
        }

        public Dictionary<int, string> GetAllPokemonThumbLinks()
        {
            return GetThumbPerPage(BULBAPEDIA_SPRITES_PAGED).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        // Use yield return to provide key-value pairs one at a time
        public IEnumerable<KeyValuePair<int, string>> GetThumbPerPage(string url)
        {
            HtmlDocument doc = web.Load(url);
            List<HtmlNode> nodeLinksPerPage = doc.DocumentNode.Descendants("img")
                           .Where(n => n.Attributes["src"]?.Value != null)
                           .ToList();
            foreach (HtmlNode node in nodeLinksPerPage)
            {
                string imageSrc = node.GetAttributeValue("src", "");
                if (!string.IsNullOrEmpty(imageSrc) && imageSrc.Contains("Menu_HOME"))
                {
                    string lastFour = imageSrc.Split(".png")[0][^4..];
                    if (int.TryParse(lastFour, out int pokedexNumber))
                    {
                        yield return new KeyValuePair<int, string>(pokedexNumber, imageSrc);
                    }
                    //else is a regional variant (galar, alolan, mega, etc)
                }
            }
            HtmlNode? nextPage = doc.DocumentNode.Descendants("a")
                           .Where(n => n.InnerText == "next page")
                           .FirstOrDefault();
            if (nextPage != null)
            {
                string href = nextPage.GetAttributeValue("href", "");
                string nextPageUrl = BULBAPEDIA_BASE_URL + href;
                GetThumbPerPage(nextPageUrl);
            }
        }
    }
}
