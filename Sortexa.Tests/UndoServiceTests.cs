using Sortexa.Models;
using Sortexa.Services;
using Sortexa.Storage;
using Xunit;

namespace Sortexa.Tests;

public sealed class UndoServiceTests
{
    [Fact]
    public async Task UndoLatestAsync_RestoresMovedFile()
    {
        using var directory = new TemporaryDirectory();
        var original = Path.Combine(directory.Path, "original.txt");
        var current = Path.Combine(directory.Path, "output", "original.txt");
        Directory.CreateDirectory(Path.GetDirectoryName(current)!);
        await File.WriteAllTextAsync(current, "content");
        var store = new OperationStore(Path.Combine(directory.Path, "operations.json"));
        await store.SaveAsync(
        [
            new OperationRecord(
                Guid.NewGuid(),
                DateTimeOffset.UtcNow,
                [new OperationMove(original, current)],
                [],
                false)
        ]);

        var result = await new UndoService(store).UndoLatestAsync();

        Assert.Single(result.RestoredPaths);
        Assert.Empty(result.Conflicts);
        Assert.True(File.Exists(original));
        Assert.False(File.Exists(current));
    }

    [Fact]
    public async Task UndoLatestAsync_DoesNotOverwriteAnOccupiedOriginal()
    {
        using var directory = new TemporaryDirectory();
        var original = Path.Combine(directory.Path, "original.txt");
        var current = Path.Combine(directory.Path, "output", "original.txt");
        Directory.CreateDirectory(Path.GetDirectoryName(current)!);
        await File.WriteAllTextAsync(original, "newer");
        await File.WriteAllTextAsync(current, "moved");
        var store = new OperationStore(Path.Combine(directory.Path, "operations.json"));
        await store.SaveAsync(
        [
            new OperationRecord(
                Guid.NewGuid(),
                DateTimeOffset.UtcNow,
                [new OperationMove(original, current)],
                [],
                false)
        ]);

        var result = await new UndoService(store).UndoLatestAsync();

        Assert.Empty(result.RestoredPaths);
        Assert.Single(result.Conflicts);
        Assert.Equal("newer", await File.ReadAllTextAsync(original));
        Assert.True(File.Exists(current));
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
