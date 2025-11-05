using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Prana.RuleEngine.BusinessObjects
{
    public class NotificationSettings
    {


        private bool _emailEnabled = false;

        public bool EmailEnabled
        {
            get { return _emailEnabled; }
            set { _emailEnabled = value; }
        }

        private bool _popUpEnabled = true;

        public bool PopUpEnabled
        {
            get { return _popUpEnabled; }
            set { _popUpEnabled = value; }
        }

        private String _emailList = String.Empty;

        public String EmailList
        {
            get { return _emailList; }
            set { _emailList = value; }
        }

        private int _limitFrequencyMinutes = 2;

        public int LimitFrequencyMinutes
        {
            get { return _limitFrequencyMinutes; }
            set { _limitFrequencyMinutes = value; }
        }

        private int _warningFrequencyMinutes = 1;

        public int WarningFrequencyMinutes
        {
            get { return _warningFrequencyMinutes; }
            set { _warningFrequencyMinutes = value; }
        }

        private bool _manualTradeEnabled = false;

        public bool ManualTradeEnabled
        {
            get { return _manualTradeEnabled; }
            set { _manualTradeEnabled = value; }
        }

        private bool _soundEnabled = false;

        public bool SoundEnabled
        {
            get { return _soundEnabled; }
            set { _soundEnabled = value; }
        }

        private String _soundFilePath = String.Empty;

        public String SoundFilePath
        {
            get { return _soundFilePath; }
            set { _soundFilePath = value; }
        }


        internal NotificationSettings(DataRow notificationRow)
        {
            _emailEnabled = Convert.ToBoolean(notificationRow["EmailEnabled"].ToString());
            _popUpEnabled = Convert.ToBoolean(notificationRow["PopUpEnabled"].ToString());
            _emailList = notificationRow["EmailToList"].ToString();
            _limitFrequencyMinutes = Convert.ToInt32(notificationRow["LimitFrequencyMinutes"].ToString());
            _warningFrequencyMinutes = Convert.ToInt32(notificationRow["WarningFrequencyMinutes"].ToString());
            _manualTradeEnabled = Convert.ToBoolean(notificationRow["ManualTradeEnabled"].ToString());
            _soundEnabled = Convert.ToBoolean(notificationRow["SoundEnabled"].ToString());
            _soundFilePath = notificationRow["SoundFilePath"].ToString();

        }

        internal NotificationSettings()
        {

        }
        
    }
}
