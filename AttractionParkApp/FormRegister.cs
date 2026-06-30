using System;
using System.Windows.Forms;

namespace AttractionParkApp
{
    public partial class FormRegister : Form
    {
        public FormRegister()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            var fullName = txtFullName.Text.Trim();
            var login = txtLogin.Text.Trim();
            var password = txtPassword.Text;
            var confirm = txtConfirmPassword.Text;

            if (string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(login) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Заполните все поля.", "Регистрация",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirm)
            {
                MessageBox.Show("Пароли не совпадают.", "Регистрация",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmPassword.Clear();
                txtConfirmPassword.Focus();
                return;
            }

            try
            {
                if (DatabaseHelper.LoginExists(login))
                {
                    MessageBox.Show("Пользователь с таким логином уже существует.", "Регистрация",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtLogin.Focus();
                    return;
                }

                DatabaseHelper.RegisterUser(fullName, login, password);
                MessageBox.Show("Регистрация успешна! Теперь можно войти.", "Регистрация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(DatabaseHelper.GetConnectionErrorMessage(ex), "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
