using DailyGames.Core.Models;
using DailyGames.Core.Services;

namespace DailyGames.Core.Tests;

public class StreakCalculatorTests
{
    private static readonly DateOnly Today = new(2026, 11, 15);
    private static DateOnly D(int daysAgo) => Today.AddDays(-daysAgo);

    [Fact]
    public void NoDays_IsEmpty() =>
        Assert.Equal(StreakInfo.Empty, StreakCalculator.Calculate([], Today));

    [Fact]
    public void DoneToday_CountsBackwards()
    {
        var s = StreakCalculator.Calculate([D(0), D(1), D(2), D(4)], Today);
        Assert.Equal(3, s.Current);
        Assert.True(s.DoneToday);
        Assert.Equal(4, s.TotalDays);
    }

    [Fact]
    public void NotDoneToday_StreakStillAliveFromYesterday()
    {
        var s = StreakCalculator.Calculate([D(1), D(2)], Today);
        Assert.Equal(2, s.Current);
        Assert.False(s.DoneToday);
    }

    [Fact]
    public void MissedYesterday_StreakIsBroken() =>
        Assert.Equal(0, StreakCalculator.Calculate([D(2), D(3)], Today).Current);

    [Fact]
    public void Longest_FindsBestRun()
    {
        var s = StreakCalculator.Calculate([D(0), D(10), D(11), D(12), D(13), D(20)], Today);
        Assert.Equal(4, s.Longest);
        Assert.Equal(1, s.Current);
    }

    [Fact]
    public void DuplicatesAndFutureDates_AreIgnored()
    {
        var s = StreakCalculator.Calculate([D(0), D(0), D(1), Today.AddDays(3)], Today);
        Assert.Equal(2, s.Current);
        Assert.Equal(2, s.TotalDays);
    }

    [Fact]
    public void PerfectDays_RequireAllFollowedGames()
    {
        var state = new TrackerState
        {
            FollowedGameIds = ["a", "b"],
            Completions =
            [
                new() { GameId = "a", Date = D(0) }, new() { GameId = "b", Date = D(0) },
                new() { GameId = "a", Date = D(1) }, new() { GameId = "b", Date = D(1) },
                new() { GameId = "a", Date = D(2) }, // b missing -> not perfect
            ],
        };
        var s = StreakCalculator.PerfectDays(state, Today);
        Assert.Equal(2, s.Current);
        Assert.Equal(2, s.TotalDays);
    }
}
