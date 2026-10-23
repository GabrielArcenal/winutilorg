using Sortexa.Models;
using Sortexa.Services;
using Xunit;

namespace Sortexa.Tests;

public sealed class OrganizationServiceTests
{
    [Fact]
    public void CreatePlan_DetectsExistingDestinationWithoutOverwriting()
    {
        using var directory = new TemporaryDirectory();
        var sourcePath = Path.Combine(directory.Path, "source.txt");
        var destinationRoot = Path.Combine(directory.Path, "output");
        Directory.CreateDirectory(Path.Combine(destinationRoot, "Documents"));
        File.WriteAllText(sourcePath, "source");
        File.WriteAllText(Path.Combine(destinationRoot, "Documents", "source.txt"), "existing");

        var file = FileMetadata.FromFileInfo(new FileInfo(sourcePath));
        var plan = new OrganizationService().CreatePlan(
            [file],
            destinationRoot,
            _ => "Documents");

        var item = Assert.Single(plan.Items);
        Assert.True(item.IsConflict);
        Assert.Equal("Destination already exists", item.ConflictReason);
    }

    [Fact]
    public void CreatePlan_DetectsDuplicateDestinations()
    {
        using var directory = new TemporaryDirectory();
        var firstPath = Path.Combine(directory.Path, "one.txt");
        var secondPath = Path.Combine(directory.Path, "two.txt");
        File.WriteAllText(firstPath, "one");
        File.WriteAllText(secondPath, "two");

        var files = new[]
        {
            new FileMetadata(firstPath, "same.txt", ".txt", 3, DateTime.Now),
            new FileMetadata(secondPath, "same.txt", ".txt", 3, DateTime.Now)
        };
        var plan = new OrganizationService().CreatePlan(
            files,
            Path.Combine(directory.Path, "output"),
            _ => "Documents");

        Assert.Equal(1, plan.ConflictCount);
        Assert.Equal("Another file has the same destination name", plan.Items[1].ConflictReason);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsPartialResultWhenCancelled()
    {
        using var directory = new TemporaryDirectory();
        var source = Path.Combine(directory.Path, "source.txt");
        File.WriteAllText(source, "source");
        var file = FileMetadata.FromFileInfo(new FileInfo(source));
        var plan = new OrganizationService().CreatePlan(
            [file],
            Path.Combine(directory.Path, "output"),
            _ => "Documents");
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        var result = await new OrganizationService().ExecuteAsync(
            plan,
            cancellationToken: cancellation.Token);

        Assert.True(result.WasCancelled);
        Assert.Empty(result.MovedPaths);
        Assert.True(File.Exists(source));
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
