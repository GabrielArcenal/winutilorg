namespace Sortexa.Models;

public sealed record RenamePlanItem(
    FileMetadata Source,
    string ProposedName,
    string DestinationPath,
    bool IsConflict,
    string? ConflictReason);

public sealed record RenamePlan(IReadOnlyList<RenamePlanItem> Items)
{
    public int ConflictCount => Items.Count(item => item.IsConflict);
}
