namespace DailyGames.Core.Models;

/// <param name="Current">Consecutive days up to today, or up to yesterday if today isn't done yet (streak is still "alive").</param>
/// <param name="Longest">Longest run of consecutive days ever.</param>
/// <param name="TotalDays">Number of distinct days with a completion.</param>
/// <param name="DoneToday">Whether today is already completed.</param>
public sealed record StreakInfo(int Current, int Longest, int TotalDays, bool DoneToday)
{
    public static readonly StreakInfo Empty = new(0, 0, 0, false);
}
