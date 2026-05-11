namespace BulbapediaTrivia.Tests
{
    public class GetLinksTest
    {
        [Test]
        public void GetAllImageLinks()
        {
            Dictionary<int, string> linksPokedex = GetLinks.GetAllImageLinks();
            foreach (var item in linksPokedex)
            {
                Console.WriteLine($"ID: {item.Key}, Name: {item.Value}");
            }
        }
    }
}
