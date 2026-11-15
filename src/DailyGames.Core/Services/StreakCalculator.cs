using DailyGames.Core.Models;

namespace DailyGames.Core.Services;

public static class StreakCalculator
{
    /// <summary>Calculates streaks from a set of days. Duplicates and order don't matter.</summary>
    public static StreakInfo Calculate(IEnumerable<DateOnly> days, DateOnly today)
    {
        var set = days.Where(d => d <= today).ToHashSet();
        if (set.Count == 0) return StreakInfo.Empty;

        var doneToday = set.Contains(today);

        // Current streak: count backwards from today (or yesterday if today isn't done yet).
        var cursor = doneToday ? today : today.AddDays(-1);
        var current = 0;
        while (set.Contains(cursor))
        {
            current++;
            cursor = cursor.AddDays(-1);
        }

        // Longest streak: only start counting at the first day of each run.
        var longest = 0;
        foreach (var day in set)
        {
            if (set.Contains(day.AddDays(-1))) continue;
            var length = 1;
            while (set.Contains(day.AddDays(length))) length++;
            longest = Math.Max(longest, length);
        }

        return new StreakInfo(current, longest, set.Count, doneToday);
    }

    /// <summary>Streak for a single game.</summary>
    public static StreakInfo ForGame(TrackerState state, string gameId, DateOnly today) =>
        Calculate(state.Completions.Where(c => c.GameId == gameId).Select(c => c.Date), today);

    /// <summary>"Perfect day" streak: days where ALL currently followed games were completed.</summary>
    public static StreakInfo PerfectDays(TrackerState state, DateOnly today)
    {
        var followed = state.FollowedGameIds.ToHashSet();
        if (followed.Count == 0) return StreakInfo.Empty;

        var perfectDays = state.Completions
            .Where(c => followed.Contains(c.GameId))
            .GroupBy(c => c.Date)
            .Where(g => g.Select(c => c.GameId).Distinct().Count() == followed.Count)
            .Select(g => g.Key);

        return Calculate(perfectDays, today);
    }
}
