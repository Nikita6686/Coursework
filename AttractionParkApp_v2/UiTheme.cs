using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AttractionParkApp
{
    public static class UiTheme
    {
        public static readonly Color Background = Color.FromArgb(241, 245, 251);
        public static readonly Color Surface = Color.White;
        public static readonly Color Header = Color.FromArgb(12, 45, 92);
        public static readonly Color HeaderDark = Color.FromArgb(8, 32, 68);
        public static readonly Color HeaderText = Color.White;
        public static readonly Color Accent = Color.FromArgb(0, 150, 136);
        public static readonly Color AccentWarm = Color.FromArgb(255, 111, 0);
        public static readonly Color Primary = Color.FromArgb(25, 118, 210);
        public static readonly Color PrimaryText = Color.White;
        public static readonly Color GridHeader = Color.FromArgb(25, 118, 210);
        public static readonly Color GridAltRow = Color.FromArgb(248, 251, 255);
        public static readonly Color GridBorder = Color.FromArgb(207, 216, 230);
        public static readonly Color TextMuted = Color.FromArgb(96, 112, 128);
        public static readonly Color Success = Color.FromArgb(46, 125, 50);
        public static readonly Color CardShadow = Color.FromArgb(220, 228, 238);

        public const int Pad = 16;
        public const int HeaderHeight = 56;
        public const int ButtonHeight = 38;
        public const int ButtonGap = 10;
        public const int FooterHeight = 62;

        private const string HeaderControlName = "uiFormHeader";
        private const string FooterControlName = "uiFooterPanel";

        private static readonly Font FormFont = new Font("Segoe UI", 9F);
        private static readonly Font HeaderFont = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
        private static readonly Font ButtonFont = new Font("Segoe UI", 9.25F, FontStyle.Bold);
        private static readonly Font StatValueFont = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
        private static readonly Font StatTitleFont = new Font("Segoe UI", 8.5F);

        public static void ApplyForm(Form form)
        {
            form.BackColor = Background;
            form.Font = FormFont;
        }

        public static void StyleHeader(Label label, string text)
        {
            label.Text = text;
            label.BackColor = Header;
            label.ForeColor = HeaderText;
            label.Font = HeaderFont;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Dock = DockStyle.Top;
            label.Height = HeaderHeight;
        }

        public static void PrepareProLogin(Form form, Panel panelBrand, Panel panelForm)
        {
            ApplyForm(form);
            form.Text = "Авторизация";

            panelBrand.BackColor = HeaderDark;
            panelBrand.Dock = DockStyle.Left;
            panelBrand.Width = 240;
            panelBrand.Padding = new Padding(24, 32, 24, 24);

            panelForm.BackColor = Surface;
            panelForm.Dock = DockStyle.Fill;
            panelForm.Padding = new Padding(28, 24, 28, 20);
        }

        public static Panel CreateStatCard(string title, string value, Color accent, int width = 132)
        {
            var card = new Panel
            {
                Size = new Size(width, 72),
                BackColor = Surface,
                Margin = new Padding(0, 0, 12, 0),
                Padding = new Padding(14, 10, 10, 10)
            };

            card.Paint += (s, e) =>
            {
                var rect = card.ClientRectangle;
                rect.Width -= 1;
                rect.Height -= 1;
                using (var pen = new Pen(GridBorder))
                    e.Graphics.DrawRectangle(pen, rect);
                using (var brush = new SolidBrush(accent))
                    e.Graphics.FillRectangle(brush, 0, 0, 4, card.Height);
            };

            var lblValue = new Label
            {
                Text = value,
                Font = StatValueFont,
                ForeColor = Header,
                Location = new Point(16, 8),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            var lblTitle = new Label
            {
                Text = title,
                Font = StatTitleFont,
                ForeColor = TextMuted,
                Location = new Point(16, 42),
                Size = new Size(width - 20, 28),
                BackColor = Color.Transparent
            };

            card.Controls.Add(lblValue);
            card.Controls.Add(lblTitle);
            card.Tag = lblValue;
            return card;
        }

        public static void UpdateStatCard(Panel card, string value)
        {
            if (card?.Tag is Label label)
                label.Text = value;
        }

        public static Panel CreateBalanceCard(Label balanceLabel)
        {
            var card = new Panel
            {
                Height = 44,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(232, 245, 233),
                Padding = new Padding(Pad, 8, Pad, 8)
            };

            balanceLabel.Dock = DockStyle.Fill;
            balanceLabel.TextAlign = ContentAlignment.MiddleCenter;
            balanceLabel.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            balanceLabel.ForeColor = Success;
            balanceLabel.BackColor = Color.Transparent;

            card.Controls.Add(balanceLabel);
            return card;
        }

        public static void SetBalanceText(Label label, decimal balance)
        {
            label.Text = "Баланс: " + balance.ToString("N0") + " руб.";
            label.ForeColor = balance > 0 ? Success : AccentWarm;
        }

        public static void PrepareDataForm(Form form, string title, DataGridView grid, Button primary, Button secondary)
        {
            ApplyForm(form);
            EnsureHeader(form, title);

            if (form.Width < 720)
                form.Width = 720;

            int bottom = Pad + ButtonHeight + Pad;
            LayoutGrid(form, grid, HeaderHeight + Pad, bottom);
            ArrangeDialogFooter(form, primary, secondary);
        }

        public static void PrepareLoginForm(Form form, Label title, Label subtitle, TextBox login, TextBox password, params Button[] buttons)
        {
            ApplyForm(form);
            title.BackColor = Header;
            title.ForeColor = HeaderText;
            title.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold);
            title.Dock = DockStyle.Top;
            title.Height = HeaderHeight;
            title.TextAlign = ContentAlignment.MiddleCenter;

            if (subtitle != null)
            {
                subtitle.ForeColor = TextMuted;
                subtitle.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            }

            StyleInput(login);
            StyleInput(password);
            StyleActionButtons(buttons);
        }

        public static void PrepareRegisterForm(Form form, Label title, TextBox[] inputs, Button register, Button cancel)
        {
            ApplyForm(form);
            StyleHeader(title, title.Text);
            foreach (var input in inputs)
                StyleInput(input);
            StylePrimaryButton(register);
            StyleSecondaryButton(cancel);
        }

        public static void LayoutContentWithFooter(Form form, DataGridView grid, params Button[] footerButtons)
        {
            var footer = ArrangeFooterButtons(form, footerButtons);
            LayoutGrid(form, grid, HeaderHeight + Pad, footer.Height + Pad);
        }

        public static void LayoutGridBelowHeader(Form form, DataGridView grid, int bottomReserved)
        {
            LayoutGrid(form, grid, HeaderHeight + Pad, bottomReserved);
        }

        public static void LayoutGrid(Form form, DataGridView grid, int top, int bottomReserved)
        {
            if (grid == null)
                return;

            grid.Dock = DockStyle.None;
            grid.Location = new Point(Pad, top);
            grid.Size = new Size(
                System.Math.Max(120, form.ClientSize.Width - Pad * 2),
                System.Math.Max(80, form.ClientSize.Height - top - bottomReserved));
            grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            StyleGrid(grid);
        }

        public static Panel ArrangeFooterButtons(Form form, params Button[] buttons)
        {
            var footer = form.Controls.Find(FooterControlName, false).FirstOrDefault() as Panel;
            if (footer == null)
            {
                footer = new Panel
                {
                    Name = FooterControlName,
                    Dock = DockStyle.Bottom,
                    Height = FooterHeight,
                    BackColor = Background,
                    Padding = new Padding(Pad, 8, Pad, 12)
                };
                form.Controls.Add(footer);
            }

            footer.Controls.Clear();
            foreach (var button in buttons)
            {
                if (button == null)
                    continue;
                if (button.Parent != null && button.Parent != footer)
                    button.Parent.Controls.Remove(button);
            }

            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = buttons.Length,
                RowCount = 1,
                BackColor = Background
            };

            for (int i = 0; i < buttons.Length; i++)
            {
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / buttons.Length));
                if (buttons[i] == null)
                    continue;
                buttons[i].Dock = DockStyle.Fill;
                buttons[i].Height = ButtonHeight;
                buttons[i].Margin = new Padding(i == 0 ? 0 : 4, 0, i == buttons.Length - 1 ? 0 : 4, 0);
                table.Controls.Add(buttons[i], i, 0);
            }

            footer.Controls.Add(table);
            footer.BringToFront();
            return footer;
        }

        public static void ArrangeDialogFooter(Form form, Button primary, Button secondary, Button center = null)
        {
            int y = form.ClientSize.Height - Pad - ButtonHeight;

            if (primary != null)
            {
                StylePrimaryButton(primary);
                primary.Size = new Size(150, ButtonHeight);
                primary.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                primary.Location = new Point(Pad, y);
            }

            if (center != null)
            {
                StyleOutlineButton(center);
                center.Size = new Size(150, ButtonHeight);
                center.Anchor = AnchorStyles.Bottom;
                center.Location = new Point((form.ClientSize.Width - center.Width) / 2, y);
            }

            if (secondary != null)
            {
                StyleSecondaryButton(secondary);
                secondary.Size = new Size(150, ButtonHeight);
                secondary.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                secondary.Location = new Point(form.ClientSize.Width - Pad - 150, y);
            }
        }

        public static void StyleActionButtons(params Button[] buttons)
        {
            foreach (var button in buttons)
            {
                if (button == null)
                    continue;

                if (button.Text == "Выйти" || button.Text == "Закрыть" || button.Text == "Выход" || button.Text == "Отмена")
                    StyleSecondaryButton(button);
                else if (button.Text.Contains("Пополнить"))
                    StyleOutlineButton(button);
                else if (button.Text.Contains("Создать") || button.Text.Contains("Завершить") || button.Text.Contains("Купить"))
                    StylePrimaryButton(button, accent: true);
                else
                    StylePrimaryButton(button);
            }
        }

        public static void StyleGrid(DataGridView grid)
        {
            grid.EnableHeadersVisualStyles = false;
            grid.BackgroundColor = Surface;
            grid.BorderStyle = BorderStyle.FixedSingle;
            grid.GridColor = GridBorder;
            grid.RowHeadersVisible = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.DefaultCellStyle.BackColor = Surface;
            grid.DefaultCellStyle.ForeColor = Color.FromArgb(40, 50, 65);
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(227, 242, 253);
            grid.DefaultCellStyle.SelectionForeColor = Color.FromArgb(13, 71, 161);
            grid.DefaultCellStyle.Padding = new Padding(6, 2, 6, 2);
            grid.AlternatingRowsDefaultCellStyle.BackColor = GridAltRow;
            grid.ColumnHeadersDefaultCellStyle.BackColor = GridHeader;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = PrimaryText;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.ColumnHeadersHeight = 36;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.RowTemplate.Height = 32;
        }

        public static void StylePrimaryButton(Button button, bool accent = false)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = accent ? AccentWarm : Primary;
            button.ForeColor = PrimaryText;
            button.Font = ButtonFont;
            button.Cursor = Cursors.Hand;
            button.FlatAppearance.MouseOverBackColor = accent
                ? Color.FromArgb(255, 143, 0)
                : Color.FromArgb(30, 136, 229);
        }

        public static void StyleSecondaryButton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = GridBorder;
            button.BackColor = Surface;
            button.ForeColor = Color.FromArgb(50, 65, 85);
            button.Font = ButtonFont;
            button.Cursor = Cursors.Hand;
        }

        public static void StyleOutlineButton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = Accent;
            button.BackColor = Surface;
            button.ForeColor = Accent;
            button.Font = ButtonFont;
            button.Cursor = Cursors.Hand;
        }

        public static void StyleMenuButton(Button button) => StylePrimaryButton(button);

        public static void StyleFieldLabel(Label label)
        {
            label.ForeColor = TextMuted;
            label.Font = FormFont;
        }

        public static void StyleInput(TextBox textBox)
        {
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Font = FormFont;
            textBox.BackColor = Surface;
        }

        private static void EnsureHeader(Form form, string title)
        {
            var header = form.Controls.Find(HeaderControlName, false).FirstOrDefault() as Label;
            if (header == null)
            {
                header = new Label { Name = HeaderControlName };
                form.Controls.Add(header);
            }

            StyleHeader(header, title);
            header.BringToFront();
        }
    }
}
