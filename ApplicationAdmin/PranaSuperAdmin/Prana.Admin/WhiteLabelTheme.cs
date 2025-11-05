using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Drawing;

namespace Prana
{
    /// <summary>
    /// Theme Helper
    /// </summary>
    public static class WhiteLabelTheme
    {
        internal static Image LoginBackGroundImage;
        internal static Image LoginUserNameLabelImage;
        internal static Image LoginPasswordLabelImage;
        internal static Icon AppIcon;
        internal static Image LoginBtnBackgroundImage;
        internal static Image CloseBtnBackgroundImage;
        internal static string AppTitle;
        internal static string WhitelableProductName;
        static bool ApplyTheme;

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

                    LoginBackGroundImage = Image.FromFile(System.Windows.Forms.Application.StartupPath + @"\Themes" + System.IO.Path.DirectorySeparatorChar + WhitelableProductName + System.IO.Path.DirectorySeparatorChar + "DefaultSplash.bmp");
                    LoginUserNameLabelImage = Image.FromFile(System.Windows.Forms.Application.StartupPath + @"\Themes" + System.IO.Path.DirectorySeparatorChar + WhitelableProductName + System.IO.Path.DirectorySeparatorChar + "username.bmp");
                    LoginPasswordLabelImage = Image.FromFile(System.Windows.Forms.Application.StartupPath + @"\Themes" + System.IO.Path.DirectorySeparatorChar + WhitelableProductName + System.IO.Path.DirectorySeparatorChar + "password.bmp");
                    LoginBtnBackgroundImage = Image.FromFile(System.Windows.Forms.Application.StartupPath + @"\Themes" + System.IO.Path.DirectorySeparatorChar + WhitelableProductName + System.IO.Path.DirectorySeparatorChar + "BtnLoginBackgroundImage.bmp");
                    CloseBtnBackgroundImage = Image.FromFile(System.Windows.Forms.Application.StartupPath + @"\Themes" + System.IO.Path.DirectorySeparatorChar + WhitelableProductName + System.IO.Path.DirectorySeparatorChar + "BtnCloseBackgroundImage.bmp");

                    AppIcon = new Icon(System.Windows.Forms.Application.StartupPath + @"\Themes" + System.IO.Path.DirectorySeparatorChar + WhitelableProductName + System.IO.Path.DirectorySeparatorChar + "AppIconDefaultON.ico");
                    if (!Boolean.TryParse(ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ThemeAndWhiteLabeling, "ApplyTheme"), out ApplyTheme))
                        ApplyTheme = false;
                }
                else
                {
                    WhitelableProductName = "Nirvana";
                    CustomThemeHelper.WHITELABELTHEME = WhitelableProductName;
                    AppTitle = WhitelableProductName;

                    LoginBackGroundImage = Prana.Admin.Properties.Resources.DefaultSplash;
                    LoginUserNameLabelImage = Prana.Admin.Properties.Resources.username;
                    LoginPasswordLabelImage = Prana.Admin.Properties.Resources.password;
                    AppIcon = Prana.Admin.Properties.Resources.AppIconDefaultON;
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
