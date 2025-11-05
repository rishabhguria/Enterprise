using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Prana.Client.Installer.Utilities
{
    public static class WhiteLabelHelper
    {
        public static Image InstallerHeader;
        public static Image InstallerSplash;
        public static string WhitelableProductName;
        public static long Whitelable
        {
            set { LoadWhiteLabelSettings(value); }
        }
        public static string Version;
        public static Icon InstallerIcon;

        static WhiteLabelHelper()
        {
            LoadWhiteLabelSettings(0);
        }

        static void LoadWhiteLabelSettings(long WLable)
        {
            Version = InstallerWhitelableTheme.Version;
            switch (WLable)
            {
                case 1:
                    InstallerHeader = InstallerWhitelableTheme.InstallerHeader1;
                    InstallerSplash = InstallerWhitelableTheme.InstallerSplash1;
                    WhitelableProductName = InstallerWhitelableTheme.ProductName1;
                    InstallerIcon = InstallerWhitelableTheme.AppIcon1ON;
                    break;
                case 2:
                    InstallerHeader = InstallerWhitelableTheme.InstallerHeader2;
                    InstallerSplash = InstallerWhitelableTheme.InstallerSplash2;
                    WhitelableProductName = InstallerWhitelableTheme.ProductName2;
                    InstallerIcon = InstallerWhitelableTheme.AppIcon2OFF;
                    break;
                default:
                    InstallerHeader = InstallerWhitelableTheme.InstallerHeaderDefault;
                    InstallerSplash = InstallerWhitelableTheme.DefaultInstallerSplash;
                    WhitelableProductName = InstallerWhitelableTheme.ProductNameDefault;
                    InstallerIcon = InstallerWhitelableTheme.AppIconDefaultON_32;
                    break;
            }
        }
    }
}
