namespace Prana.Tools
{
    partial class ZeroPositionAlertForm
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            this.btnRefresh = new Infragistics.Win.Misc.UltraButton();
            this.grdZeroPosAlert = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.grdZeroPosAlert)).BeginInit();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Location = new System.Drawing.Point(722, 9);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 0;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // grdZeroPosAlert
            // 
            appearance1.BackColor = System.Drawing.Color.Black;
            this.grdZeroPosAlert.DisplayLayout.Appearance = appearance1;
            this.grdZeroPosAlert.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdZeroPosAlert.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdZeroPosAlert.DisplayLayout.GroupByBox.Hidden = true;
            this.grdZeroPosAlert.DisplayLayout.MaxColScrollRegions = 1;
            this.grdZeroPosAlert.DisplayLayout.MaxRowScrollRegions = 1;
            appearance2.BackColor = System.Drawing.Color.LightSlateGray;
            appearance2.BackColor2 = System.Drawing.Color.DarkSlateGray;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance2.BorderColor = System.Drawing.Color.DimGray;
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            this.grdZeroPosAlert.DisplayLayout.Override.ActiveRowAppearance = appearance2;
            this.grdZeroPosAlert.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdZeroPosAlert.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdZeroPosAlert.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdZeroPosAlert.DisplayLayout.Override.CellPadding = 0;
            this.grdZeroPosAlert.DisplayLayout.Override.CellSpacing = 0;
            appearance3.FontData.Name = "Tahoma";
            appearance3.FontData.SizeInPoints = 8F;
            appearance3.TextHAlignAsString = "Center";
            this.grdZeroPosAlert.DisplayLayout.Override.HeaderAppearance = appearance3;
            this.grdZeroPosAlert.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdZeroPosAlert.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance4.ForeColor = System.Drawing.Color.White;
            appearance4.TextHAlignAsString = "Right";
            appearance4.TextVAlignAsString = "Middle";
            this.grdZeroPosAlert.DisplayLayout.Override.RowAlternateAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.Black;
            appearance5.ForeColor = System.Drawing.Color.DarkOrange;
            appearance5.TextHAlignAsString = "Right";
            appearance5.TextVAlignAsString = "Middle";
            this.grdZeroPosAlert.DisplayLayout.Override.RowAppearance = appearance5;
            appearance6.BackColor = System.Drawing.Color.Transparent;
            appearance6.BorderColor = System.Drawing.Color.Transparent;
            appearance6.FontData.BoldAsString = "True";
            this.grdZeroPosAlert.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.grdZeroPosAlert.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdZeroPosAlert.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdZeroPosAlert.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdZeroPosAlert.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdZeroPosAlert.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.grdZeroPosAlert.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            appearance7.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdZeroPosAlert.DisplayLayout.Override.TemplateAddRowAppearance = appearance7;
            this.grdZeroPosAlert.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdZeroPosAlert.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdZeroPosAlert.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdZeroPosAlert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdZeroPosAlert.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.grdZeroPosAlert.Location = new System.Drawing.Point(8, 72);
            this.grdZeroPosAlert.Name = "grdZeroPosAlert";
            this.grdZeroPosAlert.Size = new System.Drawing.Size(800, 407);
            this.grdZeroPosAlert.TabIndex = 10;
            this.grdZeroPosAlert.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdZeroPosAlert.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdZeroPosAlert_InitializeLayout);
            this.grdZeroPosAlert.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdZeroPosAlert_BeforeCustomRowFilterDialog);
            this.grdZeroPosAlert.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdZeroPosAlert_BeforeColumnChooserDisplayed);
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.btnRefresh);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanel1.Location = new System.Drawing.Point(8, 31);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(800, 41);
            this.ultraPanel1.TabIndex = 11;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _ZeroPositionAlertForm_UltraFormManager_Dock_Area_Left
            // 
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Left.Name = "_ZeroPositionAlertForm_UltraFormManager_Dock_Area_Left";
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 448);
            // 
            // _ZeroPositionAlertForm_UltraFormManager_Dock_Area_Right
            // 
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(808, 31);
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Right.Name = "_ZeroPositionAlertForm_UltraFormManager_Dock_Area_Right";
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 448);
            // 
            // _ZeroPositionAlertForm_UltraFormManager_Dock_Area_Top
            // 
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Top.Name = "_ZeroPositionAlertForm_UltraFormManager_Dock_Area_Top";
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(816, 31);
            // 
            // _ZeroPositionAlertForm_UltraFormManager_Dock_Area_Bottom
            // 
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 479);
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Bottom.Name = "_ZeroPositionAlertForm_UltraFormManager_Dock_Area_Bottom";
            this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(816, 8);
            // 
            // ZeroPositionAlertForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 487);
            this.Controls.Add(this.grdZeroPosAlert);
            this.Controls.Add(this.ultraPanel1);
            this.Controls.Add(this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ZeroPositionAlertForm_UltraFormManager_Dock_Area_Bottom);
            this.Name = "ZeroPositionAlertForm";
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Zero Position Alert";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ZeroPositionAlertForm_FormClosed);
            this.Load += new System.EventHandler(this.ZeroPositionAlertForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdZeroPosAlert)).EndInit();
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton btnRefresh;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdZeroPosAlert;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ZeroPositionAlertForm_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ZeroPositionAlertForm_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ZeroPositionAlertForm_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ZeroPositionAlertForm_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        //private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;
    }
}