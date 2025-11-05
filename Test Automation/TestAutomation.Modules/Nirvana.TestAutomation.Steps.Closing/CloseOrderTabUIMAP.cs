using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Utilities;
using System;
using System.ComponentModel;
using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.Closing
{
    [UITestFixture]
    public partial class CloseOrderTabUIMAP : UIMap
    {
        public CloseOrderTabUIMAP()
        {
            InitializeComponent();
        }

        public CloseOrderTabUIMAP(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }
        /// <summary>
        /// Minimize Closing
        /// </summary>
        internal void MinimizeClosing()
        {
            try
            {
                KeyboardUtilities.MinimizeWindow(ref CloseTrade_UltraFormManager_Dock_Area_Top);
                TitleBar.WaitForVisible();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }
    }
}
