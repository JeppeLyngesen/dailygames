namespace DailyGames.Core.Models;

/// <summary>Everything a user has stored. Serialized to localStorage now, to SQL later (see db/schema.sql).</summary>
public sealed class TrackerState
{
    public const int CurrentVersion = 1;

    public int Version { get; set; } = CurrentVersion;
    /// <summary>Ids of the games the user follows, in display order.</summary>
    public List<string> FollowedGameIds { get; set; } = [];
    public List<Game> CustomGames { get; set; } = [];
    public List<Completion> Completions { get; set; } = [];
}
