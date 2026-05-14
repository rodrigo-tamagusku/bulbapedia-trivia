using System.Text.Json.Serialization;

namespace BulbapediaTrivia.Model
{
    public record WikipediaResponse(
        [property: JsonPropertyName("warnings")] Warnings? Warnings,
        [property: JsonPropertyName("batchcomplete")] string? BatchComplete,
        [property: JsonPropertyName("query")] Query? Query
    );

    public record Warnings(
        [property: JsonPropertyName("main")] Dictionary<string, string>? Main
    );

    public record Query(
        [property: JsonPropertyName("normalized")] List<NormalizedMapping>? Normalized,
        // Using a Dictionary because "2457" is a dynamic Page ID
        [property: JsonPropertyName("pages")] Dictionary<string, PageDetail>? Pages
    );

    public record NormalizedMapping(
        [property: JsonPropertyName("from")] string? From,
        [property: JsonPropertyName("to")] string? To
    );

    public record PageDetail(
        [property: JsonPropertyName("pageid")] int PageId,
        [property: JsonPropertyName("ns")] int Ns,
        [property: JsonPropertyName("title")] string? Title,
        [property: JsonPropertyName("extract")] string? Extract
    );
}
