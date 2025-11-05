using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
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
using System.Configuration;
using Nirvana.TestAutomation.Factory;
using Nirvana.TestAutomation.UIAutomation;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    class UploadStageOrders : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                
                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    _res.ErrorMessage = upload(testData, dr);
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
        private string upload(DataSet testData, DataRow dr)
        {
            try
            {
                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["StageImport"]);
                UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();
                bool iswindowAlreadyOpened = UIAutomationHelper.DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue.ToString());
                if (!iswindowAlreadyOpened)
                {
                    OpenBlotter();

                    UploadStageOrders.Click(MouseButtons.Left);
                }
                if (!String.IsNullOrEmpty(dr[TestDataConstants.COL_FILEPATH].ToString()))
                {
                    Browse.Click(MouseButtons.Left);
                    Filename1.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_SELECT_FILE].ToString());
                    ButtonOpen.WaitForVisible();
                    ButtonOpen.Click(MouseButtons.Left);
                }

                if (dr.Table.Columns.Contains("upload") && !String.IsNullOrEmpty(dr["upload"].ToString()))
                {
                    
                  Upload.Click(MouseButtons.Left);
                }

                if (dr.Table.Columns.Contains("cancel") && !String.IsNullOrEmpty(dr["cancel"].ToString()))
                {
                    
                  Cancel1.Click(MouseButtons.Left);
                }


            }
            catch (Exception)
            {
                throw;
            }
            
            return null;

        }
    }
}
