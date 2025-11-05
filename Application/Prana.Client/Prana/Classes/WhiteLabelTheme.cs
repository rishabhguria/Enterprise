using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Drawing;

namespace Prana
{
    public static class WhiteLabelTheme
    {
        public static Image LoginBackGroundImage;
        public static Image AboutBackGroundImage;
        public static Image LoginUserNameLabelImage;
        public static Image LoginPasswordLabelImage;
        public static Icon AppIcon;
        public static string AppTitle;
        public static string WhitelableProductName;
        public static bool ApplyTheme;

        static WhiteLabelTheme()
        {
            LoadWhiteLabelSettings();
        }

        static void LoadWhiteLabelSettings()
        {
            try
            {
                WhitelableProductName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ThemeAndWhiteLabeling, "WhiteLabelingName");
                if (WhitelableProductName != null && System.IO.Directory.Exists(System.Windows.Forms.Application.StartupPath + @"\Themes" + System.IO.Path.DirectorySeparatorChar + WhitelableProductName))
                {
                    CustomThemeHelper.WHITELABELTHEME = WhitelableProductName;
                    if (WhitelableProductName.CompareTo("Nirvana") != 0)
                    {
                        AppTitle = WhitelableProductName + " - Powered by Nirvana";
                    }
                    else
                    {
                        AppTitle = WhitelableProductName;
                    }

                    AboutBackGroundImage = Image.FromFile(System.Windows.Forms.Application.StartupPath + @"\Themes" + System.IO.Path.DirectorySeparatorChar + WhitelableProductName + System.IO.Path.DirectorySeparatorChar + "DefaultAbout.png");
                    LoginBackGroundImage = Image.FromFile(System.Windows.Forms.Application.StartupPath + @"\Themes" + System.IO.Path.DirectorySeparatorChar + WhitelableProductName + System.IO.Path.DirectorySeparatorChar + "DefaultSplash.png");
                    LoginUserNameLabelImage = Image.FromFile(System.Windows.Forms.Application.StartupPath + @"\Themes" + System.IO.Path.DirectorySeparatorChar + WhitelableProductName + System.IO.Path.DirectorySeparatorChar + "username.png");
                    LoginPasswordLabelImage = Image.FromFile(System.Windows.Forms.Application.StartupPath + @"\Themes" + System.IO.Path.DirectorySeparatorChar + WhitelableProductName + System.IO.Path.DirectorySeparatorChar + "password.png");
                    AppIcon = new Icon(System.Windows.Forms.Application.StartupPath + @"\Themes" + System.IO.Path.DirectorySeparatorChar + WhitelableProductName + System.IO.Path.DirectorySeparatorChar + "AppIconDefaultON.ico");
                    if (!Boolean.TryParse(ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ThemeAndWhiteLabeling, "ApplyTheme"), out ApplyTheme))
                        ApplyTheme = false;
                }
                else
                {
                    WhitelableProductName = "Nirvana";
                    CustomThemeHelper.WHITELABELTHEME = WhitelableProductName;
                    AppTitle = WhitelableProductName;
                    AboutBackGroundImage = Prana.Properties.Resources.DefaultAbout;
                    LoginBackGroundImage = Prana.Properties.Resources.DefaultSplash;
                    LoginUserNameLabelImage = Prana.Properties.Resources.username;
                    LoginPasswordLabelImage = Prana.Properties.Resources.Password;
                    AppIcon = Prana.Properties.Resources.AppIconDefaultON_32;
                    ApplyTheme = false;
                }
                CustomThemeHelper.ApplyTheme = ApplyTheme;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
