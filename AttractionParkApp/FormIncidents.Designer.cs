namespace AttractionParkApp
{
    partial class FormIncidents
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.бДDataSet = new AttractionParkApp.БДDataSet();
            this.incidentsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.incidentsTableAdapter = new AttractionParkApp.БДDataSetTableAdapters.IncidentsTableAdapter();
            this.iDIncidentDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.attractionIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.operatorIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.techIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateOpenDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateCloseDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IncSave = new System.Windows.Forms.Button();
            this.IncClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.бДDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.incidentsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iDIncidentDataGridViewTextBoxColumn,
            this.attractionIDDataGridViewTextBoxColumn,
            this.operatorIDDataGridViewTextBoxColumn,
            this.techIDDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.statusDataGridViewTextBoxColumn,
            this.dateOpenDataGridViewTextBoxColumn,
            this.dateCloseDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.incidentsBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(12, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(846, 225);
            this.dataGridView1.TabIndex = 1;
            // 
            // бДDataSet
            // 
            this.бДDataSet.DataSetName = "БДDataSet";
            this.бДDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // incidentsBindingSource
            // 
            this.incidentsBindingSource.DataMember = "Incidents";
            this.incidentsBindingSource.DataSource = this.бДDataSet;
            // 
            // incidentsTableAdapter
            // 
            this.incidentsTableAdapter.ClearBeforeFill = true;
            // 
            // iDIncidentDataGridViewTextBoxColumn
            // 
            this.iDIncidentDataGridViewTextBoxColumn.DataPropertyName = "ID_Incident";
            this.iDIncidentDataGridViewTextBoxColumn.HeaderText = "ID_Incident";
            this.iDIncidentDataGridViewTextBoxColumn.Name = "iDIncidentDataGridViewTextBoxColumn";
            // 
            // attractionIDDataGridViewTextBoxColumn
            // 
            this.attractionIDDataGridViewTextBoxColumn.DataPropertyName = "AttractionID";
            this.attractionIDDataGridViewTextBoxColumn.HeaderText = "AttractionID";
            this.attractionIDDataGridViewTextBoxColumn.Name = "attractionIDDataGridViewTextBoxColumn";
            // 
            // operatorIDDataGridViewTextBoxColumn
            // 
            this.operatorIDDataGridViewTextBoxColumn.DataPropertyName = "OperatorID";
            this.operatorIDDataGridViewTextBoxColumn.HeaderText = "OperatorID";
            this.operatorIDDataGridViewTextBoxColumn.Name = "operatorIDDataGridViewTextBoxColumn";
            // 
            // techIDDataGridViewTextBoxColumn
            // 
            this.techIDDataGridViewTextBoxColumn.DataPropertyName = "TechID";
            this.techIDDataGridViewTextBoxColumn.HeaderText = "TechID";
            this.techIDDataGridViewTextBoxColumn.Name = "techIDDataGridViewTextBoxColumn";
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            // 
            // statusDataGridViewTextBoxColumn
            // 
            this.statusDataGridViewTextBoxColumn.DataPropertyName = "Status";
            this.statusDataGridViewTextBoxColumn.HeaderText = "Status";
            this.statusDataGridViewTextBoxColumn.Name = "statusDataGridViewTextBoxColumn";
            // 
            // dateOpenDataGridViewTextBoxColumn
            // 
            this.dateOpenDataGridViewTextBoxColumn.DataPropertyName = "DateOpen";
            this.dateOpenDataGridViewTextBoxColumn.HeaderText = "DateOpen";
            this.dateOpenDataGridViewTextBoxColumn.Name = "dateOpenDataGridViewTextBoxColumn";
            // 
            // dateCloseDataGridViewTextBoxColumn
            // 
            this.dateCloseDataGridViewTextBoxColumn.DataPropertyName = "DateClose";
            this.dateCloseDataGridViewTextBoxColumn.HeaderText = "DateClose";
            this.dateCloseDataGridViewTextBoxColumn.Name = "dateCloseDataGridViewTextBoxColumn";
            // 
            // IncSave
            // 
            this.IncSave.Location = new System.Drawing.Point(12, 234);
            this.IncSave.Name = "IncSave";
            this.IncSave.Size = new System.Drawing.Size(260, 41);
            this.IncSave.TabIndex = 2;
            this.IncSave.Text = "Сохранить";
            this.IncSave.UseVisualStyleBackColor = true;
            this.IncSave.Click += new System.EventHandler(this.IncSave_Click);
            // 
            // IncClose
            // 
            this.IncClose.Location = new System.Drawing.Point(592, 234);
            this.IncClose.Name = "IncClose";
            this.IncClose.Size = new System.Drawing.Size(260, 41);
            this.IncClose.TabIndex = 3;
            this.IncClose.Text = "Закрыть";
            this.IncClose.UseVisualStyleBackColor = true;
            this.IncClose.Click += new System.EventHandler(this.IncClose_Click);
            // 
            // FormIncidents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 285);
            this.Controls.Add(this.IncClose);
            this.Controls.Add(this.IncSave);
            this.Controls.Add(this.dataGridView1);
            this.Name = "FormIncidents";
            this.Text = "Поломки";
            this.Load += new System.EventHandler(this.FormIncidents_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.бДDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.incidentsBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private БДDataSet бДDataSet;
        private System.Windows.Forms.BindingSource incidentsBindingSource;
        private БДDataSetTableAdapters.IncidentsTableAdapter incidentsTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn iDIncidentDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn attractionIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn operatorIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn techIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateOpenDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateCloseDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button IncSave;
        private System.Windows.Forms.Button IncClose;
    }
}