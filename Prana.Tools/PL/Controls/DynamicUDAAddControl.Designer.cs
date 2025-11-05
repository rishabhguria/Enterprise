namespace Prana.Tools.PL.Controls
{
    partial class DynamicUDAAddControl
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (_dt != null)
                {
                    _dt.Dispose();
                }
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraComEdDefaultValue = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.btnDynamicUDACancel = new Infragistics.Win.Misc.UltraButton();
            this.btnDynamicUDASave = new Infragistics.Win.Misc.UltraButton();
            this.ultraListViewDynamicUDA = new Infragistics.Win.UltraWinListView.UltraListView();
            this.cntxtMnuDynamicUDA = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addMasterValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteMasterValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameMasterValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMarksAsDefault = new System.Windows.Forms.ToolStripMenuItem();
            this.ultraTextEditorDefaultValue = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraTextEditorHeaderCaption = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabelDynamicUDA = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabelMasterValues = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabelDefaultValue = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabelHeaderCaption = new Infragistics.Win.Misc.UltraLabel();
            this.statusProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraComEdDefaultValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraListViewDynamicUDA)).BeginInit();
            this.cntxtMnuDynamicUDA.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditorDefaultValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditorHeaderCaption)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraComEdDefaultValue);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnDynamicUDACancel);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnDynamicUDASave);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraListViewDynamicUDA);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraTextEditorDefaultValue);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraTextEditorHeaderCaption);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraLabelDynamicUDA);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraLabelMasterValues);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraLabelDefaultValue);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraLabelHeaderCaption);
            this.ultraPanel1.Location = new System.Drawing.Point(3, 3);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(306, 337);
            this.ultraPanel1.TabIndex = 0;
            // 
            // ultraComEdDefaultValue
            // 
            this.ultraComEdDefaultValue.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.ultraComEdDefaultValue.AutoSuggestFilterMode = Infragistics.Win.AutoSuggestFilterMode.StartsWith;
            this.ultraComEdDefaultValue.DropDownListWidth = -1;
            this.ultraComEdDefaultValue.Location = new System.Drawing.Point(166, 110);
            this.ultraComEdDefaultValue.Name = "ultraComEdDefaultValue";
            this.ultraComEdDefaultValue.Size = new System.Drawing.Size(120, 21);
            this.ultraComEdDefaultValue.SortStyle = Infragistics.Win.ValueListSortStyle.Ascending;
            this.ultraComEdDefaultValue.TabIndex = 9;
            this.ultraComEdDefaultValue.Visible = false;
            // 
            // btnDynamicUDACancel
            // 
            this.btnDynamicUDACancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDynamicUDACancel.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(5)))), ((int)(((byte)(5)))));
            this.btnDynamicUDACancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
            this.btnDynamicUDACancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDynamicUDACancel.ForeColor = System.Drawing.Color.White;
            this.btnDynamicUDACancel.Location = new System.Drawing.Point(164, 299);
            this.btnDynamicUDACancel.Name = "btnDynamicUDACancel";
            this.btnDynamicUDACancel.Size = new System.Drawing.Size(75, 23);
            this.btnDynamicUDACancel.TabIndex = 8;
            this.btnDynamicUDACancel.Text = "Cancel";
            this.btnDynamicUDACancel.UseAppStyling = false;
            this.btnDynamicUDACancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnDynamicUDACancel.Click += new System.EventHandler(this.btnDynamicUDACancel_Click);
            // 
            // btnDynamicUDASave
            // 
            this.btnDynamicUDASave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDynamicUDASave.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(156)))), ((int)(((byte)(46)))));
            this.btnDynamicUDASave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
            this.btnDynamicUDASave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDynamicUDASave.ForeColor = System.Drawing.Color.White;
            this.btnDynamicUDASave.Location = new System.Drawing.Point(62, 300);
            this.btnDynamicUDASave.Name = "btnDynamicUDASave";
            this.btnDynamicUDASave.Size = new System.Drawing.Size(75, 23);
            this.btnDynamicUDASave.TabIndex = 7;
            this.btnDynamicUDASave.Text = "Save";
            this.btnDynamicUDASave.UseAppStyling = false;
            this.btnDynamicUDASave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnDynamicUDASave.Click += new System.EventHandler(this.btnDynamicUDASave_Click);
            // 
            // ultraListViewDynamicUDA
            // 
            this.ultraListViewDynamicUDA.ContextMenuStrip = this.cntxtMnuDynamicUDA;
            this.ultraListViewDynamicUDA.ItemSettings.AllowEdit = Infragistics.Win.DefaultableBoolean.False;
            this.ultraListViewDynamicUDA.ItemSettings.HideSelection = false;
            this.ultraListViewDynamicUDA.ItemSettings.SelectionType = Infragistics.Win.UltraWinListView.SelectionType.Single;
            this.ultraListViewDynamicUDA.Location = new System.Drawing.Point(165, 152);
            this.ultraListViewDynamicUDA.MainColumn.Sorting = Infragistics.Win.UltraWinListView.Sorting.Ascending;
            this.ultraListViewDynamicUDA.Name = "ultraListViewDynamicUDA";
            this.ultraListViewDynamicUDA.Size = new System.Drawing.Size(121, 135);
            this.ultraListViewDynamicUDA.TabIndex = 6;
            this.ultraListViewDynamicUDA.Text = "ultraListView1";
            this.ultraListViewDynamicUDA.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.List;
            this.ultraListViewDynamicUDA.ViewSettingsList.MultiColumn = false;
            this.ultraListViewDynamicUDA.ViewSettingsTiles.Alignment = Infragistics.Win.UltraWinListView.ItemAlignment.TopToBottom;
            this.ultraListViewDynamicUDA.ViewSettingsTiles.ImageSize = new System.Drawing.Size(0, 0);
            this.ultraListViewDynamicUDA.ItemExitingEditMode += new Infragistics.Win.UltraWinListView.ItemExitingEditModeEventHandler(this.ultraListViewDynamicUDA_ItemExitingEditMode);
            this.ultraListViewDynamicUDA.ItemExitedEditMode += new Infragistics.Win.UltraWinListView.ItemExitedEditModeEventHandler(this.ultraListViewDynamicUDA_ItemExitedEditMode);
            this.ultraListViewDynamicUDA.Layout += new System.Windows.Forms.LayoutEventHandler(this.ultraListViewDynamicUDA_Layout);
            this.ultraListViewDynamicUDA.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ultraListViewDynamicUDA_MouseUp);
            // 
            // cntxtMnuDynamicUDA
            // 
            this.cntxtMnuDynamicUDA.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addMasterValueToolStripMenuItem,
            this.deleteMasterValueToolStripMenuItem,
            this.renameMasterValueToolStripMenuItem,
            this.toolStripMenuItemMarksAsDefault});
            this.cntxtMnuDynamicUDA.Name = "cntxtMnuPreference";
            this.cntxtMnuDynamicUDA.Size = new System.Drawing.Size(187, 92);
            this.cntxtMnuDynamicUDA.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cntxtMnuDynamicUDA_ItemClicked);
            // 
            // addMasterValueToolStripMenuItem
            // 
            this.addMasterValueToolStripMenuItem.Name = "addMasterValueToolStripMenuItem";
            this.addMasterValueToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.addMasterValueToolStripMenuItem.Tag = "AddMasterValue";
            this.addMasterValueToolStripMenuItem.Text = "Add";
            // 
            // deleteMasterValueToolStripMenuItem
            // 
            this.deleteMasterValueToolStripMenuItem.Name = "deleteMasterValueToolStripMenuItem";
            this.deleteMasterValueToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.deleteMasterValueToolStripMenuItem.Tag = "DeleteMasterValue";
            this.deleteMasterValueToolStripMenuItem.Text = "Delete";
            this.deleteMasterValueToolStripMenuItem.Visible = false;
            // 
            // renameMasterValueToolStripMenuItem
            // 
            this.renameMasterValueToolStripMenuItem.Name = "renameMasterValueToolStripMenuItem";
            this.renameMasterValueToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.renameMasterValueToolStripMenuItem.Tag = "RenameMasterValue";
            this.renameMasterValueToolStripMenuItem.Text = "Rename";
            this.renameMasterValueToolStripMenuItem.Visible = false;
            // 
            // toolStripMenuItemMarksAsDefault
            // 
            this.toolStripMenuItemMarksAsDefault.Name = "toolStripMenuItemMarksAsDefault";
            this.toolStripMenuItemMarksAsDefault.Size = new System.Drawing.Size(186, 22);
            this.toolStripMenuItemMarksAsDefault.Tag = "MarksAsDefault";
            this.toolStripMenuItemMarksAsDefault.Text = "Mark as default value";
            // 
            // ultraTextEditorDefaultValue
            // 
            this.ultraTextEditorDefaultValue.Location = new System.Drawing.Point(165, 109);
            this.ultraTextEditorDefaultValue.Name = "ultraTextEditorDefaultValue";
            this.ultraTextEditorDefaultValue.Size = new System.Drawing.Size(121, 21);
            this.ultraTextEditorDefaultValue.TabIndex = 5;
            // 
            // ultraTextEditorHeaderCaption
            // 
            this.ultraTextEditorHeaderCaption.Location = new System.Drawing.Point(165, 59);
            this.ultraTextEditorHeaderCaption.Name = "ultraTextEditorHeaderCaption";
            this.ultraTextEditorHeaderCaption.Size = new System.Drawing.Size(121, 21);
            this.ultraTextEditorHeaderCaption.TabIndex = 4;
            // 
            // ultraLabelDynamicUDA
            // 
            appearance1.TextHAlignAsString = "Center";
            appearance1.TextVAlignAsString = "Middle";
            this.ultraLabelDynamicUDA.Appearance = appearance1;
            this.ultraLabelDynamicUDA.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabelDynamicUDA.Location = new System.Drawing.Point(105, 12);
            this.ultraLabelDynamicUDA.Name = "ultraLabelDynamicUDA";
            this.ultraLabelDynamicUDA.Size = new System.Drawing.Size(200, 23);
            this.ultraLabelDynamicUDA.TabIndex = 3;
            this.ultraLabelDynamicUDA.Text = "Dynamic UDA";
            // 
            // ultraLabelMasterValues
            // 
            this.ultraLabelMasterValues.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabelMasterValues.Location = new System.Drawing.Point(19, 157);
            this.ultraLabelMasterValues.Name = "ultraLabelMasterValues";
            this.ultraLabelMasterValues.Size = new System.Drawing.Size(100, 23);
            this.ultraLabelMasterValues.TabIndex = 2;
            this.ultraLabelMasterValues.Text = "Values:";
            // 
            // ultraLabelDefaultValue
            // 
            this.ultraLabelDefaultValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabelDefaultValue.Location = new System.Drawing.Point(19, 109);
            this.ultraLabelDefaultValue.Name = "ultraLabelDefaultValue";
            this.ultraLabelDefaultValue.Size = new System.Drawing.Size(100, 23);
            this.ultraLabelDefaultValue.TabIndex = 1;
            this.ultraLabelDefaultValue.Text = "Default Value:";
            // 
            // ultraLabelHeaderCaption
            // 
            this.ultraLabelHeaderCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabelHeaderCaption.Location = new System.Drawing.Point(19, 57);
            this.ultraLabelHeaderCaption.Name = "ultraLabelHeaderCaption";
            this.ultraLabelHeaderCaption.Size = new System.Drawing.Size(100, 23);
            this.ultraLabelHeaderCaption.TabIndex = 0;
            this.ultraLabelHeaderCaption.Text = "Header Caption:";
            // 
            // statusProvider
            // 
            this.statusProvider.ContainerControl = this;
            // 
            // DynamicUDAAddControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraPanel1);
            this.Name = "DynamicUDAAddControl";
            this.Size = new System.Drawing.Size(311, 341);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ClientArea.PerformLayout();
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraComEdDefaultValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraListViewDynamicUDA)).EndInit();
            this.cntxtMnuDynamicUDA.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditorDefaultValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditorHeaderCaption)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor ultraTextEditorDefaultValue;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor ultraTextEditorHeaderCaption;
        private Infragistics.Win.Misc.UltraLabel ultraLabelDynamicUDA;
        private Infragistics.Win.Misc.UltraLabel ultraLabelMasterValues;
        private Infragistics.Win.Misc.UltraLabel ultraLabelDefaultValue;
        private Infragistics.Win.Misc.UltraLabel ultraLabelHeaderCaption;
        private Infragistics.Win.UltraWinListView.UltraListView ultraListViewDynamicUDA;
        private System.Windows.Forms.ContextMenuStrip cntxtMnuDynamicUDA;
        private System.Windows.Forms.ToolStripMenuItem addMasterValueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteMasterValueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameMasterValueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMarksAsDefault;
        private Infragistics.Win.Misc.UltraButton btnDynamicUDASave;
        private Infragistics.Win.Misc.UltraButton btnDynamicUDACancel;
        private System.Windows.Forms.ErrorProvider statusProvider;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor ultraComEdDefaultValue;
    }
}
