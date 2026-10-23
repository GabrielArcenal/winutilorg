using Sortexa.Models;

namespace Sortexa.Services;

public sealed record ScanResult(
    IReadOnlyList<FileMetadata> Files,
    IReadOnlyList<string> SkippedPaths,
    bool IsCancelled);

public sealed class FileScanner
{
    public async Task<ScanResult> ScanAsync(
        string rootPath,
        IProgress<int>? progress = null,
        CancellationToken cancellationToken = default,
        bool includeSubdirectories = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(rootPath);

        return await Task.Run(() =>
        {
            var files = new List<FileMetadata>();
            var skippedPaths = new List<string>();

            if (!Directory.Exists(rootPath))
            {
                skippedPaths.Add(rootPath);
                return new ScanResult(files, skippedPaths, false);
            }

            var pendingDirectories = new Stack<string>();
            pendingDirectories.Push(rootPath);
            var visitedFiles = 0;

            while (pendingDirectories.Count > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var directory = pendingDirectories.Pop();

                string[] childDirectories;
                try
                {
                    childDirectories = Directory.EnumerateDirectories(directory).ToArray();
                }
                catch (UnauthorizedAccessException)
                {
                    skippedPaths.Add(directory);
                    continue;
                }
                catch (IOException)
                {
                    skippedPaths.Add(directory);
                    continue;
                }

                foreach (var childDirectory in includeSubdirectories
                    ? childDirectories
                    : Array.Empty<string>())
                {
                    pendingDirectories.Push(childDirectory);
                }

                string[] childFiles;
                try
                {
                    childFiles = Directory.EnumerateFiles(directory).ToArray();
                }
                catch (UnauthorizedAccessException)
                {
                    skippedPaths.Add(directory);
                    continue;
                }
                catch (IOException)
                {
                    skippedPaths.Add(directory);
                    continue;
                }

                foreach (var path in childFiles)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    visitedFiles++;
                    try
                    {
                        files.Add(FileMetadata.FromFileInfo(new FileInfo(path)));
                    }
                    catch (UnauthorizedAccessException)
                    {
                        skippedPaths.Add(path);
                    }
                    catch (FileNotFoundException)
                    {
                        skippedPaths.Add(path);
                    }
                    catch (DirectoryNotFoundException)
                    {
                        skippedPaths.Add(path);
                    }
                    catch (IOException)
                    {
                        skippedPaths.Add(path);
                    }

                    progress?.Report(visitedFiles);
                }
            }

            return new ScanResult(files, skippedPaths, false);
        }, cancellationToken).ContinueWith(
            task => task.IsCanceled
                ? new ScanResult(Array.Empty<FileMetadata>(), Array.Empty<string>(), true)
                : task.GetAwaiter().GetResult(),
            CancellationToken.None,
            TaskContinuationOptions.ExecuteSynchronously,
            TaskScheduler.Default);
    }
}
