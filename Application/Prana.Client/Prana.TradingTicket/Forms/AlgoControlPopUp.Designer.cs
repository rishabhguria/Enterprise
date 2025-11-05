using Infragistics.Win;
using System.Drawing;

namespace Prana.TradingTicket.Forms
{
    partial class AlgoControlPopUp
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
            this.lblAlgoType = new Infragistics.Win.Misc.UltraLabel();
            this.algoStrategyControl1 = new Prana.AlgoStrategyControls.AlgoStrategyControl();
            this.strategyControl2 = new Prana.TradingTicket.StrategyControl();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.AlgoControlPopUp_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.lblAlgoMessage = new Infragistics.Win.Misc.UltraLabel();
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.btnOk = new Infragistics.Win.Misc.UltraButton();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.AlgoControlPopUp_Fill_Panel.ClientArea.SuspendLayout();
            this.AlgoControlPopUp_Fill_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblAlgoType
            // 
            this.lblAlgoType.Location = new System.Drawing.Point(22, 13);
            this.lblAlgoType.Name = "lblAlgoType";
            this.lblAlgoType.Size = new System.Drawing.Size(100, 23);
            this.lblAlgoType.TabIndex = 1;
            this.lblAlgoType.Text = "Algo Type";
            // 
            // algoStrategyControl1
            // 
            this.algoStrategyControl1.AutoScroll = true;
            this.algoStrategyControl1.AutoScrollMargin = new System.Drawing.Size(5, 5);
            this.algoStrategyControl1.AutoSize = true;
            this.algoStrategyControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.algoStrategyControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(210)))), ((int)(((byte)(212)))));
            this.algoStrategyControl1.CustomMessage = "";
            this.algoStrategyControl1.Location = new System.Drawing.Point(6, 157);
            this.algoStrategyControl1.MaxPanelHeight = 120;
            this.algoStrategyControl1.MaxPanelWidth = 280;
            this.algoStrategyControl1.Name = "algoStrategyControl1";
            this.algoStrategyControl1.Size = new System.Drawing.Size(377, 120);
            this.algoStrategyControl1.TabIndex = 2;
            // 
            // strategyControl2
            // 
            this.strategyControl2.AutoScroll = false;
            this.strategyControl2.Location = new System.Drawing.Point(12, 33);
            this.strategyControl2.Name = "strategyControl2";
            this.strategyControl2.Size = new System.Drawing.Size(218, 86);
            this.strategyControl2.TabIndex = 0;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // AlgoControlPopUp_Fill_Panel
            // 
            // 
            // AlgoControlPopUp_Fill_Panel.ClientArea
            // 
            this.AlgoControlPopUp_Fill_Panel.ClientArea.Controls.Add(this.btnOk);
            this.AlgoControlPopUp_Fill_Panel.ClientArea.Controls.Add(this.algoStrategyControl1);
            this.AlgoControlPopUp_Fill_Panel.ClientArea.Controls.Add(this.lblAlgoType);
            this.AlgoControlPopUp_Fill_Panel.ClientArea.Controls.Add(this.strategyControl2);
            this.AlgoControlPopUp_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.AlgoControlPopUp_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AlgoControlPopUp_Fill_Panel.Location = new System.Drawing.Point(8, 32);
            this.AlgoControlPopUp_Fill_Panel.Name = "AlgoControlPopUp_Fill_Panel";
            this.AlgoControlPopUp_Fill_Panel.Size = new System.Drawing.Size(885, 319);
            this.AlgoControlPopUp_Fill_Panel.TabIndex = 0;
            // 
            // lblAlgoMessage
            // 
            this.lblAlgoMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAlgoMessage.WrapText = true;
            this.lblAlgoMessage.Location = new System.Drawing.Point(8, 333);
            this.lblAlgoMessage.Margin = new System.Windows.Forms.Padding(1);
            this.lblAlgoMessage.Name = "lblAlgoMessage";
            this.lblAlgoMessage.Size = new System.Drawing.Size(526, 18);
            this.lblAlgoMessage.TabIndex = 3;
            this.lblAlgoMessage.Text = "";
            this.lblAlgoMessage.Visible = false;
            // 
            // _AlgoControlPopUp_UltraFormManager_Dock_Area_Left
            // 
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 32);
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Left.Name = "_AlgoControlPopUp_UltraFormManager_Dock_Area_Left";
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 319);
            // 
            // _AlgoControlPopUp_UltraFormManager_Dock_Area_Right
            // 
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(893, 32);
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Right.Name = "_AlgoControlPopUp_UltraFormManager_Dock_Area_Right";
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 319);
            // 
            // _AlgoControlPopUp_UltraFormManager_Dock_Area_Top
            // 
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Top.Name = "_AlgoControlPopUp_UltraFormManager_Dock_Area_Top";
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(901, 32);
            // 
            // _AlgoControlPopUp_UltraFormManager_Dock_Area_Bottom
            // 
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 351);
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Bottom.Name = "_AlgoControlPopUp_UltraFormManager_Dock_Area_Bottom";
            this._AlgoControlPopUp_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(901, 8);
            // 
            // btnOk
            //
            this.btnOk.Location = new System.Drawing.Point(131, 125);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.BackColor = Color.FromArgb(55, 67, 85);
            this.btnOk.ForeColor = Color.White;
            this.btnOk.Font = new Font(TradingTicketConstants.LIT_FONT_NAME, 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnOk.ButtonStyle = UIElementButtonStyle.Button3D;
            this.btnOk.UseAppStyling = false;
            this.btnOk.UseOsThemes = DefaultableBoolean.False;
            this.btnOk.Visible = false;
            this.btnOk.Text = "Ok";
            this.btnOk.Click += BtnOk_Click;
            // 
            // AlgoControlPopUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(901, 359);
            this.Controls.Add(this.lblAlgoMessage);
            this.Controls.Add(this.AlgoControlPopUp_Fill_Panel);
            this.Controls.Add(this._AlgoControlPopUp_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._AlgoControlPopUp_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._AlgoControlPopUp_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._AlgoControlPopUp_UltraFormManager_Dock_Area_Bottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AlgoControlPopUp";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.AlgoControlPopUp_Fill_Panel.ClientArea.ResumeLayout(false);
            this.AlgoControlPopUp_Fill_Panel.ClientArea.PerformLayout();
            this.AlgoControlPopUp_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        
        private StrategyControl strategyControl2;
        private Infragistics.Win.Misc.UltraLabel lblAlgoType;
        private AlgoStrategyControls.AlgoStrategyControl algoStrategyControl1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.Misc.UltraPanel AlgoControlPopUp_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AlgoControlPopUp_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AlgoControlPopUp_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AlgoControlPopUp_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AlgoControlPopUp_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.Misc.UltraLabel lblAlgoMessage;
        private Infragistics.Win.Misc.UltraButton btnOk;
    }
}