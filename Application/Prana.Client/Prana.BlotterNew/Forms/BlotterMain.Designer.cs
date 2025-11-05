using Prana.Global;
namespace Prana.Blotter
{
    partial class BlotterMain
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
            UnWireEvents();
            if (disposing)
            {
                if (ctrlBlotterMain != null)
                {
                    this.ctrlBlotterMain.Dispose();
                    ctrlBlotterMain = null;
                }
                if (components != null)
                {
                    components.Dispose();
                }
                if (exportToExcelFileDialog != null)
                {
                    exportToExcelFileDialog.Dispose();
                }
                if (buttonTool18 != null)
                {
                    buttonTool18.Dispose();
                }
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
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("BlotterToolBar");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnRefresh");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnRefresh");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnPreferences");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnPreferences");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnExportToExcel");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnExportToExcel");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnSaveAllLayout");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnSaveAllLayout");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnRemoveOrders");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnRemoveOrders");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnCancelAllSubs");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnCancelAllSubs");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool13 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnRolloverAllSubs");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool14 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnRolloverAllSubs");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool15 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnAddTab");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool16 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnAddTab");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool17 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnAddOrderTab");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool19 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnLinkUnlikTab");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool20 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnMergeButtons");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool21 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnMergeButtons");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool22 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnUploadStageOrders");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool23 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnUploadStageOrders");
            buttonTool18 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnLinkUnlikTab");


            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this._ClientArea_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._ClientArea_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this._ClientArea_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._BlotterMain_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._BlotterMain_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._BlotterMain_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._BlotterMain_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.ctrlBlotterMain = new Prana.Blotter.CtrlBlotterMain();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();


            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.SuspendLayout();
            //
            //buttonTool18
            //
            buttonTool18.SharedPropsInternal.Caption = BlotterConstants.CAPTION_LINK_TAB;
            buttonTool18.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool18.SharedPropsInternal.ToolTipText = BlotterConstants.CAPTION_LINK_TAB;
            // 
            // ultraPanel1
            // 
            this.ultraPanel1.AutoSize = true;
            this.ultraPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.ctrlBlotterMain);
            this.ultraPanel1.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Left);
            this.ultraPanel1.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Right);
            this.ultraPanel1.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Bottom);
            this.ultraPanel1.ClientArea.Controls.Add(this.statusStrip1);
            this.ultraPanel1.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Top);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(8, 31);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(1284, 611);
            this.ultraPanel1.TabIndex = 0;
            // 
            // _ClientArea_Toolbars_Dock_Area_Left
            // 
            this._ClientArea_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this._ClientArea_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._ClientArea_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 25);
            this._ClientArea_Toolbars_Dock_Area_Left.Name = "_ClientArea_Toolbars_Dock_Area_Left";
            this._ClientArea_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 586);
            this._ClientArea_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // ultraToolbarsManager1
            // 
            this.ultraToolbarsManager1.DesignerFlags = 1;
            this.ultraToolbarsManager1.DockWithinContainer = this.ultraPanel1.ClientArea;
            this.ultraToolbarsManager1.LockToolbars = true;
            this.ultraToolbarsManager1.ShowFullMenusDelay = 500;
            this.ultraToolbarsManager1.ShowQuickCustomizeButton = false;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1,
            buttonTool15,
            buttonTool17,
            buttonTool3,
            buttonTool5,
            buttonTool7,
            buttonTool9,
            buttonTool11,
            buttonTool13,
            buttonTool19,
            buttonTool20,
            buttonTool22});
            ultraToolbar1.Text = "BlotterToolBar";
            this.ultraToolbarsManager1.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            this.ultraToolbarsManager1.ToolbarSettings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            this.ultraToolbarsManager1.ToolbarSettings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
            buttonTool2.SharedPropsInternal.Caption = "Refresh Data";
            buttonTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool2.SharedPropsInternal.ToolTipText = "Refresh Data";
            buttonTool4.SharedPropsInternal.Caption = "Preferences";
            buttonTool4.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool4.SharedPropsInternal.ToolTipText = "Preferences";
            buttonTool6.SharedPropsInternal.Caption = "Export To Excel";
            buttonTool6.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool6.SharedPropsInternal.ToolTipText = "Export To Excel";
            buttonTool8.SharedPropsInternal.Caption = "Save All Layout";
            buttonTool8.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool8.SharedPropsInternal.ToolTipText = "Save All Layout";
            buttonTool10.SharedPropsInternal.Caption = "Remove Orders";
            buttonTool10.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool10.SharedPropsInternal.ToolTipText = "Remove Orders";
            buttonTool12.SharedPropsInternal.Caption = "Cancel All Subs";
            buttonTool12.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool12.SharedPropsInternal.ToolTipText = "Cancel All Subs";
            buttonTool14.SharedPropsInternal.Caption = "Rollover All Subs";
            buttonTool14.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool14.SharedPropsInternal.ToolTipText = "Rollover All Subs";
            buttonTool16.SharedPropsInternal.Caption = "Add Tab (Working)";
            buttonTool16.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool16.SharedPropsInternal.ToolTipText = "Add Tab (Working)";
            buttonTool17.SharedPropsInternal.Caption = "Add Tab (Order)";
            buttonTool17.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool17.SharedPropsInternal.ToolTipText = "Add Tab (Order)";

            buttonTool21.SharedPropsInternal.Caption = "Merge Orders";
            buttonTool21.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool21.SharedPropsInternal.ToolTipText = "Merge Orders";

            buttonTool23.SharedPropsInternal.Caption = "Upload Stage Orders";
            buttonTool23.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool23.SharedPropsInternal.ToolTipText = "Upload orders to stage on blotter";

            this.ultraToolbarsManager1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool4,
            buttonTool16,
            buttonTool6,
            buttonTool2,
            buttonTool12,
            buttonTool18,
            buttonTool8,
            buttonTool10,
            buttonTool14,
            buttonTool21,
            buttonTool23});
            this.ultraToolbarsManager1.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.ultraToolbarsManager1_ToolClick);
            // 
            // _ClientArea_Toolbars_Dock_Area_Right
            // 
            this._ClientArea_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this._ClientArea_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._ClientArea_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(1284, 25);
            this._ClientArea_Toolbars_Dock_Area_Right.Name = "_ClientArea_Toolbars_Dock_Area_Right";
            this._ClientArea_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 586);
            this._ClientArea_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _ClientArea_Toolbars_Dock_Area_Bottom
            // 
            this._ClientArea_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this._ClientArea_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._ClientArea_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 611);
            this._ClientArea_Toolbars_Dock_Area_Bottom.Name = "_ClientArea_Toolbars_Dock_Area_Bottom";
            this._ClientArea_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(1284, 0);
            this._ClientArea_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel3});
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2});


            this.statusStrip1.Location = new System.Drawing.Point(0, 589);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1284, 22);
            this.inboxControlStyler1.SetStyleSettings(this.statusStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.statusStrip1.BackColor = System.Drawing.Color.DimGray;
            this.statusStrip1.TabIndex = 5;
            // 
            // _ClientArea_Toolbars_Dock_Area_Top
            // 
            this._ClientArea_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this._ClientArea_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._ClientArea_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ClientArea_Toolbars_Dock_Area_Top.Name = "_ClientArea_Toolbars_Dock_Area_Top";
            this._ClientArea_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(1284, 25);
            this._ClientArea_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // helpProvider1
            // 
            this.helpProvider1.HelpNamespace = "Nirvana Help.chm";
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _BlotterMain_UltraFormManager_Dock_Area_Left
            // 
            this._BlotterMain_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._BlotterMain_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._BlotterMain_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._BlotterMain_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._BlotterMain_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._BlotterMain_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._BlotterMain_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._BlotterMain_UltraFormManager_Dock_Area_Left.Name = "_BlotterMain_UltraFormManager_Dock_Area_Left";
            this._BlotterMain_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 611);
            // 
            // _BlotterMain_UltraFormManager_Dock_Area_Right
            // 
            this._BlotterMain_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._BlotterMain_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._BlotterMain_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._BlotterMain_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._BlotterMain_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._BlotterMain_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._BlotterMain_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1292, 31);
            this._BlotterMain_UltraFormManager_Dock_Area_Right.Name = "_BlotterMain_UltraFormManager_Dock_Area_Right";
            this._BlotterMain_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 611);
            // 
            // _BlotterMain_UltraFormManager_Dock_Area_Top
            // 
            this._BlotterMain_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._BlotterMain_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._BlotterMain_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._BlotterMain_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._BlotterMain_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._BlotterMain_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._BlotterMain_UltraFormManager_Dock_Area_Top.Name = "_BlotterMain_UltraFormManager_Dock_Area_Top";
            this._BlotterMain_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1300, 31);
            // 
            // _BlotterMain_UltraFormManager_Dock_Area_Bottom
            // 
            this._BlotterMain_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._BlotterMain_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._BlotterMain_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._BlotterMain_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._BlotterMain_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._BlotterMain_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._BlotterMain_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 642);
            this._BlotterMain_UltraFormManager_Dock_Area_Bottom.Name = "_BlotterMain_UltraFormManager_Dock_Area_Bottom";
            this._BlotterMain_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1300, 8);
            // 
            // ctrlBlotterMain
            // 
            this.ctrlBlotterMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ctrlBlotterMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlBlotterMain.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctrlBlotterMain.Location = new System.Drawing.Point(0, 25);
            this.ctrlBlotterMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ctrlBlotterMain.Name = "ctrlBlotterMain";
            this.ctrlBlotterMain.Size = new System.Drawing.Size(1284, 586);
            this.ctrlBlotterMain.TabIndex = 0;
            this.ctrlBlotterMain.TradeClick += new System.EventHandler(this.ctrlBlotterMain_TradeClick);
            this.ctrlBlotterMain.LaunchAddFills += new System.EventHandler(this.ctrlBlotterMain_LaunchAddFills);
            this.ctrlBlotterMain.ReplaceOrEditOrderClicked += new System.EventHandler(this.ctrlBlotterMain_ReplaceOrEditOrderClicked);
            this.ctrlBlotterMain.LaunchAuditTrail += new System.EventHandler(this.ctrlBlotterMain_LaunchAuditTrail);
            this.ctrlBlotterMain.UpdateStatusBar += this.BlotterGrid_UpdateStatusBar;
            this.ctrlBlotterMain.UpdateCountStatusBar += this.BlotterGrid_UpdateCountStatusBar;
            this.ctrlBlotterMain.DisableRolloverButton += this.BlotterGrid_DisableRolloverButton;
            this.ctrlBlotterMain.UpdateOnRolloverComplete += this.BlotterMain_UpdateOnRolloverComplete;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.BackColor  = System.Drawing.Color.DimGray;
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.White;
            this.toolStripStatusLabel1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Left;
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(500, 17);
            this.toolStripStatusLabel2.BackColor = System.Drawing.Color.DimGray;
            this.toolStripStatusLabel2.ForeColor = System.Drawing.Color.White;
            this.toolStripStatusLabel2.AutoSize = true;
            this.toolStripStatusLabel2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.BackColor = System.Drawing.Color.DimGray;
            this.toolStripStatusLabel3.ForeColor = System.Drawing.Color.White;
            this.toolStripStatusLabel3.AutoSize = true;
            this.toolStripStatusLabel3.Text = "    ";
            this.toolStripStatusLabel3.Spring = true;

            // 
            // BlotterMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(1300, 650);
            this.Controls.Add(this.ultraPanel1);
            this.Controls.Add(this._BlotterMain_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._BlotterMain_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._BlotterMain_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._BlotterMain_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.helpProvider1.SetHelpKeyword(this, "ViewOrdersInBlotter.html");
            this.helpProvider1.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(87, 100);
            this.Name = "BlotterMain";
            this.helpProvider1.SetShowHelp(this, true);
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Blotter";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.BlotterMain_FormClosing);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.ultraPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private CtrlBlotterMain ctrlBlotterMain;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private System.Windows.Forms.HelpProvider helpProvider1;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _BlotterMain_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _BlotterMain_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _BlotterMain_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _BlotterMain_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;


        private Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool18;

    }
}