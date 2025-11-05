namespace NirvanaAlgoStrategyControlsUCEndTimeandDuration
{
    partial class UCEndTimeandDuration 
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
            this.radioButtonEndTime = new System.Windows.Forms.RadioButton();
            this.radioButtonDuration = new System.Windows.Forms.RadioButton();
            this.durationUpDown = new System.Windows.Forms.NumericUpDown();
            this.endTimeDateTime = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            ((System.ComponentModel.ISupportInitialize)(this.durationUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.endTimeDateTime)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnNow
            // 
            this.BtnNow.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
            this.BtnNow.Location = new System.Drawing.Point(183, 2);
            this.BtnNow.Name = "BtnNow";
            this.BtnNow.Size = new System.Drawing.Size(41, 23);
            this.BtnNow.TabIndex = 3;
            this.BtnNow.Text = "EOD";
            this.BtnNow.Click += new System.EventHandler(this.BtnNow_Click);
            // 
            // radioButtonEndTime
            // 
            this.radioButtonEndTime.AutoSize = true;
            this.radioButtonEndTime.ForeColor = System.Drawing.Color.Black;
            this.radioButtonEndTime.Location = new System.Drawing.Point(3, 5);
            this.radioButtonEndTime.Name = "radioButtonEndTime";
            this.radioButtonEndTime.Size = new System.Drawing.Size(100, 17);
            this.radioButtonEndTime.TabIndex = 1;
            this.radioButtonEndTime.TabStop = true;
            this.radioButtonEndTime.Text = "End Time (EST)";
            this.radioButtonEndTime.UseVisualStyleBackColor = true;
            this.radioButtonEndTime.CheckedChanged += new System.EventHandler(this.radioButtonEndTime_CheckedChanged);
            // 
            // radioButtonDuration
            // 
            this.radioButtonDuration.AutoSize = true;
            this.radioButtonDuration.ForeColor = System.Drawing.Color.Black;
            this.radioButtonDuration.Location = new System.Drawing.Point(2, 30);
            this.radioButtonDuration.Name = "radioButtonDuration";
            this.radioButtonDuration.Size = new System.Drawing.Size(91, 17);
            this.radioButtonDuration.TabIndex = 4;
            this.radioButtonDuration.TabStop = true;
            this.radioButtonDuration.Text = "Duration (Min)";
            this.radioButtonDuration.UseVisualStyleBackColor = true;
            this.radioButtonDuration.CheckedChanged += new System.EventHandler(this.radioButtonDuration_CheckedChanged);
            // 
            // durationUpDown
            // 
            this.durationUpDown.Location = new System.Drawing.Point(100, 27);
            this.durationUpDown.Name = "durationUpDown";
            this.durationUpDown.Size = new System.Drawing.Size(61, 20);
            this.durationUpDown.TabIndex = 5;
            // 
            // endTimeDateTime
            // 
            this.endTimeDateTime.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Never;
            this.endTimeDateTime.Location = new System.Drawing.Point(99, 3);
            this.endTimeDateTime.MaskInput = "{LOC}hh:mm";
            this.endTimeDateTime.Name = "endTimeDateTime";
            this.endTimeDateTime.Size = new System.Drawing.Size(83, 21);
            this.endTimeDateTime.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.endTimeDateTime.TabIndex = 2;
            // 
            // UCEndTimeandDuration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.endTimeDateTime);
            this.Controls.Add(this.durationUpDown);
            this.Controls.Add(this.radioButtonDuration);
            this.Controls.Add(this.radioButtonEndTime);
            this.Controls.Add(this.BtnNow);
            this.Name = "UCEndTimeandDuration";
            this.Size = new System.Drawing.Size(225, 50);
            ((System.ComponentModel.ISupportInitialize)(this.durationUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.endTimeDateTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton BtnNow;
        private System.Windows.Forms.RadioButton radioButtonEndTime;
        private System.Windows.Forms.RadioButton radioButtonDuration;
        private System.Windows.Forms.NumericUpDown durationUpDown;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor endTimeDateTime;
    }
}
