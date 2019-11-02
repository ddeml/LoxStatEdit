namespace LoxStatEdit
{
    partial class LoxStatFileForm
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
            System.Windows.Forms.Label fileInfoLabel;
            System.Windows.Forms.Button saveButton;
            System.Windows.Forms.Button loadButton;
            System.Windows.Forms.Button browseButton;
            System.Windows.Forms.Label fileNameLabel;
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoxStatFileForm));
            this._problemButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this._fileInfoTextBox = new System.Windows.Forms.TextBox();
            this._dataGridView = new System.Windows.Forms.DataGridView();
            this.indexColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._timestampColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._valueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._fileNameTextBox = new System.Windows.Forms.TextBox();
            fileInfoLabel = new System.Windows.Forms.Label();
            saveButton = new System.Windows.Forms.Button();
            loadButton = new System.Windows.Forms.Button();
            browseButton = new System.Windows.Forms.Button();
            fileNameLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // fileInfoLabel
            // 
            fileInfoLabel.Location = new System.Drawing.Point(12, 43);
            fileInfoLabel.Name = "fileInfoLabel";
            fileInfoLabel.Size = new System.Drawing.Size(60, 13);
            fileInfoLabel.TabIndex = 4;
            fileInfoLabel.Text = "File Info:";
            // 
            // saveButton
            // 
            saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            saveButton.Location = new System.Drawing.Point(497, 38);
            saveButton.Name = "saveButton";
            saveButton.Size = new System.Drawing.Size(75, 23);
            saveButton.TabIndex = 7;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // loadButton
            // 
            loadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            loadButton.Location = new System.Drawing.Point(497, 12);
            loadButton.Name = "loadButton";
            loadButton.Size = new System.Drawing.Size(75, 23);
            loadButton.TabIndex = 3;
            loadButton.Text = "Load";
            loadButton.UseVisualStyleBackColor = true;
            loadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // browseButton
            // 
            browseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            browseButton.Location = new System.Drawing.Point(416, 12);
            browseButton.Name = "browseButton";
            browseButton.Size = new System.Drawing.Size(75, 23);
            browseButton.TabIndex = 2;
            browseButton.Text = "Browse...";
            browseButton.UseVisualStyleBackColor = true;
            browseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // fileNameLabel
            // 
            fileNameLabel.Location = new System.Drawing.Point(12, 17);
            fileNameLabel.Name = "fileNameLabel";
            fileNameLabel.Size = new System.Drawing.Size(60, 13);
            fileNameLabel.TabIndex = 0;
            fileNameLabel.Text = "File Name:";
            // 
            // _problemButton
            // 
            this._problemButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._problemButton.Enabled = false;
            this._problemButton.ForeColor = System.Drawing.Color.Red;
            this._problemButton.Location = new System.Drawing.Point(416, 38);
            this._problemButton.Name = "_problemButton";
            this._problemButton.Size = new System.Drawing.Size(75, 23);
            this._problemButton.TabIndex = 6;
            this._problemButton.Text = "Problems";
            this._problemButton.UseVisualStyleBackColor = true;
            this._problemButton.Click += new System.EventHandler(this.ProblemButton_Click);
            // 
            // _fileInfoTextBox
            // 
            this._fileInfoTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._fileInfoTextBox.Location = new System.Drawing.Point(78, 40);
            this._fileInfoTextBox.Name = "_fileInfoTextBox";
            this._fileInfoTextBox.ReadOnly = true;
            this._fileInfoTextBox.Size = new System.Drawing.Size(332, 20);
            this._fileInfoTextBox.TabIndex = 5;
            this._fileInfoTextBox.Text = "(Enter or browse a file name end press the Load button)";
            // 
            // _dataGridView
            // 
            this._dataGridView.AllowUserToAddRows = false;
            this._dataGridView.AllowUserToDeleteRows = false;
            this._dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.indexColumn,
            this._timestampColumn,
            this._valueColumn});
            this._dataGridView.Location = new System.Drawing.Point(15, 66);
            this._dataGridView.Name = "_dataGridView";
            this._dataGridView.Size = new System.Drawing.Size(557, 205);
            this._dataGridView.TabIndex = 8;
            this._dataGridView.VirtualMode = true;
            this._dataGridView.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.DataGridView_CellValueNeeded);
            this._dataGridView.CellValuePushed += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.DataGridView_CellValuePushed);
            // 
            // indexColumn
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "N0";
            dataGridViewCellStyle1.NullValue = null;
            this.indexColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.indexColumn.HeaderText = "Index";
            this.indexColumn.Name = "indexColumn";
            this.indexColumn.ReadOnly = true;
            this.indexColumn.Width = 60;
            // 
            // _timestampColumn
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Format = "G";
            dataGridViewCellStyle2.NullValue = null;
            this._timestampColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this._timestampColumn.HeaderText = "Timestamp";
            this._timestampColumn.Name = "_timestampColumn";
            this._timestampColumn.Width = 130;
            // 
            // _valueColumn
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N3";
            dataGridViewCellStyle3.NullValue = null;
            this._valueColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this._valueColumn.HeaderText = "Value";
            this._valueColumn.Name = "_valueColumn";
            this._valueColumn.Width = 80;
            // 
            // _fileNameTextBox
            // 
            this._fileNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._fileNameTextBox.Location = new System.Drawing.Point(78, 14);
            this._fileNameTextBox.Name = "_fileNameTextBox";
            this._fileNameTextBox.Size = new System.Drawing.Size(332, 20);
            this._fileNameTextBox.TabIndex = 1;
            // 
            // LoxStatFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 283);
            this.Controls.Add(this._problemButton);
            this.Controls.Add(saveButton);
            this.Controls.Add(this._dataGridView);
            this.Controls.Add(this._fileInfoTextBox);
            this.Controls.Add(fileInfoLabel);
            this.Controls.Add(fileNameLabel);
            this.Controls.Add(loadButton);
            this.Controls.Add(this._fileNameTextBox);
            this.Controls.Add(browseButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LoxStatFileForm";
            this.Text = "Loxone Status File Editor";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this._dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TextBox _fileInfoTextBox;
        private System.Windows.Forms.DataGridView _dataGridView;
        private System.Windows.Forms.Button _problemButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn indexColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _timestampColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _valueColumn;
        private System.Windows.Forms.TextBox _fileNameTextBox;
    }
}

