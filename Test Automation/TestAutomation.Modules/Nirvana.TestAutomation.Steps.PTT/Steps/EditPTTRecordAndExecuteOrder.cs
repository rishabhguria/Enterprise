using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Factory;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Steps.PTT;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.UIAutomation;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.PTT
{
    class EditPTTRecordAndExecuteOrder : PTTUIMap,  IUIAutomationTestStep
    {
        public TestResult RunUIAutomationTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                try
                {
                    OpenPTT();
                    InputParametersPTT(UIAutomation.DataContainer.CalculatePTT, UIAutomation.DataContainer.tempsheetIndexToName);
                    Calculate.Click(MouseButtons.Left);
                    Calculate.Click(MouseButtons.Left);
                    Wait(4000);
                }
                catch (Exception ex)
                {
                    _result.IsPassed = false;
                    bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                    if (rethrow)
                        throw;
                }
                GridDataProvider gridDataProvider = new GridDataProvider();
                gridDataProvider.SelectAndEditWPFGrid("Window", testData.Tables[0].Copy());

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
        /// <summary>
        /// Disposes resources
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                base.Dispose(true);
                GC.SuppressFinalize(this);
            }
            catch(Exception)
            {
                    throw;
            }
        }
       
    }
}
