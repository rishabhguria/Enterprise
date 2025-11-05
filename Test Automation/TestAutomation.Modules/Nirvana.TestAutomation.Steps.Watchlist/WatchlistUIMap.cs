using System;
using System.ComponentModel;

using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.Watchlist
{
    [UITestFixture]
    public partial class WatchlistUIMap : UIMap
    {
        public WatchlistUIMap()
        {
            InitializeComponent();
        }

        public WatchlistUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// Opens the Watchlist.
        /// </summary>
        public string OpenWatchList()
        {
            string errorMessage = string.Empty;
            try
            {
            if (!PranaMain.IsVisible)
            {
                ExtentionMethods.WaitForVisible(ref PranaMain, 40);
            }
            
            
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_WATCHLIST"]);
                ExtentionMethods.WaitForVisible(ref WatchListMain, 500);
               // Wait(5000);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return errorMessage;
        }

        /// <summary>
        /// Colse symbol Lookup.
        /// </summary>
        public void CloseWatchlist()
        {
            try
            {
                uiUltraGrid1.BringToFront();
                KeyboardUtilities.CloseWindow(ref WatchList_UltraFormManager_Dock_Area_Top);
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// Minimze Symbol Lookup.
        /// </summary>
        public void MinimzieSybolLookup()
        {
            try
            {
                uiUltraGrid1.BringToFront();
                KeyboardUtilities.MinimizeWindow(ref WatchList_UltraFormManager_Dock_Area_Top);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }
        public void MaximizeWatchlist()
        {
            try
            {
                uiUltraGrid1.BringToFront();
                KeyboardUtilities.MaximizeWindow(ref WatchList_UltraFormManager_Dock_Area_Top);
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
