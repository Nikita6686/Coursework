using System;
using System.Data;
using System.Windows.Forms;

namespace AttractionParkApp
{
    public partial class FormUsers : Form
    {
        public FormUsers()
        {
            InitializeComponent();
        }

        private void FormUsers_Load(object sender, EventArgs e)
        {
            this.usersTableAdapter.Fill(this.бДDataSet.Users);
            SetColumnHeaders();
            GridHelper.FitColumns(dataGridView1);
        }

        private void SetColumnHeaders()
        {
            if (dataGridView1.Columns.Contains("fullNameDataGridViewTextBoxColumn"))
                dataGridView1.Columns["fullNameDataGridViewTextBoxColumn"].HeaderText = "ФИО";
            if (dataGridView1.Columns.Contains("loginDataGridViewTextBoxColumn"))
                dataGridView1.Columns["loginDataGridViewTextBoxColumn"].HeaderText = "Логин";
            if (dataGridView1.Columns.Contains("passwordDataGridViewTextBoxColumn"))
                dataGridView1.Columns["passwordDataGridViewTextBoxColumn"].HeaderText = "Пароль";
            if (dataGridView1.Columns.Contains("roleDataGridViewTextBoxColumn"))
                dataGridView1.Columns["roleDataGridViewTextBoxColumn"].HeaderText = "Роль";
        }

        private void UserSave_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.usersBindingSource.EndEdit();

            foreach (DataRow row in бДDataSet.Users.Rows)
            {
                if (row.RowState == DataRowState.Deleted)
                    continue;

                row["Role"] = UserRoles.Normalize(row["Role"] == DBNull.Value ? string.Empty : row["Role"].ToString());

                if (UserRoles.IsAdmin(row["Role"].ToString()) &&
                    !string.Equals(row["Login"].ToString(), "admin", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(
                        "Роль «Администратор» может быть только у пользователя с логином admin.",
                        "Пользователи",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }
            }

            if (DatabaseHelper.HasDuplicateLogins(бДDataSet.Users, out string duplicateLogin))
            {
                MessageBox.Show(
                    "Логин «" + duplicateLogin + "» повторяется. У каждого пользователя логин должен быть уникальным.",
                    "Пользователи",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                this.usersTableAdapter.Update(this.бДDataSet.Users);
                MessageBox.Show("Данные сохранены!", "Пользователи",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось сохранить.\n" + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UserClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
