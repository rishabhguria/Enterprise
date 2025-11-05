using Prana.BusinessObjects;

namespace Prana.PM.Client.UI
{
    public class PMPreferenceData : IPreferenceData
    {
        private bool _useClosingMark = false;

        public bool UseClosingMark
        {
            get { return _useClosingMark; }
            set { _useClosingMark = value; }
        }

        private double _xPercentOfAvgVolume = 100;

        public double XPercentofAvgVolume
        {
            get { return _xPercentOfAvgVolume; }
            set { _xPercentOfAvgVolume = value; }
        }

        /// <summary>
        /// The is show pm toolbar
        /// </summary>
        private bool _isShowPMToolbar = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is show pm toolbar.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is show pm toolbar; otherwise, <c>false</c>.
        /// </value>
        public bool IsShowPMToolbar
        {
            get { return _isShowPMToolbar; }
            set { _isShowPMToolbar = value; }
        }

    }
}
