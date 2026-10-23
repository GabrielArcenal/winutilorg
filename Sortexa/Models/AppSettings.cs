namespace Sortexa.Models;

public sealed record AppSettings(
    bool IncludeSubdirectories = true,
    bool ShowConsoleOutput = false,
    int LogRetentionLimit = 100,
    string RenameTemplate = "{name}-{index}",
    bool ShowWelcomeOnStartup = true);
