using Sortexa.Models;

namespace Sortexa.Services;

public sealed class FileCategorizer
{
    private readonly IReadOnlyList<FileCategory> categories;

    public FileCategorizer(IEnumerable<FileCategory> categories)
    {
        ArgumentNullException.ThrowIfNull(categories);
        this.categories = categories.ToArray();
    }

    public string Categorize(FileMetadata file)
    {
        ArgumentNullException.ThrowIfNull(file);
        return categories.FirstOrDefault(category => category.Extensions.Contains(file.Extension))?.Name
            ?? "Other";
    }
}
