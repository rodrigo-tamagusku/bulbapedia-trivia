using System.Text.Json;
using BulbapediaTrivia.Const;
using BulbapediaTrivia.Model;

namespace BulbapediaTrivia.Service
{
    public class PokemonImagesRepository : JsonFolderRepository<Dictionary<string, string>>
    {
        public override string FolderPath
        {
            get
            {
                // Logic moved from your GetTriviaDataPath method
                string relativePath = Path.Combine(@"..\..\..\..\", Constants.IMAGES_PATH);
                return Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), relativePath));
            }
        }
    }
}
