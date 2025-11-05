namespace Prana.PM.Client.UI.Forms
{
    partial class CreatePosition
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
            _allocationServices.Dispose();
            _proxyCashManagementServices.Dispose();
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
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreatePosition));
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.btnAddToCloseTrade = new Infragistics.Win.Misc.UltraButton();
            this.btnClear = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnRemove = new Infragistics.Win.Misc.UltraButton();
            this.ctrlCreateAndImportPosition1 = new Prana.PM.Client.UI.Controls.CtrlCreateAndImportPosition();
            this.tcCreateandImportPositions = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._CreatePosition_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._CreatePosition_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._CreatePosition_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._CreatePosition_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
           // this.ctrlImageListButtons1 = new Prana.Utilities.UI.UIUtilities.CtrlImageListButtons(this.components);
            this.ultraTabPageControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tcCreateandImportPositions)).BeginInit();
            this.tcCreateandImportPositions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Controls.Add(this.btnAddToCloseTrade);
            this.ultraTabPageControl3.Controls.Add(this.btnClear);
            this.ultraTabPageControl3.Controls.Add(this.btnSave);
            this.ultraTabPageControl3.Controls.Add(this.btnRemove);
            this.ultraTabPageControl3.Controls.Add(this.ctrlCreateAndImportPosition1);
            this.ultraTabPageControl3.Location = new System.Drawing.Point(1, 20);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(895, 333);
            // 
            // btnAddToCloseTrade
            // 
            this.btnAddToCloseTrade.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAddToCloseTrade.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnAddToCloseTrade.ImageSize = new System.Drawing.Size(75, 23);
            this.btnAddToCloseTrade.Location = new System.Drawing.Point(244, 309);
            this.btnAddToCloseTrade.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddToCloseTrade.Name = "btnAddToCloseTrade";
            this.btnAddToCloseTrade.ShowFocusRect = false;
            this.btnAddToCloseTrade.ShowOutline = false;
            this.btnAddToCloseTrade.Size = new System.Drawing.Size(77, 21);
            this.btnAddToCloseTrade.TabIndex = 125;
            this.btnAddToCloseTrade.Text = "Add New";
            this.btnAddToCloseTrade.Click += new System.EventHandler(this.btnAddToCloseTrade_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClear.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnClear.ImageSize = new System.Drawing.Size(75, 23);
            this.btnClear.Location = new System.Drawing.Point(559, 309);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4);
            this.btnClear.Name = "btnClear";
            this.btnClear.ShowFocusRect = false;
            this.btnClear.ShowOutline = false;
            this.btnClear.Size = new System.Drawing.Size(91, 21);
            this.btnClear.TabIndex = 124;
            this.btnClear.Text = "Clear Saved";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnSave.ImageSize = new System.Drawing.Size(75, 23);
            this.btnSave.Location = new System.Drawing.Point(453, 309);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.ShowFocusRect = false;
            this.btnSave.ShowOutline = false;
            this.btnSave.Size = new System.Drawing.Size(77, 21);
            this.btnSave.TabIndex = 123;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnRemove.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnRemove.ImageSize = new System.Drawing.Size(75, 23);
            this.btnRemove.Location = new System.Drawing.Point(350, 309);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.ShowFocusRect = false;
            this.btnRemove.ShowOutline = false;
            this.btnRemove.Size = new System.Drawing.Size(74, 21);
            this.btnRemove.TabIndex = 122;
            this.btnRemove.Text = "Remove";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // ctrlCreateAndImportPosition1
            // 
            this.ctrlCreateAndImportPosition1.AddButtonClicked = false;
            this.ctrlCreateAndImportPosition1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrlCreateAndImportPosition1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.ctrlCreateAndImportPosition1.IsCloseTradePopup = false;
            this.ctrlCreateAndImportPosition1.Location = new System.Drawing.Point(0, 0);
            this.ctrlCreateAndImportPosition1.Name = "ctrlCreateAndImportPosition1";
            this.ctrlCreateAndImportPosition1.NetPositions = null;
            this.ctrlCreateAndImportPosition1.OTCPositionList = null;
            this.ctrlCreateAndImportPosition1.Size = new System.Drawing.Size(895, 300);
            this.ctrlCreateAndImportPosition1.TabIndex = 0;
            this.ctrlCreateAndImportPosition1.Load += new System.EventHandler(this.ctrlCreateAndImportPosition1_Load);
            // 
            // tcCreateandImportPositions
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance1.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.tcCreateandImportPositions.ActiveTabAppearance = appearance1;
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.tcCreateandImportPositions.Appearance = appearance2;
            this.tcCreateandImportPositions.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.tcCreateandImportPositions.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tcCreateandImportPositions.Controls.Add(this.ultraTabPageControl3);
            this.tcCreateandImportPositions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcCreateandImportPositions.Location = new System.Drawing.Point(4, 27);
            this.tcCreateandImportPositions.Name = "tcCreateandImportPositions";
            this.tcCreateandImportPositions.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tcCreateandImportPositions.Size = new System.Drawing.Size(897, 354);
            this.tcCreateandImportPositions.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.tcCreateandImportPositions.TabIndex = 1;
            ultraTab1.Key = "General";
            ultraTab1.TabPage = this.ultraTabPageControl3;
            ultraTab1.Text = "Create Transaction";
            this.tcCreateandImportPositions.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1});
            this.tcCreateandImportPositions.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.VisualStudio2005;
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(895, 333);
            // 
            // helpProvider1
            // 
            this.helpProvider1.HelpNamespace = "Nirvana Help.chm";
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _CreatePosition_UltraFormManager_Dock_Area_Left
            // 
            this._CreatePosition_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CreatePosition_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._CreatePosition_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._CreatePosition_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CreatePosition_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._CreatePosition_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._CreatePosition_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._CreatePosition_UltraFormManager_Dock_Area_Left.Name = "_CreatePosition_UltraFormManager_Dock_Area_Left";
            this._CreatePosition_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 354);
            // 
            // _CreatePosition_UltraFormManager_Dock_Area_Right
            // 
            this._CreatePosition_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CreatePosition_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._CreatePosition_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._CreatePosition_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CreatePosition_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._CreatePosition_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._CreatePosition_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(901, 27);
            this._CreatePosition_UltraFormManager_Dock_Area_Right.Name = "_CreatePosition_UltraFormManager_Dock_Area_Right";
            this._CreatePosition_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 354);
            // 
            // _CreatePosition_UltraFormManager_Dock_Area_Top
            // 
            this._CreatePosition_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CreatePosition_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._CreatePosition_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._CreatePosition_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CreatePosition_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._CreatePosition_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._CreatePosition_UltraFormManager_Dock_Area_Top.Name = "_CreatePosition_UltraFormManager_Dock_Area_Top";
            this._CreatePosition_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(905, 27);
            // 
            // _CreatePosition_UltraFormManager_Dock_Area_Bottom
            // 
            this._CreatePosition_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CreatePosition_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._CreatePosition_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._CreatePosition_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CreatePosition_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._CreatePosition_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._CreatePosition_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 381);
            this._CreatePosition_UltraFormManager_Dock_Area_Bottom.Name = "_CreatePosition_UltraFormManager_Dock_Area_Bottom";
            this._CreatePosition_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(905, 4);
            // 
            // CreatePosition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(905, 385);
            this.Controls.Add(this.tcCreateandImportPositions);
            this.Controls.Add(this._CreatePosition_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._CreatePosition_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._CreatePosition_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._CreatePosition_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.helpProvider1.SetHelpKeyword(this, "CreatingManualPositions.html");
            this.helpProvider1.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(905, 385);
            this.Name = "CreatePosition";
            this.helpProvider1.SetShowHelp(this, true);
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Create Transaction";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CreatePosition_FormClosing);
            this.FormClosed += new System.EventHandler(this.CreatePosition_FormClosed);
            this.Load += new System.EventHandler(this.CreatePosition_Load);
            this.Disposed += new System.EventHandler(this.CreatePosition_Disposed);
            this.ultraTabPageControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tcCreateandImportPositions)).EndInit();
            this.tcCreateandImportPositions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);

        }

        

        

        #endregion

        private Prana.PM.Client.UI.Controls.CtrlCreateAndImportPosition ctrlCreateAndImportPosition1;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tcCreateandImportPositions;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
        private System.Windows.Forms.HelpProvider helpProvider1;
        private Infragistics.Win.Misc.UltraButton btnAddToCloseTrade;
        private Infragistics.Win.Misc.UltraButton btnClear;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnRemove;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _CreatePosition_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _CreatePosition_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _CreatePosition_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _CreatePosition_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        //private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;
    }
}