using System;
using System.Data;
using System.Windows.Forms;

namespace AttractionParkApp
{
    public partial class FormVisitor : Form
    {
        public FormVisitor()
        {
            InitializeComponent();
        }

        private void FormVisitor_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = "Добро пожаловать, " + CurrentUser.FullName;
            LoadAttractions();
        }

        private void LoadAttractions()
        {
            dataGridView1.DataSource = DatabaseHelper.GetAttractionsForVisitors();

            if (dataGridView1.Columns.Contains("ID_Attraction"))
                dataGridView1.Columns["ID_Attraction"].Visible = false;

            SetColumnHeader("Name", "Название");
            SetColumnHeader("Zone", "Зона");
            SetColumnHeader("CurrentStatus", "Статус");
            SetColumnHeader("TicketPrice", "Цена, руб.");

            GridHelper.FitColumns(dataGridView1);
        }

        private void SetColumnHeader(string columnName, string header)
        {
            if (dataGridView1.Columns.Contains(columnName))
                dataGridView1.Columns[columnName].HeaderText = header;
        }

        private void btnBuyTicket_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выберите аттракцион.", "Покупка билета",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = ((DataRowView)dataGridView1.CurrentRow.DataBoundItem).Row;
            int attractionId = Convert.ToInt32(row["ID_Attraction"]);
            string name = row["Name"].ToString();
            decimal price = row["TicketPrice"] == DBNull.Value ? 0m : Convert.ToDecimal(row["TicketPrice"]);

            var confirm = MessageBox.Show(
                "Купить билет на \"" + name + "\" за " + price.ToString("0") + " руб.?",
                "Покупка билета",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                DatabaseHelper.BuyTicket(CurrentUser.Id, attractionId, price);
                MessageBox.Show(
                    "Билет куплен!\nАттракцион: " + name + "\nСумма: " + price.ToString("0") + " руб.",
                    "Покупка билета",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Покупка билета",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAttractions();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
