namespace ConceptsChamber
{
    partial class Form1
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
            this.functionalcustomercontractBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.functionalcustomerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.contractdocidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Contractdoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Functionalcustomer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Functionalcustomerid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.functionalcustomercontractBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.functionalcustomerBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.contractdocidDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.Contractdoc,
            this.Functionalcustomer,
            this.Functionalcustomerid});
            this.dataGridView1.DataSource = this.functionalcustomercontractBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(439, 262);
            this.dataGridView1.TabIndex = 0;
            // 
            // functionalcustomercontractBindingSource
            // 
            this.functionalcustomercontractBindingSource.DataSource = typeof(MCDomain.Model.Functionalcustomercontract);
            // 
            // functionalcustomerBindingSource
            // 
            this.functionalcustomerBindingSource.DataSource = typeof(MCDomain.Model.Functionalcustomer);
            // 
            // contractdocidDataGridViewTextBoxColumn
            // 
            this.contractdocidDataGridViewTextBoxColumn.DataPropertyName = "Contractdocid";
            this.contractdocidDataGridViewTextBoxColumn.HeaderText = "Contractdocid";
            this.contractdocidDataGridViewTextBoxColumn.Name = "contractdocidDataGridViewTextBoxColumn";
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            // 
            // Contractdoc
            // 
            this.Contractdoc.DataPropertyName = "Contractdoc";
            this.Contractdoc.HeaderText = "Contractdoc";
            this.Contractdoc.Name = "Contractdoc";
            // 
            // Functionalcustomer
            // 
            this.Functionalcustomer.DataPropertyName = "Functionalcustomer";
            this.Functionalcustomer.HeaderText = "Functionalcustomer";
            this.Functionalcustomer.Name = "Functionalcustomer";
            // 
            // Functionalcustomerid
            // 
            this.Functionalcustomerid.DataPropertyName = "Functionalcustomerid";
            this.Functionalcustomerid.HeaderText = "Functionalcustomerid";
            this.Functionalcustomerid.Name = "Functionalcustomerid";
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(439, 262);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.functionalcustomercontractBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.functionalcustomerBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource functionalcustomercontractBindingSource;
        private System.Windows.Forms.BindingSource functionalcustomerBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn contractdocidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Contractdoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn Functionalcustomer;
        private System.Windows.Forms.DataGridViewTextBoxColumn Functionalcustomerid;


    }
}

