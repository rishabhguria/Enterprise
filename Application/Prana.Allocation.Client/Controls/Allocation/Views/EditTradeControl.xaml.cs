using Prana.Allocation.Client.Controls.Allocation.ViewModels;
using Prana.LogManager;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Prana.Allocation.Client.Controls.Allocation.Views
{
    /// <summary>
    /// Interaction logic for EditTradeControl.xaml
    /// </summary>
    public partial class EditTradeControl : UserControl
    {
        private bool _hasRunFirstTimeLogic = false;

        public EditTradeControl()
        {
            InitializeComponent();
            AddInfragisticsSourceDictionary();
            this.IsVisibleChanged += EditTradeControl_IsVisibleChanged;
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
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Executes initialization logic for the trade attributes the first time the control becomes visible.
        /// </summary>
        private void EditTradeControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!_hasRunFirstTimeLogic && (bool)e.NewValue == true)
            {
                _hasRunFirstTimeLogic = true;

                if (DataContext is EditTradeControlViewModel vm)
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        vm.LoadTradeAttributes();
                    }), DispatcherPriority.ContextIdle);
                }
            }
        }
    }
}
