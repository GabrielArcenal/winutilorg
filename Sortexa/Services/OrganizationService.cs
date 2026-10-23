using Sortexa.Models;

namespace Sortexa.Services;

public sealed class OrganizationService
{
    public OrganizationPlan CreatePlan(
        IEnumerable<FileMetadata> files,
        string destinationRoot,
        Func<FileMetadata, string> categorySelector)
    {
        ArgumentNullException.ThrowIfNull(files);
        ArgumentException.ThrowIfNullOrWhiteSpace(destinationRoot);
        ArgumentNullException.ThrowIfNull(categorySelector);

        var items = new List<OrganizationPlanItem>();
        var plannedDestinations = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var file in files)
        {
            var category = categorySelector(file);
            var dateFolder = file.LastModified.ToString("yyyy-MM-dd");
            var destinationDirectory = Path.Combine(destinationRoot, category, dateFolder);
            var destinationPath = Path.Combine(destinationDirectory, file.Name);
            var conflictReason = GetConflictReason(
                file,
                destinationPath,
                plannedDestinations);

            items.Add(new OrganizationPlanItem(
                file,
                category,
                destinationPath,
                conflictReason is not null,
                conflictReason));

            plannedDestinations.Add(destinationPath);
        }

        return new OrganizationPlan(items);
    }

    public async Task<OperationResult> ExecuteAsync(
        OrganizationPlan plan,
        IProgress<int>? progress = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(plan);

        var moved = new List<string>();
        var errors = new List<string>();
        var completed = 0;
        var wasCancelled = false;

        foreach (var item in plan.Items)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                wasCancelled = true;
                break;
            }

            if (item.IsConflict)
            {
                errors.Add($"{item.Source.FullPath}: {item.ConflictReason}");
                continue;
            }

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(item.DestinationPath)!);
                File.Move(item.Source.FullPath, item.DestinationPath);
                moved.Add(item.DestinationPath);
            }
            catch (UnauthorizedAccessException exception)
            {
                errors.Add($"{item.Source.FullPath}: {exception.Message}");
            }
            catch (IOException exception)
            {
                errors.Add($"{item.Source.FullPath}: {exception.Message}");
            }

            completed++;
            progress?.Report(completed);
        }

        return new OperationResult(moved, errors, wasCancelled);
    }

    private static string? GetConflictReason(
        FileMetadata file,
        string destinationPath,
        ISet<string> plannedDestinations)
    {
        if (string.Equals(file.FullPath, destinationPath, StringComparison.OrdinalIgnoreCase))
        {
            return "Source and destination are the same path";
        }

        if (File.Exists(destinationPath))
        {
            return "Destination already exists";
        }

        if (plannedDestinations.Contains(destinationPath))
        {
            return "Another file has the same destination name";
        }

        return null;
    }
}

public sealed record OperationResult(
    IReadOnlyList<string> MovedPaths,
    IReadOnlyList<string> Errors,
    bool WasCancelled);
