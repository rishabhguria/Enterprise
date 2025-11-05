namespace Prana.AdminForms
{
    partial class CalendarHolidays
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
            Infragistics.Win.UltraWinTree.Override _override1 = new Infragistics.Win.UltraWinTree.Override();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.trvCalendar = new Infragistics.Win.UltraWinTree.UltraTree();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAddYear = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAddCalendar = new System.Windows.Forms.Button();
            this.ofdCalendar = new System.Windows.Forms.OpenFileDialog();
            this.grpAdd = new System.Windows.Forms.GroupBox();
            this.cSettlementOff = new System.Windows.Forms.CheckBox();
            this.cMarketOff = new System.Windows.Forms.CheckBox();
            this.btnUpload = new System.Windows.Forms.Button();
            this.lblAuecAssociated = new System.Windows.Forms.Label();
            this.dtpicker = new System.Windows.Forms.DateTimePicker();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.btnAddHoliday = new System.Windows.Forms.Button();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.grdHoliday = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.txtCalendar = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblYear = new System.Windows.Forms.Label();
            this.txtYearValue = new System.Windows.Forms.TextBox();
            this.grpCalendar = new System.Windows.Forms.GroupBox();
            this.statusCalendar = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.trvCalendar)).BeginInit();
            this.panel1.SuspendLayout();
            this.grpAdd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdHoliday)).BeginInit();
            this.grpCalendar.SuspendLayout();
            this.statusCalendar.SuspendLayout();
            this.SuspendLayout();
            // 
            // trvCalendar
            // 
            this.trvCalendar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.trvCalendar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trvCalendar.FullRowSelect = true;
            this.trvCalendar.Location = new System.Drawing.Point(0, -1);
            this.trvCalendar.Name = "trvCalendar";
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            appearance1.FontData.BoldAsString = "True";
            _override1.ActiveNodeAppearance = appearance1;
            this.trvCalendar.Override = _override1;
            this.trvCalendar.ShowLines = false;
            this.trvCalendar.Size = new System.Drawing.Size(189, 404);
            this.trvCalendar.TabIndex = 0;
            this.trvCalendar.AfterSelect += new Infragistics.Win.UltraWinTree.AfterNodeSelectEventHandler(this.trvCalendar_AfterSelect);
            this.trvCalendar.BeforeSelect += new Infragistics.Win.UltraWinTree.BeforeNodeSelectEventHandler(this.trvCalendar_BeforeSelect);
            this.trvCalendar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trvCalendar_MouseDown);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.btnAddYear);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnAddCalendar);
            this.panel1.Location = new System.Drawing.Point(0, 406);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(741, 39);
            this.panel1.TabIndex = 2;
            // 
            // btnAddYear
            // 
            this.btnAddYear.Location = new System.Drawing.Point(24, 11);
            this.btnAddYear.Name = "btnAddYear";
            this.btnAddYear.Size = new System.Drawing.Size(75, 23);
            this.btnAddYear.TabIndex = 1;
            this.btnAddYear.Text = "Add Year";
            this.btnAddYear.UseVisualStyleBackColor = true;
            this.btnAddYear.Click += new System.EventHandler(this.btnAddYear_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(369, 11);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(96, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Delete Calendar";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(595, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(505, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAddCalendar
            // 
            this.btnAddCalendar.Location = new System.Drawing.Point(248, 11);
            this.btnAddCalendar.Name = "btnAddCalendar";
            this.btnAddCalendar.Size = new System.Drawing.Size(106, 23);
            this.btnAddCalendar.TabIndex = 2;
            this.btnAddCalendar.Text = "New Calendar";
            this.btnAddCalendar.UseVisualStyleBackColor = true;
            this.btnAddCalendar.Click += new System.EventHandler(this.btnAddCalendar_Click);
            // 
            // ofdCalendar
            // 
            this.ofdCalendar.FileName = "openFileDialog1";
            this.ofdCalendar.Filter = "Excel files(*.xls|*.xls|CSV files(*.csv)|*.csv";
            // 
            // grpAdd
            // 
            this.grpAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpAdd.Controls.Add(this.cSettlementOff);
            this.grpAdd.Controls.Add(this.cMarketOff);
            this.grpAdd.Controls.Add(this.btnUpload);
            this.grpAdd.Controls.Add(this.lblAuecAssociated);
            this.grpAdd.Controls.Add(this.dtpicker);
            this.grpAdd.Controls.Add(this.txtDescription);
            this.grpAdd.Controls.Add(this.btnAddHoliday);
            this.grpAdd.Controls.Add(this.lblDescription);
            this.grpAdd.Controls.Add(this.lblDate);
            this.grpAdd.Enabled = false;
            this.grpAdd.Location = new System.Drawing.Point(194, 322);
            this.grpAdd.Name = "grpAdd";
            this.grpAdd.Size = new System.Drawing.Size(546, 81);
            this.grpAdd.TabIndex = 1;
            this.grpAdd.TabStop = false;
            this.grpAdd.Text = "Add Holiday";
            // 
            // cSettlementOff
            // 
            this.cSettlementOff.AutoSize = true;
            this.cSettlementOff.Checked = true;
            this.cSettlementOff.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cSettlementOff.Location = new System.Drawing.Point(114, 49);
            this.cSettlementOff.Name = "cSettlementOff";
            this.cSettlementOff.Size = new System.Drawing.Size(93, 17);
            this.cSettlementOff.TabIndex = 46;
            this.cSettlementOff.Text = "Settlement Off";
            this.cSettlementOff.UseVisualStyleBackColor = true;
            // 
            // cMarketOff
            // 
            this.cMarketOff.AutoSize = true;
            this.cMarketOff.Checked = true;
            this.cMarketOff.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cMarketOff.Location = new System.Drawing.Point(19, 49);
            this.cMarketOff.Name = "cMarketOff";
            this.cMarketOff.Size = new System.Drawing.Size(76, 17);
            this.cMarketOff.TabIndex = 46;
            this.cMarketOff.Text = "Market Off";
            this.cMarketOff.UseVisualStyleBackColor = true;
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(459, 15);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(75, 23);
            this.btnUpload.TabIndex = 3;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // lblAuecAssociated
            // 
            this.lblAuecAssociated.AutoSize = true;
            this.lblAuecAssociated.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuecAssociated.ForeColor = System.Drawing.Color.Red;
            this.lblAuecAssociated.Location = new System.Drawing.Point(238, 51);
            this.lblAuecAssociated.Name = "lblAuecAssociated";
            this.lblAuecAssociated.Size = new System.Drawing.Size(0, 13);
            this.lblAuecAssociated.TabIndex = 45;
            // 
            // dtpicker
            // 
            this.dtpicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpicker.Location = new System.Drawing.Point(52, 16);
            this.dtpicker.MinDate = new System.DateTime(1800, 1, 1, 0, 0, 0, 0);
            this.dtpicker.Name = "dtpicker";
            this.dtpicker.Size = new System.Drawing.Size(104, 20);
            this.dtpicker.TabIndex = 0;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(239, 16);
            this.txtDescription.MaxLength = 50;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(113, 20);
            this.txtDescription.TabIndex = 1;
            this.txtDescription.Text = "Holiday";
            // 
            // btnAddHoliday
            // 
            this.btnAddHoliday.Location = new System.Drawing.Point(364, 15);
            this.btnAddHoliday.Name = "btnAddHoliday";
            this.btnAddHoliday.Size = new System.Drawing.Size(75, 23);
            this.btnAddHoliday.TabIndex = 2;
            this.btnAddHoliday.Text = "Add";
            this.btnAddHoliday.UseVisualStyleBackColor = true;
            this.btnAddHoliday.Click += new System.EventHandler(this.btnAddHoliday_Click);
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(174, 20);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(60, 13);
            this.lblDescription.TabIndex = 39;
            this.lblDescription.Text = "Description";
            this.lblDescription.Click += new System.EventHandler(this.lblDescription_Click);
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(16, 20);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(30, 13);
            this.lblDate.TabIndex = 37;
            this.lblDate.Text = "Date";
            // 
            // grdHoliday
            // 
            this.grdHoliday.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdHoliday.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.grdHoliday.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.HeaderIcons;
            this.grdHoliday.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdHoliday.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.Standard;
            this.grdHoliday.Location = new System.Drawing.Point(6, 42);
            this.grdHoliday.Name = "grdHoliday";
            this.grdHoliday.Size = new System.Drawing.Size(535, 256);
            this.grdHoliday.TabIndex = 1;
            this.grdHoliday.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdHoliday.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdHoliday.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdHoliday_AfterCellUpdate);
            this.grdHoliday.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdHoliday_InitializeLayout);
            this.grdHoliday.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdHoliday_CellChange);
            this.grdHoliday.Error += new Infragistics.Win.UltraWinGrid.ErrorEventHandler(this.grdHoliday_Error);
            this.grdHoliday.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdHoliday_MouseDown);
            // 
            // txtCalendar
            // 
            this.txtCalendar.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCalendar.Location = new System.Drawing.Point(324, 16);
            this.txtCalendar.MaxLength = 50;
            this.txtCalendar.Name = "txtCalendar";
            this.txtCalendar.Size = new System.Drawing.Size(123, 20);
            this.txtCalendar.TabIndex = 0;
            this.txtCalendar.TextChanged += new System.EventHandler(this.txtCalendar_TextChanged);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(238, 19);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(80, 13);
            this.lblName.TabIndex = 42;
            this.lblName.Text = "Calendar Name";
            // 
            // lblYear
            // 
            this.lblYear.AutoSize = true;
            this.lblYear.Location = new System.Drawing.Point(57, 19);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(29, 13);
            this.lblYear.TabIndex = 44;
            this.lblYear.Text = "Year";
            // 
            // txtYearValue
            // 
            this.txtYearValue.Location = new System.Drawing.Point(92, 16);
            this.txtYearValue.Name = "txtYearValue";
            this.txtYearValue.Size = new System.Drawing.Size(77, 20);
            this.txtYearValue.TabIndex = 0;
            // 
            // grpCalendar
            // 
            this.grpCalendar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpCalendar.Controls.Add(this.txtYearValue);
            this.grpCalendar.Controls.Add(this.lblYear);
            this.grpCalendar.Controls.Add(this.lblName);
            this.grpCalendar.Controls.Add(this.txtCalendar);
            this.grpCalendar.Controls.Add(this.grdHoliday);
            this.grpCalendar.Enabled = false;
            this.grpCalendar.Location = new System.Drawing.Point(194, 12);
            this.grpCalendar.Name = "grpCalendar";
            this.grpCalendar.Size = new System.Drawing.Size(547, 304);
            this.grpCalendar.TabIndex = 0;
            this.grpCalendar.TabStop = false;
            this.grpCalendar.Text = "Calendar";
            // 
            // statusCalendar
            // 
            this.statusCalendar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2});
            this.statusCalendar.Location = new System.Drawing.Point(0, 448);
            this.statusCalendar.Name = "statusCalendar";
            this.statusCalendar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.statusCalendar.Size = new System.Drawing.Size(752, 22);
            this.statusCalendar.TabIndex = 3;
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabel2.ForeColor = System.Drawing.Color.DarkRed;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 17);
            // 
            // CalendarHolidays
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 470);
            this.Controls.Add(this.statusCalendar);
            this.Controls.Add(this.grpCalendar);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.grpAdd);
            this.Controls.Add(this.trvCalendar);
            this.MinimumSize = new System.Drawing.Size(760, 504);
            this.Name = "CalendarHolidays";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Calendar";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CalendarHolidays_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CalendarHolidays_FormClosed);
            this.Load += new System.EventHandler(this.CalendarHolidays_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trvCalendar)).EndInit();
            this.panel1.ResumeLayout(false);
            this.grpAdd.ResumeLayout(false);
            this.grpAdd.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdHoliday)).EndInit();
            this.grpCalendar.ResumeLayout(false);
            this.grpCalendar.PerformLayout();
            this.statusCalendar.ResumeLayout(false);
            this.statusCalendar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.UltraWinTree.UltraTree trvCalendar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnAddCalendar;
        private System.Windows.Forms.OpenFileDialog ofdCalendar;
        private System.Windows.Forms.GroupBox grpAdd;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Label lblAuecAssociated;
        private System.Windows.Forms.DateTimePicker dtpicker;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Button btnAddHoliday;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblDate;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdHoliday;
        private System.Windows.Forms.TextBox txtCalendar;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.TextBox txtYearValue;
        private System.Windows.Forms.GroupBox grpCalendar;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAddYear;
        private System.Windows.Forms.StatusStrip statusCalendar;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.CheckBox cSettlementOff;
        private System.Windows.Forms.CheckBox cMarketOff;
    }
}