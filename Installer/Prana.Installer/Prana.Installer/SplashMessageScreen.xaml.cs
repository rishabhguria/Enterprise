using Prana.InstallerUtilities;
using System.Windows;

namespace Prana.Installer
{
    /// <summary>
    /// Interaction logic for SplashMessageScreen.xaml
    /// </summary>
    public partial class SplashMessageScreen : Window
    {
        public SplashMessageScreen()
        {
            InitializeComponent();
        }

        public void SetSplashMessage(string title, string log)
        {
            if (MainText.Dispatcher.CheckAccess())
            {
                MainText.Text = title;
                LoggingHelper.GetInstance().LoggerWrite(title);

                SetLogText(log, true);
            }
            else
            {
                MainText.Dispatcher.Invoke(() => { SetSplashMessage(title, log); });
            }
        }

        public void SetLogText(string log, bool internalCall = false)
        {
            if (SubText.Dispatcher.CheckAccess())
            {
                SubText.Text = log;
                if (!internalCall)
                {
                    LoggingHelper.GetInstance().LoggerWrite(log);
                }
            }
            else
            {
                SubText.Dispatcher.Invoke(() => { SetLogText(log); });
            }
        }
    }
}