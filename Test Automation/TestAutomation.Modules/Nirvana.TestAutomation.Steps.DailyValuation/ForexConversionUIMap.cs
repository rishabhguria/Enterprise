using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.DailyValuation
{
    [UITestFixture]
    public partial class ForexConversionUIMap : UIMap
    {
        public ForexConversionUIMap()
        {
            InitializeComponent();
        }

        public ForexConversionUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }


        /// <summary>
        /// Opens the Forex conversion tab.
        /// </summary>
        protected void OpenForexConversion()
        {
            try
            {
                //Shortcut to open daily valuation module(CTRL + SHIFT + D)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_DAILY_VAL"]);
                //Wait(5000);
                ExtentionMethods.WaitForVisible(ref MarkPriceAndForexConversion, 15);
                //DataManagement.Click(MouseButtons.Left);
                //DailyValuation.Click(MouseButtons.Left);
                MarkPriceAndForexConversion.WaitForVisible();
                ForexConversion.Click(MouseButtons.Left);
                GrdPivotDisplay.WaitForVisible();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
    }
}
