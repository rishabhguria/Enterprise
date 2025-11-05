using Prana.BusinessObjects;
using Prana.Global;
using System;

namespace Prana.Interfaces
{
    /// <summary>
    /// Its a interface for News module.
    /// </summary>
    public interface INews
    {
        System.Windows.Forms.Form Reference();
        event EventHandler OnOpenStoryWindow;
        event EventHandler NewsClosed;
        event EventHandler LaunchPreferences;
        void ApplyPreferences(object sender, EventArgs<string, IPreferenceData> e);
    }
}
