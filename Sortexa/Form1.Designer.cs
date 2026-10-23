namespace Sortexa;

partial class Form1
{
    private System.ComponentModel.IContainer components = null!;
    private TableLayoutPanel rootLayout = null!;
    private Panel sidebarPanel = null!;
    private Label brandLabel = null!;
    private Label subtitleLabel = null!;
    private FlowLayoutPanel navigationPanel = null!;
    private Panel contentPanel = null!;
    private Label pageTitleLabel = null!;
    private Label pageDescriptionLabel = null!;
    private TableLayoutPanel dashboardLayout = null!;
    private Label selectedFolderLabel = null!;
    private Panel organizerPanel = null!;
    private Button selectFolderButton = null!;
    private Button previewButton = null!;
    private Button cancelScanButton = null!;
    private Label scanProgressLabel = null!;
    private Label skippedFilesLabel = null!;
    private DataGridView fileGrid = null!;
    private TextBox destinationFolderTextBox = null!;
    private Button destinationFolderButton = null!;
    private Button previewPlanButton = null!;
    private Button executePlanButton = null!;
    private Label planSummaryLabel = null!;
    private DataGridView planGrid = null!;
    private Panel logsPanel = null!;
    private DataGridView activityGrid = null!;
    private Button refreshLogsButton = null!;
    private Panel settingsPanel = null!;
    private CheckBox IncludeSubdirectoriesCheckBox = null!;
    private CheckBox ShowConsoleOutputCheckBox = null!;
    private NumericUpDown LogRetentionNumeric = null!;
    private TextBox RenameTemplateTextBox = null!;
    private Button saveSettingsButton = null!;
    private Button resetSettingsButton = null!;
    private Panel helpPanel = null!;
    private Button replayWelcomeButton = null!;
    private StatusStrip statusStrip = null!;
    private ToolStripStatusLabel statusLabel = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        rootLayout = new TableLayoutPanel();
        sidebarPanel = new Panel();
        brandLabel = new Label();
        subtitleLabel = new Label();
        navigationPanel = new FlowLayoutPanel();
        contentPanel = new Panel();
        pageTitleLabel = new Label();
        pageDescriptionLabel = new Label();
        dashboardLayout = new TableLayoutPanel();
        selectedFolderLabel = new Label();
        organizerPanel = new Panel();
        selectFolderButton = new Button();
        previewButton = new Button();
        cancelScanButton = new Button();
        scanProgressLabel = new Label();
        skippedFilesLabel = new Label();
        fileGrid = new DataGridView();
        destinationFolderTextBox = new TextBox();
        destinationFolderButton = new Button();
        previewPlanButton = new Button();
        executePlanButton = new Button();
        planSummaryLabel = new Label();
        planGrid = new DataGridView();
        logsPanel = new Panel();
        activityGrid = new DataGridView();
        refreshLogsButton = new Button();
        settingsPanel = new Panel();
        IncludeSubdirectoriesCheckBox = new CheckBox();
        ShowConsoleOutputCheckBox = new CheckBox();
        LogRetentionNumeric = new NumericUpDown();
        RenameTemplateTextBox = new TextBox();
        saveSettingsButton = new Button();
        resetSettingsButton = new Button();
        helpPanel = new Panel();
        replayWelcomeButton = new Button();
        statusStrip = new StatusStrip();
        statusLabel = new ToolStripStatusLabel();
        rootLayout.SuspendLayout();
        sidebarPanel.SuspendLayout();
        contentPanel.SuspendLayout();
        dashboardLayout.SuspendLayout();
        statusStrip.SuspendLayout();
        SuspendLayout();

        rootLayout.ColumnCount = 2;
        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 210F));
        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        rootLayout.Dock = DockStyle.Fill;
        rootLayout.RowCount = 1;
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        rootLayout.Controls.Add(sidebarPanel, 0, 0);
        rootLayout.Controls.Add(contentPanel, 1, 0);

        sidebarPanel.BackColor = Color.FromArgb(29, 43, 58);
        sidebarPanel.Dock = DockStyle.Fill;
        sidebarPanel.Padding = new Padding(18, 24, 18, 18);
        sidebarPanel.Controls.Add(navigationPanel);
        sidebarPanel.Controls.Add(subtitleLabel);
        sidebarPanel.Controls.Add(brandLabel);

        brandLabel.AutoSize = true;
        brandLabel.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
        brandLabel.ForeColor = Color.White;
        brandLabel.Location = new Point(18, 24);
        brandLabel.Text = "Sortexa";

        subtitleLabel.AutoSize = true;
        subtitleLabel.Font = new Font("Segoe UI", 9F);
        subtitleLabel.ForeColor = Color.FromArgb(185, 201, 215);
        subtitleLabel.Location = new Point(19, 55);
        subtitleLabel.Text = "Safe file organization";

        navigationPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        navigationPanel.FlowDirection = FlowDirection.TopDown;
        navigationPanel.Location = new Point(18, 104);
        navigationPanel.Margin = new Padding(0);
        navigationPanel.Size = new Size(174, 390);
        navigationPanel.WrapContents = false;
        AddNavigationButton("Dashboard", true);
        AddNavigationButton("Organizer");
        AddNavigationButton("Activity Logs");
        AddNavigationButton("Settings");
        AddNavigationButton("Help & About");

        contentPanel.BackColor = Color.FromArgb(247, 249, 251);
        contentPanel.Dock = DockStyle.Fill;
        contentPanel.Padding = new Padding(34, 125, 34, 26);
        contentPanel.Controls.Add(dashboardLayout);
        contentPanel.Controls.Add(organizerPanel);
        contentPanel.Controls.Add(logsPanel);
        contentPanel.Controls.Add(settingsPanel);
        contentPanel.Controls.Add(helpPanel);
        contentPanel.Controls.Add(pageDescriptionLabel);
        contentPanel.Controls.Add(pageTitleLabel);

        pageTitleLabel.AutoSize = true;
        pageTitleLabel.Font = new Font("Segoe UI Semibold", 24F, FontStyle.Bold);
        pageTitleLabel.ForeColor = Color.FromArgb(29, 43, 58);
        pageTitleLabel.Location = new Point(34, 30);
        pageTitleLabel.Text = "Dashboard";

        pageDescriptionLabel.AutoSize = true;
        pageDescriptionLabel.Font = new Font("Segoe UI", 10F);
        pageDescriptionLabel.ForeColor = Color.FromArgb(91, 106, 120);
        pageDescriptionLabel.Location = new Point(37, 75);
        pageDescriptionLabel.Text = "Organize your files safely from one place.";

        dashboardLayout.Dock = DockStyle.Fill;
        dashboardLayout.ColumnCount = 2;
        dashboardLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        dashboardLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        dashboardLayout.RowCount = 3;
        dashboardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 118F));
        dashboardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 118F));
        dashboardLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        dashboardLayout.Margin = new Padding(0);
        dashboardLayout.Controls.Add(CreateActionButton("Select Folder", "Choose a source folder", SelectFolderButton_Click), 0, 0);
        dashboardLayout.Controls.Add(CreateActionButton("Preview Changes", "Review the plan before changes", PreviewButton_Click), 1, 0);
        dashboardLayout.Controls.Add(CreateActionButton("Organize Files", "Apply the approved organization plan", OrganizeButton_Click), 0, 1);
        dashboardLayout.Controls.Add(CreateActionButton("Undo Last Operation", "Restore files when it is safe", UndoButton_Click), 1, 1);
        dashboardLayout.Controls.Add(CreateFolderSummary(), 0, 2);
        dashboardLayout.SetColumnSpan(dashboardLayout.GetControlFromPosition(0, 2)!, 2);

        organizerPanel.Dock = DockStyle.Fill;
        organizerPanel.BackColor = Color.White;
        organizerPanel.Padding = new Padding(12);
        organizerPanel.Visible = false;
        organizerPanel.Controls.Add(fileGrid);
        organizerPanel.Controls.Add(planGrid);
        organizerPanel.Controls.Add(planSummaryLabel);
        organizerPanel.Controls.Add(executePlanButton);
        organizerPanel.Controls.Add(previewPlanButton);
        organizerPanel.Controls.Add(destinationFolderButton);
        organizerPanel.Controls.Add(destinationFolderTextBox);
        organizerPanel.Controls.Add(skippedFilesLabel);
        organizerPanel.Controls.Add(scanProgressLabel);
        organizerPanel.Controls.Add(cancelScanButton);
        organizerPanel.Controls.Add(previewButton);
        organizerPanel.Controls.Add(selectFolderButton);

        selectFolderButton.Location = new Point(12, 12);
        selectFolderButton.Size = new Size(112, 30);
        selectFolderButton.Text = "Select Folder";
        selectFolderButton.Click += SelectFolderButton_Click;

        previewButton.Location = new Point(130, 12);
        previewButton.Size = new Size(112, 30);
        previewButton.Text = "Scan Folder";
        previewButton.Click += PreviewButton_Click;

        cancelScanButton.Enabled = false;
        cancelScanButton.Location = new Point(248, 12);
        cancelScanButton.Size = new Size(112, 30);
        cancelScanButton.Text = "Cancel Scan";
        cancelScanButton.Click += CancelScanButton_Click;

        scanProgressLabel.AutoSize = true;
        scanProgressLabel.ForeColor = Color.FromArgb(91, 106, 120);
        scanProgressLabel.Location = new Point(375, 20);
        scanProgressLabel.Text = "0 files inspected";
        scanProgressLabel.Visible = false;

        skippedFilesLabel.AutoSize = true;
        skippedFilesLabel.ForeColor = Color.FromArgb(91, 106, 120);
        skippedFilesLabel.Location = new Point(12, 49);
        skippedFilesLabel.Text = "Select a folder to scan";

        fileGrid.AllowUserToAddRows = false;
        fileGrid.AllowUserToDeleteRows = false;
        fileGrid.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        fileGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        fileGrid.BackgroundColor = Color.White;
        fileGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        fileGrid.Location = new Point(12, 75);
        fileGrid.ReadOnly = true;
        fileGrid.RowHeadersVisible = false;
        fileGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        fileGrid.Size = new Size(588, 204);
        fileGrid.Columns.Add("Name", "Name");
        fileGrid.Columns.Add("Extension", "Extension");
        fileGrid.Columns.Add("Size", "Size");
        fileGrid.Columns.Add("Modified", "Modified");
        fileGrid.Columns.Add("Category", "Category");

        destinationFolderTextBox.Location = new Point(12, 290);
        destinationFolderTextBox.Size = new Size(350, 23);
        destinationFolderTextBox.PlaceholderText = "Destination folder";

        destinationFolderButton.Location = new Point(368, 288);
        destinationFolderButton.Size = new Size(105, 27);
        destinationFolderButton.Text = "Choose...";
        destinationFolderButton.Click += DestinationFolderButton_Click;

        previewPlanButton.Location = new Point(481, 288);
        previewPlanButton.Size = new Size(105, 27);
        previewPlanButton.Text = "Preview Plan";
        previewPlanButton.Click += PreviewPlanButton_Click;

        executePlanButton.Enabled = false;
        executePlanButton.Location = new Point(12, 320);
        executePlanButton.Size = new Size(125, 27);
        executePlanButton.Text = "Organize Files";
        executePlanButton.Click += ExecutePlanButton_Click;

        planSummaryLabel.AutoSize = true;
        planSummaryLabel.ForeColor = Color.FromArgb(91, 106, 120);
        planSummaryLabel.Location = new Point(150, 327);
        planSummaryLabel.Text = "Preview a plan before organizing";

        planGrid.AllowUserToAddRows = false;
        planGrid.AllowUserToDeleteRows = false;
        planGrid.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        planGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        planGrid.BackgroundColor = Color.White;
        planGrid.Location = new Point(12, 75);
        planGrid.ReadOnly = true;
        planGrid.RowHeadersVisible = false;
        planGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        planGrid.Size = new Size(588, 204);
        planGrid.Visible = false;
        planGrid.Columns.Add("Name", "Name");
        planGrid.Columns.Add("Category", "Category");
        planGrid.Columns.Add("Destination", "Destination");
        planGrid.Columns.Add("Status", "Status");

        logsPanel.Dock = DockStyle.Fill;
        logsPanel.BackColor = Color.White;
        logsPanel.Padding = new Padding(12);
        logsPanel.Visible = false;
        logsPanel.Controls.Add(activityGrid);
        logsPanel.Controls.Add(refreshLogsButton);
        refreshLogsButton.Dock = DockStyle.Top;
        refreshLogsButton.Margin = new Padding(0, 0, 0, 10);
        refreshLogsButton.Size = new Size(110, 32);
        refreshLogsButton.Text = "Refresh";
        refreshLogsButton.Click += RefreshLogsButton_Click;
        activityGrid.AllowUserToAddRows = false;
        activityGrid.AllowUserToDeleteRows = false;
        activityGrid.Dock = DockStyle.Fill;
        activityGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        activityGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        activityGrid.BackgroundColor = Color.White;
        activityGrid.ReadOnly = true;
        activityGrid.RowHeadersVisible = false;
        activityGrid.ScrollBars = ScrollBars.Vertical;
        activityGrid.Columns.Add("Timestamp", "Timestamp");
        activityGrid.Columns.Add("Action", "Action");
        activityGrid.Columns.Add("Status", "Status");
        activityGrid.Columns.Add("Details", "Details");
        activityGrid.Columns["Timestamp"]!.FillWeight = 22;
        activityGrid.Columns["Action"]!.FillWeight = 18;
        activityGrid.Columns["Status"]!.FillWeight = 18;
        activityGrid.Columns["Details"]!.FillWeight = 42;

        settingsPanel.Dock = DockStyle.Fill;
        settingsPanel.BackColor = Color.White;
        settingsPanel.Padding = new Padding(18);
        settingsPanel.Visible = false;
        IncludeSubdirectoriesCheckBox.AutoSize = true;
        IncludeSubdirectoriesCheckBox.Location = new Point(18, 20);
        IncludeSubdirectoriesCheckBox.Text = "Scan subdirectories";
        ShowConsoleOutputCheckBox.AutoSize = true;
        ShowConsoleOutputCheckBox.Location = new Point(18, 52);
        ShowConsoleOutputCheckBox.Text = "Enable optional console output";
        var retentionLabel = new Label { AutoSize = true, Location = new Point(18, 90), Text = "Log retention limit:" };
        LogRetentionNumeric.Location = new Point(140, 87);
        LogRetentionNumeric.Maximum = 1000;
        LogRetentionNumeric.Minimum = 1;
        LogRetentionNumeric.Value = 100;
        var templateLabel = new Label { AutoSize = true, Location = new Point(18, 130), Text = "Rename template:" };
        RenameTemplateTextBox.Location = new Point(140, 127);
        RenameTemplateTextBox.Size = new Size(300, 23);
        saveSettingsButton.Location = new Point(18, 180);
        saveSettingsButton.Size = new Size(110, 30);
        saveSettingsButton.Text = "Save Settings";
        saveSettingsButton.Click += SaveSettingsButton_Click;
        resetSettingsButton.Location = new Point(140, 180);
        resetSettingsButton.Size = new Size(125, 30);
        resetSettingsButton.Text = "Reset Defaults";
        resetSettingsButton.Click += ResetSettingsButton_Click;
        settingsPanel.Controls.AddRange([
            IncludeSubdirectoriesCheckBox, ShowConsoleOutputCheckBox, retentionLabel,
            LogRetentionNumeric, templateLabel, RenameTemplateTextBox,
            saveSettingsButton, resetSettingsButton
        ]);

        helpPanel.Dock = DockStyle.Fill;
        helpPanel.BackColor = Color.White;
        helpPanel.Padding = new Padding(22);
        helpPanel.Visible = false;
        var helpText = new Label
        {
            AutoSize = false,
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 10F),
            ForeColor = Color.FromArgb(45, 58, 70),
            Text = "Quick start\r\n\r\n1. Select a source folder.\r\n2. Scan and review the files.\r\n3. Choose a destination and preview the plan.\r\n4. Resolve conflicts before organizing.\r\n5. Use Undo Last Operation when the original files are still available.\r\n\r\nSortexa\r\nA local-first Windows file organization toolkit.",
        };
        helpPanel.Controls.Add(helpText);
        replayWelcomeButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        replayWelcomeButton.BackColor = Color.FromArgb(42, 107, 153);
        replayWelcomeButton.FlatStyle = FlatStyle.Flat;
        replayWelcomeButton.ForeColor = Color.White;
        replayWelcomeButton.Location = new Point(22, 285);
        replayWelcomeButton.Size = new Size(155, 32);
        replayWelcomeButton.Text = "Show welcome again";
        replayWelcomeButton.UseVisualStyleBackColor = false;
        replayWelcomeButton.FlatAppearance.BorderSize = 0;
        replayWelcomeButton.Click += ReplayWelcomeButton_Click;
        helpPanel.Controls.Add(replayWelcomeButton);

        statusStrip.Items.Add(statusLabel);
        statusStrip.Dock = DockStyle.Bottom;
        statusLabel.Text = "Ready";
        statusLabel.ForeColor = Color.FromArgb(91, 106, 120);

        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(900, 600);
        Controls.Add(rootLayout);
        Controls.Add(statusStrip);
        MinimumSize = new Size(700, 480);
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Sortexa";
        rootLayout.ResumeLayout(false);
        sidebarPanel.ResumeLayout(false);
        sidebarPanel.PerformLayout();
        contentPanel.ResumeLayout(false);
        contentPanel.PerformLayout();
        dashboardLayout.ResumeLayout(false);
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private void AddNavigationButton(string text, bool selected = false)
    {
        var button = new Button
        {
            BackColor = selected ? Color.FromArgb(55, 83, 108) : Color.FromArgb(29, 43, 58),
            FlatStyle = FlatStyle.Flat,
            ForeColor = Color.White,
            Height = 42,
            Margin = new Padding(0, 0, 0, 7),
            Text = text,
            TextAlign = ContentAlignment.MiddleLeft,
            UseVisualStyleBackColor = false,
            Width = 174
        };
        button.FlatAppearance.BorderSize = 0;
        button.Cursor = Cursors.Hand;
        button.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
        button.Padding = new Padding(10, 0, 8, 0);
        button.Click += NavigationButton_Click;
        button.MouseEnter += NavigationButton_MouseEnter;
        button.MouseLeave += NavigationButton_MouseLeave;
        navigationPanel.Controls.Add(button);
    }

    private Button CreateActionButton(string title, string description, EventHandler handler)
    {
        var button = new Button
        {
            BackColor = Color.White,
            Dock = DockStyle.Fill,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
            ForeColor = Color.FromArgb(29, 43, 58),
            Margin = new Padding(0, 0, 12, 12),
            Text = $"{title}{Environment.NewLine}{description}",
            TextAlign = ContentAlignment.MiddleLeft,
            UseVisualStyleBackColor = false,
            Cursor = Cursors.Hand,
            Padding = new Padding(18, 12, 12, 10)
        };
        button.FlatAppearance.BorderColor = Color.FromArgb(220, 226, 232);
        button.FlatAppearance.BorderSize = 1;
        button.MouseEnter += ActionButton_MouseEnter;
        button.MouseLeave += ActionButton_MouseLeave;
        button.Click += handler;
        return button;
    }

    private Control CreateFolderSummary()
    {
        var panel = new Panel
        {
            BackColor = Color.FromArgb(232, 239, 245),
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 4, 12, 0),
            Padding = new Padding(16)
        };
        var heading = new Label
        {
            AutoSize = true,
            Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
            ForeColor = Color.FromArgb(29, 43, 58),
            Location = new Point(16, 12),
            Text = "Selected source folder"
        };
        selectedFolderLabel = new Label
        {
            AutoEllipsis = true,
            AutoSize = false,
            ForeColor = Color.FromArgb(91, 106, 120),
            Location = new Point(16, 40),
            Size = new Size(560, 32),
            Text = "No folder selected"
        };
        panel.Controls.Add(selectedFolderLabel);
        panel.Controls.Add(heading);
        return panel;
    }
}
