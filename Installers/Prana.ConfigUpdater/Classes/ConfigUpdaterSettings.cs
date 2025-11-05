using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Windows.Forms;

namespace Prana.ConfigUpdater
{
    public class ConfigUpdaterSettings
    {
        private static ConfigUpdaterSettings m_ConfigUpdaterSettingsInstance = null;
        private string m_InstallSettingsFile = Application.StartupPath + "\\InstallSettings.config";
        private string m_DestinationFile = Application.StartupPath + "\\Prana.exe.config";

        #region Public properties
        public string InstallSettingsFile
        {
            get
            {
                return m_InstallSettingsFile;
            }
            set
            {
                m_InstallSettingsFile = value;
            }
        }
        public string DestinationFile
        {
            get
            {
                return m_DestinationFile;
            }
            set
            {
                m_DestinationFile = value;
            }
        }
        #endregion

        private ConfigUpdaterSettings() { }
        public static ConfigUpdaterSettings GetInstance()
        {
            if (null == m_ConfigUpdaterSettingsInstance)
            {
                m_ConfigUpdaterSettingsInstance = new ConfigUpdaterSettings();
            }
            return m_ConfigUpdaterSettingsInstance;
        }
    }
}
