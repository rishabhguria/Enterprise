using Prana.LogManager;
using Prana.Utilities.UI.CronUtility;
using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;

namespace Prana.Utilities.UI.UIUtilities
{
    /// <summary>
    /// class to schedule task according to cron expression
    /// </summary>
    public partial class TaskScheduler : UserControl
    {
        /// <summary>
        /// Constructor to initialize UI components and start date
        /// </summary>
        public TaskScheduler()
        {
            InitializeComponent();
            dateTimePickerStartDate.Value = DateTime.Today;
        }

        /// <summary>
        /// Applying Black Gray Theme
        /// </summary>
        public void ApplyTheme()
        {
            try
            {
                this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));

                this.splitContainer1.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.splitContainer1.Panel1.ForeColor = System.Drawing.Color.White;
                this.splitContainer1.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
                this.splitContainer1.Panel2.ForeColor = System.Drawing.Color.White;

                this.groupBoxMonthly.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.groupBoxMonthly.ForeColor = System.Drawing.Color.White;

                this.groupBoxDaily.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.groupBoxDaily.ForeColor = System.Drawing.Color.White;

                this.groupBoxWeekly.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.groupBoxWeekly.ForeColor = System.Drawing.Color.White;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Radiobutton Event called on selection of one time schedule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbOneTime_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                clear();
                groupBoxDaily.Visible = false;
                groupBoxWeekly.Visible = false;
                groupBoxMonthly.Visible = false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Radiobutton Event called on selection of daily schedule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbDaily_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                clear();
                groupBoxDaily.Visible = true;
                groupBoxWeekly.Visible = false;
                groupBoxMonthly.Visible = false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Radiobutton Event called on selection of weekly schedule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbWeekly_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                clear();
                groupBoxWeekly.Visible = true;
                groupBoxDaily.Visible = false;
                groupBoxMonthly.Visible = false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Radiobutton Event called on selection of monthly schedule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbMonthly_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                clear();
                groupBoxMonthly.Visible = true;
                groupBoxDaily.Visible = false;
                groupBoxWeekly.Visible = false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// Function to build cron expression of 6 fields with format (seconds, minutes, hours, dayOfMonth, month, dayOfWeek)
        /// </summary>
        /// <returns>cron string</returns>
        public String GetCronExpression()
        {
            try
            {
                StringBuilder sbCronExp = new StringBuilder();
                string seconds = string.Empty;
                string minutes = string.Empty;
                string hours = string.Empty;
                string daysOfMonth = string.Empty;
                string month = string.Empty;
                string daysOfWeek = string.Empty;
                int choiceType = 0;

                seconds = dateTimePickerTriggerTime.Value.Second.ToString();
                minutes = dateTimePickerTriggerTime.Value.Minute.ToString();
                hours = dateTimePickerTriggerTime.Value.Hour.ToString();

                if (rbOneTime.Checked == true)
                    choiceType = 1;
                else if (rbDaily.Checked == true)
                    choiceType = 2;
                else if (rbWeekly.Checked == true)
                    choiceType = 3;
                else if (rbMonthly.Checked == true)
                    choiceType = 4;

                switch (choiceType)
                {
                    // For One time task Scheduling
                    case 1:
                        month = dateTimePickerStartDate.Value.Month.ToString();
                        //daysOfWeek = dateTimePickerStartDate.Value.DayOfWeek.ToString().Substring(0, 3).ToUpper();
                        daysOfWeek = "?";
                        daysOfMonth = dateTimePickerStartDate.Value.Day.ToString();

                        sbCronExp.AppendFormat("{0} {1} {2} {3} {4} {5}", seconds, minutes, hours, daysOfMonth, month, daysOfWeek);
                        break;
                    // For Daily task Scheduling
                    case 2:
                        daysOfMonth = "1/" + numericUpDownDaily.Value.ToString();
                        month = "*";
                        daysOfWeek = "?";

                        sbCronExp.AppendFormat("{0} {1} {2} {3} {4} {5}", seconds, minutes, hours, daysOfMonth, month, daysOfWeek);
                        break;

                    // For weekly task Scheduling
                    case 3:
                        StringBuilder s = new StringBuilder();
                        daysOfMonth = "?";
                        month = "*";
                        if (checkedListBoxWeeklyDays.CheckedItems.Count != 0)
                        {
                            for (int x = 0; x <= checkedListBoxWeeklyDays.CheckedItems.Count - 1; x++)
                            {
                                s = s.Append(checkedListBoxWeeklyDays.CheckedItems[x].ToString().Substring(0, 3).ToUpper() + ",");
                            }
                            s.Length = s.Length - 1;
                            daysOfWeek = s.ToString();
                        }
                        else
                            daysOfWeek = "*";

                        sbCronExp.AppendFormat("{0} {1} {2} {3} {4} {5}", seconds, minutes, hours, daysOfMonth, month, daysOfWeek);
                        break;

                    // For monthly task Scheduling

                    // Note : Multiple day of week and weeday combination not supported in Quartz.net 1.2.0.0
                    // Last day not supported in Quartz.net 1.2.0.0 with other days.
                    case 4:
                        StringBuilder sbMonth = new StringBuilder();
                        StringBuilder sbMonthlyDays = new StringBuilder();
                        StringBuilder sbMonthlyWeekNo = new StringBuilder();
                        ArrayList strDayofWeek1 = new ArrayList();
                        ArrayList strDayofWeek2 = new ArrayList();

                        if (checkedListBoxMonthlyMonths.CheckedItems.Count != 0)
                        {
                            for (int x = 0; x <= checkedListBoxMonthlyMonths.CheckedItems.Count - 1; x++)
                            {
                                sbMonth = sbMonth.Append(checkedListBoxMonthlyMonths.CheckedItems[x].ToString().Substring(0, 3).ToUpper() + ",");
                            }
                            sbMonth.Length = sbMonth.Length - 1;
                            month = sbMonth.ToString();
                        }
                        else
                            month = "*";

                        if (tabControlMonthlyMode.SelectedTab.Text == "Day of Month")
                        {
                            if (checkedListBoxMonthlyDays.CheckedItems.Count != 0)
                            {
                                for (int x = 0; x <= checkedListBoxMonthlyDays.CheckedItems.Count - 1; x++)
                                {
                                    if (checkedListBoxMonthlyDays.CheckedItems[x].ToString() == "Last Day")
                                        sbMonthlyDays = sbMonthlyDays.Append("L" + ",");
                                    else
                                        sbMonthlyDays = sbMonthlyDays.Append(checkedListBoxMonthlyDays.CheckedItems[x].ToString() + ",");
                                }
                                sbMonthlyDays.Length = sbMonthlyDays.Length - 1;
                                daysOfMonth = sbMonthlyDays.ToString();
                                daysOfWeek = "?";
                            }
                            else
                            {
                                daysOfMonth = "?";
                                daysOfWeek = "*";
                            }
                        }
                        else
                        {
                            if (checkedListBoxMonthlyWeekNumber.CheckedItems.Count != 0)
                            {
                                for (int x = 0; x <= checkedListBoxMonthlyWeekNumber.CheckedItems.Count - 1; x++)
                                {
                                    if (checkedListBoxMonthlyWeekNumber.CheckedItems[x].ToString() == "First")
                                        strDayofWeek1.Add("1");
                                    if (checkedListBoxMonthlyWeekNumber.CheckedItems[x].ToString() == "Second")
                                        strDayofWeek1.Add("2");
                                    if (checkedListBoxMonthlyWeekNumber.CheckedItems[x].ToString() == "Third")
                                        strDayofWeek1.Add("3");
                                    if (checkedListBoxMonthlyWeekNumber.CheckedItems[x].ToString() == "Fourth")
                                        strDayofWeek1.Add("4");
                                    if (checkedListBoxMonthlyWeekNumber.CheckedItems[x].ToString() == "Fifth")
                                        // Last option not supported in Quartz.net 1.2.0.0
                                        //if (checkedListBoxMonthlyWeekNumber.CheckedItems[x].ToString() == "Last")
                                        strDayofWeek1.Add("5");
                                }
                            }
                            else
                                strDayofWeek1.Add("*");

                            if (checkedListBoxMonthlyWeekDay.CheckedItems.Count != 0)
                            {
                                for (int x = 0; x <= checkedListBoxMonthlyWeekDay.CheckedItems.Count - 1; x++)
                                {
                                    if (checkedListBoxMonthlyWeekDay.CheckedItems[x].ToString() == "Sunday")
                                        strDayofWeek2.Add("1");
                                    if (checkedListBoxMonthlyWeekDay.CheckedItems[x].ToString() == "Monday")
                                        strDayofWeek2.Add("2");
                                    if (checkedListBoxMonthlyWeekDay.CheckedItems[x].ToString() == "Tuesday")
                                        strDayofWeek2.Add("3");
                                    if (checkedListBoxMonthlyWeekDay.CheckedItems[x].ToString() == "Wednesday")
                                        strDayofWeek2.Add("4");
                                    if (checkedListBoxMonthlyWeekDay.CheckedItems[x].ToString() == "Thursday")
                                        strDayofWeek2.Add("5");
                                    if (checkedListBoxMonthlyWeekDay.CheckedItems[x].ToString() == "Friday")
                                        strDayofWeek2.Add("6");
                                    if (checkedListBoxMonthlyWeekDay.CheckedItems[x].ToString() == "Saturday")
                                        strDayofWeek2.Add("7");
                                }
                            }
                            else
                                strDayofWeek2.Add("*");

                            foreach (string str1 in strDayofWeek1)
                            {
                                foreach (string str2 in strDayofWeek2)
                                {
                                    sbMonthlyWeekNo = sbMonthlyWeekNo.Append(str2 + "#" + str1 + ",");
                                }
                            }
                            sbMonthlyWeekNo.Length = sbMonthlyWeekNo.Length - 1;
                            daysOfWeek = sbMonthlyWeekNo.ToString();
                            daysOfMonth = "?";
                        }
                        sbCronExp.AppendFormat("{0} {1} {2} {3} {4} {5}", seconds, minutes, hours, daysOfMonth, month, daysOfWeek);
                        break;
                }
                return sbCronExp.ToString();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return String.Empty;
            }
        }

        /// <summary>
        /// Function to fill UI according to passed cron Expression
        /// </summary>
        public void FillUI(string cronExpression)
        {
            try
            {
                clear();

                if (String.IsNullOrEmpty(cronExpression))
                    return;
                CronDescription objUIDesc = CronUtility.CronUtility.GetCronDescriptionObject(cronExpression);

                dateTimePickerTriggerTime.Value = objUIDesc.StartTime;

                // For Daily Schedule
                if (objUIDesc.Type == ScheduleType.Daily)
                {
                    rbDaily.Checked = true;
                    numericUpDownDaily.Value = objUIDesc.RecurDay;
                }

                // For One time Schedule
                else if (objUIDesc.Type == ScheduleType.OneTime)
                {
                    rbOneTime.Checked = true;
                    dateTimePickerStartDate.Value = objUIDesc.StartDate;
                }
                // For Weekly Schedule
                else if (objUIDesc.Type == ScheduleType.Weekly)
                {
                    string strWeek = "";
                    rbWeekly.Checked = true;
                    foreach (string item in objUIDesc.WeeklyDaysList)
                    {
                        if (item == "MON")
                            strWeek = "Monday";
                        else if (item == "TUE")
                            strWeek = "Tuesday";
                        else if (item == "WED")
                            strWeek = "Wednesday";
                        else if (item == "THU")
                            strWeek = "Thursday";
                        else if (item == "FRI")
                            strWeek = "Friday";
                        else if (item == "SAT")
                            strWeek = "Saturday";
                        else if (item == "SUN")
                            strWeek = "Sunday";
                        int index = checkedListBoxWeeklyDays.FindStringExact(strWeek);
                        if (index > -1)
                            checkedListBoxWeeklyDays.SetItemChecked(index, true);
                    }
                }
                // For Monthly Schedule
                else
                {
                    string strMonth = "";
                    rbMonthly.Checked = true;
                    foreach (string item in objUIDesc.MonthlyMonthsList)
                    {
                        if (item == "JAN")
                            strMonth = "January";
                        else if (item == "FEB")
                            strMonth = "February";
                        else if (item == "MAR")
                            strMonth = "March";
                        else if (item == "APR")
                            strMonth = "April";
                        else if (item == "MAY")
                            strMonth = "May";
                        else if (item == "JUN")
                            strMonth = "June";
                        else if (item == "JUL")
                            strMonth = "July";
                        else if (item == "AUG")
                            strMonth = "August";
                        else if (item == "SEP")
                            strMonth = "September";
                        else if (item == "OCT")
                            strMonth = "October";
                        else if (item == "NOV")
                            strMonth = "November";
                        else if (item == "DEC")
                            strMonth = "December";
                        int index = checkedListBoxMonthlyMonths.FindStringExact(strMonth);
                        if (index > -1)
                            checkedListBoxMonthlyMonths.SetItemChecked(index, true);
                    }
                    if (objUIDesc.MonthlyModeDayOfMonth)                        // Day of Month Tab
                    {
                        tabControlMonthlyMode.SelectTab("tabPageMonthlyDayOfMonth");
                        foreach (string item in objUIDesc.MonthlyDaysList)
                        {
                            int index = checkedListBoxMonthlyDays.FindStringExact(item);
                            if (index > -1)
                                checkedListBoxMonthlyDays.SetItemChecked(index, true);
                        }
                    }
                    else                                                         // WeekDay Tab
                    {
                        tabControlMonthlyMode.SelectTab("tabPageMonthlyWeekDay");

                        ArrayList strWeekNo = new ArrayList();
                        ArrayList strDay = new ArrayList();
                        foreach (string item in objUIDesc.MonthlyWeekNumbersList)
                        {
                            if (item == "1")
                                strWeekNo.Add("First");
                            else if (item == "2")
                                strWeekNo.Add("Second");
                            else if (item == "3")
                                strWeekNo.Add("Third");
                            else if (item == "4")
                                strWeekNo.Add("Fourth");
                            else if (item == "5")
                                strWeekNo.Add("Fifth");
                            // Last option not supported in Quartz.net 1.2.0.0
                            //strWeekNo.Add("Last");
                        }
                        foreach (string item in objUIDesc.MonthlyWeekDaysList)
                        {
                            if (item == "1")
                                strDay.Add("Sunday");
                            else if (item == "2")
                                strDay.Add("Monday");
                            else if (item == "3")
                                strDay.Add("Tuesday");
                            else if (item == "4")
                                strDay.Add("Wednesday");
                            else if (item == "5")
                                strDay.Add("Thursday");
                            else if (item == "6")
                                strDay.Add("Friday");
                            else if (item == "7")
                                strDay.Add("Saturday");
                        }
                        foreach (string item in strWeekNo)
                        {
                            int index = checkedListBoxMonthlyWeekNumber.FindStringExact(item);
                            if (index > -1)
                                checkedListBoxMonthlyWeekNumber.SetItemChecked(index, true);
                        }
                        foreach (string item in strDay)
                        {
                            int index = checkedListBoxMonthlyWeekDay.FindStringExact(item);
                            if (index > -1)
                                checkedListBoxMonthlyWeekDay.SetItemChecked(index, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Function to reset UI
        /// </summary>
        /// <param name="ctrl"></param> 
        private void clear()
        {
            try
            {
                numericUpDownDaily.Value = 1;
                CheckBoxListState(checkedListBoxWeeklyDays, false);
                CheckBoxListState(checkedListBoxMonthlyDays, false);
                CheckBoxListState(checkedListBoxMonthlyMonths, false);
                CheckBoxListState(checkedListBoxMonthlyWeekDay, false);
                CheckBoxListState(checkedListBoxMonthlyWeekNumber, false);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        ///  Function to clear CheckBoxList
        /// </summary>
        /// <param name="clbControl"></param>
        /// <param name="state"></param>
        private void CheckBoxListState(CheckedListBox clbControl, bool state)
        {
            try
            {
                // Currently whole checkBoxList iterates
                for (int x = 0; x <= clbControl.Items.Count - 1; x++)
                {
                    clbControl.SetItemChecked(x, state);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region Multiple day of week and weeday combination not supported in Quartz.net 1.2.0.0
        // Single selection of day of week and weeday implemented for Quartz.net 1.2.0.0
        /// <summary> 
        /// To select only single item in MonthlyWeekNumber checkedListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedListBoxMonthlyWeekNumber_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                if (e.NewValue == CheckState.Checked)
                    for (int ix = 0; ix < checkedListBoxMonthlyWeekNumber.Items.Count; ++ix)
                        if (e.Index != ix) checkedListBoxMonthlyWeekNumber.SetItemChecked(ix, false);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// To select only single item in MonthlyWeekDay checkedListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedListBoxMonthlyWeekDay_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                if (e.NewValue == CheckState.Checked)
                    for (int ix = 0; ix < checkedListBoxMonthlyWeekDay.Items.Count; ++ix)
                        if (e.Index != ix) checkedListBoxMonthlyWeekDay.SetItemChecked(ix, false);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion
    }
}
