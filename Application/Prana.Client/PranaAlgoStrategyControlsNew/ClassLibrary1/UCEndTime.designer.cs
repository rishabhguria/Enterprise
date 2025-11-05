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
            this.BtnNow = new Infragistics.Win.Misc.UltraButton();
            this.ultraDateTimeEditor1 = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDateTimeEditor1)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnNow
            // 
            this.BtnNow.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
            this.BtnNow.Location = new System.Drawing.Point(134, 0);
            this.BtnNow.Name = "BtnNow";
            this.BtnNow.Size = new System.Drawing.Size(45, 21);
            this.BtnNow.TabIndex = 3;
            this.BtnNow.Text = "EOD";
            this.BtnNow.Click += new System.EventHandler(this.BtnNow_Click);

            // 
            // label1
            // 
            //this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 21);
            this.label1.TabIndex = 2;
            this.label1.Text = "End Time";

            // 
            // ultraDateTimeEditor1
            // 
            this.ultraDateTimeEditor1.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Never;
            this.ultraDateTimeEditor1.Location = new System.Drawing.Point(69, 0);
            this.ultraDateTimeEditor1.MaskInput = "{LOC}hh:mm";
            this.ultraDateTimeEditor1.Name = "ultraDateTimeEditor1";
            this.ultraDateTimeEditor1.Size = new System.Drawing.Size(59, 21);
            this.ultraDateTimeEditor1.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.ultraDateTimeEditor1.TabIndex = 2;
            // 
            // checkBox1
            // 
            //this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(0, 0);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(14, 21);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // UCEndTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ultraDateTimeEditor1);
            this.Controls.Add(this.BtnNow);
            this.Name = "UCEndTime";
            this.Size = new System.Drawing.Size(177, 21);
            ((System.ComponentModel.ISupportInitialize)(this.ultraDateTimeEditor1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton BtnNow;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor ultraDateTimeEditor1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label1;
    }
}
