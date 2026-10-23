using System.Text.Json;
using Sortexa.Models;

namespace Sortexa.Storage;

public sealed class ActivityLogStore
{
    private readonly string filePath;
    private readonly JsonSerializerOptions options = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public ActivityLogStore(string? filePath = null)
    {
        filePath = filePath ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Sortexa",
            "logs",
            "activity.json");
        this.filePath = filePath;
    }

    public async Task<IReadOnlyList<ActivityEntry>> LoadAsync(CancellationToken cancellationToken = default)
    {
        if (!File.Exists(filePath))
        {
            return Array.Empty<ActivityEntry>();
        }

        await using var stream = File.OpenRead(filePath);
        return await JsonSerializer.DeserializeAsync<List<ActivityEntry>>(stream, options, cancellationToken)
            ?? [];
    }

    public async Task AppendAsync(
        ActivityEntry entry,
        int retentionLimit,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entry);
        var entries = (await LoadAsync(cancellationToken))
            .Append(entry)
            .OrderByDescending(item => item.Timestamp)
            .Take(Math.Max(1, retentionLimit))
            .ToArray();
        var directory = Path.GetDirectoryName(filePath);
        if (string.IsNullOrWhiteSpace(directory))
        {
            throw new IOException("Activity log path has no parent directory.");
        }

        Directory.CreateDirectory(directory);
        await using var stream = File.Create(filePath);
        await JsonSerializer.SerializeAsync(stream, entries, options, cancellationToken);
    }
}
