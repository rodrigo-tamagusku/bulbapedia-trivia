using BulbapediaTrivia.Const;
using BulbapediaTrivia.Service;
using Xunit;

namespace BulbapediaTrivia.Tests
{
    public class MediaWikiServiceTest
    {
        private HttpClient httpClient;
        private MediaWikiService mediaWikiService;

        public MediaWikiServiceTest()
        {
            this.httpClient = new HttpClient();
            this.mediaWikiService = new MediaWikiService(Constants.BULBAPEDIA_WIKI, httpClient);
        }

        [Theory]
        [InlineData("Ditto_(Pokémon)")]
        [InlineData("Metagross_(Pokémon)")]
        public async Task FullPagePlainTextQuery(string pageTitle)
        {
            var result = await this.mediaWikiService.FullPagePlainTextQuery(pageTitle);
        }
    }
}
