using System;
using System.ComponentModel;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.DailyValuation
{
    [UITestFixture]
    public partial class DailyCreditLimitUIMap : UIMap
    {
        public DailyCreditLimitUIMap()
        {
            InitializeComponent();
        }

        public DailyCreditLimitUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void OpenDailyCreditLimitTab()
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
                Next.Click(MouseButtons.Left);
                Next.Click(MouseButtons.Left);
                Next.Click(MouseButtons.Left);
                Next.Click(MouseButtons.Left);
                Next.Click(MouseButtons.Left);
                Next.Click(MouseButtons.Left);
                DailyCreditLimit.Click(MouseButtons.Left);
                GrdPivotDisplay.WaitForVisible();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Setting cell in the gird.
        /// </summary>
        /// <param name="columToIndexMapping"></param>
        /// <param name="dataRow"></param>
        /// <param name="gridRow"></param>
        /// <param name="columnName"></param>
        public void SetValueInCell(Dictionary<string, int> columToIndexMapping, DataRow dataRow, MsaaObject gridRow, string columnName)
        {
            try
            {
                if (!String.IsNullOrEmpty(dataRow[columnName].ToString()))
                {
                    if (columToIndexMapping.ContainsKey(columnName))
                    {
                        int columnIndex = columToIndexMapping[columnName];
                        //GrdPivotDisplay.Click(MouseButtons.Left);
                        GrdPivotDisplay.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, columnName);
                        Wait(1000);
                        gridRow.CachedChildren[columnIndex].Click(MouseButtons.Left);
                        Keyboard.SendKeys(dataRow[columnName].ToString());
                    }
                }
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
