using System;
using System.ComponentModel;
using Nirvana.TestAutomation.Utilities;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.PortfolioManagement
{
    [UITestFixture]
    public partial class PortfolioManagementUIMap : UIMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PortfolioManagementUIMap"/> class.
        /// </summary>
        public PortfolioManagementUIMap()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PortfolioManagementUIMap"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public PortfolioManagementUIMap(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        /// <summary>
        /// Opens the consolidation view.
        /// </summary>
        public void OpenConsolidationView()
        {
            try
            {
                if (!PranaMain.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref PranaMain, 60);
                }
                //Shortcut to open PM module(CTRL + SHIFT + P)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_PM"]);
                ExtentionMethods.WaitForVisible(ref PM, 15);
                //Wait(5000);
                //PortfolioManagement.Click(MouseButtons.Left);
                //CustomPMViews.WaitForVisible();
                //CustomPMViews.Click(MouseButtons.Left);
                PMGrid.WaitForResponding();
                PmDashboard.WaitForResponding();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Close the PM
        /// </summary>
        public void PMclose()
        {
            try
            {
                KeyboardUtilities.CloseWindow(ref PM_UltraFormManager_Dock_Area_Top);
              
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
