namespace Sortexa;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        UpdateStatus("Ready");
    }

    private void NavigationButton_Click(object? sender, EventArgs e)
    {
        if (sender is not Button button)
        {
            return;
        }

        pageTitleLabel.Text = button.Text;
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
            UpdateStatus("Folder selected");
        }
    }

    private void PreviewButton_Click(object? sender, EventArgs e)
    {
        UpdateStatus("Select a folder to preview changes");
    }

    private void OrganizeButton_Click(object? sender, EventArgs e)
    {
        UpdateStatus("Select a folder to organize files");
    }

    private void UndoButton_Click(object? sender, EventArgs e)
    {
        UpdateStatus("No completed operation is available to undo");
    }

    private void UpdateStatus(string message)
    {
        statusLabel.Text = message;
    }
}
