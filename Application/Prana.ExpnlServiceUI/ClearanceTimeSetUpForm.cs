using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace Prana.ExpnlServiceUI
{
    public partial class ClearanceTimeSetUpForm : Form
    {
        private BusinessObjects.TimeZone _currentBaseTimeZone;
        private Dictionary<int, MarketTimes> _marketStartEndTimes;
        private bool _moveClearanceTimesTogether;

        public ClearanceTimeSetUpForm()
        {
            try
            {
                InitializeComponent();

                _moveClearanceTimesTogether = true;
                InitControl();
                timeSlider1.ValueChanged += new EventHandler(timeSlider1_ValueChanged);
                ultraTimeZoneEditor2.ValueChanged += new EventHandler(ultraTimeZoneEditor2_ValueChanged);
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

        public delegate void ClearanceTimeUpdated(Dictionary<int, DateTime> updatedclearanceTimeDictionary);
        public event EventHandler<EventArgs<Dictionary<int, DateTime>>> ClearanceUpdated;

        public async System.Threading.Tasks.Task DrawFromDBClearanceTimes(Dictionary<int, DateTime> clearanceTime)
        {
            try
            {
                _marketStartEndTimes = await ExpnlServiceManager.ExpnlServiceManager.GetInstance.GetMarketStartEndTime();

                TimeZoneAndTime timeZoneAndTime = await ExpnlServiceManager.ExpnlServiceManager.GetInstance.GetBaseTimeZoneAndBaseTimeZoneTime();
                if (!String.IsNullOrEmpty(timeZoneAndTime.TimeZone))
                {
                    _currentBaseTimeZone = Prana.BusinessObjects.TimeZoneInfo.FindTimeZone(timeZoneAndTime.TimeZone);
                }
                else
                {
                    _currentBaseTimeZone = Prana.BusinessObjects.TimeZoneInfo.FindTimeZoneByStandardName(System.TimeZoneInfo.Local.StandardName);
                }

                if (_currentBaseTimeZone != null)
                {
                    ultraTimeZoneEditor2.Text = _currentBaseTimeZone.DisplayName;
                }

                if (timeZoneAndTime.BaseTime != DateTimeConstants.MinValue)
                {
                    DateTime currentDayClearanceTime = DateTime.Now.Date + timeZoneAndTime.BaseTime.TimeOfDay;
                    timeSlider1.Value = currentDayClearanceTime;
                    timeSlider1.ShowSegment = false;
                }

                await SetUIForDisplay();
                if (clearanceTime == null)
                {
                    clearanceTime = await ExpnlServiceManager.ExpnlServiceManager.GetInstance.FetchClearanceTime();
                }

                foreach (Control timeSlider in tableLayoutPanel1.Controls)
                {
                    if (_marketStartEndTimes.ContainsKey(((TimerSchedulerControl)timeSlider).AUECID))
                    {
                        ((TimerSchedulerControl)timeSlider).MarketStartTime = _marketStartEndTimes[((TimerSchedulerControl)timeSlider).AUECID].MarketStartTime;
                        ((TimerSchedulerControl)timeSlider).MarketEndTime = _marketStartEndTimes[((TimerSchedulerControl)timeSlider).AUECID].MarketEndTime;
                        ((TimerSchedulerControl)timeSlider).SetTimeSliderColor();
                    }
                    if (clearanceTime.ContainsKey(((TimerSchedulerControl)timeSlider).AUECID))
                    {
                        DateTime currentAUECClearanceTime = DateTime.Now.Date + clearanceTime[((TimerSchedulerControl)timeSlider).AUECID].TimeOfDay;
                        await ((TimerSchedulerControl)timeSlider).SetTime(currentAUECClearanceTime);
                    }
                }
                tableLayoutPanel1.Refresh();
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateClearanceTimeSelected())
                {
                    Dictionary<int, DateTime> updatedClearanceDict = new Dictionary<int, DateTime>(tableLayoutPanel1.Controls.Count);

                    DataTable clearanceTable = new DataTable("ClearanceTable");
                    clearanceTable.Columns.Add("AUECID", typeof(int));
                    clearanceTable.Columns.Add("ClearanceTime", typeof(string));
                    foreach (Control sliderControl in tableLayoutPanel1.Controls)
                    {
                        DataRow row = clearanceTable.NewRow();
                        int auecID = ((TimerSchedulerControl)sliderControl).AUECID;
                        DateTime clearanceTime = ((TimerSchedulerControl)sliderControl).GetAUECLocalTime();
                        row[0] = auecID;
                        row[1] = clearanceTime.ToString();
                        clearanceTable.Rows.Add(row);
                        updatedClearanceDict.Add(auecID, clearanceTime);
                    }
                    await ExpnlServiceManager.ExpnlServiceManager.GetInstance.SaveBaseTimeZoneAndBaseTimeZoneTime(this.ultraTimeZoneEditor2.Text, timeSlider1.Value);
                    await ExpnlServiceManager.ExpnlServiceManager.GetInstance.SaveClearanceTime(clearanceTable);
                    clearanceTable = null;
                    if (ClearanceUpdated != null)
                    {
                        ClearanceUpdated(this, new EventArgs<Dictionary<int, DateTime>>(updatedClearanceDict));
                    }
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Problem saving Clearance Times in DB", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _moveClearanceTimesTogether = checkBox1.Checked;
        }

        private void ClearanceTimeSetUpForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.Focus();
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

        private void InitControl()
        {
            try
            {
                timeSlider1.Minimum = DateTime.Now.Date;
                timeSlider1.Maximum = DateTime.Now.Date.AddDays(1);
                ultraTimeZoneEditor2.CausesValidation = false;
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

        private async System.Threading.Tasks.Task SetUIForDisplay()
        {
            try
            {
                if (_currentBaseTimeZone != null)
                {
                    ultraTimeZoneEditor2.Text = _currentBaseTimeZone.DisplayName;
                    tableLayoutPanel1.RowCount = 0;
                    tableLayoutPanel1.RowCount = _marketStartEndTimes.Count;

                    int rowNumber = 0;
                    //Set sorted Data on display (Jira no. PRANA-5400) Date-15/01/2015
                    Dictionary<int, Prana.BusinessObjects.TimeZone> sortedAUECTimeZones = await ExpnlServiceManager.ExpnlServiceManager.GetInstance.GetAllAUECTimeZones();

                    foreach (KeyValuePair<int, Prana.BusinessObjects.TimeZone> auecIDTimeZone in sortedAUECTimeZones)
                    {
                        if (_marketStartEndTimes.ContainsKey(auecIDTimeZone.Key))
                        {
                            TimerSchedulerControl controlForTime = new TimerSchedulerControl();
                            controlForTime.AUECID = auecIDTimeZone.Key;
                            controlForTime.SelectedBaseTimeZone = this._currentBaseTimeZone;
                            controlForTime.AUECName = await ExpnlServiceManager.ExpnlServiceManager.GetInstance.GetAUECText(auecIDTimeZone.Key);
                            tableLayoutPanel1.Controls.Add(controlForTime, 0, rowNumber);
                            rowNumber++;
                        }
                    }
                    lblBaseAUECSelectedTime.Text = timeSlider1.Value.ToString("HH:mm");
                }
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

        private async void timeSlider1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime selectedTimeInBaseTimeZone = ((TimeSlider)sender).Value;
                lblBaseAUECSelectedTime.Text = selectedTimeInBaseTimeZone.ToString("HH:mm");

                if (_moveClearanceTimesTogether)
                {
                    DateTime selectedTimeInUTC = Prana.BusinessObjects.TimeZoneInfo.ConvertLocalTimeToUTC(selectedTimeInBaseTimeZone, _currentBaseTimeZone);
                    foreach (Control var in tableLayoutPanel1.Controls)
                    {
                        await ((Prana.ExpnlServiceUI.TimerSchedulerControl)var).SetTime(selectedTimeInBaseTimeZone, selectedTimeInUTC);
                    }
                }
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

        private void ultraTimeZoneEditor2_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                _currentBaseTimeZone = Prana.BusinessObjects.TimeZoneInfo.FindTimeZone(ultraTimeZoneEditor2.Text);

                if (_currentBaseTimeZone == null)
                {
                    _currentBaseTimeZone = Prana.BusinessObjects.TimeZoneInfo.FindTimeZoneByDayLightName(ultraTimeZoneEditor2.Text);
                }
                if (_currentBaseTimeZone == null)
                {
                    _currentBaseTimeZone = Prana.BusinessObjects.TimeZoneInfo.FindTimeZoneByStandardName(ultraTimeZoneEditor2.Text);
                }
                if (_currentBaseTimeZone != null)
                {
                    foreach (Control sliderControl in tableLayoutPanel1.Controls)
                    {
                        ((TimerSchedulerControl)sliderControl).SelectedBaseTimeZone = _currentBaseTimeZone;
                    }
                }
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

        private bool ValidateClearanceTimeSelected()
        {
            bool isValid = true;
            try
            {
                StringBuilder errorString = new StringBuilder("Clearance time lies during regular market hours! Please check these AUECs");
                errorString.AppendLine("");

                foreach (Control sliderControl in tableLayoutPanel1.Controls)
                {
                    if (((TimerSchedulerControl)sliderControl).IsclearanceTimeOutsideMarketHours())
                    {
                    }
                    else
                    {
                        isValid = false;
                        errorString.Append(((TimerSchedulerControl)sliderControl).AUECName);
                        errorString.AppendLine(",");
                    }
                }
                if (!isValid)
                {
                    DialogResult userChoice = DialogResult.OK;
                    errorString.AppendLine("Do you want to continue ?");
                    userChoice = MessageBox.Show(errorString.ToString(), "Critical Warning !", MessageBoxButtons.YesNo);
                    if (userChoice == DialogResult.Yes)
                    {
                        isValid = true;
                    }
                    else
                    {
                        isValid = false;
                    }
                }
                return isValid;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isValid;
        }
    }
}