using System.Windows;

namespace Prana.SM.OTC.View
{
    /// <summary>
    /// Interaction logic for AddOTCTemplateView.xaml
    /// </summary>
    public partial class OTCTradeDetailsView : Window
    {
        public OTCTradeDetailsView()
        {
            InitializeComponent();
            DataContext = new OTCTradeDetailsViewModel();
            this.Closing += new System.ComponentModel.CancelEventHandler(OTCTradeDetailsViewClosing);
        }

        public OTCTradeDetailsView(int templateID)
        {
            InitializeComponent();
            DataContext = new OTCTradeDetailsViewModel(templateID);
        }

        void OTCTradeDetailsViewClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //e.Cancel = true;
        }
    }
}
