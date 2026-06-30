namespace AttractionParkApp

{

    partial class FormVisitor

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

            this.lblWelcome = new System.Windows.Forms.Label();

            this.dataGridView1 = new System.Windows.Forms.DataGridView();

            this.btnBuyTicket = new System.Windows.Forms.Button();

            this.btnTopUp = new System.Windows.Forms.Button();

            this.btnMyTickets = new System.Windows.Forms.Button();

            this.btnRefresh = new System.Windows.Forms.Button();

            this.btnLogout = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();

            this.SuspendLayout();

            this.lblWelcome.Dock = System.Windows.Forms.DockStyle.Top;

            this.lblWelcome.Name = "lblWelcome";

            this.lblWelcome.Size = new System.Drawing.Size(720, 56);

            this.lblWelcome.TabIndex = 0;

            this.lblWelcome.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.dataGridView1.AllowUserToAddRows = false;

            this.dataGridView1.AllowUserToDeleteRows = false;

            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            this.dataGridView1.Location = new System.Drawing.Point(16, 120);

            this.dataGridView1.MultiSelect = false;

            this.dataGridView1.Name = "dataGridView1";

            this.dataGridView1.ReadOnly = true;

            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            this.dataGridView1.Size = new System.Drawing.Size(688, 260);

            this.dataGridView1.TabIndex = 1;

            this.btnBuyTicket.Name = "btnBuyTicket";

            this.btnBuyTicket.Text = "Купить";

            this.btnBuyTicket.UseVisualStyleBackColor = true;

            this.btnBuyTicket.Click += new System.EventHandler(this.btnBuyTicket_Click);

            this.btnTopUp.Name = "btnTopUp";

            this.btnTopUp.Text = "Пополнить";

            this.btnTopUp.UseVisualStyleBackColor = true;

            this.btnTopUp.Click += new System.EventHandler(this.btnTopUp_Click);

            this.btnMyTickets.Name = "btnMyTickets";

            this.btnMyTickets.Text = "Мои билеты";

            this.btnMyTickets.UseVisualStyleBackColor = true;

            this.btnMyTickets.Click += new System.EventHandler(this.btnMyTickets_Click);

            this.btnRefresh.Name = "btnRefresh";

            this.btnRefresh.Text = "Обновить";

            this.btnRefresh.UseVisualStyleBackColor = true;

            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            this.btnLogout.Name = "btnLogout";

            this.btnLogout.Text = "Выйти";

            this.btnLogout.UseVisualStyleBackColor = true;

            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);

            this.ClientSize = new System.Drawing.Size(720, 440);

            this.Controls.Add(this.btnLogout);

            this.Controls.Add(this.btnRefresh);

            this.Controls.Add(this.btnMyTickets);

            this.Controls.Add(this.btnTopUp);

            this.Controls.Add(this.btnBuyTicket);

            this.Controls.Add(this.dataGridView1);

            this.Controls.Add(this.lblWelcome);

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;

            this.MaximizeBox = false;

            this.Name = "FormVisitor";

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            this.Text = "Личный кабинет посетителя";

            this.Load += new System.EventHandler(this.FormVisitor_Load);

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();

            this.ResumeLayout(false);

        }



        private System.Windows.Forms.Label lblWelcome;

        private System.Windows.Forms.DataGridView dataGridView1;

        private System.Windows.Forms.Button btnBuyTicket;

        private System.Windows.Forms.Button btnTopUp;

        private System.Windows.Forms.Button btnMyTickets;

        private System.Windows.Forms.Button btnRefresh;

        private System.Windows.Forms.Button btnLogout;

    }

}


