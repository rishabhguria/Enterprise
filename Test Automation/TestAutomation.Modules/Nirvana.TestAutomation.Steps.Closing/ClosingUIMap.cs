using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Utilities;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.Closing
{
    [UITestFixture]
    public partial class ClosingUIMap : UIMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClosingUIMap"/> class.
        /// </summary>
        public ClosingUIMap()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClosingUIMap"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public ClosingUIMap(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        /// <summary>
        /// Open Closing UI
        /// </summary>
        internal void OpenClosingUI()
        {
            try
            {
                //PranaMain.WaitForVisible();
                if (!PranaMain.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref PranaMain, 40);
                }
                //Shortcut to open closing module(CTRL + ALT + C )
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_CLOSING"]);
                if (NoData.IsVisible)
                    NoDataButtonOK.Click(MouseButtons.Left);
                if (!CloseTrade.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref CloseTrade, 10);
                    //PortfolioManagement.Click(MouseButtons.Left);
                    //ClosePositions.Click(MouseButtons.Left);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Close Closing
        /// </summary>
        internal void CloseClosing()
        {
            try
            {
                KeyboardUtilities.CloseWindow(ref CloseTrade_UltraFormManager_Dock_Area_Top);
               // Wait(500);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
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
