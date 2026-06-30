using System;
using System.Windows.Forms;

namespace AttractionParkApp
{
    public partial class FormIncidents : Form
    {
        public FormIncidents()
        {
            InitializeComponent();
        }

        private void FormIncidents_Load(object sender, EventArgs e)
        {
            UiTheme.PrepareDataForm(this, "Заявки на ремонт", dataGridView1, null, IncClose);
            IncSave.Visible = false;

            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.DataSource = DatabaseHelper.GetIncidentsForAdmin();

            SetHeader("ID_Incident", "№");
            SetHeader("AttractionName", "Аттракцион");
            SetHeader("OperatorName", "Оператор");
            SetHeader("TechnicianName", "Техник");
            SetHeader("Description", "Описание");
            SetHeader("Status", "Статус");
            SetHeader("DateOpen", "Открыта");
            SetHeader("DateClose", "Закрыта");

            GridHelper.FitColumns(dataGridView1);
        }

        private void SetHeader(string column, string text)
        {
            if (dataGridView1.Columns.Contains(column))
                dataGridView1.Columns[column].HeaderText = text;
        }

        private void IncClose_Click(object sender, EventArgs e) => Close();
    }
}
