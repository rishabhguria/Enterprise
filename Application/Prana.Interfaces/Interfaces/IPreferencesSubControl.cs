namespace Prana.Interfaces
{
    /// <summary>
    /// Its a interface for all LiveFeed SubModules
    /// </summary>
    public interface IPreferencesSubControl
    {
        /// <summary>
        /// Load the preference control with the saved values in xml
        /// </summary>
        void LoadPreferencesControl();

        /// <summary>
        /// Save the preference control's values in the xml
        /// </summary>
        void SavePreferencesControl();
    }
}
