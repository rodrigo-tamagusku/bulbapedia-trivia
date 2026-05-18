using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace BulbapediaTrivia
{
    public static class JsonHelper
    {
        public static void SaveObject(string name, object someJsonCompatibleObject, string folderPath)
        {
            string jsonString = JsonSerializer.Serialize(someJsonCompatibleObject,
                new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                    WriteIndented = true
                });
            string filename = $"{name}.json";
            string current = Directory.GetCurrentDirectory();
            string directory = Path.Combine(current, folderPath);
            string fullPath = Path.Combine(directory, filename);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            File.WriteAllText(fullPath, jsonString);
        }
    }
}
