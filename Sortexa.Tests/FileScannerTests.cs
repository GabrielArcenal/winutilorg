using Sortexa.Models;
using Sortexa.Services;
using System.Diagnostics;
using Xunit;

namespace Sortexa.Tests;

public sealed class FileScannerTests
{
    [Fact]
    public async Task ScanAsync_ReturnsMetadataForNestedFiles()
    {
        using var directory = new TemporaryDirectory();
        Directory.CreateDirectory(Path.Combine(directory.Path, "nested"));
        await File.WriteAllTextAsync(Path.Combine(directory.Path, "photo.JPG"), "image");
        await File.WriteAllTextAsync(Path.Combine(directory.Path, "nested", "notes.txt"), "notes");

        var result = await new FileScanner().ScanAsync(
            directory.Path,
            includeSubdirectories: true);

        Assert.False(result.IsCancelled);
        Assert.Equal(2, result.Files.Count);
        Assert.Contains(result.Files, file => file.Name == "photo.JPG" && file.Extension == ".JPG");
        Assert.Contains(result.Files, file => file.Name == "notes.txt");
    }

    [Fact]
    public async Task ScanAsync_ReportsMissingDirectoryAsSkipped()
    {
        var result = await new FileScanner().ScanAsync(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")));

        Assert.Empty(result.Files);
        Assert.Contains(result.SkippedPaths, path => path.Length > 0);
    }

    [Fact]
    public async Task ScanAsync_HonorsCancellation()
    {
        using var directory = new TemporaryDirectory();
        for (var index = 0; index < 100; index++)
        {
            await File.WriteAllTextAsync(Path.Combine(directory.Path, $"{index}.txt"), "content");
        }

        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        var result = await new FileScanner().ScanAsync(directory.Path, cancellationToken: cancellation.Token);

        Assert.True(result.IsCancelled);
    }

    [Fact]
    public void Categorizer_MatchesExtensionsWithoutCaseSensitivity()
    {
        var categorizer = new FileCategorizer(
        [
            FileCategory.Create("Images", [".jpg", "png"])
        ]);

        Assert.Equal("Images", categorizer.Categorize(
            new FileMetadata("C:\\photo.JPG", "photo.JPG", ".JPG", 10, DateTime.Now)));
        Assert.Equal("Other", categorizer.Categorize(
            new FileMetadata("C:\\archive.zip", "archive.zip", ".zip", 10, DateTime.Now)));
    }

    [Fact]
    public async Task ScanAsync_ScansSyntheticDatasetWithinExpectedTime()
    {
        using var directory = new TemporaryDirectory();
        for (var index = 0; index < 300; index++)
        {
            await File.WriteAllTextAsync(
                Path.Combine(directory.Path, $"file-{index:D3}.txt"),
                "synthetic test content");
        }

        var stopwatch = Stopwatch.StartNew();
        var result = await new FileScanner().ScanAsync(directory.Path);
        stopwatch.Stop();

        Assert.Equal(300, result.Files.Count);
        Assert.True(
            stopwatch.Elapsed < TimeSpan.FromSeconds(10),
            $"Scanning 300 synthetic files took {stopwatch.Elapsed.TotalSeconds:0.###} seconds.");
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
