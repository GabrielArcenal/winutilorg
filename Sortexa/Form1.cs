namespace Sortexa;

using Sortexa.Models;
using Sortexa.Services;
using Sortexa.Storage;

public partial class Form1 : Form
{
    private readonly FileScanner fileScanner = new();
    private readonly FileCategorizer fileCategorizer = new(
    [
        FileCategory.Create("Documents", [".doc", ".docx", ".pdf", ".txt", ".rtf"]),
        FileCategory.Create("Images", [".bmp", ".gif", ".jpeg", ".jpg", ".png", ".webp"]),
        FileCategory.Create("Audio", [".aac", ".flac", ".mp3", ".wav"]),
        FileCategory.Create("Video", [".avi", ".mkv", ".mov", ".mp4", ".webm"])
    ]);
    private CancellationTokenSource? scanCancellation;
    private readonly OrganizationService organizationService = new();
    private readonly OperationStore operationStore = new();
    private readonly UndoService undoService;
    private readonly SettingsStore settingsStore = new();
    private readonly ActivityLogStore activityLogStore = new();
    private AppSettings settings = new();
    private IReadOnlyList<FileMetadata> scannedFiles = Array.Empty<FileMetadata>();
    private OrganizationPlan? currentPlan;

    public Form1()
    {
        InitializeComponent();
        undoService = new UndoService(operationStore);
        Load += Form1_Load;
        FormClosing += Form1_FormClosing;
        UpdateStatus("Ready");
    }

    private async void Form1_Load(object? sender, EventArgs e)
    {
        try
        {
            settings = await settingsStore.LoadAsync();
            IncludeSubdirectoriesCheckBox.Checked = settings.IncludeSubdirectories;
            ShowConsoleOutputCheckBox.Checked = settings.ShowConsoleOutput;
            LogRetentionNumeric.Value = Math.Clamp(settings.LogRetentionLimit, 1, 1000);
            RenameTemplateTextBox.Text = settings.RenameTemplate;
            RestoreWindowState();
        }
        catch (IOException exception)
        {
            UpdateStatus($"Settings could not be loaded: {exception.Message}");
        }
    }

    private async void Form1_FormClosing(object? sender, FormClosingEventArgs e)
    {
        try
        {
            settings = settings with
            {
                IncludeSubdirectories = IncludeSubdirectoriesCheckBox.Checked,
                ShowConsoleOutput = ShowConsoleOutputCheckBox.Checked,
                LogRetentionLimit = (int)LogRetentionNumeric.Value,
                RenameTemplate = RenameTemplateTextBox.Text
            };
            await settingsStore.SaveAsync(settings);
            SaveWindowState();
        }
        catch (IOException exception)
        {
            UpdateStatus($"Settings could not be saved: {exception.Message}");
        }
    }

    private void NavigationButton_Click(object? sender, EventArgs e)
    {
        if (sender is not Button button)
        {
            return;
        }

        pageTitleLabel.Text = button.Text;
        dashboardLayout.Visible = button.Text != "Organizer";
        organizerPanel.Visible = button.Text == "Organizer";
        logsPanel.Visible = button.Text == "Activity Logs";
        settingsPanel.Visible = button.Text == "Settings";
        helpPanel.Visible = button.Text == "Help & About";
        pageDescriptionLabel.Text = button.Text switch
        {
            "Dashboard" => "Organize your files safely from one place.",
            "Organizer" => "Choose a folder and preview how files will be organized.",
            "Activity Logs" => "Review recent operations and their results.",
            "Settings" => "Configure categories, safety, and application preferences.",
            "Help & About" => "Learn how Sortexa works and get quick-start guidance.",
            _ => string.Empty
        };

        UpdateStatus($"{button.Text} selected");
    }

    private void SelectFolderButton_Click(object? sender, EventArgs e)
    {
        using var dialog = new FolderBrowserDialog
        {
            Description = "Select a folder to organize",
            UseDescriptionForTitle = true,
            ShowNewFolderButton = false
        };

        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            selectedFolderLabel.Text = dialog.SelectedPath;
            selectedFolderLabel.Tag = dialog.SelectedPath;
            UpdateStatus("Folder selected");
        }
    }

    private async Task ScanSelectedFolderAsync()
    {
        var folder = selectedFolderLabel.Tag as string;
        if (string.IsNullOrWhiteSpace(folder))
        {
            return;
        }

        scanCancellation?.Dispose();
        scanCancellation = new CancellationTokenSource();
        SetScanInProgress(true);
        fileGrid.Rows.Clear();
        fileGrid.Visible = true;
        planGrid.Visible = false;
        skippedFilesLabel.Text = "Scanning...";
        UpdateStatus("Scanning files...");

        try
        {
            var progress = new Progress<int>(count => scanProgressLabel.Text = $"{count:N0} files inspected");
            var result = await fileScanner.ScanAsync(
                folder,
                progress,
                scanCancellation.Token,
                settings.IncludeSubdirectories);

            foreach (var file in result.Files)
            {
                fileGrid.Rows.Add(
                    file.Name,
                    string.IsNullOrEmpty(file.Extension) ? "(none)" : file.Extension,
                    FormatFileSize(file.Size),
                    file.LastModified.ToString("g"),
                    fileCategorizer.Categorize(file));
            }
            scannedFiles = result.Files;

            skippedFilesLabel.Text = result.SkippedPaths.Count == 0
                ? $"{result.Files.Count:N0} files found"
                : $"{result.Files.Count:N0} files found, {result.SkippedPaths.Count:N0} skipped";
            UpdateStatus(result.SkippedPaths.Count == 0
                ? "Scan complete"
                : "Scan complete with skipped files");
        }
        catch (OperationCanceledException)
        {
            skippedFilesLabel.Text = "Scan cancelled";
            UpdateStatus("Scan cancelled");
        }
        finally
        {
            SetScanInProgress(false);
            scanCancellation?.Dispose();
            scanCancellation = null;
        }
    }

    private void CancelScanButton_Click(object? sender, EventArgs e)
    {
        scanCancellation?.Cancel();
    }

    private void SetScanInProgress(bool isScanning)
    {
        cancelScanButton.Enabled = isScanning;
        previewButton.Enabled = !isScanning;
        selectFolderButton.Enabled = !isScanning;
        scanProgressLabel.Visible = isScanning;
    }

    private static string FormatFileSize(long bytes)
    {
        const long kilobyte = 1024;
        const long megabyte = kilobyte * 1024;
        const long gigabyte = megabyte * 1024;

        return bytes switch
        {
            >= gigabyte => $"{bytes / (double)gigabyte:0.##} GB",
            >= megabyte => $"{bytes / (double)megabyte:0.##} MB",
            >= kilobyte => $"{bytes / (double)kilobyte:0.##} KB",
            _ => $"{bytes} B"
        };
    }

    private void PreviewButton_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(selectedFolderLabel.Tag as string))
        {
            UpdateStatus("Select a folder to preview changes");
            return;
        }

        _ = ScanSelectedFolderAsync();
    }

    private void OrganizeButton_Click(object? sender, EventArgs e)
    {
        pageTitleLabel.Text = "Organizer";
        pageDescriptionLabel.Text = "Preview and apply a safe organization plan.";
        dashboardLayout.Visible = false;
        organizerPanel.Visible = true;
        _ = ScanSelectedFolderAsync();
    }

    private async void UndoButton_Click(object? sender, EventArgs e)
    {
        try
        {
            var result = await undoService.UndoLatestAsync();
            await LogAsync("Undo", result.Conflicts.Count == 0 ? "Completed" : "Partial",
                $"{result.RestoredPaths.Count} restored; {result.Conflicts.Count} conflict(s)");
            UpdateStatus(result.Conflicts.Count == 0
                ? $"{result.RestoredPaths.Count:N0} files restored"
                : $"{result.RestoredPaths.Count:N0} restored, {result.Conflicts.Count:N0} conflict(s)");
        }
        catch (IOException exception)
        {
            UpdateStatus($"Undo history error: {exception.Message}");
        }
        catch (UnauthorizedAccessException exception)
        {
            UpdateStatus($"Undo access error: {exception.Message}");
        }
    }

    private void UpdateStatus(string message)
    {
        statusLabel.Text = message;
    }

    private void DestinationFolderButton_Click(object? sender, EventArgs e)
    {
        using var dialog = new FolderBrowserDialog
        {
            Description = "Select a destination folder",
            UseDescriptionForTitle = true,
            ShowNewFolderButton = true
        };

        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            destinationFolderTextBox.Text = dialog.SelectedPath;
            currentPlan = null;
            UpdateStatus("Destination selected");
        }
    }

    private void PreviewPlanButton_Click(object? sender, EventArgs e)
    {
        if (scannedFiles.Count == 0)
        {
            UpdateStatus("Scan a folder before previewing an organization plan");
            return;
        }

        if (string.IsNullOrWhiteSpace(destinationFolderTextBox.Text))
        {
            UpdateStatus("Select a destination folder before previewing");
            return;
        }

        currentPlan = organizationService.CreatePlan(
            scannedFiles,
            destinationFolderTextBox.Text,
            fileCategorizer.Categorize);
        planGrid.Rows.Clear();
        fileGrid.Visible = false;
        planGrid.Visible = true;

        foreach (var item in currentPlan.Items)
        {
            planGrid.Rows.Add(
                item.Source.Name,
                item.Category,
                item.DestinationPath,
                item.IsConflict ? item.ConflictReason ?? "Conflict" : "Ready");
        }

        executePlanButton.Enabled = currentPlan.ConflictCount == 0 && currentPlan.Items.Count > 0;
        planSummaryLabel.Text = currentPlan.ConflictCount == 0
            ? $"{currentPlan.Items.Count:N0} files ready to organize"
            : $"{currentPlan.ConflictCount:N0} conflict(s) require attention";
        UpdateStatus(currentPlan.ConflictCount == 0
            ? "Organization plan ready"
            : "Organization plan contains conflicts");
    }

    private async void ExecutePlanButton_Click(object? sender, EventArgs e)
    {
        if (currentPlan is null || currentPlan.ConflictCount > 0)
        {
            UpdateStatus("Preview a conflict-free plan before organizing");
            return;
        }

        executePlanButton.Enabled = false;
        try
        {
            var result = await organizationService.ExecuteAsync(currentPlan);
            await LogAsync("Organize", result.Errors.Count == 0 ? "Completed" : "Partial",
                $"{result.MovedPaths.Count} moved; {result.Errors.Count} error(s)");
            var moved = currentPlan.Items
                .Where(item => result.MovedPaths.Contains(item.DestinationPath, StringComparer.OrdinalIgnoreCase))
                .Select(item => new OperationMove(item.Source.FullPath, item.DestinationPath))
                .ToArray();
            if (moved.Length > 0)
            {
                try
                {
                    var records = (await operationStore.LoadAsync()).ToList();
                    records.Add(new OperationRecord(
                        Guid.NewGuid(),
                        DateTimeOffset.UtcNow,
                        moved,
                        result.Errors,
                        false));
                    await operationStore.SaveAsync(records);
                }
                catch (IOException exception)
                {
                    UpdateStatus($"Files organized, but undo history could not be saved: {exception.Message}");
                }
                catch (UnauthorizedAccessException exception)
                {
                    UpdateStatus($"Files organized, but undo history could not be saved: {exception.Message}");
                }

            }
            var movedSources = currentPlan.Items
                .Where(item => result.MovedPaths.Contains(item.DestinationPath, StringComparer.OrdinalIgnoreCase))
                .Select(item => item.Source.FullPath)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
            scannedFiles = scannedFiles
                .Where(file => !movedSources.Contains(file.FullPath))
                .ToArray();
            UpdateStatus(result.WasCancelled
                ? $"{result.MovedPaths.Count:N0} files organized before cancellation"
                : result.Errors.Count == 0
                    ? $"{result.MovedPaths.Count:N0} files organized"
                    : $"{result.MovedPaths.Count:N0} files organized, {result.Errors.Count:N0} failed");
            planSummaryLabel.Text = result.WasCancelled
                ? "Organization cancelled after partial completion"
                : result.Errors.Count == 0
                    ? "Organization complete"
                    : "Organization completed with errors";
        }
        catch (OperationCanceledException)
        {
            UpdateStatus("Organization cancelled");
        }
        finally
        {
            executePlanButton.Enabled = false;
        }
    }

    private async Task LogAsync(string action, string status, string details)
    {
        try
        {
            await activityLogStore.AppendAsync(
                new ActivityEntry(DateTimeOffset.UtcNow, action, status, details),
                settings.LogRetentionLimit);
        }
        catch (IOException exception)
        {
            UpdateStatus($"Activity log error: {exception.Message}");
        }
    }

    private async void RefreshLogsButton_Click(object? sender, EventArgs e)
    {
        try
        {
            activityGrid.Rows.Clear();
            foreach (var entry in await activityLogStore.LoadAsync())
            {
                activityGrid.Rows.Add(entry.Timestamp.ToLocalTime().ToString("g"),
                    entry.Action, entry.Status, entry.Details);
            }
        }
        catch (IOException exception)
        {
            UpdateStatus($"Activity logs could not be loaded: {exception.Message}");
        }
    }

    private async void SaveSettingsButton_Click(object? sender, EventArgs e)
    {
        settings = settings with
        {
            IncludeSubdirectories = IncludeSubdirectoriesCheckBox.Checked,
            ShowConsoleOutput = ShowConsoleOutputCheckBox.Checked,
            LogRetentionLimit = (int)LogRetentionNumeric.Value,
            RenameTemplate = RenameTemplateTextBox.Text
        };
        await settingsStore.SaveAsync(settings);
        UpdateStatus("Settings saved");
    }

    private async void ResetSettingsButton_Click(object? sender, EventArgs e)
    {
        if (MessageBox.Show(this, "Reset all settings to defaults?", "Reset settings",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        {
            return;
        }

        settings = new AppSettings();
        IncludeSubdirectoriesCheckBox.Checked = settings.IncludeSubdirectories;
        ShowConsoleOutputCheckBox.Checked = settings.ShowConsoleOutput;
        LogRetentionNumeric.Value = settings.LogRetentionLimit;
        RenameTemplateTextBox.Text = settings.RenameTemplate;
        await settingsStore.SaveAsync(settings);
        UpdateStatus("Settings reset to defaults");
    }

    private void RestoreWindowState()
    {
        var statePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Sortexa",
            "window.state");
        if (!File.Exists(statePath))
        {
            return;
        }

        var values = File.ReadAllText(statePath).Split(',');
        if (values.Length == 5 &&
            int.TryParse(values[0], out var x) &&
            int.TryParse(values[1], out var y) &&
            int.TryParse(values[2], out var width) &&
            int.TryParse(values[3], out var height))
        {
            StartPosition = FormStartPosition.Manual;
            Location = new Point(x, y);
            Size = new Size(Math.Max(MinimumSize.Width, width), Math.Max(MinimumSize.Height, height));
            WindowState = values[4] == "Maximized" ? FormWindowState.Maximized : FormWindowState.Normal;
        }
    }

    private void SaveWindowState()
    {
        var directory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Sortexa");
        Directory.CreateDirectory(directory);
        var bounds = WindowState == FormWindowState.Normal ? Bounds : RestoreBounds;
        File.WriteAllText(Path.Combine(directory, "window.state"),
            $"{bounds.X},{bounds.Y},{bounds.Width},{bounds.Height},{WindowState}");
    }
}
