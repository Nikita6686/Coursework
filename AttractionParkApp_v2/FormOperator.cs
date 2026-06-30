using System;
using System.Data;
using System.Windows.Forms;

namespace AttractionParkApp
{
    public partial class FormOperator : Form
    {
        public FormOperator()
        {
            InitializeComponent();
        }

        private void FormOperator_Load(object sender, EventArgs e)
        {
            UiTheme.ApplyForm(this);
            UiTheme.StyleHeader(lblHeader, "Оператор — " + CurrentUser.FullName);
            UiTheme.StyleFieldLabel(lblDescription);
            UiTheme.StyleInput(txtDescription);
            UiTheme.StyleActionButtons(btnCreateRequest, btnRefresh, btnLogout);

            var footer = UiTheme.ArrangeFooterButtons(this, btnCreateRequest, btnRefresh, btnLogout);
            LayoutOperatorContent(footer.Height);
            LoadAttractions();
        }

        private void LayoutOperatorContent(int footerHeight)
        {
            const int descBlock = 88;
            int descTop = ClientSize.Height - footerHeight - descBlock - UiTheme.Pad;

            lblDescription.Location = new System.Drawing.Point(UiTheme.Pad, descTop);
            lblDescription.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;

            txtDescription.Location = new System.Drawing.Point(UiTheme.Pad, descTop + 20);
            txtDescription.Size = new System.Drawing.Size(ClientSize.Width - UiTheme.Pad * 2, descBlock - 24);
            txtDescription.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            UiTheme.LayoutGrid(this, dataGridView1, UiTheme.HeaderHeight + UiTheme.Pad, footerHeight + descBlock + UiTheme.Pad * 2);
        }

        private void LoadAttractions()
        {
            dataGridView1.DataSource = DatabaseHelper.GetAttractionsForOperator();
            if (dataGridView1.Columns.Contains("ID_Attraction"))
                dataGridView1.Columns["ID_Attraction"].Visible = false;

            SetColumnHeader("Name", "Название");
            SetColumnHeader("Zone", "Зона");
            SetColumnHeader("CurrentStatus", "Статус");
            SetColumnHeader("LastServiceDate", "Дата обслуживания");
            GridHelper.FitColumns(dataGridView1);
        }

        private void SetColumnHeader(string columnName, string header)
        {
            if (dataGridView1.Columns.Contains(columnName))
                dataGridView1.Columns[columnName].HeaderText = header;
        }

        private void btnCreateRequest_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выберите аттракцион.", "Заявка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = ((DataRowView)dataGridView1.CurrentRow.DataBoundItem).Row;
            int id = Convert.ToInt32(row["ID_Attraction"]);
            string name = row["Name"].ToString();

            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Опишите поломку.", "Заявка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDescription.Focus();
                return;
            }

            try
            {
                DatabaseHelper.CreateIncident(id, CurrentUser.Id, txtDescription.Text);
                MessageBox.Show("Заявка по аттракциону \"" + name + "\" создана.",
                    "Заявка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDescription.Clear();
                LoadAttractions();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e) => LoadAttractions();
        private void btnLogout_Click(object sender, EventArgs e) => Close();
    }
}
