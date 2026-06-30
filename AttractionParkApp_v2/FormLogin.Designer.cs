namespace AttractionParkApp
{
    partial class FormLogin
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelBrand = new System.Windows.Forms.Panel();
            this.lblBrandTitle = new System.Windows.Forms.Label();
            this.lblBrandSubtitle = new System.Windows.Forms.Label();
            this.panelForm = new System.Windows.Forms.Panel();
            this.lblFormTitle = new System.Windows.Forms.Label();
            this.lblLogin = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnRegister = new System.Windows.Forms.Button();
            this.panelBrand.SuspendLayout();
            this.panelForm.SuspendLayout();
            this.SuspendLayout();
            this.panelBrand.Controls.Add(this.lblBrandSubtitle);
            this.panelBrand.Controls.Add(this.lblBrandTitle);
            this.panelBrand.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelBrand.Location = new System.Drawing.Point(0, 0);
            this.panelBrand.Name = "panelBrand";
            this.panelBrand.Size = new System.Drawing.Size(240, 300);
            this.panelBrand.TabIndex = 0;
            this.lblBrandTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.lblBrandTitle.ForeColor = System.Drawing.Color.White;
            this.lblBrandTitle.Location = new System.Drawing.Point(24, 48);
            this.lblBrandTitle.Name = "lblBrandTitle";
            this.lblBrandTitle.Size = new System.Drawing.Size(190, 72);
            this.lblBrandTitle.Text = "Парк\r\nаттракционов";
            this.lblBrandSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBrandSubtitle.ForeColor = System.Drawing.Color.FromArgb(190, 210, 235);
            this.lblBrandSubtitle.Location = new System.Drawing.Point(24, 128);
            this.lblBrandSubtitle.Name = "lblBrandSubtitle";
            this.lblBrandSubtitle.Size = new System.Drawing.Size(190, 40);
            this.lblBrandSubtitle.Text = "Сопровождение информационной системы";
            this.panelForm.Controls.Add(this.btnRegister);
            this.panelForm.Controls.Add(this.btnExit);
            this.panelForm.Controls.Add(this.btnLogin);
            this.panelForm.Controls.Add(this.txtPassword);
            this.panelForm.Controls.Add(this.txtLogin);
            this.panelForm.Controls.Add(this.lblPassword);
            this.panelForm.Controls.Add(this.lblLogin);
            this.panelForm.Controls.Add(this.lblFormTitle);
            this.panelForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelForm.Location = new System.Drawing.Point(240, 0);
            this.panelForm.Name = "panelForm";
            this.panelForm.Padding = new System.Windows.Forms.Padding(28, 24, 28, 20);
            this.panelForm.Size = new System.Drawing.Size(340, 300);
            this.panelForm.TabIndex = 1;
            this.lblFormTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lblFormTitle.Location = new System.Drawing.Point(28, 24);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(280, 28);
            this.lblFormTitle.Text = "Вход в систему";
            this.lblLogin.AutoSize = true;
            this.lblLogin.Location = new System.Drawing.Point(28, 68);
            this.lblLogin.Text = "Логин";
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(28, 118);
            this.lblPassword.Text = "Пароль";
            this.txtLogin.Location = new System.Drawing.Point(28, 86);
            this.txtLogin.Size = new System.Drawing.Size(280, 22);
            this.txtPassword.Location = new System.Drawing.Point(28, 136);
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(280, 22);
            this.btnLogin.Location = new System.Drawing.Point(28, 180);
            this.btnLogin.Size = new System.Drawing.Size(135, 38);
            this.btnLogin.Text = "Войти";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            this.btnExit.Location = new System.Drawing.Point(173, 180);
            this.btnExit.Size = new System.Drawing.Size(135, 38);
            this.btnExit.Text = "Выход";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            this.btnRegister.Location = new System.Drawing.Point(28, 230);
            this.btnRegister.Size = new System.Drawing.Size(280, 36);
            this.btnRegister.Text = "Регистрация";
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            this.AcceptButton = this.btnLogin;
            this.ClientSize = new System.Drawing.Size(580, 300);
            this.Controls.Add(this.panelForm);
            this.Controls.Add(this.panelBrand);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FormLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Авторизация";
            this.Load += new System.EventHandler(this.FormLogin_Load);
            this.panelBrand.ResumeLayout(false);
            this.panelForm.ResumeLayout(false);
            this.panelForm.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panelBrand;
        private System.Windows.Forms.Label lblBrandTitle;
        private System.Windows.Forms.Label lblBrandSubtitle;
        private System.Windows.Forms.Panel panelForm;
        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.Label lblLogin;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnRegister;
    }
}
