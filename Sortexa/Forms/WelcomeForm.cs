namespace Sortexa.Forms;

public sealed class WelcomeForm : Form
{
    private readonly CheckBox dontShowAgainCheckBox;

    public WelcomeForm()
    {
        Text = "Welcome to Sortexa";
        ClientSize = new Size(520, 360);
        MinimumSize = new Size(520, 360);
        MaximumSize = new Size(520, 360);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        BackColor = Color.White;

        var content = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            Padding = new Padding(30, 26, 30, 24),
            RowCount = 5
        };
        content.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        content.RowStyles.Add(new RowStyle(SizeType.Absolute, 54));
        content.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        content.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        content.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));

        var title = new Label
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold),
            ForeColor = Color.FromArgb(29, 43, 58),
            Text = "Welcome to Sortexa",
            TextAlign = ContentAlignment.MiddleLeft
        };
        var subtitle = new Label
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 10F),
            ForeColor = Color.FromArgb(91, 106, 120),
            Text = "A safer, clearer way to organize files on your PC.",
            TextAlign = ContentAlignment.MiddleLeft
        };
        var instructions = new Label
        {
            AutoSize = false,
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 10F),
            ForeColor = Color.FromArgb(45, 58, 70),
            Text = "Start by selecting a folder from the Dashboard. Sortexa scans files without changing them, lets you review every destination, and only performs changes after you approve the plan.\r\n\r\nYou can change categories, logging, and safety preferences any time in Settings.",
            TextAlign = ContentAlignment.TopLeft
        };
        dontShowAgainCheckBox = new CheckBox
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 9F),
            Text = "Don't show this again",
            TextAlign = ContentAlignment.MiddleLeft
        };
        var buttonPanel = new FlowLayoutPanel
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false
        };
        var getStartedButton = CreateButton("Get started", DialogResult.OK, true);
        var skipButton = CreateButton("Skip", DialogResult.Cancel, false);
        buttonPanel.Controls.Add(getStartedButton);
        buttonPanel.Controls.Add(skipButton);

        content.Controls.Add(title, 0, 0);
        content.Controls.Add(subtitle, 0, 1);
        content.Controls.Add(instructions, 0, 2);
        content.Controls.Add(dontShowAgainCheckBox, 0, 3);
        content.Controls.Add(buttonPanel, 0, 4);
        Controls.Add(content);
        AcceptButton = getStartedButton;
        CancelButton = skipButton;
    }

    public bool DontShowAgain => dontShowAgainCheckBox.Checked;

    private static Button CreateButton(string text, DialogResult result, bool primary)
    {
        var button = new Button
        {
            AutoSize = true,
            BackColor = primary ? Color.FromArgb(42, 107, 153) : Color.White,
            DialogResult = result,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
            ForeColor = primary ? Color.White : Color.FromArgb(29, 43, 58),
            Height = 32,
            Margin = new Padding(8, 0, 0, 0),
            Padding = new Padding(12, 0, 12, 0),
            Text = text,
            UseVisualStyleBackColor = false
        };
        button.FlatAppearance.BorderColor = primary
            ? Color.FromArgb(42, 107, 153)
            : Color.FromArgb(205, 214, 222);
        return button;
    }
}
