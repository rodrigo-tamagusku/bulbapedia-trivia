using System.Net.Http.Headers;
using System.Net.Http.Json;
using BulbapediaTrivia.Model;

namespace BulbapediaTrivia.Service
{
    public class MediaWikiService
    {
        private readonly string domain = "";
        private readonly string query;
        private readonly HttpClient httpClient;
        private readonly string contact = "nimbus.soneca@gmail.com";

        public MediaWikiService(string domain, HttpClient httpClient)
        {
            this.domain = domain;
            this.query = "/w/api.php?action=query&prop=extracts&explaintext=1&titles={0}&sectiontitle=Trivia&format=json";
            this.httpClient = httpClient;
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd($"MyPokemonApp/1.0 ({contact})");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<WikipediaResponse?> FullPagePlainTextQuery(string title)
        {
            string url = domain + string.Format(query, title);
            WikipediaResponse? result = await this.httpClient.GetFromJsonAsync<WikipediaResponse>(url);
            return result;
        }

    }
}
