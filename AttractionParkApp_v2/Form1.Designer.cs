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
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.flowStats = new System.Windows.Forms.FlowLayoutPanel();
            this.tableMenu = new System.Windows.Forms.TableLayoutPanel();
            this.MainButtonIncidents = new System.Windows.Forms.Button();
            this.MainButtonTickets = new System.Windows.Forms.Button();
            this.MainButtonUsers = new System.Windows.Forms.Button();
            this.MainButtonAtt = new System.Windows.Forms.Button();
            this.MainButtonLogout = new System.Windows.Forms.Button();
            this.panelMain.SuspendLayout();
            this.layoutMain.SuspendLayout();
            this.tableMenu.SuspendLayout();
            this.SuspendLayout();
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.TabIndex = 0;
            this.lblSubtitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.lblSubtitle.Size = new System.Drawing.Size(760, 24);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.panelMain.Controls.Add(this.layoutMain);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 80);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(20, 12, 20, 16);
            this.panelMain.Size = new System.Drawing.Size(760, 420);
            this.panelMain.TabIndex = 2;
            this.layoutMain.ColumnCount = 1;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Controls.Add(this.flowStats, 0, 0);
            this.layoutMain.Controls.Add(this.tableMenu, 0, 1);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 2;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Size = new System.Drawing.Size(720, 392);
            this.layoutMain.TabIndex = 0;
            this.flowStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowStats.Name = "flowStats";
            this.flowStats.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.flowStats.WrapContents = false;
            this.tableMenu.ColumnCount = 1;
            this.tableMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableMenu.Controls.Add(this.MainButtonIncidents, 0, 0);
            this.tableMenu.Controls.Add(this.MainButtonTickets, 0, 1);
            this.tableMenu.Controls.Add(this.MainButtonUsers, 0, 2);
            this.tableMenu.Controls.Add(this.MainButtonAtt, 0, 3);
            this.tableMenu.Controls.Add(this.MainButtonLogout, 0, 5);
            this.tableMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableMenu.Name = "tableMenu";
            this.tableMenu.RowCount = 6;
            this.tableMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.MainButtonIncidents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainButtonIncidents.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.MainButtonIncidents.Name = "MainButtonIncidents";
            this.MainButtonIncidents.Text = "Заявки на ремонт";
            this.MainButtonIncidents.UseVisualStyleBackColor = true;
            this.MainButtonIncidents.Click += new System.EventHandler(this.MainButtonIncidents_Click);
            this.MainButtonTickets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainButtonTickets.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.MainButtonTickets.Name = "MainButtonTickets";
            this.MainButtonTickets.Text = "Билеты";
            this.MainButtonTickets.UseVisualStyleBackColor = true;
            this.MainButtonTickets.Click += new System.EventHandler(this.MainButtonTickets_Click);
            this.MainButtonUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainButtonUsers.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.MainButtonUsers.Name = "MainButtonUsers";
            this.MainButtonUsers.Text = "Пользователи";
            this.MainButtonUsers.UseVisualStyleBackColor = true;
            this.MainButtonUsers.Click += new System.EventHandler(this.MainButtonUsers_Click);
            this.MainButtonAtt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainButtonAtt.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.MainButtonAtt.Name = "MainButtonAtt";
            this.MainButtonAtt.Text = "Аттракционы";
            this.MainButtonAtt.UseVisualStyleBackColor = true;
            this.MainButtonAtt.Click += new System.EventHandler(this.MainButtonAtt_Click);
            this.MainButtonLogout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainButtonLogout.Name = "MainButtonLogout";
            this.MainButtonLogout.Text = "Выйти";
            this.MainButtonLogout.UseVisualStyleBackColor = true;
            this.MainButtonLogout.Click += new System.EventHandler(this.MainButtonLogout_Click);
            this.ClientSize = new System.Drawing.Size(760, 500);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.lblSubtitle);
            this.Controls.Add(this.lblHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Панель управления";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelMain.ResumeLayout(false);
            this.layoutMain.ResumeLayout(false);
            this.tableMenu.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.TableLayoutPanel layoutMain;
        private System.Windows.Forms.FlowLayoutPanel flowStats;
        private System.Windows.Forms.TableLayoutPanel tableMenu;
        private System.Windows.Forms.Button MainButtonAtt;
        private System.Windows.Forms.Button MainButtonUsers;
        private System.Windows.Forms.Button MainButtonTickets;
        private System.Windows.Forms.Button MainButtonIncidents;
        private System.Windows.Forms.Button MainButtonLogout;
    }
}
