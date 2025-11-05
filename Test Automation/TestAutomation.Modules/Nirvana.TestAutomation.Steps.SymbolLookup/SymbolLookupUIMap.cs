using System;
using System.ComponentModel;

using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.SymbolLookup
{
    [UITestFixture]
    public partial class SymbolLookupUIMap : UIMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolLookupUIMap"/> class.
        /// </summary>
        public SymbolLookupUIMap()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolLookupUIMap"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public SymbolLookupUIMap(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        /// <summary>
        /// Colse symbol Lookup.
        /// </summary>
        public void CloseSymbolLookup()
        {
            try
            {
                KeyboardUtilities.CloseWindow(ref SymbolLookUp_UltraFormManager_Dock_Area_Top);
                //ButtonNo.Click(MouseButtons.Left);
                //KeyboardUtilities.CloseWindow(ref SymbolLookUp_UltraFormManager_Dock_Area_Top);
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
                KeyboardUtilities.MinimizeWindow(ref SymbolLookUp_UltraFormManager_Dock_Area_Top);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Opens the symbol lookup.
        /// </summary>
        public string OpenSymbolLookup()
        {
            string errorMessage = string.Empty;
            try
            {
                if (!PranaMain.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref PranaMain, 40);
                }
                //Shortcut to open Security Master (CTRL + SHIFT + S)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_SM"]);
                //Wait(5000);
                ExtentionMethods.WaitForVisible(ref SymbolLookUp1, 15);
                //DataManagement.Click(MouseButtons.Left);
                //SecurityMaster.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return errorMessage;
        }
    }
}
