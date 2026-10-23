using System.Text.Json;
using Sortexa.Models;

namespace Sortexa.Storage;

public sealed class OperationStore
{
    private readonly string filePath;
    private readonly JsonSerializerOptions serializerOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public OperationStore(string? filePath = null)
    {
        this.filePath = filePath ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Sortexa",
            "operations.json");
    }

    public async Task<IReadOnlyList<OperationRecord>> LoadAsync(CancellationToken cancellationToken = default)
    {
        if (!File.Exists(filePath))
        {
            return Array.Empty<OperationRecord>();
        }

        await using var stream = File.OpenRead(filePath);
        return await JsonSerializer.DeserializeAsync<List<OperationRecord>>(
            stream,
            serializerOptions,
            cancellationToken) ?? [];
    }

    public async Task SaveAsync(
        IEnumerable<OperationRecord> records,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(records);
        var directory = Path.GetDirectoryName(filePath);
        if (string.IsNullOrWhiteSpace(directory))
        {
            throw new IOException("Operation history path has no parent directory.");
        }

        Directory.CreateDirectory(directory);
        var boundedRecords = records
            .OrderByDescending(record => record.CompletedAt)
            .Take(100)
            .ToArray();

        var temporaryPath = $"{filePath}.{Guid.NewGuid():N}.tmp";
        try
        {
            await using (var stream = File.Create(temporaryPath))
            {
                await JsonSerializer.SerializeAsync(stream, boundedRecords, serializerOptions, cancellationToken);
            }

            File.Move(temporaryPath, filePath, overwrite: true);
        }
        catch
        {
            if (File.Exists(temporaryPath))
            {
                File.Delete(temporaryPath);
            }

            throw;
        }
    }
}
