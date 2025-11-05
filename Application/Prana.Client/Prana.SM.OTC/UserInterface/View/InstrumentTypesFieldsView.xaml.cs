using System.Windows;

namespace Prana.SM.OTC.View
{
    /// <summary>
    /// Interaction logic for InstrumentTypesFieldsView.xaml
    /// </summary>
    public partial class InstrumentTypesFieldsView : Window
    {
        public InstrumentTypesFieldsView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        public void HideCheckBoxColumn(bool isHide)
        {
            var context = this.DataContext as InstrumentTypeFieldsViewModel;
            if (context != null && isHide)
            {
                context.IsShowHideControls = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
