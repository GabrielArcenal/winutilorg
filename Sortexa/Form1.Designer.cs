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
        contentPanel.Padding = new Padding(34, 30, 34, 26);
        contentPanel.Controls.Add(dashboardLayout);
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

        dashboardLayout.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dashboardLayout.ColumnCount = 2;
        dashboardLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        dashboardLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        dashboardLayout.RowCount = 3;
        dashboardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 118F));
        dashboardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 118F));
        dashboardLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        dashboardLayout.Location = new Point(34, 125);
        dashboardLayout.Margin = new Padding(0);
        dashboardLayout.Size = new Size(612, 341);
        dashboardLayout.Controls.Add(CreateActionButton("Select Folder", "Choose a source folder", SelectFolderButton_Click), 0, 0);
        dashboardLayout.Controls.Add(CreateActionButton("Preview Changes", "Review the plan before changes", PreviewButton_Click), 1, 0);
        dashboardLayout.Controls.Add(CreateActionButton("Organize Files", "Apply the approved organization plan", OrganizeButton_Click), 0, 1);
        dashboardLayout.Controls.Add(CreateActionButton("Undo Last Operation", "Restore files when it is safe", UndoButton_Click), 1, 1);
        dashboardLayout.Controls.Add(CreateFolderSummary(), 0, 2);
        dashboardLayout.SetColumnSpan(dashboardLayout.GetControlFromPosition(0, 2)!, 2);

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
        button.Click += NavigationButton_Click;
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
            UseVisualStyleBackColor = false
        };
        button.FlatAppearance.BorderColor = Color.FromArgb(220, 226, 232);
        button.FlatAppearance.BorderSize = 1;
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
