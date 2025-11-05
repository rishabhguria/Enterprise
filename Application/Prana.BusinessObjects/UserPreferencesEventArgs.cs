using System;

namespace Prana.BusinessObjects
{
    public class UserPreferencesEventArgs : EventArgs
    {
        private bool _useClosingMark = false;

        public bool UseClosingMark
        {
            get { return _useClosingMark; }
            set { _useClosingMark = value; }
        }

        private double _xPercentOfAvgVolume = 100;

        public double XPercentOfAvgVolume
        {
            get { return _xPercentOfAvgVolume; }
            set { _xPercentOfAvgVolume = value; }
        }

    }
}
