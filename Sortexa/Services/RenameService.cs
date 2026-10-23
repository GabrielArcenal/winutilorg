using Sortexa.Models;

namespace Sortexa.Services;

public sealed class RenameService
{
    public RenamePlan CreatePlan(
        IEnumerable<FileMetadata> files,
        string template)
    {
        ArgumentNullException.ThrowIfNull(files);
        ArgumentException.ThrowIfNullOrWhiteSpace(template);

        var items = new List<RenamePlanItem>();
        var destinations = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var index = 1;

        foreach (var file in files)
        {
            var proposedName = ApplyTemplate(file, template, index++);
            var destinationPath = Path.Combine(Path.GetDirectoryName(file.FullPath)!, proposedName);
            var conflictReason = GetConflictReason(file, destinationPath, destinations);
            items.Add(new RenamePlanItem(
                file,
                proposedName,
                destinationPath,
                conflictReason is not null,
                conflictReason));
            destinations.Add(destinationPath);
        }

        return new RenamePlan(items);
    }

    public async Task<OperationResult> ExecuteAsync(
        RenamePlan plan,
        IProgress<int>? progress = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(plan);
        var renamed = new List<string>();
        var errors = new List<string>();
        var completed = 0;

        foreach (var item in plan.Items)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (item.IsConflict)
            {
                errors.Add($"{item.Source.FullPath}: {item.ConflictReason}");
                continue;
            }

            try
            {
                File.Move(item.Source.FullPath, item.DestinationPath);
                renamed.Add(item.DestinationPath);
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

        return new OperationResult(renamed, errors, completed < plan.Items.Count);
    }

    private static string ApplyTemplate(FileMetadata file, string template, int index)
    {
        var baseName = Path.GetFileNameWithoutExtension(file.Name);
        return template
            .Replace("{name}", baseName, StringComparison.OrdinalIgnoreCase)
            .Replace("{index}", index.ToString("D3"), StringComparison.OrdinalIgnoreCase)
            .Replace("{extension}", file.Extension.TrimStart('.'), StringComparison.OrdinalIgnoreCase)
            + (template.Contains("{extension}", StringComparison.OrdinalIgnoreCase)
                ? string.Empty
                : file.Extension);
    }

    private static string? GetConflictReason(
        FileMetadata file,
        string destinationPath,
        ISet<string> destinations)
    {
        if (string.Equals(file.FullPath, destinationPath, StringComparison.OrdinalIgnoreCase))
        {
            return "Source and destination are the same path";
        }

        if (File.Exists(destinationPath))
        {
            return "Destination already exists";
        }

        return destinations.Contains(destinationPath)
            ? "Another file has the same destination name"
            : null;
    }
}
