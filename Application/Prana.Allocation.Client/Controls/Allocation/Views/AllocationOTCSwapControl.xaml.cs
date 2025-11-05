using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Infragistics.Windows.Themes;
using Prana.Logging;
using Prana.Global;

namespace Prana.Allocation.Client.Controls.Allocation.Views
{
    /// <summary>
    /// Interaction logic for AllocationSwapControl.xaml
    /// </summary>
    public partial class AllocationOTCSwapControl : UserControl
    {
        public AllocationOTCSwapControl()
        {
            InitializeComponent();
            AddInfragisticsSourceDictionary();
        }

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
                ExceptionLogger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
    }
}
