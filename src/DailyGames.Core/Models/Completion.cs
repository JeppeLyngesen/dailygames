namespace DailyGames.Core.Models;

/// <summary>One finished game on one calendar day (user's local date).</summary>
public sealed record Completion
{
    public required string GameId { get; init; }
    public required DateOnly Date { get; init; }
    /// <summary>Optional: did the user win/solve it (false = played but failed).</summary>
    public bool Solved { get; init; } = true;
    /// <summary>Optional free text, e.g. "3/6" or a pasted share result.</summary>
    public string? Note { get; init; }
}
