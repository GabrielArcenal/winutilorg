namespace Sortexa.Models;

public sealed record OperationMove(string OriginalPath, string CurrentPath);

public sealed record OperationRecord(
    Guid Id,
    DateTimeOffset CompletedAt,
    IReadOnlyList<OperationMove> Moves,
    IReadOnlyList<string> Errors,
    bool IsUndone);
