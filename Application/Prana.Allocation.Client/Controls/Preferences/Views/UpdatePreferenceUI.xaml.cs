using Prana.LogManager;
using System;
using System.ComponentModel;
using System.Windows;

namespace Prana.Allocation.Client.Controls.Preferences.Views
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    public partial class UpdatePreferenceUI : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdatePreferenceUI"/> class.
        /// </summary>
        public UpdatePreferenceUI()
        {
            InitializeComponent();
            AddInfragisticsSourceDictionary();
        }

        /// <summary>
        /// Adds the infragistics source dictionary.
        /// </summary>
        private void AddInfragisticsSourceDictionary()
        {
            try
            {
                if (!DesignerProperties.GetIsInDesignMode(this))
                {
                    ResourceDictionary rd = new ResourceDictionary();
                    rd.Source = new Uri(@"/Prana.Allocation.Client;component/Themes/IG/IG.xamDataPresenter.xaml", UriKind.Relative);
                    this.Resources.MergedDictionaries.Add(rd);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
    }
}
