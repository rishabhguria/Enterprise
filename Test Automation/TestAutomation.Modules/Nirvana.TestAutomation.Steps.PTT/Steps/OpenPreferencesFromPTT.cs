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

namespace Nirvana.TestAutomation.Steps.PTT
{
    class OpenPreferencesFromPTT : PTTUIMap, ITestStep
    {
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenPTT();
               // Wait(3000);

                Preferences.Click(MouseButtons.Left);
               // Wait(3000);
                if (!CtrlPTTPreferences.IsVisible)
                {
                    throw new Exception("PTT UI PREF WINDOW NOT VISIBLE");
                }

                PercentTradingTool.Click(MouseButtons.Left);
                PercentTradingTool.Click(MouseButtons.Right);
                KeyboardUtilities.CloseWindow(ref PercentTradingTool);
            

            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;

            }
            
            return _result;
        }
               
            
        
    }

}
