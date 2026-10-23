namespace Sortexa.Models;

public sealed record FileMetadata(
    string FullPath,
    string Name,
    string Extension,
    long Size,
    DateTime LastModified)
{
    public static FileMetadata FromFileInfo(FileInfo file)
    {
        ArgumentNullException.ThrowIfNull(file);
        return new FileMetadata(
            file.FullName,
            file.Name,
            file.Extension,
            file.Length,
            file.LastWriteTime);
    }
}
