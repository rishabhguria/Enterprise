using Prana.Global;
namespace Prana.Tools
{
    partial class UDAUIForm
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
            this.usrControlUDA1 = new Prana.Tools.UserControlUDA();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._UDAUIForm_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._UDAUIForm_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._UDAUIForm_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._UDAUIForm_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            //this.ctrlImageListButtons1 = new Prana.Utilities.UI.UIUtilities.CtrlImageListButtons(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // usrControlUDA1
            // 
            this.usrControlUDA1.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.usrControlUDA1.DictInUsedUDAs = null;
            this.usrControlUDA1.Location = new System.Drawing.Point(4, 27);
            this.usrControlUDA1.Name = "usrControlUDA1";
            this.usrControlUDA1.Size = new System.Drawing.Size(824, 498);
            this.usrControlUDA1.TabIndex = 7;
            this.usrControlUDA1.DictUDAAttributes = null;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _UDAUIForm_UltraFormManager_Dock_Area_Left
            // 
            this._UDAUIForm_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._UDAUIForm_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._UDAUIForm_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._UDAUIForm_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._UDAUIForm_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._UDAUIForm_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._UDAUIForm_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._UDAUIForm_UltraFormManager_Dock_Area_Left.Name = "_UDAUIForm_UltraFormManager_Dock_Area_Left";
            this._UDAUIForm_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 498);
            // 
            // _UDAUIForm_UltraFormManager_Dock_Area_Right
            // 
            this._UDAUIForm_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._UDAUIForm_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._UDAUIForm_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._UDAUIForm_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._UDAUIForm_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._UDAUIForm_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._UDAUIForm_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(828, 27);
            this._UDAUIForm_UltraFormManager_Dock_Area_Right.Name = "_UDAUIForm_UltraFormManager_Dock_Area_Right";
            this._UDAUIForm_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 498);
            // 
            // _UDAUIForm_UltraFormManager_Dock_Area_Top
            // 
            this._UDAUIForm_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._UDAUIForm_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._UDAUIForm_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._UDAUIForm_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._UDAUIForm_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._UDAUIForm_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._UDAUIForm_UltraFormManager_Dock_Area_Top.Name = "_UDAUIForm_UltraFormManager_Dock_Area_Top";
            this._UDAUIForm_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(832, 27);
            // 
            // _UDAUIForm_UltraFormManager_Dock_Area_Bottom
            // 
            this._UDAUIForm_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._UDAUIForm_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._UDAUIForm_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._UDAUIForm_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._UDAUIForm_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._UDAUIForm_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._UDAUIForm_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 525);
            this._UDAUIForm_UltraFormManager_Dock_Area_Bottom.Name = "_UDAUIForm_UltraFormManager_Dock_Area_Bottom";
            this._UDAUIForm_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(832, 4);
            // 
            // UDAUIForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(832, 529);
            this.Controls.Add(this.usrControlUDA1);
            this.Controls.Add(this._UDAUIForm_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._UDAUIForm_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._UDAUIForm_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._UDAUIForm_UltraFormManager_Dock_Area_Bottom);
            this.Name = "UDAUIForm";
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "User Defined Attributes (UDA)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UDAForm_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ResumeLayout(false);

        }

        
        #endregion

        private UserControlUDA usrControlUDA1;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _UDAUIForm_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _UDAUIForm_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _UDAUIForm_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _UDAUIForm_UltraFormManager_Dock_Area_Bottom;
        //private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;


    }
}

