using System.Windows.Forms;
namespace NirvanaAlgoStrategyControlsUCStartTime
{
    partial class UCStartTime
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            //System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCStartTime));
            this.BtnNow = new Infragistics.Win.Misc.UltraButton();
            this.label1 = new Label();
            this.ultraDateTimeEditor1 = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDateTimeEditor1)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnNow
            // 
            this.BtnNow.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
            this.BtnNow.Location = new System.Drawing.Point(126, 0);
            this.BtnNow.Name = "BtnNow";
            this.BtnNow.Size = new System.Drawing.Size(45, 21);
            this.BtnNow.TabIndex = 2;
            this.BtnNow.Text = "NOW";
            this.BtnNow.Click += new System.EventHandler(this.BtnNow_Click);
            // 
            // label1
            // 
            //this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 21);
            this.label1.TabIndex = 2;
            this.label1.Text = "StartTime";
           // this.label1.TextAlign=
            // 
            // ultraDateTimeEditor1
            // 
            this.ultraDateTimeEditor1.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Never;
            this.ultraDateTimeEditor1.Location = new System.Drawing.Point(65, 0);
            this.ultraDateTimeEditor1.MaskInput = "{LOC}hh:mm";
            this.ultraDateTimeEditor1.Name = "ultraDateTimeEditor1";
            this.ultraDateTimeEditor1.Size = new System.Drawing.Size(59, 21);
            this.ultraDateTimeEditor1.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.ultraDateTimeEditor1.TabIndex = 1;
            // 
            // UCStartTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraDateTimeEditor1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnNow);
            this.Name = "UCStartTime";
            this.Size = new System.Drawing.Size(170, 21);
            ((System.ComponentModel.ISupportInitialize)(this.ultraDateTimeEditor1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton BtnNow;
        private Label label1;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor ultraDateTimeEditor1;
    }
}
