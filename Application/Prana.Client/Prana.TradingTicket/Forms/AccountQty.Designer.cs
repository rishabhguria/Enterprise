using Prana.Global;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
namespace Prana.TradingTicket.Forms
{
    partial class AccountQty
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
                if (dt != null)
                {
                    dt.Dispose();
                }
                if (vlAccount != null)
                {
                    vlAccount.Dispose();
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
            this.btnOK = new Infragistics.Win.Misc.UltraButton();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.grdAccounts = new PranaUltraGrid();
            this.btnClear = new Infragistics.Win.Misc.UltraButton();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.AccountQty_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this._AccountQty_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AccountQty_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AccountQty_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AccountQty_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            ((System.ComponentModel.ISupportInitialize)(this.grdAccounts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.AccountQty_Fill_Panel.ClientArea.SuspendLayout();
            this.AccountQty_Fill_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(328, 225);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(422, 225);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            // 
            // grdAccounts
            // 
            appearance1.BackColor = System.Drawing.Color.Black;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdAccounts.DisplayLayout.Appearance = appearance1;
            this.grdAccounts.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.grdAccounts.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdAccounts.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.grdAccounts.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdAccounts.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.grdAccounts.DisplayLayout.MaxColScrollRegions = 1;
            this.grdAccounts.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.Color.Gold;
            appearance5.ForeColor = System.Drawing.Color.Black;
            this.grdAccounts.DisplayLayout.Override.ActiveRowAppearance = appearance5;
            this.grdAccounts.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
            this.grdAccounts.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.None;
            this.grdAccounts.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdAccounts.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdAccounts.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance6.BackColor = System.Drawing.SystemColors.Window;
            this.grdAccounts.DisplayLayout.Override.CardAreaAppearance = appearance6;
            appearance7.BorderColor = System.Drawing.Color.Silver;
            appearance7.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdAccounts.DisplayLayout.Override.CellAppearance = appearance7;
            this.grdAccounts.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdAccounts.DisplayLayout.Override.CellPadding = 0;
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance8.TextHAlignAsString = "Left";
            this.grdAccounts.DisplayLayout.Override.HeaderAppearance = appearance8;
            this.grdAccounts.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.Select;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance9.BorderColor = System.Drawing.Color.Transparent;
            appearance9.ForeColor = System.Drawing.Color.Gold;
            this.grdAccounts.DisplayLayout.Override.RowAlternateAppearance = appearance9;
            appearance10.BackColor = System.Drawing.Color.Black;
            appearance10.BorderColor = System.Drawing.Color.Transparent;
            appearance10.ForeColor = System.Drawing.Color.Gold;
            this.grdAccounts.DisplayLayout.Override.RowAppearance = appearance10;
            this.grdAccounts.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdAccounts.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            appearance11.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdAccounts.DisplayLayout.Override.TemplateAddRowAppearance = appearance11;
            this.grdAccounts.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdAccounts.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdAccounts.Dock = System.Windows.Forms.DockStyle.Top;
            this.grdAccounts.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdAccounts.Location = new System.Drawing.Point(0, 0);
            this.grdAccounts.Name = "grdAccounts";
            this.grdAccounts.Size = new System.Drawing.Size(539, 220);
            this.grdAccounts.TabIndex = 8;
            this.grdAccounts.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnUpdate;
            this.grdAccounts.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdAccounts_AfterCellUpdate);
            this.grdAccounts.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdAccounts_InitializeLayout);
            this.grdAccounts.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdAccounts_InitializeRow);
            this.grdAccounts.InitializeTemplateAddRow += new Infragistics.Win.UltraWinGrid.InitializeTemplateAddRowEventHandler(this.grdAccounts_InitializeTemplateAddRow);
            this.grdAccounts.AfterExitEditMode += new System.EventHandler(this.grdAccounts_AfterExitEditMode);
            this.grdAccounts.KeyUp += new System.Windows.Forms.KeyEventHandler(this.grdAccounts_KeyUp);
            this.grdAccounts.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdAccounts_MouseUp);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(13, 225);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // AccountQty_Fill_Panel
            // 
            // 
            // AccountQty_Fill_Panel.ClientArea
            // 
            this.AccountQty_Fill_Panel.ClientArea.Controls.Add(this.btnClear);
            this.AccountQty_Fill_Panel.ClientArea.Controls.Add(this.grdAccounts);
            this.AccountQty_Fill_Panel.ClientArea.Controls.Add(this.btnCancel);
            this.AccountQty_Fill_Panel.ClientArea.Controls.Add(this.btnOK);
            this.AccountQty_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.AccountQty_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AccountQty_Fill_Panel.Location = new System.Drawing.Point(8, 32);
            this.AccountQty_Fill_Panel.Name = "AccountQty_Fill_Panel";
            this.AccountQty_Fill_Panel.Size = new System.Drawing.Size(539, 257);
            this.AccountQty_Fill_Panel.TabIndex = 0;
            // 
            // _AccountQty_UltraFormManager_Dock_Area_Left
            // 
            this._AccountQty_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AccountQty_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AccountQty_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._AccountQty_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AccountQty_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._AccountQty_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._AccountQty_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 32);
            this._AccountQty_UltraFormManager_Dock_Area_Left.Name = "_AccountQty_UltraFormManager_Dock_Area_Left";
            this._AccountQty_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 257);
            // 
            // _AccountQty_UltraFormManager_Dock_Area_Right
            // 
            this._AccountQty_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AccountQty_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AccountQty_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._AccountQty_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AccountQty_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._AccountQty_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._AccountQty_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(547, 32);
            this._AccountQty_UltraFormManager_Dock_Area_Right.Name = "_AccountQty_UltraFormManager_Dock_Area_Right";
            this._AccountQty_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 257);
            // 
            // _AccountQty_UltraFormManager_Dock_Area_Top
            // 
            this._AccountQty_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AccountQty_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AccountQty_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._AccountQty_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AccountQty_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._AccountQty_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._AccountQty_UltraFormManager_Dock_Area_Top.Name = "_AccountQty_UltraFormManager_Dock_Area_Top";
            this._AccountQty_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(555, 32);
            // 
            // _AccountQty_UltraFormManager_Dock_Area_Bottom
            // 
            this._AccountQty_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AccountQty_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AccountQty_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._AccountQty_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AccountQty_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._AccountQty_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._AccountQty_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 289);
            this._AccountQty_UltraFormManager_Dock_Area_Bottom.Name = "_AccountQty_UltraFormManager_Dock_Area_Bottom";
            this._AccountQty_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(555, 8);
            // 
            // AccountQty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(555, 297);
            this.Controls.Add(this.AccountQty_Fill_Panel);
            this.Controls.Add(this._AccountQty_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._AccountQty_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._AccountQty_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._AccountQty_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AccountQty";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Order Qty";
            this.Load += new System.EventHandler(this.AccountQty_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdAccounts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.AccountQty_Fill_Panel.ClientArea.ResumeLayout(false);
            this.AccountQty_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton btnOK;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private PranaUltraGrid grdAccounts;
        private Infragistics.Win.Misc.UltraButton btnClear;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.Misc.UltraPanel AccountQty_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AccountQty_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AccountQty_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AccountQty_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AccountQty_UltraFormManager_Dock_Area_Bottom;
    }
}