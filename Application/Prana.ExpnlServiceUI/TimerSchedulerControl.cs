using Prana.LogManager;
using System;
using System.Windows.Forms;

namespace Prana.ExpnlServiceUI
{
    public partial class TimerSchedulerControl : UserControl
    {
        #region Properties
        private int _AUECID;
        public int AUECID
        {
            get { return _AUECID; }
            set { _AUECID = value; }
        }

        private string _AUECName = string.Empty;
        public string AUECName
        {
            get { return _AUECName; }
            set
            {
                _AUECName = value;
                label1.Text = _AUECName;
            }
        }

        private BusinessObjects.TimeZone _selectedbaseTimeZone;
        public BusinessObjects.TimeZone SelectedBaseTimeZone
        {
            get { return _selectedbaseTimeZone; }
            set { _selectedbaseTimeZone = value; }
        }

        private DateTime _marketStartTime;
        public DateTime MarketStartTime
        {
            get { return _marketStartTime; }
            set { _marketStartTime = value; }
        }

        private DateTime _marketEndTime;
        public DateTime MarketEndTime
        {
            get { return _marketEndTime; }
            set { _marketEndTime = value; }
        }
        #endregion

        public TimerSchedulerControl()
        {
            try
            {
                InitializeComponent();
                _marketStartTime = DateTime.Now.Date;
                _marketEndTime = DateTime.Now.Date.AddDays(1);
                slider.Minimum = DateTime.Now.Date;
                slider.Maximum = DateTime.Now.Date.AddDays(1);
                this.slider.ValueChanged += new EventHandler(slider_ValueChanged);
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

        private async void slider_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                label2.Text = slider.Value.ToString("HH:mm");
                DateTime utcTime = Prana.BusinessObjects.TimeZoneInfo.ConvertLocalTimeToUTC(slider.Value, await ExpnlServiceManager.ExpnlServiceManager.GetInstance.GetAUECTimeZone(_AUECID));
                label3.Text = utcTime.ToString("HH:mm");
                DateTime timeInSelectedBase = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(utcTime, _selectedbaseTimeZone);
                label4.Text = timeInSelectedBase.ToString("HH:mm");
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

        public async System.Threading.Tasks.Task SetTime(DateTime baseTime, DateTime utcTime)
        {
            try
            {
                this.slider.ValueChanged -= new EventHandler(slider_ValueChanged);
                DateTime auecLocalTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(utcTime, await ExpnlServiceManager.ExpnlServiceManager.GetInstance.GetAUECTimeZone(this._AUECID));
                slider.Value = DateTime.Now.Date.Add(auecLocalTime.TimeOfDay);

                label2.Text = auecLocalTime.ToString("HH:mm");
                label3.Text = utcTime.ToString("HH:mm");
                label4.Text = baseTime.ToString("HH:mm");
                this.slider.ValueChanged += new EventHandler(slider_ValueChanged);
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

        public async System.Threading.Tasks.Task SetTime(DateTime auecLocalTime)
        {
            try
            {
                this.slider.ValueChanged -= new EventHandler(slider_ValueChanged);
                label2.Text = auecLocalTime.ToString("HH:mm");

                DateTime utcTime = Prana.BusinessObjects.TimeZoneInfo.ConvertLocalTimeToUTC(auecLocalTime, await ExpnlServiceManager.ExpnlServiceManager.GetInstance.GetAUECTimeZone(_AUECID));
                label3.Text = utcTime.ToString("HH:mm");
                slider.Value = auecLocalTime;
                this.slider.ValueChanged += new EventHandler(slider_ValueChanged);
                DateTime baseTime = Prana.BusinessObjects.TimeZoneInfo.ConvertLocalTimeToUTC(utcTime, _selectedbaseTimeZone);
                label4.Text = baseTime.ToString("HH:mm");
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

        public DateTime GetAUECLocalTime()
        {
            try
            {
                return slider.Value;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return DateTime.MinValue;
            }
        }

        public void SetTimeSliderColor()
        {
            try
            {
                slider.SetSegment(_marketStartTime, _marketEndTime);
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

        public bool IsclearanceTimeOutsideMarketHours()
        {
            try
            {
                if (slider.Value.TimeOfDay > _marketStartTime.TimeOfDay && slider.Value.TimeOfDay <= _marketEndTime.TimeOfDay)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return true;
            }
        }
    }
}
