namespace Prana.Tools
{
    partial class ctrlSMBatchStatusReport
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeViewSMBatch = new Infragistics.Win.UltraWinTree.UltraTree();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.btnGetData = new Infragistics.Win.Misc.UltraButton();
            this.udtSMBatchDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.lblStartDate = new Infragistics.Win.Misc.UltraLabel();
            this.grdSMBatch = new Infragistics.Win.UltraWinGrid.UltraGrid();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeViewSMBatch)).BeginInit();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udtSMBatchDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSMBatch)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeViewSMBatch);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ultraPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(658, 408);
            this.splitContainer1.SplitterDistance = 100;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeViewSMBatch
            // 
            this.treeViewSMBatch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewSMBatch.Location = new System.Drawing.Point(0, 0);
            this.treeViewSMBatch.Name = "treeViewSMBatch";
            this.treeViewSMBatch.Size = new System.Drawing.Size(100, 408);
            this.treeViewSMBatch.TabIndex = 0;
            this.treeViewSMBatch.AfterSelect += new Infragistics.Win.UltraWinTree.AfterNodeSelectEventHandler(this.treeViewSMBatch_AfterSelect);
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.btnGetData);
            this.ultraPanel1.ClientArea.Controls.Add(this.udtSMBatchDate);
            this.ultraPanel1.ClientArea.Controls.Add(this.lblStartDate);
            this.ultraPanel1.ClientArea.Controls.Add(this.grdSMBatch);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(554, 408);
            this.ultraPanel1.TabIndex = 0;
            // 
            // btnGetData
            // 
            this.btnGetData.Location = new System.Drawing.Point(223, 22);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(90, 23);
            this.btnGetData.TabIndex = 16;
            this.btnGetData.Text = "Get Data";
            this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
            // 
            // udtSMBatchDate
            // 
            this.udtSMBatchDate.Location = new System.Drawing.Point(50, 23);
            this.udtSMBatchDate.Name = "udtSMBatchDate";
            this.udtSMBatchDate.Size = new System.Drawing.Size(150, 21);
            this.udtSMBatchDate.TabIndex = 15;
            // 
            // lblStartDate
            // 
            this.lblStartDate.Location = new System.Drawing.Point(6, 25);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(38, 23);
            this.lblStartDate.TabIndex = 14;
            this.lblStartDate.Text = "Date";
            // 
            // grdSMBatch
            // 
            this.grdSMBatch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdSMBatch.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance1.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance1.BorderColor = System.Drawing.SystemColors.Window;
            this.grdSMBatch.DisplayLayout.GroupByBox.Appearance = appearance1;
            appearance2.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdSMBatch.DisplayLayout.GroupByBox.BandLabelAppearance = appearance2;
            this.grdSMBatch.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance3.BackColor2 = System.Drawing.SystemColors.Control;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdSMBatch.DisplayLayout.GroupByBox.PromptAppearance = appearance3;
            appearance4.BackColor = System.Drawing.SystemColors.Window;
            appearance4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdSMBatch.DisplayLayout.Override.ActiveCellAppearance = appearance4;
            appearance5.BackColor = System.Drawing.SystemColors.Highlight;
            appearance5.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdSMBatch.DisplayLayout.Override.ActiveRowAppearance = appearance5;
            this.grdSMBatch.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdSMBatch.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance6.BackColor = System.Drawing.SystemColors.Window;
            this.grdSMBatch.DisplayLayout.Override.CardAreaAppearance = appearance6;
            appearance7.BorderColor = System.Drawing.Color.Silver;
            appearance7.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdSMBatch.DisplayLayout.Override.CellAppearance = appearance7;
            this.grdSMBatch.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdSMBatch.DisplayLayout.Override.CellPadding = 0;
            appearance8.BackColor = System.Drawing.SystemColors.Control;
            appearance8.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance8.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance8.BorderColor = System.Drawing.SystemColors.Window;
            this.grdSMBatch.DisplayLayout.Override.GroupByRowAppearance = appearance8;
            appearance9.TextHAlignAsString = "Left";
            this.grdSMBatch.DisplayLayout.Override.HeaderAppearance = appearance9;
            this.grdSMBatch.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdSMBatch.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance10.BackColor = System.Drawing.SystemColors.Window;
            appearance10.BorderColor = System.Drawing.Color.Silver;
            this.grdSMBatch.DisplayLayout.Override.RowAppearance = appearance10;
            this.grdSMBatch.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            appearance11.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdSMBatch.DisplayLayout.Override.TemplateAddRowAppearance = appearance11;
            this.grdSMBatch.Location = new System.Drawing.Point(0, 61);
            this.grdSMBatch.Name = "grdSMBatch";
            this.grdSMBatch.Size = new System.Drawing.Size(554, 347);
            this.grdSMBatch.TabIndex = 1;
            this.grdSMBatch.Text = "ultraGrid1";
            this.grdSMBatch.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdSMBatch_InitializeLayout);
            // 
            // ctrlSMBatchStatusReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ctrlSMBatchStatusReport";
            this.Size = new System.Drawing.Size(658, 408);
            this.Load += new System.EventHandler(this.ctrlSMBatchStatusReport_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeViewSMBatch)).EndInit();
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ClientArea.PerformLayout();
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.udtSMBatchDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSMBatch)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdSMBatch;
        private Infragistics.Win.Misc.UltraButton btnGetData;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor udtSMBatchDate;
        private Infragistics.Win.Misc.UltraLabel lblStartDate;
        private Infragistics.Win.UltraWinTree.UltraTree treeViewSMBatch;

    }
}
