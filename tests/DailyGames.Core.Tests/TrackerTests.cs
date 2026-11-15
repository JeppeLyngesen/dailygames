using DailyGames.Core.Models;
using DailyGames.Core.Services;

namespace DailyGames.Core.Tests;

public class TrackerTests
{
    private static readonly DateOnly Today = new(2026, 11, 15);
    private static readonly Game[] Catalog =
    [
        new() { Id = "wordle", Name = "Wordle", Url = "https://example.com/w" },
        new() { Id = "connections", Name = "Connections", Url = "https://example.com/c" },
    ];

    private static Tracker NewTracker() => new(new TrackerState(), Catalog);

    [Fact]
    public void Toggle_AddsThenRemoves()
    {
        var t = NewTracker();
        Assert.True(t.Toggle("wordle", Today));
        Assert.True(t.IsCompleted("wordle", Today));
        Assert.False(t.Toggle("wordle", Today));
        Assert.Empty(t.State.Completions);
    }

    [Fact]
    public void Follow_IgnoresUnknownAndDuplicates()
    {
        var t = NewTracker();
        t.Follow("wordle");
        t.Follow("wordle");
        t.Follow("does-not-exist");
        Assert.Equal(["wordle"], t.State.FollowedGameIds);
    }

    [Fact]
    public void Move_SwapsOrder()
    {
        var t = NewTracker();
        t.Follow("wordle");
        t.Follow("connections");
        t.Move("connections", -1);
        Assert.Equal(["connections", "wordle"], t.State.FollowedGameIds);
        t.Move("connections", -1); // already first: no-op
        Assert.Equal(["connections", "wordle"], t.State.FollowedGameIds);
    }

    [Fact]
    public void AddCustomGame_ValidatesAndFollows()
    {
        var t = NewTracker();
        var g = t.AddCustomGame("  My Game ", "https://mygame.example");
        Assert.Equal("My Game", g.Name);
        Assert.True(t.IsFollowed(g.Id));
        Assert.Throws<ArgumentException>(() => t.AddCustomGame("x", "javascript:alert(1)"));
        Assert.Throws<ArgumentException>(() => t.AddCustomGame(" ", "https://ok.example"));
    }

    [Fact]
    public void RemoveCustomGame_CleansUpEverything()
    {
        var t = NewTracker();
        var g = t.AddCustomGame("Mine", "https://mine.example");
        t.Toggle(g.Id, Today);
        t.RemoveCustomGame(g.Id);
        Assert.Empty(t.State.CustomGames);
        Assert.Empty(t.State.FollowedGameIds);
        Assert.Empty(t.State.Completions);
    }
}
