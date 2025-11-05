using System;
using System.Threading;
using System.Windows;

namespace Prana.SM.OTC.View
{
    /// <summary>
    /// Interaction logic for OTCTemplateView.xaml
    /// </summary>
    public partial class OTCTemplateView : Window, IDisposable
    {
        private readonly SynchronizationContext _syncContext;
        public OTCTemplateView()
        {
            InitializeComponent();
            DataContext = new OTCTemplateViewModel();
            _syncContext = SynchronizationContext.Current;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {

        }
    }
}
