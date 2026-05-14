using System.Text;
using BulbapediaTrivia.Model;

namespace BulbapediaTrivia.Service
{
    public class TextProcessorService
    {
        public List<Trivia> GetTriviaFromPageContent(string name, string wikiText)
        {
            List<Trivia> trivias = new List<Trivia>();
            using var reader = new StringReader(wikiText);
            string? line;

            string currentHeader = "Introduction"; // Default header if text starts without one
            var currentContent = new StringBuilder();

            while ((line = reader.ReadLine()) != null)
            {
                string trimmedLine = line.Trim();

                // Check if the line is a header (starts and ends with at least ==)
                if (trimmedLine.StartsWith("==") && trimmedLine.EndsWith("=="))
                {
                    // 1. Flush the previous section if it has content
                    trivias.AddRange(FlushSection(currentHeader, currentContent.ToString(), name));

                    // 2. Extract the new header name by trimming off the '=' characters
                    currentHeader = trimmedLine.Trim('=').Trim();

                    // 3. Reset the content accumulator for the next section
                    currentContent.Clear();
                }
                else
                {
                    // Accumulate the body text line
                    if (!string.IsNullOrWhiteSpace(trimmedLine))
                    {
                        currentContent.AppendLine(trimmedLine);
                    }
                }
            }

            // Don't forget to flush the very last section after the loop ends!
            trivias.AddRange(FlushSection(currentHeader, currentContent.ToString(), name));
            return trivias;
        }

        private static IEnumerable<Trivia> FlushSection(string header, string content, string name)
        {
            if (header == "Trivia")
            {
                string[] lines = content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    Trivia trivia = new Trivia()
                    {
                        Pokemon = name,
                        Fact = line
                    };
                    yield return trivia;
                }
            }
        }
    }
}
