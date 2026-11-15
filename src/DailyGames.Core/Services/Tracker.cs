using DailyGames.Core.Models;

namespace DailyGames.Core.Services;

/// <summary>All rules for changing the state live here, so the UI stays thin and the rules are unit-tested.</summary>
public sealed class Tracker(TrackerState state, IReadOnlyList<Game> catalog)
{
    public TrackerState State { get; } = state;

    public IEnumerable<Game> AllGames => catalog.Concat(State.CustomGames);

    public IReadOnlyList<Game> FollowedGames =>
        State.FollowedGameIds
            .Select(id => AllGames.FirstOrDefault(g => g.Id == id))
            .OfType<Game>()
            .ToList();

    public bool IsFollowed(string gameId) => State.FollowedGameIds.Contains(gameId);

    public void Follow(string gameId)
    {
        if (!IsFollowed(gameId) && AllGames.Any(g => g.Id == gameId))
            State.FollowedGameIds.Add(gameId);
    }

    public void Unfollow(string gameId) => State.FollowedGameIds.Remove(gameId);

    public void Move(string gameId, int offset)
    {
        var list = State.FollowedGameIds;
        var index = list.IndexOf(gameId);
        var target = index + offset;
        if (index < 0 || target < 0 || target >= list.Count) return;
        (list[index], list[target]) = (list[target], list[index]);
    }

    public bool IsCompleted(string gameId, DateOnly date) =>
        State.Completions.Any(c => c.GameId == gameId && c.Date == date);

    /// <summary>Toggles completion. Returns true if the game is now marked completed.</summary>
    public bool Toggle(string gameId, DateOnly date)
    {
        var removed = State.Completions.RemoveAll(c => c.GameId == gameId && c.Date == date);
        if (removed > 0) return false;
        State.Completions.Add(new Completion { GameId = gameId, Date = date });
        return true;
    }

    public Game AddCustomGame(string name, string url)
    {
        name = name.Trim();
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
        if (!Uri.TryCreate(url.Trim(), UriKind.Absolute, out var uri) || (uri.Scheme != "https" && uri.Scheme != "http"))
            throw new ArgumentException("URL must start with http:// or https://", nameof(url));

        var game = new Game
        {
            Id = "custom-" + Guid.NewGuid().ToString("N")[..8],
            Name = name,
            Url = uri.ToString(),
            Category = "Custom",
            IsCustom = true,
        };
        State.CustomGames.Add(game);
        Follow(game.Id);
        return game;
    }

    public void RemoveCustomGame(string gameId)
    {
        State.CustomGames.RemoveAll(g => g.Id == gameId);
        State.FollowedGameIds.Remove(gameId);
        State.Completions.RemoveAll(c => c.GameId == gameId);
    }

    public int CompletedCount(DateOnly date) =>
        FollowedGames.Count(g => IsCompleted(g.Id, date));
}
