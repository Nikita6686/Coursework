using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace AttractionParkApp
{
    public partial class FormMyTickets : Form
    {
        private Label lblSubtitle;
        private Label lblSummary;
        private Button btnRefresh;

        public FormMyTickets()
        {
            InitializeComponent();
        }

        private void FormMyTickets_Load(object sender, EventArgs e)
        {
            UiTheme.ApplyForm(this);
            var header = new Label();
            UiTheme.StyleHeader(header, "История покупок");
            Controls.Add(header);
            header.BringToFront();

            lblSubtitle = new Label
            {
                Text = "Сумма указана на дату покупки и не меняется при изменении цены аттракциона.",
                ForeColor = UiTheme.TextMuted,
                Font = new Font("Segoe UI", 9F),
                AutoSize = false,
                Location = new Point(UiTheme.Pad, UiTheme.HeaderHeight + 4),
                Size = new Size(ClientSize.Width - UiTheme.Pad * 2, 20),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            Controls.Add(lblSubtitle);

            lblSummary = new Label
            {
                ForeColor = UiTheme.Header,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                AutoSize = false,
                Location = new Point(UiTheme.Pad, UiTheme.HeaderHeight + 26),
                Size = new Size(ClientSize.Width - UiTheme.Pad * 2, 22),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            Controls.Add(lblSummary);

            btnRefresh = new Button { Text = "Обновить" };
            UiTheme.ArrangeDialogFooter(this, btnRefresh, btnClose);
            btnRefresh.Click += btnRefresh_Click;

            int gridTop = UiTheme.HeaderHeight + 54;
            UiTheme.LayoutGrid(this, dataGridView1, gridTop, UiTheme.Pad + UiTheme.ButtonHeight + UiTheme.Pad);

            LoadTickets();
        }

        private void LoadTickets()
        {
            var table = DatabaseHelper.GetTicketsForUser(CurrentUser.Id);
            dataGridView1.DataSource = table;
            dataGridView1.ReadOnly = true;

            SetHeader("ID_Ticket", "№");
            SetHeader("AttractionName", "Аттракцион");
            SetHeader("PurchaseDate", "Дата покупки");
            SetHeader("Price", "Сумма, руб.");
            GridHelper.FitColumns(dataGridView1);

            UpdateSummary(table);
        }

        private void UpdateSummary(DataTable table)
        {
            decimal total = 0m;
            foreach (DataRow row in table.Rows)
            {
                if (row["Price"] != DBNull.Value)
                    total += Convert.ToDecimal(row["Price"]);
            }

            lblSummary.Text = "Всего билетов: " + table.Rows.Count + " · Потрачено: " + total.ToString("N0") + " руб.";
        }

        private void SetHeader(string column, string text)
        {
            if (dataGridView1.Columns.Contains(column))
                dataGridView1.Columns[column].HeaderText = text;
        }

        private void btnRefresh_Click(object sender, EventArgs e) => LoadTickets();

        private void btnClose_Click(object sender, EventArgs e) => Close();
    }
}
