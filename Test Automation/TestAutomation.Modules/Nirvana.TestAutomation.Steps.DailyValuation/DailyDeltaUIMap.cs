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
    public partial class DailyDeltaUIMap : UIMap
    {
        public DailyDeltaUIMap()
        {
            InitializeComponent();
        }

        public DailyDeltaUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        /// <summary>
        /// Open daily delta tab.
        /// </summary>
        protected void OpenDailyDeltaTab()
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
                DailyDelta.Click(MouseButtons.Left);
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
