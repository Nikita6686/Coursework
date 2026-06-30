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
            this.incidentsTableAdapter.Fill(this.бДDataSet.Incidents);
            GridHelper.FitColumns(dataGridView1);
        }

        private void IncSave_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.incidentsBindingSource.EndEdit();
            this.incidentsTableAdapter.Update(this.бДDataSet.Incidents);
            MessageBox.Show("Данные сохранены!", "Поломки",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void IncClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
