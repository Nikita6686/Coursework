using System;
using System.Windows.Forms;

namespace AttractionParkApp
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            try
            {
                DatabaseHelper.EnsureDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    DatabaseHelper.GetConnectionErrorMessage(ex),
                    "Ошибка базы данных",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                btnLogin.Enabled = false;
                btnRegister.Enabled = false;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLogin.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Введите логин и пароль.", "Авторизация",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (!DatabaseHelper.TryAuthenticate(
                    txtLogin.Text,
                    txtPassword.Text,
                    out int userId,
                    out string fullName,
                    out string role))
                {
                    MessageBox.Show("Неверный логин или пароль.", "Авторизация",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Clear();
                    txtPassword.Focus();
                    return;
                }

                CurrentUser.Id = userId;
                CurrentUser.Login = txtLogin.Text.Trim();
                CurrentUser.FullName = fullName;
                CurrentUser.Role = role;

                OpenMainForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(DatabaseHelper.GetConnectionErrorMessage(ex), "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenMainForm()
        {
            Form nextForm;
            if (CurrentUser.IsAdmin)
                nextForm = new Form1();
            else if (CurrentUser.IsVisitor)
                nextForm = new FormVisitor();
            else
            {
                MessageBox.Show(
                    "Эта роль доступна только во второй версии программы.",
                    "Авторизация",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                CurrentUser.Clear();
                return;
            }

            Hide();
            nextForm.FormClosed += OnMainFormClosed;
            nextForm.Show();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            using (var form = new FormRegister())
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                    txtLogin.Focus();
            }
        }

        private void OnMainFormClosed(object sender, FormClosedEventArgs e)
        {
            CurrentUser.Clear();
            txtPassword.Clear();
            Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
