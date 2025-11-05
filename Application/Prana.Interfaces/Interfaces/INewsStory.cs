using System;

namespace Prana.Interfaces
{
    /// <summary>
    /// Its a interface for NewsStory module.
    /// </summary>
    public interface INewsStory
    {
        System.Windows.Forms.Form Reference();

        event EventHandler LaunchPreferences;
        //		void ApplyPreferences(string moduleName,IPreferenceData prefs);
    }

}
