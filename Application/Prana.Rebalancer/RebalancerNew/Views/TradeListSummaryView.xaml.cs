using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Prana.Rebalancer.RebalancerNew.Views
{
    /// <summary>
    /// Interaction logic for TradeListSummaryView.xaml
    /// </summary>
    public partial class TradeListSummaryView : Window
    {
        public TradeListSummaryView()
        {
            InitializeComponent();
        }

        public void SetUpForSaveTradeList(bool popUpForSave)
        {
            this.CancelButton.Visibility = popUpForSave ? Visibility.Visible : Visibility.Collapsed;
            this.NoButton.Template = popUpForSave ? FindResource("OrangeButtonSkinTemplate") as ControlTemplate : FindResource("RedButtonSkinTemplate") as ControlTemplate;
        }
    }
}
