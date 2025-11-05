using Infragistics.Win;
using Prana.Global;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using System.Drawing;

namespace Prana.TradingTicket.Forms
{
    partial class BrokersConnectionStatus
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
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.BrokersConnectionStatus_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.grdBrokers = new Prana.Utilities.UI.UIUtilities.PranaUltraGrid();
            this.btnOK = new Infragistics.Win.Misc.UltraButton();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.BrokersConnectionStatus_Fill_Panel.ClientArea.SuspendLayout();
            this.BrokersConnectionStatus_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdBrokers)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _BrokersConnectionStatus_UltraFormManager_Dock_Area_Left
            // 
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 9;
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 39);
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Left.Name = "_BrokersConnectionStatus_UltraFormManager_Dock_Area_Left";
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(9, 249);
            // 
            // _BrokersConnectionStatus_UltraFormManager_Dock_Area_Right
            // 
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 9;
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(546, 39);
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Right.Name = "_BrokersConnectionStatus_UltraFormManager_Dock_Area_Right";
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(9, 249);
            // 
            // _BrokersConnectionStatus_UltraFormManager_Dock_Area_Top
            // 
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Top.Name = "_BrokersConnectionStatus_UltraFormManager_Dock_Area_Top";
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(555, 39);
            // 
            // _BrokersConnectionStatus_UltraFormManager_Dock_Area_Bottom
            // 
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 9;
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 288);
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Bottom.Name = "_BrokersConnectionStatus_UltraFormManager_Dock_Area_Bottom";
            this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(555, 9);
            // 
            // BrokersConnectionStatus_Fill_Panel
            // 
            // 
            // BrokersConnectionStatus_Fill_Panel.ClientArea
            // 
            this.BrokersConnectionStatus_Fill_Panel.ClientArea.Controls.Add(this.grdBrokers);
            this.BrokersConnectionStatus_Fill_Panel.ClientArea.Controls.Add(this.btnOK);
            this.BrokersConnectionStatus_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.BrokersConnectionStatus_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BrokersConnectionStatus_Fill_Panel.Location = new System.Drawing.Point(9, 39);
            this.BrokersConnectionStatus_Fill_Panel.Name = "BrokersConnectionStatus_Fill_Panel";
            this.BrokersConnectionStatus_Fill_Panel.Size = new System.Drawing.Size(537, 249);
            this.BrokersConnectionStatus_Fill_Panel.TabIndex = 5;
            // 
            // grdBrokers
            // 
            appearance1.BackColor = System.Drawing.Color.Black;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdBrokers.DisplayLayout.Appearance = appearance1;
            this.grdBrokers.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.grdBrokers.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.grdBrokers.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdBrokers.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.grdBrokers.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdBrokers.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.grdBrokers.DisplayLayout.MaxColScrollRegions = 1;
            this.grdBrokers.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.Color.Gold;
            appearance5.ForeColor = System.Drawing.Color.Black;
            this.grdBrokers.DisplayLayout.Override.ActiveRowAppearance = appearance5;
            this.grdBrokers.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
            this.grdBrokers.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.None;
            this.grdBrokers.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this.grdBrokers.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.False;
            this.grdBrokers.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdBrokers.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance6.BackColor = System.Drawing.SystemColors.Window;
            this.grdBrokers.DisplayLayout.Override.CardAreaAppearance = appearance6;
            appearance7.BorderColor = System.Drawing.Color.Silver;
            appearance7.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            appearance7.ForeColorDisabled = System.Drawing.Color.DimGray;
            this.grdBrokers.DisplayLayout.Override.CellAppearance = appearance7;
            this.grdBrokers.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdBrokers.DisplayLayout.Override.CellPadding = 0;
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance8.TextHAlignAsString = "Left";
            this.grdBrokers.DisplayLayout.Override.HeaderAppearance = appearance8;
            this.grdBrokers.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.Select;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance9.BorderColor = System.Drawing.Color.Transparent;
            appearance9.ForeColor = System.Drawing.Color.Gold;
            this.grdBrokers.DisplayLayout.Override.RowAlternateAppearance = appearance9;
            appearance10.BackColor = System.Drawing.Color.Black;
            appearance10.BorderColor = System.Drawing.Color.Transparent;
            appearance10.ForeColor = System.Drawing.Color.Gold;
            this.grdBrokers.DisplayLayout.Override.RowAppearance = appearance10;
            this.grdBrokers.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdBrokers.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            appearance11.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdBrokers.DisplayLayout.Override.TemplateAddRowAppearance = appearance11;
            this.grdBrokers.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdBrokers.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdBrokers.Dock = System.Windows.Forms.DockStyle.Top;
            this.grdBrokers.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdBrokers.Location = new System.Drawing.Point(0, 0);
            this.grdBrokers.Name = "grdBrokers";
            this.grdBrokers.Size = new System.Drawing.Size(537, 220);
            this.grdBrokers.TabIndex = 8;
            this.grdBrokers.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdBrokers_AfterCellUpdate);
            this.grdBrokers.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdBrokers_InitializeLayout);
            this.grdBrokers.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdBrokers_InitializeRow);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(240, 225);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // BrokersConnectionStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
            this.ClientSize = new System.Drawing.Size(555, 297);
            this.Controls.Add(this.BrokersConnectionStatus_Fill_Panel);
            this.Controls.Add(this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._BrokersConnectionStatus_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BrokersConnectionStatus";
            this.ShowIcon = false;
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Brokers Connection Status";
            this.Load += new System.EventHandler(this.BrokerMapping_Load);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.BrokersConnectionStatus_Fill_Panel.ClientArea.ResumeLayout(false);
            this.BrokersConnectionStatus_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdBrokers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _BrokersConnectionStatus_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _BrokersConnectionStatus_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _BrokersConnectionStatus_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _BrokersConnectionStatus_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.Misc.UltraPanel BrokersConnectionStatus_Fill_Panel;
        private PranaUltraGrid grdBrokers;
        private Infragistics.Win.Misc.UltraButton btnOK;
    }
}