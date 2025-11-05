using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.CorporateAction
{
    class CleanUp:CorporateActionUIMap,ITestStep
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
                OpenCorporateActions();
                HistoricalCorpActions.Click(MouseButtons.Left);

                string[] actions = new string[7] { "Exchange", "Cash Dividend", "Merger", "Name Change", "SpinOff", "Stock Dividend", "Split" };

                foreach (string csaction in actions)
                {
                    CmbCATypeUndo.Click(MouseButtons.Left);
                    Keyboard.SendKeys(csaction + KeyboardConstants.ENTERKEY);
                    DtFromDateUndo.Click(MouseButtons.Left);
                    DtFromDateUndo.Properties["Text"] = TestDataConstants.CONST_DEFAULT_START_DATE.ToString();
                    DtToDateUndo.Click(MouseButtons.Left);
                    DtToDateUndo.Properties["Text"] = DateTime.Now.AddYears(1).ToString("MM/dd/yyyy");
                    BtnGetCAUndo.Click(MouseButtons.Left);
                    //Wait(2000);
                    DataTable dtgrid = CSVHelper.CSVAsDataTable(this.GrdCorporateActionEntry1.Properties[ExcelStructureConstants.COL_DESCRIPTION].ToString());
                    if (dtgrid.Rows.Count >= 1)
                    {
                        for (int i = 0; i < dtgrid.Rows.Count; i++)
                        {
                            var mssaobject = GrdCorporateActionEntry1.MsaaObject;
                            var Row = mssaobject.CachedChildren[0].CachedChildren[1];
                            Row.CachedChildren[0].Click(MouseButtons.Left);
                            BtnPreviewUndo.Click(MouseButtons.Left);
                            BtnSaveUndo.Click(MouseButtons.Left);
                            Wait(1000);
                            if (CorporateAction1.IsVisible)
                            {
                                ButtonYes.Click(MouseButtons.Left);
                            }
                            Wait(1000);
                        }
                    }
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
                KeyboardUtilities.CloseWindow(ref FrmCorporateActionNew_UltraFormManager_Dock_Area_Top);
            }
            return _result;
        }

        /// <summary>
        /// Opens the corporate actions.
        /// </summary>
        private void OpenCorporateActions()
        {
            try
            {
                PortfolioManagement.Click(MouseButtons.Left);
                CorporateActions.Click(MouseButtons.Left);
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
