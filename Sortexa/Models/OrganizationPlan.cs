namespace Sortexa.Models;

public sealed record OrganizationPlanItem(
    FileMetadata Source,
    string Category,
    string DestinationPath,
    bool IsConflict,
    string? ConflictReason);

public sealed record OrganizationPlan(IReadOnlyList<OrganizationPlanItem> Items)
{
    public int ConflictCount => Items.Count(item => item.IsConflict);
}
