namespace AttractionParkApp
{
    partial class Form1
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
            this.MainButtonAtt = new System.Windows.Forms.Button();
            this.MainButtonUsers = new System.Windows.Forms.Button();
            this.MainButtonTickets = new System.Windows.Forms.Button();
            this.MainButtonLogout = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MainButtonAtt
            // 
            this.MainButtonAtt.Location = new System.Drawing.Point(12, 12);
            this.MainButtonAtt.Name = "MainButtonAtt";
            this.MainButtonAtt.Size = new System.Drawing.Size(428, 70);
            this.MainButtonAtt.TabIndex = 0;
            this.MainButtonAtt.Text = "Аттракционы";
            this.MainButtonAtt.UseVisualStyleBackColor = true;
            this.MainButtonAtt.Click += new System.EventHandler(this.MainButtonAtt_Click);
            // 
            // MainButtonUsers
            // 
            this.MainButtonUsers.Location = new System.Drawing.Point(12, 98);
            this.MainButtonUsers.Name = "MainButtonUsers";
            this.MainButtonUsers.Size = new System.Drawing.Size(428, 70);
            this.MainButtonUsers.TabIndex = 1;
            this.MainButtonUsers.Text = "Пользователи";
            this.MainButtonUsers.UseVisualStyleBackColor = true;
            this.MainButtonUsers.Click += new System.EventHandler(this.MainButtonUsers_Click);
            // 
            // MainButtonTickets
            // 
            this.MainButtonTickets.Location = new System.Drawing.Point(12, 184);
            this.MainButtonTickets.Name = "MainButtonTickets";
            this.MainButtonTickets.Size = new System.Drawing.Size(428, 70);
            this.MainButtonTickets.TabIndex = 2;
            this.MainButtonTickets.Text = "Билеты";
            this.MainButtonTickets.UseVisualStyleBackColor = true;
            this.MainButtonTickets.Click += new System.EventHandler(this.MainButtonTickets_Click);
            // 
            // MainButtonLogout
            // 
            this.MainButtonLogout.Location = new System.Drawing.Point(12, 270);
            this.MainButtonLogout.Name = "MainButtonLogout";
            this.MainButtonLogout.Size = new System.Drawing.Size(428, 50);
            this.MainButtonLogout.TabIndex = 3;
            this.MainButtonLogout.Text = "Выйти";
            this.MainButtonLogout.UseVisualStyleBackColor = true;
            this.MainButtonLogout.Click += new System.EventHandler(this.MainButtonLogout_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 335);
            this.Controls.Add(this.MainButtonLogout);
            this.Controls.Add(this.MainButtonTickets);
            this.Controls.Add(this.MainButtonUsers);
            this.Controls.Add(this.MainButtonAtt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Меню администратора";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button MainButtonAtt;
        private System.Windows.Forms.Button MainButtonUsers;
        private System.Windows.Forms.Button MainButtonTickets;
        private System.Windows.Forms.Button MainButtonLogout;
    }
}
