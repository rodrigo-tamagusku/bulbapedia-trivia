using HtmlAgilityPack;

namespace BulbapediaTrivia.Service
{
    public class PokemonDbNetHtmlService
    {
        private HtmlWeb web;
        private const string WEB_SPRITES = "https://pokemondb.net/sprites";

        public PokemonDbNetHtmlService()
        {
            this.web = new HtmlWeb();
        }

        public Dictionary<string, string> GetAllPokemonThumbLinks()
        {
            return GetThumbPerPage(WEB_SPRITES)
                        .GroupBy(kvp => kvp.Key)
                        .ToDictionary(g => g.Key, g => g.Last().Value);
        }

        // Use yield return to provide key-value pairs one at a time
        public IEnumerable<KeyValuePair<string, string>> GetThumbPerPage(string url)
        {
            HtmlDocument doc = web.Load(url);
            IEnumerable<HtmlNode> nodeLinksPerPage = doc.DocumentNode.Descendants("img")
                           .Where(n => n.Attributes["src"]?.Value != null);
            foreach (HtmlNode node in nodeLinksPerPage)
            {
                string imageSrc = node.GetAttributeValue("src", "");
                KeyValuePair<string, string>? valuePair = ValuePairFromSrc(imageSrc);
                if (valuePair != null)
                    yield return valuePair.Value;
            }
        }

        public KeyValuePair<string, string>? ValuePairFromSrc(string? imageSrc)
        {
            if (!string.IsNullOrEmpty(imageSrc) && imageSrc.Contains("sprites"))
            {
                string pokemonName = imageSrc.Split(".png").First().Split("/").Last();
                return new KeyValuePair<string, string>(pokemonName, imageSrc);
            }
            return null;
        }
    }
}
