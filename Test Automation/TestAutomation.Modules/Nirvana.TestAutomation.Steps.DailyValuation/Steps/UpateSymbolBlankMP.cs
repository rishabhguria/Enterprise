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
    public class UpateSymbolBlankMP : BlankMarkPriceUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenMarkPriceTab();
                UpdateSymbol(testData.Tables[0].Rows[0]);

                return _result;
            }
            catch (Exception)
            {
                throw;
            }
              

        }
        private void UpdateSymbol(DataRow dr)
        {
            try
            {
                BtnClearFilter.Click(MouseButtons.Left);

                if (!dr[TestDataConstants.COL_MARKPRICE_SYMBOL].ToString().Equals(string.Empty))
                {

                    TxtSymbolFilteration.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_MARKPRICE_SYMBOL].ToString();

                    BtnGetFilteredData.Click(MouseButtons.Left);


                }
                if (dr[TestDataConstants.COL_CLICK_ON_COPY].ToString() == "Yes")
                {
                    CmbCopyFromAccount.Click(MouseButtons.Left);


                    if (dr[TestDataConstants.COL_COPY_FROM].ToString() == "Allocation1")
                    {
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{ENTER}");
                    }
                    if (dr[TestDataConstants.COL_COPY_FROM].ToString() == "Allocation2")
                    {
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{ENTER}");
                    }
                    if (dr[TestDataConstants.COL_COPY_FROM].ToString() == "Allocation3")
                    {
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{ENTER}");
                    }
                    if (dr[TestDataConstants.COL_COPY_FROM].ToString() == "LP C/O")
                    {
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{ENTER}");
                    }
                    if (dr[TestDataConstants.COL_COPY_FROM].ToString() == "OFFSHORE")
                    {
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{ENTER}");
                    }
                    if (dr[TestDataConstants.COL_COPY_FROM].ToString() == "Allocation4")
                    {
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{ENTER}");
                    }
                    if (dr[TestDataConstants.COL_COPY_FROM].ToString() == "rt")
                    {
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{ENTER}");
                    }


                    MultiSelectDropDown1.Click(MouseButtons.Left);
                    if (dr[TestDataConstants.COL_COPY_TO].ToString() == "Select All")
                    {

                        SendKeys.Send("{DOWN}");
                        Keyboard.SendKeys(KeyboardConstants.SPACE);
                    }
                    if (dr[TestDataConstants.COL_COPY_TO].ToString() == "Allocation1")
                    {
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        Keyboard.SendKeys(KeyboardConstants.SPACE);
                    }
                    if (dr[TestDataConstants.COL_COPY_TO].ToString() == "Allocation2")
                    {
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        Keyboard.SendKeys(KeyboardConstants.SPACE);
                    }
                    if (dr[TestDataConstants.COL_COPY_TO].ToString() == "Allocation3")
                    {
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        Keyboard.SendKeys(KeyboardConstants.SPACE);
                    }
                    if (dr[TestDataConstants.COL_COPY_TO].ToString() == "Allocation4")
                    {
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        Keyboard.SendKeys(KeyboardConstants.SPACE);
                    }
                    if (dr[TestDataConstants.COL_COPY_TO].ToString() == "LP C/O")
                    {
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{DOWN}");
                        Keyboard.SendKeys(KeyboardConstants.SPACE);
                    }
                    LblCopyTo.Click(MouseButtons.Left);
                    BtnAccountCopy.Click(MouseButtons.Left);

                }

            }
            catch (Exception) { throw; }
        }
      
    }
}
