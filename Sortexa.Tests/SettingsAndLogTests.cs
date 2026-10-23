using Sortexa.Models;
using Sortexa.Storage;
using Xunit;

namespace Sortexa.Tests;

public sealed class SettingsAndLogTests
{
    [Fact]
    public async Task SettingsStore_RoundTripsSettings()
    {
        using var directory = new TemporaryDirectory();
        var store = new SettingsStore(Path.Combine(directory.Path, "settings.json"));
        var expected = new AppSettings(false, true, 25, "{index}-{name}");

        await store.SaveAsync(expected);

        Assert.Equal(expected, await store.LoadAsync());
    }

    [Fact]
    public async Task ActivityLogStore_RetainsNewestEntriesWithinLimit()
    {
        using var directory = new TemporaryDirectory();
        var store = new ActivityLogStore(Path.Combine(directory.Path, "logs", "activity.json"));

        for (var index = 0; index < 3; index++)
        {
            await store.AppendAsync(
                new ActivityEntry(DateTimeOffset.UtcNow.AddMinutes(index), "Test", index.ToString(), "details"),
                2);
        }

        var entries = await store.LoadAsync();
        Assert.Equal(2, entries.Count);
        Assert.Equal("2", entries[0].Status);
    }

    private sealed class TemporaryDirectory : IDisposable
    {
        public TemporaryDirectory()
        {
            Path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"SortexaTests-{Guid.NewGuid():N}");
            Directory.CreateDirectory(Path);
        }

        public string Path { get; }

        public void Dispose()
        {
            if (Directory.Exists(Path))
            {
                Directory.Delete(Path, recursive: true);
            }
        }
    }
}
