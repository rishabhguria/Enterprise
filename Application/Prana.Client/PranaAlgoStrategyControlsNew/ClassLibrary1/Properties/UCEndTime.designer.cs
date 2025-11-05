namespace NirvanaAlgoStrategyControlsUCEndTime
{
    partial class UCEndTime
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
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.BtnNow = new Infragistics.Win.Misc.UltraButton();
            this.ultraDateTimeEditor1 = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDateTimeEditor1)).BeginInit();
            this.SuspendLayout();
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(2, 4);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(100, 17);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "End Time (EST)";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // BtnNow
            // 
            this.BtnNow.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
            this.BtnNow.Location = new System.Drawing.Point(183, 1);
            this.BtnNow.Name = "BtnNow";
            this.BtnNow.Size = new System.Drawing.Size(41, 23);
            this.BtnNow.TabIndex = 3;
            this.BtnNow.Text = "EOD";
            // 
            // ultraDateTimeEditor1
            // 
            this.ultraDateTimeEditor1.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Never;
            this.ultraDateTimeEditor1.Location = new System.Drawing.Point(103, 2);
            this.ultraDateTimeEditor1.MaskInput = "{LOC}hh:mm";
            this.ultraDateTimeEditor1.Name = "ultraDateTimeEditor1";
            this.ultraDateTimeEditor1.Size = new System.Drawing.Size(79, 21);
            this.ultraDateTimeEditor1.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.ultraDateTimeEditor1.TabIndex = 2;
            // 
            // UCEndTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraDateTimeEditor1);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.BtnNow);
            this.Name = "UCEndTime";
            this.Size = new System.Drawing.Size(225, 25);
            ((System.ComponentModel.ISupportInitialize)(this.ultraDateTimeEditor1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButton1;
        private Infragistics.Win.Misc.UltraButton BtnNow;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor ultraDateTimeEditor1;
    }
}
