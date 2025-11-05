using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects.Compliance.Definition
{
    /// <summary>
    /// Properties for Notification Settings
    /// PopUpEnabled- Pop up for the alert to be shown or not.
    /// EmailEnabled- Email to be send or not.
    /// EmailToList- List of Email Id in TO
    /// EmailCCList- List of Email Id in CC
    /// LimitFrequencyMinutes- Sets frequency of alerts.
    /// StopAlertOnHolidays- We need alerts on holidays or not.
    /// StopAlertAfterMarketHours- Alerts at non market hours is required or not.
    /// </summary>
    public class NotificationSetting
    {
        private List<int> _popUpEnabledUsers = new List<int>();

        public List<int> PopUpEnabledUsers
        {
            get { return _popUpEnabledUsers; }
            set { _popUpEnabledUsers = value; }
        }

        public bool PopUpEnabled { get; set; }
        public bool EmailEnabled { get; set; }
        public String EmailToList { get; set; }
        public String EmailCCList { get; set; }
        public int LimitFrequencyMinutes { get; set; }
        public bool StopAlertOnHolidays { get; set; }
        public bool AlertInTimeRange { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [all in one lot].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [all in one lot]; otherwise, <c>false</c>.
        /// </value>
        public bool SendInOneEmail { get; set; }

        /// <summary>
        /// The time slots for send alerts on these time like a batch.
        /// </summary>
        public List<DateTime> TimeSlots = new List<DateTime>();

        public String EmailSubject { get; set; }
        //public int WarningFrequencyMinutes { get; set; }
        //public bool ManualTradeEnabled { get; set; }
        //public bool SoundEnabled { get; set; }
        //public String SoundFilePath { get; set; }


        public NotificationSetting DeepClone()
        {
            NotificationSetting temp = new NotificationSetting();
            temp.EmailCCList = this.EmailCCList;
            temp.EmailEnabled = this.EmailEnabled;
            temp.EmailToList = this.EmailToList;
            temp.LimitFrequencyMinutes = this.LimitFrequencyMinutes;
            temp.PopUpEnabled = this.PopUpEnabled;
            temp.PopUpEnabledUsers = this.PopUpEnabledUsers;
            temp.AlertInTimeRange = this.AlertInTimeRange;
            temp.StopAlertOnHolidays = this.StopAlertOnHolidays;
            temp.StartTime = this.StartTime;
            temp.EndTime = this.EndTime;
            temp.SendInOneEmail = this.SendInOneEmail;
            temp.TimeSlots[0] = this.TimeSlots[0];
            temp.TimeSlots[1] = this.TimeSlots[1];
            temp.TimeSlots[2] = this.TimeSlots[2];
            temp.TimeSlots[3] = this.TimeSlots[3];
            temp.TimeSlots[4] = this.TimeSlots[4];
            temp.EmailSubject = this.EmailSubject;
            return temp;
        }


        public NotificationSetting()
        {
            this.EmailCCList = "";
            this.EmailEnabled = false;
            this.EmailToList = "";
            this.LimitFrequencyMinutes = 2;
            this.PopUpEnabled = true;
            this.PopUpEnabledUsers = new List<int>();
            this.StopAlertOnHolidays = true;
            this.AlertInTimeRange = false;
            this.StartTime = DateTime.Now;
            this.EndTime = DateTime.Now;
            this.SendInOneEmail = false;
            this.TimeSlots.Add(DateTimeConstants.MinValue);
            this.TimeSlots.Add(DateTimeConstants.MinValue);
            this.TimeSlots.Add(DateTimeConstants.MinValue);
            this.TimeSlots.Add(DateTimeConstants.MinValue);
            this.TimeSlots.Add(DateTimeConstants.MinValue);
            this.EmailSubject = string.Empty;
        }


    }
}
