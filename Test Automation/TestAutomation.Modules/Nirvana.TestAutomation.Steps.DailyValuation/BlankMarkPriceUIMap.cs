using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.DailyValuation
{
    [UITestFixture]
    public partial class BlankMarkPriceUIMap :  UIMap
    {
        public BlankMarkPriceUIMap()
        {
            InitializeComponent();
        }

        public BlankMarkPriceUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        protected void OpenMarkPriceTab()
        {
            try
            {
                //Shortcut to open daily valuation module(CTRL + SHIFT + D)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_DAILY_VAL"]);
                ExtentionMethods.WaitForVisible(ref MarkPriceAndForexConversion, 15);
                //Wait(5000);
                //DataManagement.Click(MouseButtons.Left);
                //DailyValuation.Click(MouseButtons.Left);
                MarkPriceAndForexConversion.WaitForVisible();
                MarkPrice.Click(MouseButtons.Left);
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
