using HtmlAgilityPack;

namespace BulbapediaTrivia.Service
{
    public class HtmlService
    {
        private HtmlWeb web;

        public HtmlService()
        {
            this.web = new HtmlWeb();
        }

        public Dictionary<int, string> GetAllImageLinks()
        {
            return GetLinks.GetAllImageLinks(GetImageOnlyLink);
        }

        public string GetImageOnlyLink(string url)
        {
            string imageName = url.Split("File:").Last();
            var doc = web.Load(url);
            var link = doc.DocumentNode.Descendants("a")
                           .Where(n => n.InnerText.Contains(imageName))
                           .FirstOrDefault();
            return link?.GetAttributeValue("href", "") ?? url;
        }
    }
}
