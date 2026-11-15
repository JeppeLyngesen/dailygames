using System.Net.Http.Json;
using System.Text.Json;
using DailyGames.Core.Models;
using DailyGames.Core.Services;
using Microsoft.JSInterop;

namespace DailyGames.Web.Services;

/// <summary>
/// Loads the game catalog and the user's data, and saves changes to localStorage.
/// Swap the storage part for an API call later (phase 2) without touching the pages.
/// </summary>
public sealed class TrackerService(HttpClient http, IJSRuntime js)
{
    private const string StorageKey = "dailygames.state.v1";
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web) { WriteIndented = false };
    private static readonly string[] DefaultFollowed = ["wordle", "connections", "strands", "mini-crossword"];

    private Tracker? _tracker;
    private Task? _loading;

    public event Action? Changed;

    public Tracker Tracker => _tracker ?? throw new InvalidOperationException("Call EnsureLoadedAsync first.");

    public static DateOnly Today => DateOnly.FromDateTime(DateTime.Now);

    public Task EnsureLoadedAsync() => _loading ??= LoadAsync();

    private async Task LoadAsync()
    {
        var catalog = await http.GetFromJsonAsync<List<Game>>("data/games.json", Json) ?? [];
        var state = await ReadStateAsync();
        if (state is null)
        {
            state = new TrackerState();
            state.FollowedGameIds.AddRange(DefaultFollowed.Where(id => catalog.Any(g => g.Id == id)));
        }
        _tracker = new Tracker(state, catalog);
    }

    private async Task<TrackerState?> ReadStateAsync()
    {
        try
        {
            var raw = await js.InvokeAsync<string?>("localStorage.getItem", StorageKey);
            return string.IsNullOrEmpty(raw) ? null : JsonSerializer.Deserialize<TrackerState>(raw, Json);
        }
        catch (Exception ex) when (ex is JsonException or JSException)
        {
            Console.Error.WriteLine($"Could not read saved data: {ex.Message}");
            return null;
        }
    }

    public async Task SaveAsync()
    {
        await js.InvokeVoidAsync("localStorage.setItem", StorageKey, JsonSerializer.Serialize(Tracker.State, Json));
        Changed?.Invoke();
    }

    public string Export() => JsonSerializer.Serialize(Tracker.State, new JsonSerializerOptions(Json) { WriteIndented = true });

    public async Task ImportAsync(string json)
    {
        var state = JsonSerializer.Deserialize<TrackerState>(json, Json)
                    ?? throw new JsonException("The file is empty.");
        Tracker.State.FollowedGameIds = state.FollowedGameIds;
        Tracker.State.CustomGames = state.CustomGames;
        Tracker.State.Completions = state.Completions;
        await SaveAsync();
    }
}
