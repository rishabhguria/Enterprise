using System.Windows.Forms;

namespace Prana.Utilities.UI.UIUtilities
{
    public partial class CustomMessageBox
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
            this.MessageLabel = new Infragistics.Win.Misc.UltraLabel();
            this.ultraOkButton = new Infragistics.Win.Misc.UltraButton();
            this.ultraPanelBottom = new System.Windows.Forms.FlowLayoutPanel();
            this.ultraCancelButton = new Infragistics.Win.Misc.UltraButton();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ultraPanelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MessageLabel
            // 
            this.MessageLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.ImageVAlign = Infragistics.Win.VAlign.Middle;
            appearance1.TextHAlignAsString = "Center";
            appearance1.TextVAlignAsString = "Middle";
            this.MessageLabel.Appearance = appearance1;
            this.MessageLabel.AutoSize = true;
            this.MessageLabel.MaximumSize = new System.Drawing.Size(407, 0);
            this.MessageLabel.MinimumSize = new System.Drawing.Size(407, 60);
            this.MessageLabel.Name = "MessageLabel";
            this.MessageLabel.TabIndex = 0;
            this.MessageLabel.Text = "Text";
            this.MessageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // ultraOkButton
            // 
            this.ultraOkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ultraOkButton.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
            this.ultraOkButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraOkButton.Name = "ultraOkButton";
            this.ultraOkButton.Size = new System.Drawing.Size(84, 24);
            this.ultraOkButton.TabIndex = 3;
            this.ultraOkButton.Text = "OK";
            this.ultraOkButton.Margin = new System.Windows.Forms.Padding(10, 0, 10, 7);
            this.ultraOkButton.Click += new System.EventHandler(this.ultraOkButton_Click);
            

            // 
            // ultraCancelButton
            // 
            this.ultraCancelButton.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
            this.ultraCancelButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ultraCancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraCancelButton.Name = "ultraCancelButton";
            this.ultraCancelButton.Size = new System.Drawing.Size(84, 24);
            this.ultraCancelButton.TabIndex = 4;
            this.ultraCancelButton.Text = "Cancel";
            this.ultraCancelButton.Margin = new System.Windows.Forms.Padding(10, 0, 10, 7);
            this.ultraCancelButton.Click += new System.EventHandler(this.ultraCancelButton_Click);
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Left
            // 
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 32);
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Left.Name = "_ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Left";
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 138);
            // 
            // _ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Right
            // 
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(439, 32);
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Right.Name = "_ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Right";
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 138);
            // 
            // _ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Top
            // 
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Top.Name = "_ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Top";
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(447, 32);
            // 
            // _ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Bottom
            // 
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 170);
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Bottom.Name = "_ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Bottom";
            this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(447, 8);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.Controls.Add(this.MessageLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ultraPanelBottom, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            // 
            // ultraPanelBottom
            // 
            this.ultraPanelBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.ultraPanelBottom.AutoSize = true;
            this.ultraPanelBottom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ultraPanelBottom.BackColor = System.Drawing.Color.Transparent;
            this.ultraPanelBottom.Controls.Add(this.ultraOkButton);
            this.ultraPanelBottom.Controls.Add(this.ultraCancelButton);
            this.ultraPanelBottom.Name = "ultraPanelBottom";
            this.ultraPanelBottom.Size = new System.Drawing.Size(450, 30);
            this.ultraPanelBottom.TabIndex = 2;
            // 
            // CustomMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 150);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Bottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CustomMessageBox";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ultraPanelBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FlowLayoutPanel ultraPanelBottom;
        private Infragistics.Win.Misc.UltraLabel MessageLabel;
        private Infragistics.Win.Misc.UltraButton ultraOkButton;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ColoredWindowsCustomMessageBox_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Infragistics.Win.Misc.UltraButton ultraCancelButton;
        //#region Windows Form Designer generated code

        ///// <summary>
        ///// Required method for Designer support - do not modify
        ///// the contents of this method with the code editor.
        ///// </summary>
        //private void InitializeComponent()
        //{
        //    this.components = new System.ComponentModel.Container();
        //    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        //    this.Text = "ColoredWindowsCustomMessageBox";
        //}
    }
}