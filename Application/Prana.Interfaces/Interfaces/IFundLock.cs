
using System;
namespace Prana.Interfaces
{
    /// <summary>
    /// Its a interface for Charts module.
    /// </summary>
    public interface IAccountlock
    {
        System.Windows.Forms.Form Reference();

        //event EventHandler LaunchPreferences;
        //void ApplyPreferences(string moduleName,IPreferenceData prefs);	
        event EventHandler AccountLockFlatFileClosed;

        Prana.BusinessObjects.CompanyUser LoginUser
        {
            get;
            set;
        }
    }
}
