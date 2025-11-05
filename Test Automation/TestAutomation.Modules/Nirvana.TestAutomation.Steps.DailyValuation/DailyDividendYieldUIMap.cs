using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.DailyValuation
{
    [UITestFixture]
    public partial class DailyDividendYieldUIMap : UIMap
    {
        public DailyDividendYieldUIMap()
        {
            InitializeComponent();
        }

        public DailyDividendYieldUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// Open Dividned Yield tab.
        /// </summary>
        protected void OpenDividnedYieldTab()
        {
            try
            {
                //Shortcut to open daily valuation module(CTRL + SHIFT + D)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_DAILY_VAL"]);
                ExtentionMethods.WaitForVisible(ref MarkPriceAndForexConversion, 15);
               // Wait(5000);
                //DataManagement.Click(MouseButtons.Left);
                //DailyValuation.Click(MouseButtons.Left);
                MarkPriceAndForexConversion.WaitForVisible();
                DailyDividendYield1.Click(MouseButtons.Left);
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
