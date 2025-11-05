using Prana.BusinessObjects;
using Prana.Global;
using System;

namespace Prana.Interfaces
{
    /// <summary>
    /// Summary description for IPNL.
    /// </summary>
    public interface IPNL
    {
        System.Windows.Forms.Form Reference();

        int UserID
        {
            get;
            set;
        }

        event EventHandler PNLClosed;

        event EventHandler LaunchPreferences;

        void ApplyPreferences(object sender, EventArgs<string, IPreferenceData> e);
    }
}
