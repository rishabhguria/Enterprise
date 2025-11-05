namespace Prana.Tools
{
    partial class ctrlReconReportLayout
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolTip.UltraToolTipInfo ultraToolTipInfo1 = new Infragistics.Win.UltraWinToolTip.UltraToolTipInfo("Drag Columns From Available Columns to Selected Columns", Infragistics.Win.ToolTipImage.Default, null, Infragistics.Win.DefaultableBoolean.Default);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.lblAvailableColumns = new Infragistics.Win.Misc.UltraLabel();
            this.lblSelectedColumns = new Infragistics.Win.Misc.UltraLabel();
            this.tooltipShowDragMessage = new Infragistics.Win.UltraWinToolTip.UltraToolTipManager(this.components);
            this.listviewAvailableColumns = new Infragistics.Win.UltraWinListView.UltraListView();
            this.listviewSelectedColumns = new Infragistics.Win.UltraWinListView.UltraListView();
            this.groupBox2 = new Infragistics.Win.Misc.UltraGroupBox();
            this.chkbxMatchedinTol = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkbxExactlyMatched = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.groupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.rbPDFFormat = new System.Windows.Forms.RadioButton();
            this.rbXLSFormat = new System.Windows.Forms.RadioButton();
            this.rbCSVFormat = new System.Windows.Forms.RadioButton();
            this.btnSortAscending = new Infragistics.Win.Misc.UltraButton();
            this.btnSortDescending = new Infragistics.Win.Misc.UltraButton();
            this.listviewColumnForSorting = new Infragistics.Win.UltraWinListView.UltraListView();
            this.lblColumntoSort = new Infragistics.Win.Misc.UltraLabel();
            this.btnFlipSortOrder = new Infragistics.Win.Misc.UltraButton();
            this.btnDeleteSortOrder = new Infragistics.Win.Misc.UltraButton();
            this.listviewGroupByColumns = new Infragistics.Win.UltraWinListView.UltraListView();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.btnGroupBy = new Infragistics.Win.Misc.UltraButton();
            this.btnDeleteGroupBy = new Infragistics.Win.Misc.UltraButton();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.listviewAvailableColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listviewSelectedColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox2)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxMatchedinTol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxExactlyMatched)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listviewColumnForSorting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listviewGroupByColumns)).BeginInit();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblAvailableColumns
            // 
            this.lblAvailableColumns.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvailableColumns.Location = new System.Drawing.Point(33, 83);
            this.lblAvailableColumns.Name = "lblAvailableColumns";
            this.lblAvailableColumns.Size = new System.Drawing.Size(220, 23);
            this.lblAvailableColumns.TabIndex = 2;
            this.lblAvailableColumns.Text = "Columns available for display:";
            // 
            // lblSelectedColumns
            // 
            this.lblSelectedColumns.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedColumns.Location = new System.Drawing.Point(393, 83);
            this.lblSelectedColumns.Name = "lblSelectedColumns";
            this.lblSelectedColumns.Size = new System.Drawing.Size(220, 23);
            this.lblSelectedColumns.TabIndex = 3;
            this.lblSelectedColumns.Text = "Your selected columns:";
            // 
            // tooltipShowDragMessage
            // 
            this.tooltipShowDragMessage.ContainingControl = this;
            // 
            // listviewAvailableColumns
            // 
            this.listviewAvailableColumns.AllowDrop = true;
            this.listviewAvailableColumns.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance5.FontData.BoldAsString = "True";
            appearance5.FontData.ItalicAsString = "True";
            this.listviewAvailableColumns.GroupAppearance = appearance5;
            appearance6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.listviewAvailableColumns.ItemSettings.ActiveAppearance = appearance6;
            this.listviewAvailableColumns.ItemSettings.AllowEdit = Infragistics.Win.DefaultableBoolean.False;
            appearance7.Cursor = System.Windows.Forms.Cursors.Default;
            this.listviewAvailableColumns.ItemSettings.Appearance = appearance7;
            this.listviewAvailableColumns.Location = new System.Drawing.Point(33, 103);
            this.listviewAvailableColumns.Name = "listviewAvailableColumns";
            this.listviewAvailableColumns.Size = new System.Drawing.Size(263, 325);
            this.listviewAvailableColumns.TabIndex = 4;
            ultraToolTipInfo1.ToolTipText = "Drag Columns From Available Columns to Selected Columns";
            this.tooltipShowDragMessage.SetUltraToolTip(this.listviewAvailableColumns, ultraToolTipInfo1);
            this.listviewAvailableColumns.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.Details;
            this.listviewAvailableColumns.DragDrop += new System.Windows.Forms.DragEventHandler(this.WinListView_DragDrop);
            this.listviewAvailableColumns.DragOver += new System.Windows.Forms.DragEventHandler(this.WinListView_DragOver);
            this.listviewAvailableColumns.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WinListView_MouseDown);
            this.listviewAvailableColumns.MouseHover += new System.EventHandler(this.listAvailableColumns_MouseHover);
            this.listviewAvailableColumns.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WinListView_MouseMove);
            // 
            // listviewSelectedColumns
            // 
            this.listviewSelectedColumns.AllowDrop = true;
            appearance2.FontData.BoldAsString = "True";
            appearance2.FontData.ItalicAsString = "True";
            this.listviewSelectedColumns.GroupAppearance = appearance2;
            appearance3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.listviewSelectedColumns.ItemSettings.ActiveAppearance = appearance3;
            appearance4.Cursor = System.Windows.Forms.Cursors.Default;
            this.listviewSelectedColumns.ItemSettings.Appearance = appearance4;
            this.listviewSelectedColumns.Location = new System.Drawing.Point(372, 103);
            this.listviewSelectedColumns.Name = "listviewSelectedColumns";
            this.listviewSelectedColumns.Size = new System.Drawing.Size(263, 325);
            this.listviewSelectedColumns.TabIndex = 5;
            this.listviewSelectedColumns.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.Details;
            this.listviewSelectedColumns.ViewSettingsDetails.AllowColumnSorting = false;
            this.listviewSelectedColumns.DragDrop += new System.Windows.Forms.DragEventHandler(this.WinListView_DragDrop);
            this.listviewSelectedColumns.DragOver += new System.Windows.Forms.DragEventHandler(this.WinListView_DragOver);
            this.listviewSelectedColumns.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WinListView_MouseDown);
            this.listviewSelectedColumns.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WinListView_MouseMove);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkbxMatchedinTol);
            this.groupBox2.Controls.Add(this.chkbxExactlyMatched);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(42, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(254, 73);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.Text = "Exception Report Optional Items:";
            // 
            // chkbxMatchedinTol
            // 
            this.chkbxMatchedinTol.AutoSize = true;
            this.chkbxMatchedinTol.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkbxMatchedinTol.Location = new System.Drawing.Point(6, 39);
            this.chkbxMatchedinTol.Name = "chkbxMatchedinTol";
            this.chkbxMatchedinTol.Size = new System.Drawing.Size(156, 17);
            this.chkbxMatchedinTol.TabIndex = 17;
            this.chkbxMatchedinTol.Text = "Matched With in Tolerance";
            this.chkbxMatchedinTol.CheckedChanged += new System.EventHandler(this.chkbxMatchedinTol_CheckedChanged);
            // 
            // chkbxExactlyMatched
            // 
            this.chkbxExactlyMatched.AutoSize = true;
            this.chkbxExactlyMatched.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkbxExactlyMatched.Location = new System.Drawing.Point(6, 18);
            this.chkbxExactlyMatched.Name = "chkbxExactlyMatched";
            this.chkbxExactlyMatched.Size = new System.Drawing.Size(104, 17);
            this.chkbxExactlyMatched.TabIndex = 16;
            this.chkbxExactlyMatched.Text = "Exactly Matched";
            this.chkbxExactlyMatched.CheckedChanged += new System.EventHandler(this.chkbxExactlyMatched_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbPDFFormat);
            this.groupBox1.Controls.Add(this.rbXLSFormat);
            this.groupBox1.Controls.Add(this.rbCSVFormat);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(393, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(173, 73);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.Text = "Exception Report Format";
            // 
            // rbPDFFormat
            // 
            this.rbPDFFormat.AutoSize = true;
            this.rbPDFFormat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbPDFFormat.Location = new System.Drawing.Point(95, 29);
            this.rbPDFFormat.Name = "rbPDFFormat";
            this.rbPDFFormat.Size = new System.Drawing.Size(46, 17);
            this.inboxControlStyler1.SetStyleSettings(this.rbPDFFormat, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.rbPDFFormat.TabIndex = 29;
            this.rbPDFFormat.TabStop = true;
            this.rbPDFFormat.Text = "PDF";
            this.rbPDFFormat.CheckedChanged += new System.EventHandler(this.rbPDFFormat_CheckedChanged);
            // 
            // rbXLSFormat
            // 
            this.rbXLSFormat.AutoSize = true;
            this.rbXLSFormat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbXLSFormat.Location = new System.Drawing.Point(22, 42);
            this.rbXLSFormat.Name = "rbXLSFormat";
            this.rbXLSFormat.Size = new System.Drawing.Size(45, 17);
            this.inboxControlStyler1.SetStyleSettings(this.rbXLSFormat, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.rbXLSFormat.TabIndex = 19;
            this.rbXLSFormat.Text = "XLS";
            this.rbXLSFormat.CheckedChanged += new System.EventHandler(this.rbXLSFormat_CheckedChanged);
            // 
            // rbCSVFormat
            // 
            this.rbCSVFormat.AutoSize = true;
            this.rbCSVFormat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbCSVFormat.Location = new System.Drawing.Point(22, 19);
            this.rbCSVFormat.Name = "rbCSVFormat";
            this.rbCSVFormat.Size = new System.Drawing.Size(46, 17);
            this.inboxControlStyler1.SetStyleSettings(this.rbCSVFormat, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.rbCSVFormat.TabIndex = 18;
            this.rbCSVFormat.Text = "CSV";
            this.rbCSVFormat.CheckedChanged += new System.EventHandler(this.rbCSVFormat_CheckedChanged);
            // 
            // btnSortAscending
            // 
            this.btnSortAscending.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSortAscending.Location = new System.Drawing.Point(647, 158);
            this.btnSortAscending.Name = "btnSortAscending";
            this.btnSortAscending.Size = new System.Drawing.Size(115, 23);
            this.btnSortAscending.TabIndex = 18;
            this.btnSortAscending.Text = "Sort Ascending";
            this.btnSortAscending.Click += new System.EventHandler(this.btnSortAscending_Click);
            // 
            // btnSortDescending
            // 
            this.btnSortDescending.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSortDescending.Location = new System.Drawing.Point(647, 198);
            this.btnSortDescending.Name = "btnSortDescending";
            this.btnSortDescending.Size = new System.Drawing.Size(115, 23);
            this.btnSortDescending.TabIndex = 19;
            this.btnSortDescending.Text = "Sort Descending";
            this.btnSortDescending.Click += new System.EventHandler(this.btnSortDescending_Click);
            // 
            // listviewColumnForSorting
            // 
            this.listviewColumnForSorting.AllowDrop = true;
            this.listviewColumnForSorting.Location = new System.Drawing.Point(372, 455);
            this.listviewColumnForSorting.Name = "listviewColumnForSorting";
            this.listviewColumnForSorting.Size = new System.Drawing.Size(263, 100);
            this.listviewColumnForSorting.TabIndex = 20;
            this.listviewColumnForSorting.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.Details;
            this.listviewColumnForSorting.ViewSettingsDetails.AllowColumnSorting = false;
            appearance1.BackColor2 = System.Drawing.Color.White;
            this.listviewColumnForSorting.ViewSettingsDetails.ColumnHeaderAppearance = appearance1;
            this.listviewColumnForSorting.DragDrop += new System.Windows.Forms.DragEventHandler(this.listSelectedColumns_DragDrop);
            this.listviewColumnForSorting.DragOver += new System.Windows.Forms.DragEventHandler(this.listSelectedColumns_DragOver);
            this.listviewColumnForSorting.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listSelectedColumns_MouseDown);
            this.listviewColumnForSorting.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listSelectedColumns_MouseMove);
            // 
            // lblColumntoSort
            // 
            this.lblColumntoSort.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblColumntoSort.Location = new System.Drawing.Point(372, 429);
            this.lblColumntoSort.Name = "lblColumntoSort";
            this.lblColumntoSort.Size = new System.Drawing.Size(220, 23);
            this.lblColumntoSort.TabIndex = 21;
            this.lblColumntoSort.Text = "Columns to be used for sorting:";
            // 
            // btnFlipSortOrder
            // 
            this.btnFlipSortOrder.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFlipSortOrder.Location = new System.Drawing.Point(647, 456);
            this.btnFlipSortOrder.Name = "btnFlipSortOrder";
            this.btnFlipSortOrder.Size = new System.Drawing.Size(115, 23);
            this.btnFlipSortOrder.TabIndex = 22;
            this.btnFlipSortOrder.Text = "Flip Asc/Desc";
            this.btnFlipSortOrder.Click += new System.EventHandler(this.btnFlipSortOrder_Click);
            // 
            // btnDeleteSortOrder
            // 
            this.btnDeleteSortOrder.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteSortOrder.Location = new System.Drawing.Point(647, 499);
            this.btnDeleteSortOrder.Name = "btnDeleteSortOrder";
            this.btnDeleteSortOrder.Size = new System.Drawing.Size(115, 23);
            this.btnDeleteSortOrder.TabIndex = 23;
            this.btnDeleteSortOrder.Text = "Delete";
            this.btnDeleteSortOrder.Click += new System.EventHandler(this.btnDeleteSortOrder_Click);
            // 
            // listviewGroupByColumns
            // 
            this.listviewGroupByColumns.AllowDrop = true;
            this.listviewGroupByColumns.Location = new System.Drawing.Point(33, 456);
            this.listviewGroupByColumns.Name = "listviewGroupByColumns";
            this.listviewGroupByColumns.Size = new System.Drawing.Size(263, 100);
            this.listviewGroupByColumns.TabIndex = 24;
            this.listviewGroupByColumns.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.Details;
            this.listviewGroupByColumns.ViewSettingsDetails.AllowColumnSorting = false;
            this.listviewGroupByColumns.DragDrop += new System.Windows.Forms.DragEventHandler(this.listSelectedColumns_DragDrop);
            this.listviewGroupByColumns.DragOver += new System.Windows.Forms.DragEventHandler(this.listSelectedColumns_DragOver);
            this.listviewGroupByColumns.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listSelectedColumns_MouseDown);
            this.listviewGroupByColumns.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listSelectedColumns_MouseMove);
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(36, 429);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(220, 23);
            this.ultraLabel1.TabIndex = 25;
            this.ultraLabel1.Text = "Columns to be used for Grouping:";
            // 
            // btnGroupBy
            // 
            this.btnGroupBy.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGroupBy.Location = new System.Drawing.Point(647, 238);
            this.btnGroupBy.Name = "btnGroupBy";
            this.btnGroupBy.Size = new System.Drawing.Size(115, 23);
            this.btnGroupBy.TabIndex = 26;
            this.btnGroupBy.Text = "Group By";
            this.btnGroupBy.Click += new System.EventHandler(this.btnGroupBy_Click);
            // 
            // btnDeleteGroupBy
            // 
            this.btnDeleteGroupBy.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteGroupBy.Location = new System.Drawing.Point(302, 499);
            this.btnDeleteGroupBy.Name = "btnDeleteGroupBy";
            this.btnDeleteGroupBy.Size = new System.Drawing.Size(64, 23);
            this.btnDeleteGroupBy.TabIndex = 27;
            this.btnDeleteGroupBy.Text = "Delete";
            this.btnDeleteGroupBy.Click += new System.EventHandler(this.btnDeleteGroupBy_Click);
            // 
            // ultraPanel1
            // 
            this.ultraPanel1.AutoScroll = true;
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.btnDeleteGroupBy);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnGroupBy);
            this.ultraPanel1.ClientArea.Controls.Add(this.listviewGroupByColumns);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraLabel1);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnDeleteSortOrder);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnFlipSortOrder);
            this.ultraPanel1.ClientArea.Controls.Add(this.listviewColumnForSorting);
            this.ultraPanel1.ClientArea.Controls.Add(this.lblColumntoSort);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnSortDescending);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnSortAscending);
            this.ultraPanel1.ClientArea.Controls.Add(this.groupBox1);
            this.ultraPanel1.ClientArea.Controls.Add(this.groupBox2);
            this.ultraPanel1.ClientArea.Controls.Add(this.listviewSelectedColumns);
            this.ultraPanel1.ClientArea.Controls.Add(this.listviewAvailableColumns);
            this.ultraPanel1.ClientArea.Controls.Add(this.lblSelectedColumns);
            this.ultraPanel1.ClientArea.Controls.Add(this.lblAvailableColumns);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(905, 559);
            this.ultraPanel1.TabIndex = 28;
            // 
            // ctrlReconReportLayout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.ultraPanel1);
            this.Name = "ctrlReconReportLayout";
            this.Size = new System.Drawing.Size(905, 559);
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            ((System.ComponentModel.ISupportInitialize)(this.listviewAvailableColumns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listviewSelectedColumns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox2)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxMatchedinTol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbxExactlyMatched)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listviewColumnForSorting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listviewGroupByColumns)).EndInit();
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel lblAvailableColumns;
        private Infragistics.Win.Misc.UltraLabel lblSelectedColumns;
        private Infragistics.Win.UltraWinToolTip.UltraToolTipManager tooltipShowDragMessage;
        private Infragistics.Win.UltraWinListView.UltraListView listviewAvailableColumns;
        private Infragistics.Win.UltraWinListView.UltraListView listviewSelectedColumns;
        private Infragistics.Win.Misc.UltraGroupBox groupBox2;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxMatchedinTol;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxExactlyMatched;
        public Infragistics.Win.Misc.UltraGroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbXLSFormat;
        private System.Windows.Forms.RadioButton rbCSVFormat;
        private Infragistics.Win.Misc.UltraButton btnSortAscending;
        private Infragistics.Win.Misc.UltraButton btnSortDescending;
        private Infragistics.Win.UltraWinListView.UltraListView listviewColumnForSorting;
        private Infragistics.Win.Misc.UltraLabel lblColumntoSort;
        private Infragistics.Win.Misc.UltraButton btnFlipSortOrder;
        private Infragistics.Win.Misc.UltraButton btnDeleteSortOrder;
        private Infragistics.Win.UltraWinListView.UltraListView listviewGroupByColumns;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraButton btnGroupBy;
        private Infragistics.Win.Misc.UltraButton btnDeleteGroupBy;
        private System.Windows.Forms.RadioButton rbPDFFormat;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        
    }
}
