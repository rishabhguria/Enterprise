using Prana.BusinessObjects;


namespace Prana.Interfaces
{
    /// <summary>
    /// Its a interface for Time & Sales module.
    /// </summary>
    public interface ITimeSales
    {
        System.Windows.Forms.Form Reference();

        void ApplyPreferences(string moduleName, IPreferenceData prefs);
    }
}