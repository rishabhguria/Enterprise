using Prana.BusinessObjects;

namespace Prana.Preferences
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Prana.BusinessObjects.IPreferenceData" />
    public class GeneralPreferenceData : IPreferenceData
    {
        /// <summary>
        /// The is show service icons
        /// </summary>
        private bool _isShowServiceIcons = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is show service icons.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is show service icons; otherwise, <c>false</c>.
        /// </value>
        public bool IsShowServiceIcons
        {
            get { return _isShowServiceIcons; }
            set { _isShowServiceIcons = value; }
        }
    }
}
