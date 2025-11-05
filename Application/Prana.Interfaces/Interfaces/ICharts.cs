using Prana.BusinessObjects;
using System;

namespace Prana.Interfaces
{
    /// <summary>
    /// Its a interface for Charts module.
    /// </summary>
    public interface ICharts
    {
        System.Windows.Forms.Form Reference();

        event EventHandler LaunchPreferences;
        void ApplyPreferences(string moduleName, IPreferenceData prefs);
    }
}
