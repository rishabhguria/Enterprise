using Infragistics.Win;
using Prana.Global;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using System.Drawing;

namespace Prana.TradingTicket.Forms
{
    partial class StagedOrderAllocationView
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
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._StagedOrders_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._StagedOrders_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._StagedOrders_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._StagedOrders_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.StagedOrders_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.grdStagedOrder = new Prana.Utilities.UI.UIUtilities.PranaUltraGrid();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.StagedOrders_Fill_Panel.ClientArea.SuspendLayout();
            this.StagedOrders_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdStagedOrder)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _StagedOrders_UltraFormManager_Dock_Area_Left
            // 
            this._StagedOrders_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._StagedOrders_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._StagedOrders_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._StagedOrders_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._StagedOrders_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._StagedOrders_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._StagedOrders_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 32);
            this._StagedOrders_UltraFormManager_Dock_Area_Left.Name = "_StagedOrders_UltraFormManager_Dock_Area_Left";
            this._StagedOrders_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 196);
            // 
            // _StagedOrders_UltraFormManager_Dock_Area_Right
            // 
            this._StagedOrders_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._StagedOrders_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._StagedOrders_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._StagedOrders_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._StagedOrders_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._StagedOrders_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._StagedOrders_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(452, 32);
            this._StagedOrders_UltraFormManager_Dock_Area_Right.Name = "_StagedOrders_UltraFormManager_Dock_Area_Right";
            this._StagedOrders_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 196);
            // 
            // _StagedOrders_UltraFormManager_Dock_Area_Top
            // 
            this._StagedOrders_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._StagedOrders_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._StagedOrders_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._StagedOrders_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._StagedOrders_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._StagedOrders_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._StagedOrders_UltraFormManager_Dock_Area_Top.Name = "_StagedOrders_UltraFormManager_Dock_Area_Top";
            this._StagedOrders_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(460, 32);
            // 
            // _StagedOrders_UltraFormManager_Dock_Area_Bottom
            // 
            this._StagedOrders_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._StagedOrders_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._StagedOrders_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._StagedOrders_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._StagedOrders_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._StagedOrders_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._StagedOrders_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 228);
            this._StagedOrders_UltraFormManager_Dock_Area_Bottom.Name = "_StagedOrders_UltraFormManager_Dock_Area_Bottom";
            this._StagedOrders_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(460, 8);
            // 
            // StagedOrders_Fill_Panel
            // 
            // 
            // StagedOrders_Fill_Panel.ClientArea
            // 
            this.StagedOrders_Fill_Panel.ClientArea.Controls.Add(this.grdStagedOrder);
            this.StagedOrders_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.StagedOrders_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StagedOrders_Fill_Panel.Location = new System.Drawing.Point(8, 32);
            this.StagedOrders_Fill_Panel.Name = "StagedOrders_Fill_Panel";
            this.StagedOrders_Fill_Panel.Size = new System.Drawing.Size(444, 196);
            this.StagedOrders_Fill_Panel.TabIndex = 6;
            // 
            // grdStagedOrder
            // 
            appearance1.BackColor = System.Drawing.Color.Black;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdStagedOrder.DisplayLayout.Appearance = appearance1;
            this.grdStagedOrder.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.grdStagedOrder.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.grdStagedOrder.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdStagedOrder.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.grdStagedOrder.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdStagedOrder.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.grdStagedOrder.DisplayLayout.MaxColScrollRegions = 1;
            this.grdStagedOrder.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.Color.Gold;
            appearance5.ForeColor = System.Drawing.Color.Black;
            this.grdStagedOrder.DisplayLayout.Override.ActiveRowAppearance = appearance5;
            this.grdStagedOrder.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
            this.grdStagedOrder.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.None;
            this.grdStagedOrder.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this.grdStagedOrder.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.False;
            this.grdStagedOrder.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdStagedOrder.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance6.BackColor = System.Drawing.SystemColors.Window;
            this.grdStagedOrder.DisplayLayout.Override.CardAreaAppearance = appearance6;
            appearance7.BorderColor = System.Drawing.Color.Silver;
            appearance7.ForeColorDisabled = System.Drawing.Color.DimGray;
            appearance7.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdStagedOrder.DisplayLayout.Override.CellAppearance = appearance7;
            this.grdStagedOrder.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdStagedOrder.DisplayLayout.Override.CellPadding = 0;
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance8.TextHAlignAsString = "Left";
            this.grdStagedOrder.DisplayLayout.Override.HeaderAppearance = appearance8;
            this.grdStagedOrder.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.Select;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance9.BorderColor = System.Drawing.Color.Transparent;
            appearance9.ForeColor = System.Drawing.Color.Gold;
            this.grdStagedOrder.DisplayLayout.Override.RowAlternateAppearance = appearance9;
            appearance10.BackColor = System.Drawing.Color.Black;
            appearance10.BorderColor = System.Drawing.Color.Transparent;
            appearance10.ForeColor = System.Drawing.Color.Gold;
            this.grdStagedOrder.DisplayLayout.Override.RowAppearance = appearance10;
            this.grdStagedOrder.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdStagedOrder.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            appearance11.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdStagedOrder.DisplayLayout.Override.TemplateAddRowAppearance = appearance11;
            this.grdStagedOrder.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdStagedOrder.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdStagedOrder.Dock = System.Windows.Forms.DockStyle.Top;
            this.grdStagedOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdStagedOrder.Location = new System.Drawing.Point(0, 0);
            this.grdStagedOrder.Name = "grdStagedOrder";
            this.grdStagedOrder.Size = new System.Drawing.Size(444, 194);
            this.grdStagedOrder.TabIndex = 8;
            this.grdStagedOrder.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdStagedOrder_InitializeLayout);
            // 
            // StagedOrderAllocationView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
            this.ClientSize = new System.Drawing.Size(460, 236);
            this.Controls.Add(this.StagedOrders_Fill_Panel);
            this.Controls.Add(this._StagedOrders_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._StagedOrders_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._StagedOrders_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._StagedOrders_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StagedOrderAllocationView";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Custom Allocation";
            this.Load += new System.EventHandler(this.StagedOrderAllocationView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.StagedOrders_Fill_Panel.ClientArea.ResumeLayout(false);
            this.StagedOrders_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdStagedOrder)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _StagedOrders_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _StagedOrders_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _StagedOrders_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _StagedOrders_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.Misc.UltraPanel StagedOrders_Fill_Panel;
        private Utilities.UI.UIUtilities.PranaUltraGrid grdStagedOrder;
    }
}