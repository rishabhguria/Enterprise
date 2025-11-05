namespace Prana.ComplianceEngine.RuleDefinition.UI.UserControls
{
    partial class ImportRuleSelection
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ulstView = new System.Windows.Forms.ListView();
            this.Key = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RuleName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.OriginalRuleName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Category = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Package = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ultraButton2 = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton1 = new Infragistics.Win.Misc.UltraButton();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            //this.ctrlImageListButtons1 = new Prana.Utilities.UI.UIUtilities.CtrlImageListButtons(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(4, 27);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ulstView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ultraButton2);
            this.splitContainer1.Panel2.Controls.Add(this.ultraButton1);
            this.splitContainer1.Size = new System.Drawing.Size(487, 321);
            this.splitContainer1.SplitterDistance = 289;
            this.inboxControlStyler1.SetStyleSettings(this.splitContainer1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.splitContainer1.TabIndex = 0;
            // 
            // ulstView
            // 
            this.ulstView.CheckBoxes = true;
            this.ulstView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Key,
            this.RuleName,
            this.OriginalRuleName,
            this.Category,
            this.Package});
            this.ulstView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ulstView.ForeColor = System.Drawing.SystemColors.Window;
            this.ulstView.FullRowSelect = true;
            this.ulstView.Location = new System.Drawing.Point(0, 0);
            this.ulstView.Name = "ulstView";
            this.ulstView.Size = new System.Drawing.Size(487, 289);
            this.inboxControlStyler1.SetStyleSettings(this.ulstView, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.False));
            this.ulstView.TabIndex = 0;
            this.ulstView.UseCompatibleStateImageBehavior = false;
            this.ulstView.View = System.Windows.Forms.View.Details;
            // 
            // Key
            // 
            this.Key.Tag = "Key";
            this.Key.Text = "";
            this.Key.Width = 20;
            // 
            // RuleName
            // 
            this.RuleName.Tag = "RuleName";
            this.RuleName.Text = "RuleName";
            this.RuleName.Width = 164;
            // 
            // OriginalRuleName
            // 
            this.OriginalRuleName.Tag = "OriginalRuleName";
            this.OriginalRuleName.Text = "OriginalRuleName";
            this.OriginalRuleName.Width = 0;
            // 
            // Category
            // 
            this.Category.Tag = "Category";
            this.Category.Text = "Category";
            this.Category.Width = 162;
            // 
            // Package
            // 
            this.Package.Tag = "Package";
            this.Package.Text = "Package";
            this.Package.Width = 149;
            // 
            // ultraButton2
            // 
            this.ultraButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraButton2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ultraButton2.Location = new System.Drawing.Point(328, 2);
            this.ultraButton2.Name = "ultraButton2";
            this.ultraButton2.Size = new System.Drawing.Size(75, 23);
            this.ultraButton2.TabIndex = 1;
            this.ultraButton2.Text = "OK";
            this.ultraButton2.Click += new System.EventHandler(this.ultraButton2_Click);
            // 
            // ultraButton1
            // 
            this.ultraButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ultraButton1.Location = new System.Drawing.Point(409, 3);
            this.ultraButton1.Name = "ultraButton1";
            this.ultraButton1.Size = new System.Drawing.Size(75, 23);
            this.ultraButton1.TabIndex = 0;
            this.ultraButton1.Text = "Cancel";
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _ImportRuleSelection_UltraFormManager_Dock_Area_Left
            // 
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Left.Name = "_ImportRuleSelection_UltraFormManager_Dock_Area_Left";
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 321);
            // 
            // _ImportRuleSelection_UltraFormManager_Dock_Area_Right
            // 
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(491, 27);
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Right.Name = "_ImportRuleSelection_UltraFormManager_Dock_Area_Right";
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 321);
            // 
            // _ImportRuleSelection_UltraFormManager_Dock_Area_Top
            // 
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Top.Name = "_ImportRuleSelection_UltraFormManager_Dock_Area_Top";
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(495, 27);
            // 
            // _ImportRuleSelection_UltraFormManager_Dock_Area_Bottom
            // 
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 348);
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Bottom.Name = "_ImportRuleSelection_UltraFormManager_Dock_Area_Bottom";
            this._ImportRuleSelection_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(495, 4);
            // 
            // ImportRuleSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(495, 352);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this._ImportRuleSelection_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ImportRuleSelection_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ImportRuleSelection_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ImportRuleSelection_UltraFormManager_Dock_Area_Bottom);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(511, 390);
            this.MinimizeBox = false;
            this.Name = "ImportRuleSelection";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Nirvana Compliance - Select rules to import";
            this.Load += new System.EventHandler(this.ImportRuleSelection_Load);
            this.Shown += new System.EventHandler(this.ImportRuleSelection_Shown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Infragistics.Win.Misc.UltraButton ultraButton2;
        private Infragistics.Win.Misc.UltraButton ultraButton1;
        private System.Windows.Forms.ListView ulstView;
        private System.Windows.Forms.ColumnHeader RuleName;
        private System.Windows.Forms.ColumnHeader Category;
        private System.Windows.Forms.ColumnHeader Package;
        private System.Windows.Forms.ColumnHeader OriginalRuleName;
        private System.Windows.Forms.ColumnHeader Key;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ImportRuleSelection_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ImportRuleSelection_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ImportRuleSelection_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ImportRuleSelection_UltraFormManager_Dock_Area_Bottom;
        //private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;

    }
}