namespace BulbapediaTrivia
{
    public static class GetLinks
    {
        private static readonly int MAX_POKEDEX_SIZE = 1025;

        public static Dictionary<int, string> GetAllImageLinks(Func<string, string> getImageOnlyLink)
        {
            var dict = new Dictionary<int, string> { };

            for (int i = 1; i <= MAX_POKEDEX_SIZE; i++)
            {
                string link = GetIconFromPokedexNumber(i);
                string imageOnlyLink = getImageOnlyLink(link);
                dict.Add(i, imageOnlyLink);
                Console.WriteLine(imageOnlyLink);
            }
            return dict;
        }

        private static string GetIconFromPokedexNumber(int i)
        {
            string numberXXXX = i.ToString("0000");
            return $"https://archives.bulbagarden.net/wiki/File:Menu_HOME_{numberXXXX}.png";
        }
    }
}
