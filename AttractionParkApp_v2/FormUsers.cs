using System;
using System.Data;
using System.Windows.Forms;

namespace AttractionParkApp
{
    public partial class FormUsers : Form
    {
        private DataTable usersTable;

        public FormUsers()
        {
            InitializeComponent();
        }

        private void FormUsers_Load(object sender, EventArgs e)
        {
            UiTheme.PrepareDataForm(this, "Пользователи", dataGridView1, UserSave, UserClose);
            UiTheme.ArrangeDialogFooter(this, UserSave, UserClose, btnTopUp);
            UiTheme.StyleOutlineButton(btnTopUp);
            LoadUsers();
        }

        private void LoadUsers()
        {
            usersTable = DatabaseHelper.GetUsersForAdmin();

            foreach (DataRow row in usersTable.Rows)
            {
                row["Role"] = UserRoles.Normalize(row["Role"]?.ToString());
                if (!UserRoles.IsVisitor(row["Role"].ToString()))
                    row["Balance"] = 0m;
            }

            dataGridView1.DataSource = usersTable;
            dataGridView1.AutoGenerateColumns = true;

            if (dataGridView1.Columns.Contains("ID_User"))
                dataGridView1.Columns["ID_User"].HeaderText = "№";
            if (dataGridView1.Columns.Contains("FullName"))
                dataGridView1.Columns["FullName"].HeaderText = "ФИО";
            if (dataGridView1.Columns.Contains("Login"))
                dataGridView1.Columns["Login"].HeaderText = "Логин";
            if (dataGridView1.Columns.Contains("Password"))
                dataGridView1.Columns["Password"].HeaderText = "Пароль";
            if (dataGridView1.Columns.Contains("Role"))
                dataGridView1.Columns["Role"].HeaderText = "Роль";
            if (dataGridView1.Columns.Contains("Balance"))
            {
                dataGridView1.Columns["Balance"].HeaderText = "Баланс, руб.";
                dataGridView1.Columns["Balance"].DefaultCellStyle.Format = "N0";
            }

            GridHelper.FitColumns(dataGridView1);
            dataGridView1.CellFormatting += DataGridView1_CellFormatting;
        }

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (dataGridView1.Columns[e.ColumnIndex].Name != "Balance" &&
                dataGridView1.Columns[e.ColumnIndex].DataPropertyName != "Balance")
                return;

            var rowView = dataGridView1.Rows[e.RowIndex].DataBoundItem as DataRowView;
            if (rowView == null)
                return;

            if (!UserRoles.IsVisitor(rowView.Row["Role"]?.ToString()))
            {
                e.Value = "—";
                e.FormattingApplied = true;
            }
        }

        private DataRow GetSelectedRow()
        {
            if (dataGridView1.CurrentRow?.DataBoundItem is DataRowView view)
                return view.Row;
            return null;
        }

        private void UserSave_Click(object sender, EventArgs e)
        {
            dataGridView1.EndEdit();

            foreach (DataRow row in usersTable.Rows)
            {
                if (row.RowState == DataRowState.Deleted)
                    continue;

                row["Role"] = UserRoles.Normalize(row["Role"] == DBNull.Value ? string.Empty : row["Role"].ToString());

                if (!UserRoles.IsVisitor(row["Role"].ToString()))
                    row["Balance"] = 0m;

                if (UserRoles.IsAdmin(row["Role"].ToString()) &&
                    !string.Equals(row["Login"].ToString(), "admin", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(
                        "Роль «Администратор» может быть только у пользователя с логином admin.",
                        "Пользователи", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (DatabaseHelper.HasDuplicateLogins(usersTable, out string duplicateLogin))
            {
                MessageBox.Show("Логин «" + duplicateLogin + "» повторяется.",
                    "Пользователи", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DatabaseHelper.SaveUsers(usersTable);
                MessageBox.Show("Данные сохранены!", "Пользователи",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось сохранить.\n" + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTopUp_Click(object sender, EventArgs e)
        {
            var row = GetSelectedRow();
            if (row == null)
            {
                MessageBox.Show("Выберите пользователя.", "Пополнение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!UserRoles.IsVisitor(row["Role"].ToString()))
            {
                MessageBox.Show("Баланс доступен только для посетителей.", "Пополнение",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var form = new FormTopUp())
            {
                if (form.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    int userId = Convert.ToInt32(row["ID_User"]);
                    DatabaseHelper.TopUpBalance(userId, form.SelectedAmount);
                    MessageBox.Show("Баланс пополнен.", "Пополнение",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUsers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Пополнение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UserClose_Click(object sender, EventArgs e) => Close();
    }
}
