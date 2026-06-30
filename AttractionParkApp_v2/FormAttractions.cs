using System;
using System.Data;
using System.Windows.Forms;

namespace AttractionParkApp
{
    public partial class FormAttractions : Form
    {
        private DataTable attractionsTable;

        public FormAttractions()
        {
            InitializeComponent();
        }

        private void FormAttractions_Load(object sender, EventArgs e)
        {
            UiTheme.PrepareDataForm(this, "Управление аттракционами", dataGridView1, AttSave, AttClose);
            LoadAttractions();
        }

        private void LoadAttractions()
        {
            attractionsTable = DatabaseHelper.GetAttractionsForAdmin();
            dataGridView1.DataSource = attractionsTable;

            SetColumnHeader("ID_Attraction", "№");
            SetColumnHeader("Name", "Название");
            SetColumnHeader("Zone", "Зона");
            SetColumnHeader("CurrentStatus", "Статус");
            SetColumnHeader("LastServiceDate", "Дата обслуживания");
            SetColumnHeader("TicketPrice", "Цена билета, руб.");

            if (dataGridView1.Columns.Contains("ID_Attraction"))
                dataGridView1.Columns["ID_Attraction"].ReadOnly = true;

            GridHelper.FitColumns(dataGridView1);
        }

        private void SetColumnHeader(string columnName, string header)
        {
            if (dataGridView1.Columns.Contains(columnName))
                dataGridView1.Columns[columnName].HeaderText = header;
        }

        private void AttSave_Click(object sender, EventArgs e)
        {
            dataGridView1.EndEdit();

            foreach (DataRow row in attractionsTable.Rows)
            {
                if (row.RowState == DataRowState.Deleted)
                    continue;

                if (row["CurrentStatus"] != DBNull.Value)
                    row["CurrentStatus"] = AttractionStatus.Normalize(row["CurrentStatus"].ToString());
            }

            try
            {
                DatabaseHelper.SaveAttractions(attractionsTable);
                MessageBox.Show("Данные сохранены!", "Аттракционы",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadAttractions();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось сохранить.\n" + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AttClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
