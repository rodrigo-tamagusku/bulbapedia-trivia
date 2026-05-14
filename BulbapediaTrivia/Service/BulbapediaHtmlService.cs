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
                KeyValuePair<int, string>? valuePair = ValuePairFromSrc(imageSrc);
                if (valuePair != null)
                    yield return valuePair.Value;
            }
            if (nextPage != null)
            {
                string href = nextPage.GetAttributeValue("href", "");
                string nextPageUrl = GetNextPageUrl(href);
                foreach (KeyValuePair<int, string> pair in GetThumbPerPage(nextPageUrl))
                {
                    yield return pair;
                }
            }
        }

        public KeyValuePair<int, string>? ValuePairFromSrc(string? imageSrc)
        {
            if (!string.IsNullOrEmpty(imageSrc) && imageSrc.Contains("Menu_HOME"))
            {
                string lastFour = imageSrc.Split(".png")[0][^4..];
                if (int.TryParse(lastFour, out int pokedexNumber))
                {
                    return new KeyValuePair<int, string>(pokedexNumber, imageSrc);
                }
                //else is a regional variant (galar, alolan, mega, etc)
            }
            return null;
        }

        private static string GetNextPageUrl(string href)
        {
            string[] fileFrom = href.Split("filefrom");
            string subRoute = href;
            if (fileFrom.Length == 2)
            {
                string appendBlock = fileFrom.Last().Split("#").First();
                subRoute = fileFrom[0] + "filefrom" + appendBlock + "&filefrom" + fileFrom[1];
            }
            return BULBAPEDIA_BASE_URL + subRoute;
        }

        public Dictionary<int, string> GetExamplePokemonThumbLinks()
        {
            return new Dictionary<int, string>() {
                { 1, "https://archives.bulbagarden.net/media/upload/7/70/Menu_HOME_0001.png" } ,
                { 2, "https://archives.bulbagarden.net/media/upload/b/b7/Menu_HOME_0002.png" },
                { 3, "https://archives.bulbagarden.net/media/upload/9/99/Menu_HOME_0003.png" },
                { 4, "https://archives.bulbagarden.net/media/upload/9/9c/Menu_HOME_0004.png" },
                { 5, "https://archives.bulbagarden.net/media/upload/1/1b/Menu_HOME_0005.png" },
                { 6, "https://archives.bulbagarden.net/media/upload/b/b7/Menu_HOME_0006.png" },
                { 7, "https://archives.bulbagarden.net/media/upload/1/18/Menu_HOME_0007.png" },
                { 8, "https://archives.bulbagarden.net/media/upload/9/97/Menu_HOME_0008.png" },
                { 9, "https://archives.bulbagarden.net/media/upload/9/9e/Menu_HOME_0009.png" }
            };
        }
    }
}
