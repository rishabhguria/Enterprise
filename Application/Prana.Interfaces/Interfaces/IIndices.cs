using Prana.BusinessObjects;
using System;

namespace Prana.Interfaces
{
    /// <summary>
    /// Its a interface for WatchList module.
    /// </summary>
    public interface IIndices
    {
        System.Windows.Forms.Form Reference();
        event EventHandler IndicesClosed;
        void ApplyPreferences(string moduleName, IPreferenceData prefs);
    }
}
