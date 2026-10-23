using Sortexa.Models;
using Sortexa.Services;
using Xunit;

namespace Sortexa.Tests;

public sealed class RenameServiceTests
{
    [Fact]
    public void CreatePlan_AppliesTemplateAndPreservesExtension()
    {
        var plan = new RenameService().CreatePlan(
        [
            new FileMetadata("C:\\files\\report.pdf", "report.pdf", ".pdf", 10, DateTime.Now)
        ], "document-{index}");

        var item = Assert.Single(plan.Items);
        Assert.Equal("document-001.pdf", item.ProposedName);
        Assert.False(item.IsConflict);
    }

    [Fact]
    public void CreatePlan_DetectsDuplicateProposedNames()
    {
        var plan = new RenameService().CreatePlan(
        [
            new FileMetadata("C:\\files\\one.txt", "one.txt", ".txt", 10, DateTime.Now),
            new FileMetadata("C:\\files\\two.txt", "two.txt", ".txt", 10, DateTime.Now)
        ], "same");

        Assert.Equal(1, plan.ConflictCount);
        Assert.Equal("same.txt", plan.Items[0].ProposedName);
        Assert.Equal("same.txt", plan.Items[1].ProposedName);
        Assert.True(plan.Items[1].IsConflict);
    }
}
