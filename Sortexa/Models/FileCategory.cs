namespace Sortexa.Models;

public sealed record FileCategory(string Name, IReadOnlySet<string> Extensions)
{
    public static FileCategory Create(string name, IEnumerable<string> extensions)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(extensions);

        var normalized = extensions
            .Where(extension => !string.IsNullOrWhiteSpace(extension))
            .Select(extension => extension.StartsWith('.') ? extension : $".{extension}")
            .Select(extension => extension.ToLowerInvariant())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        return new FileCategory(name, normalized);
    }
}
