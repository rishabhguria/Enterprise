using Prana.Utilities.UI;
namespace Prana.ExpnlServiceUI
{
    partial class ClearanceTimeSetUpForm
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
            this.grpbxBaseTime = new Infragistics.Win.Misc.UltraGroupBox();
            this.lblSelectTime = new System.Windows.Forms.Label();
            this.lblBaseAUECSelectedTime = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.timeSlider1 = new TimeSlider();
            this.ultraTimeZoneEditor2 = new Infragistics.Win.UltraWinEditors.UltraTimeZoneEditor();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblBaseTime = new System.Windows.Forms.Label();
            this.lblGMTTime = new System.Windows.Forms.Label();
            this.lblLocalTime = new System.Windows.Forms.Label();
            this.lblSliders = new System.Windows.Forms.Label();
            this.lblAUEC = new System.Windows.Forms.Label();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.grpbxBaseTime)).BeginInit();
            this.grpbxBaseTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeSlider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTimeZoneEditor2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpbxBaseTime
            // 
            this.grpbxBaseTime.Controls.Add(this.lblSelectTime);
            this.grpbxBaseTime.Controls.Add(this.lblBaseAUECSelectedTime);
            this.grpbxBaseTime.Controls.Add(this.checkBox1);
            this.grpbxBaseTime.Controls.Add(this.timeSlider1);
            this.grpbxBaseTime.Controls.Add(this.ultraTimeZoneEditor2);
            this.grpbxBaseTime.Location = new System.Drawing.Point(12, 0);
            this.grpbxBaseTime.Name = "grpbxBaseTime";
            this.grpbxBaseTime.Size = new System.Drawing.Size(703, 112);
            this.grpbxBaseTime.TabIndex = 0;
            this.grpbxBaseTime.Text = "Base Time";
            // 
            // lblSelectTime
            // 
            this.lblSelectTime.Location = new System.Drawing.Point(9, 70);
            this.lblSelectTime.Name = "lblSelectTime";
            this.lblSelectTime.Size = new System.Drawing.Size(87, 21);
            this.lblSelectTime.TabIndex = 5;
            this.lblSelectTime.Text = "Select Time";
            // 
            // lblBaseAUECSelectedTime
            // 
            this.lblBaseAUECSelectedTime.Location = new System.Drawing.Point(499, 70);
            this.lblBaseAUECSelectedTime.Name = "lblBaseAUECSelectedTime";
            this.lblBaseAUECSelectedTime.Size = new System.Drawing.Size(70, 21);
            this.lblBaseAUECSelectedTime.TabIndex = 4;
            // 
            // checkBox1
            // 
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(12, 47);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(481, 19);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "Automatically calculate the best possible clearance Times for all AUECs based on " +
                "this AUEC";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // timeSlider1
            // 
            this.timeSlider1.AllowSlidingMaximum = false;
            this.timeSlider1.AutoSize = false;
            this.timeSlider1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.timeSlider1.CustomFormat = "MM/dd/yy HH:mm";
            this.timeSlider1.Enabled = false;
            this.timeSlider1.Format = DateFormatEnum.NoDateShortTime;
            this.timeSlider1.LargeChange = System.TimeSpan.Parse("00:30:00");
            this.timeSlider1.Location = new System.Drawing.Point(93, 72);
            this.timeSlider1.Maximum = new System.DateTime(2009, 4, 29, 0, 0, 0, 0);
            this.timeSlider1.Minimum = new System.DateTime(((long)(0)));
            this.timeSlider1.Name = "timeSlider1";
            this.timeSlider1.SegmentEnd = new System.DateTime(((long)(0)));
            this.timeSlider1.SegmentStart = new System.DateTime(((long)(0)));
            this.timeSlider1.ShowLabelsAsDuration = false;
            this.timeSlider1.ShowMaximumLabel = false;
            this.timeSlider1.ShowMinimumLabel = false;
            this.timeSlider1.ShowSegment = true;
            this.timeSlider1.ShowValueLabel = true;
            this.timeSlider1.Size = new System.Drawing.Size(400, 19);
            this.timeSlider1.SmallChange = System.TimeSpan.Parse("00:30:00");
            this.timeSlider1.TabIndex = 2;
            this.timeSlider1.TickFrequency = System.TimeSpan.Parse("00:30:00");
            this.timeSlider1.Value = new System.DateTime(((long)(0)));
            // 
            // ultraTimeZoneEditor2
            // 
            this.ultraTimeZoneEditor2.Location = new System.Drawing.Point(12, 19);
            this.ultraTimeZoneEditor2.Name = "ultraTimeZoneEditor2";
            this.ultraTimeZoneEditor2.Size = new System.Drawing.Size(368, 21);
            this.ultraTimeZoneEditor2.TabIndex = 1;
            this.ultraTimeZoneEditor2.Text = "(GMT+05:30) Chennai, Kolkata, Mumbai, New Delhi";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.AutoScrollMinSize = new System.Drawing.Size(4, 4);
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(704, 207);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // lblBaseTime
            // 
            this.lblBaseTime.AutoSize = true;
            this.lblBaseTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBaseTime.Location = new System.Drawing.Point(622, 0);
            this.lblBaseTime.Name = "lblBaseTime";
            this.lblBaseTime.Size = new System.Drawing.Size(62, 13);
            this.lblBaseTime.TabIndex = 4;
            this.lblBaseTime.Text = "BaseTime";
            // 
            // lblGMTTime
            // 
            this.lblGMTTime.AutoSize = true;
            this.lblGMTTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGMTTime.Location = new System.Drawing.Point(556, 0);
            this.lblGMTTime.Name = "lblGMTTime";
            this.lblGMTTime.Size = new System.Drawing.Size(61, 13);
            this.lblGMTTime.TabIndex = 3;
            this.lblGMTTime.Text = "GMTTime";
            // 
            // lblLocalTime
            // 
            this.lblLocalTime.AutoSize = true;
            this.lblLocalTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocalTime.Location = new System.Drawing.Point(488, 0);
            this.lblLocalTime.Name = "lblLocalTime";
            this.lblLocalTime.Size = new System.Drawing.Size(65, 13);
            this.lblLocalTime.TabIndex = 2;
            this.lblLocalTime.Text = "LocalTime";
            // 
            // lblSliders
            // 
            this.lblSliders.AutoSize = true;
            this.lblSliders.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSliders.Location = new System.Drawing.Point(147, 0);
            this.lblSliders.Name = "lblSliders";
            this.lblSliders.Size = new System.Drawing.Size(39, 13);
            this.lblSliders.TabIndex = 1;
            this.lblSliders.Text = "Slider";
            // 
            // lblAUEC
            // 
            this.lblAUEC.AutoSize = true;
            this.lblAUEC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAUEC.Location = new System.Drawing.Point(21, 0);
            this.lblAUEC.Name = "lblAUEC";
            this.lblAUEC.Size = new System.Drawing.Size(40, 13);
            this.lblAUEC.TabIndex = 0;
            this.lblAUEC.Text = "AUEC";
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.BorderStyle = Infragistics.Win.Misc.GroupBoxBorderStyle.None;
            this.ultraGroupBox1.Controls.Add(this.lblLocalTime);
            this.ultraGroupBox1.Controls.Add(this.lblBaseTime);
            this.ultraGroupBox1.Controls.Add(this.lblSliders);
            this.ultraGroupBox1.Controls.Add(this.lblAUEC);
            this.ultraGroupBox1.Controls.Add(this.lblGMTTime);
            this.ultraGroupBox1.Location = new System.Drawing.Point(12, 112);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(688, 19);
            this.ultraGroupBox1.TabIndex = 5;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(222, 361);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(95, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save and Close";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(372, 361);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Location = new System.Drawing.Point(12, 138);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(706, 209);
            this.panel1.TabIndex = 0;
            // 
            // ClearanceTimeSetUpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(727, 392);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.ultraGroupBox1);
            this.Controls.Add(this.grpbxBaseTime);
            this.MaximumSize = new System.Drawing.Size(735, 800);
            this.MinimumSize = new System.Drawing.Size(735, 419);
            this.Name = "ClearanceTimeSetUpForm";
            this.Text = "ClearanceTimeSetUp";
            this.Load += new System.EventHandler(this.ClearanceTimeSetUpForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grpbxBaseTime)).EndInit();
            this.grpbxBaseTime.ResumeLayout(false);
            this.grpbxBaseTime.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeSlider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTimeZoneEditor2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ultraGroupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox grpbxBaseTime;
        private Infragistics.Win.UltraWinEditors.UltraTimeZoneEditor ultraTimeZoneEditor2;
        private TimeSlider timeSlider1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblAUEC;
        private System.Windows.Forms.Label lblBaseTime;
        private System.Windows.Forms.Label lblGMTTime;
        private System.Windows.Forms.Label lblLocalTime;
        private System.Windows.Forms.Label lblSliders;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private System.Windows.Forms.Label lblBaseAUECSelectedTime;
        private System.Windows.Forms.Label lblSelectTime;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private System.Windows.Forms.Panel panel1;
    }
}