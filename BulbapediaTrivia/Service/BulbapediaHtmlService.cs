using HtmlAgilityPack;

namespace BulbapediaTrivia.Service
{
    public class BulbapediaHtmlService
    {
        private HtmlWeb web;
        private const string BULBAPEDIA_SPRITES_PAGED = "https://archives.bulbagarden.net/wiki/Category:HOME_menu_sprites";
        private const string BULBAPEDIA_BASE_URL = "https://archives.bulbagarden.net";

        public BulbapediaHtmlService()
        {
            this.web = new HtmlWeb();
        }

        public Dictionary<int, string> GetAllPokemonThumbLinks()
        {
            return GetThumbPerPage(BULBAPEDIA_SPRITES_PAGED)
                        .GroupBy(kvp => kvp.Key)
                        .ToDictionary(g => g.Key, g => g.Last().Value);
        }

        // Use yield return to provide key-value pairs one at a time
        public IEnumerable<KeyValuePair<int, string>> GetThumbPerPage(string url)
        {
            HtmlDocument doc = web.Load(url);
            IEnumerable<HtmlNode> nodeLinksPerPage = doc.DocumentNode.Descendants("img")
                           .Where(n => n.Attributes["src"]?.Value != null);
            HtmlNode? nextPage = doc.DocumentNode.Descendants("a")
                   .FirstOrDefault(n => n.InnerText.Trim().ToLower() == "next page");
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
            if (nextPage != null)
            {
                string href = nextPage.GetAttributeValue("href", "");
                string nextPageUrl = GetNextPageUrl(href);
                foreach (var pair in GetThumbPerPage(nextPageUrl))
                {
                    yield return pair;
                }
            }
        }

        private static string GetNextPageUrl(string href)
        {
            string[] fileFrom = href.Split("filefrom");
            string subRoute = href;
            if (fileFrom.Length == 2)
            {
                string appendBlock = fileFrom.Last().Split("#").First();
                subRoute = fileFrom[0] + "filefrom"+ appendBlock + "&filefrom"+ fileFrom[1];
            }
            return BULBAPEDIA_BASE_URL + subRoute;
        }
    }
}
