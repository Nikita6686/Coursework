using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace AttractionParkApp
{
    public partial class FormTechnician : Form
    {
        private static readonly Color RowNew = Color.FromArgb(255, 249, 230);
        private static readonly Color RowMine = Color.FromArgb(220, 237, 255);
        private static readonly Color RowOther = Color.FromArgb(242, 242, 242);

        public FormTechnician()
        {
            InitializeComponent();
        }

        private void FormTechnician_Load(object sender, EventArgs e)
        {
            UiTheme.ApplyForm(this);
            UiTheme.StyleHeader(lblHeader, "Техник — " + CurrentUser.FullName);
            UiTheme.StyleActionButtons(btnTake, btnComplete, btnRefresh, btnLogout);
            UiTheme.LayoutContentWithFooter(this, dataGridView1, btnTake, btnComplete, btnRefresh, btnLogout);
            dataGridView1.DataBindingComplete += DataGridView1_DataBindingComplete;
            LoadIncidents();
        }

        private void LoadIncidents()
        {
            dataGridView1.DataSource = DatabaseHelper.GetIncidentsForTechnician(CurrentUser.Id);

            if (dataGridView1.Columns.Contains("ID_Incident"))
                dataGridView1.Columns["ID_Incident"].Visible = false;
            if (dataGridView1.Columns.Contains("OperatorID"))
                dataGridView1.Columns["OperatorID"].Visible = false;
            if (dataGridView1.Columns.Contains("AttractionID"))
                dataGridView1.Columns["AttractionID"].Visible = false;
            if (dataGridView1.Columns.Contains("TechID"))
                dataGridView1.Columns["TechID"].Visible = false;
            if (dataGridView1.Columns.Contains("TechnicianName"))
                dataGridView1.Columns["TechnicianName"].Visible = false;

            SetColumnHeader("AttractionName", "Аттракцион");
            SetColumnHeader("OperatorName", "Оператор");
            SetColumnHeader("Description", "Описание");
            SetColumnHeader("Status", "Статус");
            SetColumnHeader("Assignment", "Назначение");
            SetColumnHeader("DateOpen", "Дата заявки");

            if (dataGridView1.Columns.Contains("Assignment"))
                dataGridView1.Columns["Assignment"].DisplayIndex = 3;

            GridHelper.FitColumns(dataGridView1);
            ApplyIncidentRowStyles();
        }

        private void DataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            ApplyIncidentRowStyles();
        }

        private void ApplyIncidentRowStyles()
        {
            foreach (DataGridViewRow gridRow in dataGridView1.Rows)
            {
                if (gridRow.DataBoundItem is DataRowView view)
                {
                    var assignment = view.Row["Assignment"]?.ToString() ?? string.Empty;
                    if (assignment == "Моя заявка")
                    {
                        gridRow.DefaultCellStyle.BackColor = RowMine;
                        gridRow.DefaultCellStyle.ForeColor = Color.FromArgb(13, 71, 161);
                        gridRow.DefaultCellStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);
                    }
                    else if (assignment == "Не назначена")
                    {
                        gridRow.DefaultCellStyle.BackColor = RowNew;
                        gridRow.DefaultCellStyle.ForeColor = Color.FromArgb(40, 50, 65);
                        gridRow.DefaultCellStyle.Font = dataGridView1.Font;
                    }
                    else
                    {
                        gridRow.DefaultCellStyle.BackColor = RowOther;
                        gridRow.DefaultCellStyle.ForeColor = Color.FromArgb(110, 110, 110);
                        gridRow.DefaultCellStyle.Font = dataGridView1.Font;
                    }
                }
            }
        }

        private void SetColumnHeader(string columnName, string header)
        {
            if (dataGridView1.Columns.Contains(columnName))
                dataGridView1.Columns[columnName].HeaderText = header;
        }

        private int? GetSelectedIncidentId()
        {
            if (dataGridView1.CurrentRow == null)
                return null;
            return Convert.ToInt32(
                ((DataRowView)dataGridView1.CurrentRow.DataBoundItem).Row["ID_Incident"]);
        }

        private string GetSelectedAssignment()
        {
            if (dataGridView1.CurrentRow == null)
                return null;
            return ((DataRowView)dataGridView1.CurrentRow.DataBoundItem).Row["Assignment"]?.ToString();
        }

        private void btnTake_Click(object sender, EventArgs e)
        {
            var id = GetSelectedIncidentId();
            if (!id.HasValue)
            {
                MessageBox.Show("Выберите заявку.", "Заявки",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (GetSelectedAssignment() != "Не назначена")
            {
                MessageBox.Show("Эту заявку уже взял другой техник.", "Заявки",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                DatabaseHelper.TakeIncident(id.Value, CurrentUser.Id);
                MessageBox.Show("Заявка взята в работу.", "Заявки",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadIncidents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Заявки", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnComplete_Click(object sender, EventArgs e)
        {
            var id = GetSelectedIncidentId();
            if (!id.HasValue)
            {
                MessageBox.Show("Выберите заявку.", "Заявки",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (GetSelectedAssignment() != "Моя заявка")
            {
                MessageBox.Show("Завершить можно только свою заявку.", "Заявки",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Отметить заявку как исправленную?",
                    "Заявки", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                DatabaseHelper.CompleteIncident(id.Value, CurrentUser.Id);
                MessageBox.Show("Ремонт завершён. Аттракцион снова работает.", "Заявки",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadIncidents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Заявки", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e) => LoadIncidents();
        private void btnLogout_Click(object sender, EventArgs e) => Close();
    }
}
