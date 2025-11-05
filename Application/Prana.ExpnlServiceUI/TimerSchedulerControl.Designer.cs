using Prana.Utilities.UI;
namespace Prana.ExpnlServiceUI
{
    partial class TimerSchedulerControl
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
            this.slider = new TimeSlider();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.slider)).BeginInit();
            this.SuspendLayout();
            // 
            // slider
            // 
            this.slider.AllowSlidingMaximum = false;
            this.slider.AutoSize = false;
            this.slider.Cursor = System.Windows.Forms.Cursors.Hand;
            this.slider.CustomFormat = "MM/dd/yy HH:mm";
            this.slider.Format = DateFormatEnum.Custom;
            this.slider.LargeChange = System.TimeSpan.Parse("00:30:00");
            this.slider.Location = new System.Drawing.Point(107, 3);
            this.slider.Maximum = new System.DateTime(2009, 3, 26, 0, 0, 0, 0);
            this.slider.Minimum = new System.DateTime(2009, 3, 19, 0, 0, 0, 0);
            this.slider.Name = "slider";
            this.slider.SegmentEnd = new System.DateTime(((long)(0)));
            this.slider.SegmentStart = new System.DateTime(((long)(0)));
            this.slider.ShowLabelsAsDuration = false;
            this.slider.ShowMaximumLabel = false;
            this.slider.ShowMinimumLabel = false;
            this.slider.ShowSegment = true;
            this.slider.ShowValueLabel = true;
            this.slider.Size = new System.Drawing.Size(400, 19);
            this.slider.SmallChange = System.TimeSpan.Parse("00:30:00");
            this.slider.TabIndex = 0;
            this.slider.TickFrequency = System.TimeSpan.Parse("00:30:00");
            this.slider.Value = new System.DateTime(2009, 3, 19, 0, 0, 0, 0);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 25);
            this.label1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(506, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 25);
            this.label2.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(570, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 25);
            this.label3.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(629, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 25);
            this.label4.TabIndex = 4;
            // 
            // TimerSchedulerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.slider);
            this.Name = "TimerSchedulerControl";
            this.Size = new System.Drawing.Size(693, 28);
            ((System.ComponentModel.ISupportInitialize)(this.slider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Prana.Utilities.UI.TimeSlider slider;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}
