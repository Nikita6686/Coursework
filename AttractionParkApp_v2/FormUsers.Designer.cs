namespace AttractionParkApp
{
    partial class FormUsers
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.UserSave = new System.Windows.Forms.Button();
            this.UserClose = new System.Windows.Forms.Button();
            this.btnTopUp = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(760, 280);
            this.dataGridView1.TabIndex = 0;
            this.UserSave.Location = new System.Drawing.Point(12, 305);
            this.UserSave.Name = "UserSave";
            this.UserSave.Size = new System.Drawing.Size(180, 35);
            this.UserSave.TabIndex = 1;
            this.UserSave.Text = "Сохранить";
            this.UserSave.UseVisualStyleBackColor = true;
            this.UserSave.Click += new System.EventHandler(this.UserSave_Click);
            this.btnTopUp.Location = new System.Drawing.Point(300, 305);
            this.btnTopUp.Name = "btnTopUp";
            this.btnTopUp.Size = new System.Drawing.Size(180, 35);
            this.btnTopUp.TabIndex = 2;
            this.btnTopUp.Text = "Пополнить";
            this.btnTopUp.UseVisualStyleBackColor = true;
            this.btnTopUp.Click += new System.EventHandler(this.btnTopUp_Click);
            this.UserClose.Location = new System.Drawing.Point(592, 305);
            this.UserClose.Name = "UserClose";
            this.UserClose.Size = new System.Drawing.Size(180, 35);
            this.UserClose.TabIndex = 3;
            this.UserClose.Text = "Закрыть";
            this.UserClose.UseVisualStyleBackColor = true;
            this.UserClose.Click += new System.EventHandler(this.UserClose_Click);
            this.ClientSize = new System.Drawing.Size(784, 355);
            this.Controls.Add(this.UserClose);
            this.Controls.Add(this.btnTopUp);
            this.Controls.Add(this.UserSave);
            this.Controls.Add(this.dataGridView1);
            this.Name = "FormUsers";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Пользователи";
            this.Load += new System.EventHandler(this.FormUsers_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button UserSave;
        private System.Windows.Forms.Button UserClose;
        private System.Windows.Forms.Button btnTopUp;
    }
}
