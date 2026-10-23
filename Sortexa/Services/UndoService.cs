using Sortexa.Models;
using Sortexa.Storage;

namespace Sortexa.Services;

public sealed record UndoResult(
    IReadOnlyList<string> RestoredPaths,
    IReadOnlyList<string> Conflicts);

public sealed class UndoService
{
    private readonly OperationStore store;

    public UndoService(OperationStore store)
    {
        this.store = store;
    }

    public async Task<OperationRecord?> GetLatestUndoableAsync(CancellationToken cancellationToken = default)
    {
        var records = await store.LoadAsync(cancellationToken);
        return records.FirstOrDefault(record => !record.IsUndone && record.Moves.Count > 0);
    }

    public async Task<UndoResult> UndoLatestAsync(CancellationToken cancellationToken = default)
    {
        var records = (await store.LoadAsync(cancellationToken)).ToList();
        var record = records.FirstOrDefault(item => !item.IsUndone && item.Moves.Count > 0);
        if (record is null)
        {
            return new UndoResult([], ["No completed operation is available to undo."]);
        }

        var restored = new List<string>();
        var conflicts = new List<string>();
        var remainingMoves = new List<OperationMove>();
        foreach (var move in record.Moves)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!File.Exists(move.CurrentPath))
            {
                conflicts.Add($"{move.CurrentPath}: moved file is missing");
                remainingMoves.Add(move);
                continue;
            }

            if (File.Exists(move.OriginalPath))
            {
                conflicts.Add($"{move.OriginalPath}: original path is occupied");
                remainingMoves.Add(move);
                continue;
            }

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(move.OriginalPath)!);
                File.Move(move.CurrentPath, move.OriginalPath);
                restored.Add(move.OriginalPath);
            }
            catch (UnauthorizedAccessException exception)
            {
                conflicts.Add($"{move.CurrentPath}: {exception.Message}");
                remainingMoves.Add(move);
            }
            catch (IOException exception)
            {
                conflicts.Add($"{move.CurrentPath}: {exception.Message}");
                remainingMoves.Add(move);
            }
        }

        var updated = record with
        {
            Moves = remainingMoves,
            IsUndone = remainingMoves.Count == 0
        };
        records[records.IndexOf(record)] = updated;
        await store.SaveAsync(records, cancellationToken);
        return new UndoResult(restored, conflicts);
    }
}
