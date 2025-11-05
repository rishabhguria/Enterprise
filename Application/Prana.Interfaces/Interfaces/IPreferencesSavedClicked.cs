using System;


namespace Prana.Interfaces
{
    public interface IPreferencesSavedClicked : IPreferences
    {
        event EventHandler SaveClicked;

        /// <summary>
        /// cheks if any newly added preference is invalid
        /// </summary>
        /// <returns>false if user wants to delete invalid preference, true otherwise</returns>
        bool RemoveInvalidNewPreferences();
    }
}
