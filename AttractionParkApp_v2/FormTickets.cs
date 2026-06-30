using System;
using System.Data;
using System.Windows.Forms;

namespace AttractionParkApp
{
    public partial class FormTickets : Form
    {
        private DataTable ticketsTable;

        public FormTickets()
        {
            InitializeComponent();
        }

        private void FormTickets_Load(object sender, EventArgs e)
        {
            UiTheme.PrepareDataForm(this, "Проданные билеты", dataGridView1, btnRefresh, btnClose);
            LoadTickets();
        }

        private void LoadTickets()
        {
            ticketsTable = DatabaseHelper.GetTickets();
            dataGridView1.DataSource = ticketsTable;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;

            HideColumn("UserID");
            HideColumn("AttractionID");

            SetColumnHeader("ID_Ticket", "№ билета");
            SetColumnHeader("UserName", "Покупатель");
            SetColumnHeader("AttractionName", "Аттракцион");
            SetColumnHeader("PurchaseDate", "Дата покупки");
            SetColumnHeader("Price", "Цена");
            GridHelper.FitColumns(dataGridView1);
        }

        private void SetColumnHeader(string columnName, string header)
        {
            if (dataGridView1.Columns.Contains(columnName))
                dataGridView1.Columns[columnName].HeaderText = header;
        }

        private void HideColumn(string columnName)
        {
            if (dataGridView1.Columns.Contains(columnName))
                dataGridView1.Columns[columnName].Visible = false;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadTickets();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
