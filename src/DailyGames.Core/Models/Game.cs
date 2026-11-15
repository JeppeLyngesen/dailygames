namespace DailyGames.Core.Models;

/// <summary>A daily game that users can track. Catalog games come from games.json; users can add custom ones.</summary>
public sealed record Game
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Url { get; init; }
    public string Category { get; init; } = "Other";
    public string Description { get; init; } = "";
    public bool IsCustom { get; init; }
}
