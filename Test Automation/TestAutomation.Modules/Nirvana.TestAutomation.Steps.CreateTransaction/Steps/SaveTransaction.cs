using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.CreateTransaction
{
    public class SaveTransaction : CreateTransactionUIMap, ITestStep
    {
            /// <summary>
            /// Runs the test.
            /// </summary>
            /// <param name="testData">The test data.</param>
            /// <returns></returns>
            public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
            {
                TestResult _result = new TestResult();
                try
                {
                    OpenCreateTransaction();
                    BtnSave.Click(MouseButtons.Left);
                    if (CreateTransactionsSave.IsVisible)
                    {
                        ButtonSaveYes.Click(MouseButtons.Left);
                    }
                    
                }
                catch (Exception ex)
                {
                    _result.IsPassed = false;
                    bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                    if (rethrow)
                        throw;
                }
                finally
                {
                    KeyboardUtilities.CloseWindow(ref CreatePosition_UltraFormManager_Dock_Area_Top);
                }
                return _result;
            }

        }
   }

