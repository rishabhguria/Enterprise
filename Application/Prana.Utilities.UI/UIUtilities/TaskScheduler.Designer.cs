namespace Prana.Utilities.UI.UIUtilities
{
    partial class TaskScheduler
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
            this.components = new System.ComponentModel.Container();
            this.numericUpDownDaily = new System.Windows.Forms.NumericUpDown();
            this.labelDailyEvery = new System.Windows.Forms.Label();
            this.labelDailyDay = new System.Windows.Forms.Label();
            this.checkedListBoxWeeklyDays = new System.Windows.Forms.CheckedListBox();
            this.checkedListBoxMonthlyMonths = new System.Windows.Forms.CheckedListBox();
            this.checkedListBoxMonthlyDays = new System.Windows.Forms.CheckedListBox();
            this.checkedListBoxMonthlyWeekNumber = new System.Windows.Forms.CheckedListBox();
            this.checkedListBoxMonthlyWeekDay = new System.Windows.Forms.CheckedListBox();
            this.labelWeeklyDays = new System.Windows.Forms.Label();
            this.tabControlMonthlyMode = new System.Windows.Forms.TabControl();
            this.tabPageMonthlyDayOfMonth = new System.Windows.Forms.TabPage();
            this.tabPageMonthlyWeekDay = new System.Windows.Forms.TabPage();
            this.labelMonthlyMonth = new System.Windows.Forms.Label();
            this.labelStartDate = new System.Windows.Forms.Label();
            this.dateTimePickerStartDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerTriggerTime = new System.Windows.Forms.DateTimePicker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.rbMonthly = new System.Windows.Forms.RadioButton();
            this.rbWeekly = new System.Windows.Forms.RadioButton();
            this.rbDaily = new System.Windows.Forms.RadioButton();
            this.rbOneTime = new System.Windows.Forms.RadioButton();
            this.groupBoxMonthly = new System.Windows.Forms.GroupBox();
            this.groupBoxWeekly = new System.Windows.Forms.GroupBox();
            this.groupBoxDaily = new System.Windows.Forms.GroupBox();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDaily)).BeginInit();
            this.tabControlMonthlyMode.SuspendLayout();
            this.tabPageMonthlyDayOfMonth.SuspendLayout();
            this.tabPageMonthlyWeekDay.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBoxMonthly.SuspendLayout();
            this.groupBoxWeekly.SuspendLayout();
            this.groupBoxDaily.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // numericUpDownDaily
            // 
            this.numericUpDownDaily.Location = new System.Drawing.Point(89, 24);
            this.numericUpDownDaily.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownDaily.Name = "numericUpDownDaily";
            this.numericUpDownDaily.Size = new System.Drawing.Size(49, 20);
            this.inboxControlStyler1.SetStyleSettings(this.numericUpDownDaily, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.numericUpDownDaily.TabIndex = 3;
            this.numericUpDownDaily.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelDailyEvery
            // 
            this.labelDailyEvery.AutoSize = true;
            this.labelDailyEvery.Location = new System.Drawing.Point(15, 26);
            this.labelDailyEvery.Name = "labelDailyEvery";
            this.labelDailyEvery.Size = new System.Drawing.Size(68, 13);
            this.inboxControlStyler1.SetStyleSettings(this.labelDailyEvery, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.labelDailyEvery.TabIndex = 4;
            this.labelDailyEvery.Text = "Recur every:";
            // 
            // labelDailyDay
            // 
            this.labelDailyDay.AutoSize = true;
            this.labelDailyDay.Location = new System.Drawing.Point(144, 26);
            this.labelDailyDay.Name = "labelDailyDay";
            this.labelDailyDay.Size = new System.Drawing.Size(29, 13);
            this.inboxControlStyler1.SetStyleSettings(this.labelDailyDay, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.labelDailyDay.TabIndex = 5;
            this.labelDailyDay.Text = "days";
            // 
            // checkedListBoxWeeklyDays
            // 
            this.checkedListBoxWeeklyDays.CheckOnClick = true;
            this.checkedListBoxWeeklyDays.FormattingEnabled = true;
            this.checkedListBoxWeeklyDays.Items.AddRange(new object[] {
            "Sunday",
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday"});
            this.checkedListBoxWeeklyDays.Location = new System.Drawing.Point(55, 16);
            this.checkedListBoxWeeklyDays.Name = "checkedListBoxWeeklyDays";
            this.checkedListBoxWeeklyDays.Size = new System.Drawing.Size(104, 109);
            this.inboxControlStyler1.SetStyleSettings(this.checkedListBoxWeeklyDays, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.checkedListBoxWeeklyDays.TabIndex = 27;
            // 
            // checkedListBoxMonthlyMonths
            // 
            this.checkedListBoxMonthlyMonths.CheckOnClick = true;
            this.checkedListBoxMonthlyMonths.FormattingEnabled = true;
            this.checkedListBoxMonthlyMonths.Items.AddRange(new object[] {
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"});
            this.checkedListBoxMonthlyMonths.Location = new System.Drawing.Point(53, 19);
            this.checkedListBoxMonthlyMonths.Name = "checkedListBoxMonthlyMonths";
            this.checkedListBoxMonthlyMonths.Size = new System.Drawing.Size(120, 154);
            this.inboxControlStyler1.SetStyleSettings(this.checkedListBoxMonthlyMonths, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.checkedListBoxMonthlyMonths.TabIndex = 28;
            // 
            // checkedListBoxMonthlyDays
            // 
            this.checkedListBoxMonthlyDays.CheckOnClick = true;
            this.checkedListBoxMonthlyDays.FormattingEnabled = true;
            this.checkedListBoxMonthlyDays.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31"});
            // Last Day option not supported in Quartz.net 1.2.0.0
            //"Last Day"});
            this.checkedListBoxMonthlyDays.Location = new System.Drawing.Point(8, 8);
            this.checkedListBoxMonthlyDays.Name = "checkedListBoxMonthlyDays";
            this.checkedListBoxMonthlyDays.Size = new System.Drawing.Size(229, 109);
            this.inboxControlStyler1.SetStyleSettings(this.checkedListBoxMonthlyDays, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.checkedListBoxMonthlyDays.TabIndex = 29;
            // 
            // checkedListBoxMonthlyWeekNumber
            // 
            this.checkedListBoxMonthlyWeekNumber.CheckOnClick = true;
            this.checkedListBoxMonthlyWeekNumber.FormattingEnabled = true;
            this.checkedListBoxMonthlyWeekNumber.Items.AddRange(new object[] {
            "First",
            "Second",
            "Third",
            "Fourth",
            "Fifth"});
            // Last option not supported in Quartz.net 1.2.0.0
            //"Last"});
            this.checkedListBoxMonthlyWeekNumber.Location = new System.Drawing.Point(8, 8);
            this.checkedListBoxMonthlyWeekNumber.Name = "checkedListBoxMonthlyWeekNumber";
            this.checkedListBoxMonthlyWeekNumber.Size = new System.Drawing.Size(120, 79);
            this.inboxControlStyler1.SetStyleSettings(this.checkedListBoxMonthlyWeekNumber, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.checkedListBoxMonthlyWeekNumber.TabIndex = 33;
            this.checkedListBoxMonthlyWeekNumber.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxMonthlyWeekNumber_ItemCheck);
            // 
            // checkedListBoxMonthlyWeekDay
            // 
            this.checkedListBoxMonthlyWeekDay.CheckOnClick = true;
            this.checkedListBoxMonthlyWeekDay.FormattingEnabled = true;
            this.checkedListBoxMonthlyWeekDay.Items.AddRange(new object[] {
            "Sunday",
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday"});
            this.checkedListBoxMonthlyWeekDay.Location = new System.Drawing.Point(132, 8);
            this.checkedListBoxMonthlyWeekDay.Name = "checkedListBoxMonthlyWeekDay";
            this.checkedListBoxMonthlyWeekDay.Size = new System.Drawing.Size(104, 109);
            this.inboxControlStyler1.SetStyleSettings(this.checkedListBoxMonthlyWeekDay, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.checkedListBoxMonthlyWeekDay.TabIndex = 34;
            this.checkedListBoxMonthlyWeekDay.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxMonthlyWeekDay_ItemCheck);
            // 
            // labelWeeklyDays
            // 
            this.labelWeeklyDays.AutoSize = true;
            this.labelWeeklyDays.Location = new System.Drawing.Point(15, 16);
            this.labelWeeklyDays.Name = "labelWeeklyDays";
            this.labelWeeklyDays.Size = new System.Drawing.Size(34, 13);
            this.inboxControlStyler1.SetStyleSettings(this.labelWeeklyDays, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.labelWeeklyDays.TabIndex = 28;
            this.labelWeeklyDays.Text = "Days:";
            // 
            // tabControlMonthlyMode
            // 
            this.tabControlMonthlyMode.Controls.Add(this.tabPageMonthlyDayOfMonth);
            this.tabControlMonthlyMode.Controls.Add(this.tabPageMonthlyWeekDay);
            this.tabControlMonthlyMode.Location = new System.Drawing.Point(190, 19);
            this.tabControlMonthlyMode.Name = "tabControlMonthlyMode";
            this.tabControlMonthlyMode.SelectedIndex = 0;
            this.tabControlMonthlyMode.Size = new System.Drawing.Size(251, 154);
            this.inboxControlStyler1.SetStyleSettings(this.tabControlMonthlyMode, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.tabControlMonthlyMode.TabIndex = 30;
            // 
            // tabPageMonthlyDayOfMonth
            // 
            this.tabPageMonthlyDayOfMonth.Controls.Add(this.checkedListBoxMonthlyDays);
            this.tabPageMonthlyDayOfMonth.Location = new System.Drawing.Point(4, 22);
            this.tabPageMonthlyDayOfMonth.Name = "tabPageMonthlyDayOfMonth";
            this.tabPageMonthlyDayOfMonth.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMonthlyDayOfMonth.Size = new System.Drawing.Size(243, 128);
            this.inboxControlStyler1.SetStyleSettings(this.tabPageMonthlyDayOfMonth, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.tabPageMonthlyDayOfMonth.TabIndex = 0;
            this.tabPageMonthlyDayOfMonth.Text = "Day of Month";
            this.tabPageMonthlyDayOfMonth.UseVisualStyleBackColor = true;
            // 
            // tabPageMonthlyWeekDay
            // 
            this.tabPageMonthlyWeekDay.Controls.Add(this.checkedListBoxMonthlyWeekNumber);
            this.tabPageMonthlyWeekDay.Controls.Add(this.checkedListBoxMonthlyWeekDay);
            this.tabPageMonthlyWeekDay.Location = new System.Drawing.Point(4, 22);
            this.tabPageMonthlyWeekDay.Name = "tabPageMonthlyWeekDay";
            this.tabPageMonthlyWeekDay.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMonthlyWeekDay.Size = new System.Drawing.Size(243, 128);
            this.inboxControlStyler1.SetStyleSettings(this.tabPageMonthlyWeekDay, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.tabPageMonthlyWeekDay.TabIndex = 1;
            this.tabPageMonthlyWeekDay.Text = "Weekday";
            this.tabPageMonthlyWeekDay.UseVisualStyleBackColor = true;
            // 
            // labelMonthlyMonth
            // 
            this.labelMonthlyMonth.AutoSize = true;
            this.labelMonthlyMonth.Location = new System.Drawing.Point(15, 22);
            this.labelMonthlyMonth.Name = "labelMonthlyMonth";
            this.labelMonthlyMonth.Size = new System.Drawing.Size(40, 13);
            this.inboxControlStyler1.SetStyleSettings(this.labelMonthlyMonth, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.labelMonthlyMonth.TabIndex = 29;
            this.labelMonthlyMonth.Text = "Month:";
            // 
            // labelStartDate
            // 
            this.labelStartDate.AutoSize = true;
            this.labelStartDate.Location = new System.Drawing.Point(3, 7);
            this.labelStartDate.Name = "labelStartDate";
            this.labelStartDate.Size = new System.Drawing.Size(32, 13);
            this.inboxControlStyler1.SetStyleSettings(this.labelStartDate, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.labelStartDate.TabIndex = 36;
            this.labelStartDate.Text = "Start:";
            // 
            // dateTimePickerStartDate
            // 
            this.dateTimePickerStartDate.Location = new System.Drawing.Point(41, 3);
            this.dateTimePickerStartDate.Name = "dateTimePickerStartDate";
            this.dateTimePickerStartDate.Size = new System.Drawing.Size(200, 20);
            this.inboxControlStyler1.SetStyleSettings(this.dateTimePickerStartDate, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.dateTimePickerStartDate.TabIndex = 37;
            // 
            // dateTimePickerTriggerTime
            // 
            this.dateTimePickerTriggerTime.CustomFormat = "";
            this.dateTimePickerTriggerTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePickerTriggerTime.Location = new System.Drawing.Point(247, 3);
            this.dateTimePickerTriggerTime.Name = "dateTimePickerTriggerTime";
            this.dateTimePickerTriggerTime.ShowUpDown = true;
            this.dateTimePickerTriggerTime.Size = new System.Drawing.Size(96, 20);
            this.inboxControlStyler1.SetStyleSettings(this.dateTimePickerTriggerTime, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.dateTimePickerTriggerTime.TabIndex = 55;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.splitContainer1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(605, 270);
            this.inboxControlStyler1.SetStyleSettings(this.groupBox2, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.groupBox2.TabIndex = 57;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Schedule Task";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 16);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.rbMonthly);
            this.splitContainer1.Panel1.Controls.Add(this.rbWeekly);
            this.splitContainer1.Panel1.Controls.Add(this.rbDaily);
            this.splitContainer1.Panel1.Controls.Add(this.rbOneTime);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBoxMonthly);
            this.splitContainer1.Panel2.Controls.Add(this.groupBoxWeekly);
            this.splitContainer1.Panel2.Controls.Add(this.groupBoxDaily);
            this.splitContainer1.Panel2.Controls.Add(this.dateTimePickerStartDate);
            this.splitContainer1.Panel2.Controls.Add(this.labelStartDate);
            this.splitContainer1.Panel2.Controls.Add(this.dateTimePickerTriggerTime);
            this.splitContainer1.Size = new System.Drawing.Size(599, 251);
            this.splitContainer1.SplitterDistance = 131;
            this.inboxControlStyler1.SetStyleSettings(this.splitContainer1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.splitContainer1.TabIndex = 0;
            // 
            // rbMonthly
            // 
            this.rbMonthly.AutoSize = true;
            this.rbMonthly.Location = new System.Drawing.Point(9, 79);
            this.rbMonthly.Name = "rbMonthly";
            this.rbMonthly.Size = new System.Drawing.Size(62, 17);
            this.inboxControlStyler1.SetStyleSettings(this.rbMonthly, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.rbMonthly.TabIndex = 3;
            this.rbMonthly.TabStop = true;
            this.rbMonthly.Text = "Monthly";
            this.rbMonthly.UseVisualStyleBackColor = true;
            this.rbMonthly.CheckedChanged += new System.EventHandler(this.rbMonthly_CheckedChanged);
            // 
            // rbWeekly
            // 
            this.rbWeekly.AutoSize = true;
            this.rbWeekly.Location = new System.Drawing.Point(9, 55);
            this.rbWeekly.Name = "rbWeekly";
            this.rbWeekly.Size = new System.Drawing.Size(61, 17);
            this.inboxControlStyler1.SetStyleSettings(this.rbWeekly, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.rbWeekly.TabIndex = 2;
            this.rbWeekly.TabStop = true;
            this.rbWeekly.Text = "Weekly";
            this.rbWeekly.UseVisualStyleBackColor = true;
            this.rbWeekly.CheckedChanged += new System.EventHandler(this.rbWeekly_CheckedChanged);
            // 
            // rbDaily
            // 
            this.rbDaily.AutoSize = true;
            this.rbDaily.Location = new System.Drawing.Point(9, 31);
            this.rbDaily.Name = "rbDaily";
            this.rbDaily.Size = new System.Drawing.Size(48, 17);
            this.inboxControlStyler1.SetStyleSettings(this.rbDaily, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.rbDaily.TabIndex = 1;
            this.rbDaily.TabStop = true;
            this.rbDaily.Text = "Daily";
            this.rbDaily.UseVisualStyleBackColor = true;
            this.rbDaily.CheckedChanged += new System.EventHandler(this.rbDaily_CheckedChanged);
            // 
            // rbOneTime
            // 
            this.rbOneTime.AutoSize = true;
            this.rbOneTime.Location = new System.Drawing.Point(9, 7);
            this.rbOneTime.Name = "rbOneTime";
            this.rbOneTime.Size = new System.Drawing.Size(67, 17);
            this.inboxControlStyler1.SetStyleSettings(this.rbOneTime, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.rbOneTime.TabIndex = 0;
            this.rbOneTime.TabStop = true;
            this.rbOneTime.Text = "One time";
            this.rbOneTime.UseVisualStyleBackColor = true;
            this.rbOneTime.CheckedChanged += new System.EventHandler(this.rbOneTime_CheckedChanged);
            // 
            // groupBoxMonthly
            // 
            this.groupBoxMonthly.Controls.Add(this.tabControlMonthlyMode);
            this.groupBoxMonthly.Controls.Add(this.checkedListBoxMonthlyMonths);
            this.groupBoxMonthly.Controls.Add(this.labelMonthlyMonth);
            this.groupBoxMonthly.Location = new System.Drawing.Point(6, 29);
            this.groupBoxMonthly.Name = "groupBoxMonthly";
            this.groupBoxMonthly.Size = new System.Drawing.Size(450, 177);
            this.inboxControlStyler1.SetStyleSettings(this.groupBoxMonthly, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.groupBoxMonthly.TabIndex = 58;
            this.groupBoxMonthly.TabStop = false;
            this.groupBoxMonthly.Visible = false;
            // 
            // groupBoxWeekly
            // 
            this.groupBoxWeekly.AutoSize = true;
            this.groupBoxWeekly.Controls.Add(this.checkedListBoxWeeklyDays);
            this.groupBoxWeekly.Controls.Add(this.labelWeeklyDays);
            this.groupBoxWeekly.Location = new System.Drawing.Point(6, 29);
            this.groupBoxWeekly.Name = "groupBoxWeekly";
            this.groupBoxWeekly.Size = new System.Drawing.Size(450, 177);
            this.inboxControlStyler1.SetStyleSettings(this.groupBoxWeekly, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.groupBoxWeekly.TabIndex = 57;
            this.groupBoxWeekly.TabStop = false;
            this.groupBoxWeekly.Visible = false;
            // 
            // groupBoxDaily
            // 
            this.groupBoxDaily.AutoSize = true;
            this.groupBoxDaily.Controls.Add(this.labelDailyDay);
            this.groupBoxDaily.Controls.Add(this.labelDailyEvery);
            this.groupBoxDaily.Controls.Add(this.numericUpDownDaily);
            this.groupBoxDaily.Location = new System.Drawing.Point(6, 29);
            this.groupBoxDaily.Name = "groupBoxDaily";
            this.groupBoxDaily.Size = new System.Drawing.Size(450, 63);
            this.inboxControlStyler1.SetStyleSettings(this.groupBoxDaily, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.groupBoxDaily.TabIndex = 56;
            this.groupBoxDaily.TabStop = false;
            this.groupBoxDaily.Visible = false;
            // 
            // TaskScheduler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.groupBox2);
            this.MinimumSize = new System.Drawing.Size(605, 270);
            this.Name = "TaskScheduler";
            this.Size = new System.Drawing.Size(605, 270);
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDaily)).EndInit();
            this.tabControlMonthlyMode.ResumeLayout(false);
            this.tabPageMonthlyDayOfMonth.ResumeLayout(false);
            this.tabPageMonthlyWeekDay.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBoxMonthly.ResumeLayout(false);
            this.groupBoxMonthly.PerformLayout();
            this.groupBoxWeekly.ResumeLayout(false);
            this.groupBoxWeekly.PerformLayout();
            this.groupBoxDaily.ResumeLayout(false);
            this.groupBoxDaily.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numericUpDownDaily;
        private System.Windows.Forms.Label labelDailyEvery;
        private System.Windows.Forms.Label labelDailyDay;
        private System.Windows.Forms.CheckedListBox checkedListBoxWeeklyDays;
        private System.Windows.Forms.CheckedListBox checkedListBoxMonthlyMonths;
        private System.Windows.Forms.CheckedListBox checkedListBoxMonthlyDays;
        private System.Windows.Forms.CheckedListBox checkedListBoxMonthlyWeekNumber;
        private System.Windows.Forms.CheckedListBox checkedListBoxMonthlyWeekDay;
        private System.Windows.Forms.Label labelWeeklyDays;
        private System.Windows.Forms.Label labelMonthlyMonth;
        private System.Windows.Forms.TabControl tabControlMonthlyMode;
        private System.Windows.Forms.TabPage tabPageMonthlyDayOfMonth;
        private System.Windows.Forms.TabPage tabPageMonthlyWeekDay;
        private System.Windows.Forms.Label labelStartDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerStartDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerTriggerTime;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBoxMonthly;
        private System.Windows.Forms.GroupBox groupBoxWeekly;
        private System.Windows.Forms.GroupBox groupBoxDaily;
        private System.Windows.Forms.RadioButton rbMonthly;
        private System.Windows.Forms.RadioButton rbWeekly;
        private System.Windows.Forms.RadioButton rbDaily;
        private System.Windows.Forms.RadioButton rbOneTime;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
    }
}
