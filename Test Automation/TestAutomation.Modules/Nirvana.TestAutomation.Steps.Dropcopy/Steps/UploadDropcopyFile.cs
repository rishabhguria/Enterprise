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

namespace Nirvana.TestAutomation.Steps.Dropcopy
{
     class UploadDropcopyFile : Dropcopy, ITestStep
    {
         public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                PranaDropCopyFileReader.BringToFront();
                BtnStart.Click(MouseButtons.Left);
                RdRegression.Click(MouseButtons.Left);
                TextBoxAppendedText.Click(MouseButtons.Left);
                Keyboard.SendKeys(KeyboardConstants.CTRLA);
                Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY); 
                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    if (dr[DropcopyConstants.Col_AppendToOrderId].ToString() != String.Empty)
                    {
                        Keyboard.SendKeys(dr[DropcopyConstants.Col_AppendToOrderId].ToString());
                    }
                }
                BtnImport.Click(MouseButtons.Left);
                Wait(3000);
                DataRow dr1 = testData.Tables[sheetIndexToName[0]].Rows[0];
                List<string> colList = new List<string>();
                foreach (DataColumn col in testData.Tables[sheetIndexToName[0]].Columns)
                {
                    colList.Add(col.ColumnName);
                }
                foreach (string colName in colList)
                {
                    if (colName.Equals(DropcopyConstants.Col_UploadFile))
                    {
                        Filename.Click(MouseButtons.Left);
                        Keyboard.SendKeys(KeyboardConstants.CTRLA);
                        Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY); 
                        Keyboard.SendKeys(dr1[colName].ToString());
                    }
                }
                ButtonOpen.Click(MouseButtons.Left);
                Wait(10000);


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

