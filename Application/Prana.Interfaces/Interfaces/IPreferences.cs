using Prana.BusinessObjects;
namespace Prana.Interfaces
{
    /// <summary>
    /// Summary description for IPreferences.
    /// </summary>
    public interface IPreferences
    {
        void SetUp(CompanyUser user);
        System.Windows.Forms.UserControl Reference();
        bool Save();
        void RestoreDefault();
        IPreferenceData GetPrefs();
        string SetModuleActive
        {
            set;
        }
    }
}