namespace BulbapediaTrivia
{
    public static class GetLinks
    {
        private static readonly int MAX_POKEDEX_SIZE = 1025;

        public static Dictionary<int, string> GetAllImageLinks()
        {
            var dict = new Dictionary<int, string> { };

            for (int i = 0; i <= MAX_POKEDEX_SIZE; i++)
            {
                string link = GetIconeFromPokedexNumber(i);
                dict.Add(i, link);
            }
            return dict;
        }

        private static string GetIconeFromPokedexNumber(int i)
        {
            string numberXXXX = i.ToString("0000");
            return $"https://archives.bulbagarden.net/wiki/File:Menu_HOME_{numberXXXX}.png";
        }
    }
}
