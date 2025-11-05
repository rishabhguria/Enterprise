using System.Windows.Forms;

namespace Prana.ThirdPartyUI.Forms
{
    partial class EventLogs
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
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._EventLogs_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._EventLogs_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._EventLogs_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._EventLogs_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.grdEventLog = new Prana.Utilities.UI.UIUtilities.PranaUltraGrid();
            this.ultraGridBagLayoutManager1 = new Infragistics.Win.Misc.UltraGridBagLayoutManager(this.components);
            this.txtFixMessage = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdEventLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridBagLayoutManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _EventLogs_UltraFormManager_Dock_Area_Left
            // 
            this._EventLogs_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._EventLogs_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._EventLogs_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._EventLogs_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._EventLogs_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._EventLogs_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._EventLogs_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 32);
            this._EventLogs_UltraFormManager_Dock_Area_Left.Name = "_EventLogs_UltraFormManager_Dock_Area_Left";
            this._EventLogs_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 370);
            // 
            // _EventLogs_UltraFormManager_Dock_Area_Right
            // 
            this._EventLogs_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._EventLogs_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._EventLogs_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._EventLogs_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._EventLogs_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._EventLogs_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._EventLogs_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(692, 32);
            this._EventLogs_UltraFormManager_Dock_Area_Right.Name = "_EventLogs_UltraFormManager_Dock_Area_Right";
            this._EventLogs_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 370);
            // 
            // _EventLogs_UltraFormManager_Dock_Area_Top
            // 
            this._EventLogs_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._EventLogs_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._EventLogs_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._EventLogs_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._EventLogs_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._EventLogs_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._EventLogs_UltraFormManager_Dock_Area_Top.Name = "_EventLogs_UltraFormManager_Dock_Area_Top";
            this._EventLogs_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(700, 32);
            // 
            // _EventLogs_UltraFormManager_Dock_Area_Bottom
            // 
            this._EventLogs_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._EventLogs_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._EventLogs_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._EventLogs_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._EventLogs_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._EventLogs_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._EventLogs_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 402);
            this._EventLogs_UltraFormManager_Dock_Area_Bottom.Name = "_EventLogs_UltraFormManager_Dock_Area_Bottom";
            this._EventLogs_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(700, 8);
            //             
            // grdEventLog
            // 
            this.grdEventLog.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.grdEventLog.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdEventLog.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdEventLog.Location = new System.Drawing.Point(7, 32);
            this.grdEventLog.Name = "grdEventLog";
            this.grdEventLog.Size = new System.Drawing.Size(685, 260);
            this.grdEventLog.TabIndex = 5;
            this.grdEventLog.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdEventLog_InitializeLayout);
            this.grdEventLog.AfterRowActivate += new System.EventHandler(this.grdEventLog_AfterRowActivate);
            // 
            // lblFixMessage
            // 
            this.txtFixMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFixMessage.Location = new System.Drawing.Point(16, 302);
            this.txtFixMessage.Name = "lblFixMessage";
            this.txtFixMessage.Size = new System.Drawing.Size(667, 90);
            this.txtFixMessage.TabIndex = 10;
            this.txtFixMessage.Multiline = true;
            this.txtFixMessage.ScrollBars = RichTextBoxScrollBars.Vertical;
            this.txtFixMessage.ReadOnly = true;
            // 
            // EventLogs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 410);
            this.Controls.Add(this.txtFixMessage);
            this.Controls.Add(this.grdEventLog);
            this.Controls.Add(this._EventLogs_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._EventLogs_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._EventLogs_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._EventLogs_UltraFormManager_Dock_Area_Bottom);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(700, 410);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(700, 410);
            this.Name = "EventLogs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Event Logs";
            this.Load += new System.EventHandler(this.EventLogs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdEventLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridBagLayoutManager1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _EventLogs_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _EventLogs_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _EventLogs_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _EventLogs_UltraFormManager_Dock_Area_Bottom;
        private Utilities.UI.UIUtilities.PranaUltraGrid grdEventLog;
        private Infragistics.Win.Misc.UltraGridBagLayoutManager ultraGridBagLayoutManager1;
        private System.Windows.Forms.RichTextBox txtFixMessage;
    }
}