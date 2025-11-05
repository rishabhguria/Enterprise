using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Runtime.InteropServices;
using UIAutomationClient;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    class SwitchTabBlotter : BlotterUIMap, IUIAutomationTestStep
    {
        private static CUIAutomation automation = new CUIAutomation();
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenBlotter();
                MaximizeBlotter();
                foreach (DataRow dr in testData.Tables[0].Rows) {
                    string tabName = string.Empty;
                    if (!string.IsNullOrEmpty(dr["TabItem Key Orders"].ToString())) 
                    {
                        tabName = dr["TabItem Key Orders"].ToString();
                    }

                    if (!string.IsNullOrEmpty(dr["TabItem Key WorkingSubs"].ToString()))
                    {
                        tabName = dr["TabItem Key WorkingSubs"].ToString();
                    }

                    if (!string.IsNullOrEmpty(dr["TabItem Key Summary"].ToString()))
                    {
                        tabName = dr["TabItem Key Summary"].ToString();
                    }

                    if (!string.IsNullOrEmpty(dr["TabItem Key Dynamic"].ToString()))
                    {
                        tabName = dr["TabItem Key Dynamic"].ToString();
                    }
                    try
                    {
                        UIAutomationElement accountComboItem = new UIAutomationElement();
                        accountComboItem.AutomationName = tabName;
                        accountComboItem.Comment = null;
                        accountComboItem.ItemType = "";
                        accountComboItem.MatchedIndex = 0;
                        accountComboItem.Name = tabName;
                        accountComboItem.Parent = this.BlotterTabControl1;
                        accountComboItem.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Unknown;
                        accountComboItem.UseCoordinatesOnClick = true;
                        accountComboItem.Click(MouseButtons.Left);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(tabName + " not available\n" + ex);
                    }
                }
            }

            catch (Exception ex)
            {
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }
    }
}
