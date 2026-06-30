namespace AttractionParkApp
{
    partial class FormAttractions
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
            this.AttSave = new System.Windows.Forms.Button();
            this.AttClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(760, 280);
            this.dataGridView1.TabIndex = 0;
            // 
            // AttSave
            // 
            this.AttSave.Location = new System.Drawing.Point(12, 305);
            this.AttSave.Name = "AttSave";
            this.AttSave.Size = new System.Drawing.Size(180, 35);
            this.AttSave.TabIndex = 1;
            this.AttSave.Text = "Сохранить";
            this.AttSave.UseVisualStyleBackColor = true;
            this.AttSave.Click += new System.EventHandler(this.AttSave_Click);
            // 
            // AttClose
            // 
            this.AttClose.Location = new System.Drawing.Point(592, 305);
            this.AttClose.Name = "AttClose";
            this.AttClose.Size = new System.Drawing.Size(180, 35);
            this.AttClose.TabIndex = 2;
            this.AttClose.Text = "Закрыть";
            this.AttClose.UseVisualStyleBackColor = true;
            this.AttClose.Click += new System.EventHandler(this.AttClose_Click);
            // 
            // FormAttractions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 355);
            this.Controls.Add(this.AttClose);
            this.Controls.Add(this.AttSave);
            this.Controls.Add(this.dataGridView1);
            this.Name = "FormAttractions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Аттракционы";
            this.Load += new System.EventHandler(this.FormAttractions_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button AttSave;
        private System.Windows.Forms.Button AttClose;
    }
}
