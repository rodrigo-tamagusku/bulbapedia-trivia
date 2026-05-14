using BulbapediaTrivia.Service;
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit.v3;
using System.Text.Json;
using Xunit;

namespace BulbapediaTrivia.Tests
{
    public class GetLinksTest : PlaywrightTest
    {
        private const string SELECTOR_IMAGE_SRC = "img[src]";
        private const bool HEADLESS = false;
        private const string BULBAPEDIA_SPRITES_PAGED = "https://archives.bulbagarden.net/wiki/Category:HOME_menu_sprites";
        private const string BULBAPEDIA_BASE_URL = "https://archives.bulbagarden.net";
        private BulbapediaHtmlService httpService;
        private Dictionary<int, string> linksPokedex;

        public GetLinksTest()
        {
            this.httpService = new BulbapediaHtmlService();
            this.linksPokedex = new();
        }

        [Fact]
        public async Task GetAllImageLinks()
        {
            // "C:\Program Files\Google\Chrome\Application\chrome.exe" --remote-debugging-port=9222 --user-data-dir="C:\temp\playwright_profile"
            var browser = await Playwright.Chromium.ConnectOverCDPAsync("http://localhost:9222");

            //await using var browser = await Playwright.Chromium.LaunchPersistentContextAsync(Directory.GetCurrentDirectory(),
            //    new BrowserTypeLaunchPersistentContextOptions
            //    {
            //        Channel = "chrome",
            //        Headless = HEADLESS, // Set to false to see the browser
            //        SlowMo = 50,     // Optional: slow down operations by 50ms to see what's happening
            //        Args = new[] { "--disable-infobars" }, // Legacy method for some Chromium versions
            //        IgnoreDefaultArgs = new[] { "--enable-automation" }
            //    });
            var page = await browser.NewPageAsync();
            await page.GotoAsync(BULBAPEDIA_SPRITES_PAGED);
            await page.PauseAsync();

            var imageLocators = page.Locator(SELECTOR_IMAGE_SRC);

            // Get all matching elements as a list of locators
            var allImages = await imageLocators.AllAsync();

            foreach (var img in allImages)
            {
                // Retrieve the 'src' attribute value for each image
                string? imageSrc = await img.GetAttributeAsync("src");
                KeyValuePair<int, string>? valuePair = httpService.ValuePairFromSrc(imageSrc);
                if (valuePair != null)
                    linksPokedex.Append(valuePair.Value);
            }

            string jsonString = JsonSerializer.Serialize(linksPokedex, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("PokedexImageLinks.json", jsonString);
        }
    }
}
