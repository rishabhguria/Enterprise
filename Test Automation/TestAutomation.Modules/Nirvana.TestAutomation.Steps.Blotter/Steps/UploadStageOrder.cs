using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using TestAutomationFX.Core;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    class UploadStageOrder : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenBlotter();
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
            finally
            {
                CloseBlotter();
            }
            return _res;
        }


        private string upload(DataSet testData, DataRow dr)
        {
            try {
                UploadStageOrders.Click(MouseButtons.Left);
                if (!String.IsNullOrEmpty(dr[TestDataConstants.COL_SELECT_FILE].ToString()))
                {
                    Browse.Click(MouseButtons.Left);
                    Filename1.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_SELECT_FILE].ToString());
                    ButtonOpen.WaitForVisible();
                    ButtonOpen.Click(MouseButtons.Left);
                }

                if (!String.IsNullOrEmpty(dr[TestDataConstants.COL_ACTION].ToString()))
                {
                    if (dr[TestDataConstants.COL_ACTION].ToString().ToUpper().Equals("UPLOAD"))
                    {
                        Upload.Click(MouseButtons.Left);

                        if (!String.IsNullOrEmpty(dr[TestDataConstants.WrongFormatPopUp].ToString()))
                        {
                            if (dr[TestDataConstants.WrongFormatPopUp].ToString().ToUpper().Equals("TRUE"))
                            {
                                if (FileFormatError.IsVisible)
                                {
                                    ButtonOK7.Click(MouseButtons.Left);
                                    Cancel1.Click(MouseButtons.Left);
                                }
                                if (XMLValidation.IsVisible)
                                {
                                    Wait(1000);
                                    ButtonOK8.Click(MouseButtons.Left);
                                    KeyboardUtilities.CloseWindow(ref StageImport_UltraFormManager_Dock_Area_Top);
                                }

                            }

                        }
                        else
                        {
                            try
                            {
                                if (ImportPositionsDisplayForm.IsVisible)
                                {
                                    ImportPositionsDisplayForm.BringToFront();
                                    if (ValidationStatus.IsVisible)
                                    {
                                        ValidationStatus.Click(MouseButtons.Left);
                                    }
                                    // Unable to find children without any break point so update the condition to find the correct object by their name
                                    var mssaObj = GrdImportData1.MsaaObject.FindDescendantByName("Column Headers", 3000);
                                    mssaObj.FindDescendantByName("", 5000).Click(MouseButtons.Left);
                                    /*if (ColumnHeader.IsVisible)
                                    {
                                        ColumnHeader.Click(MouseButtons.Left);
                                    }*/
                                    if (testData.Tables[0].Columns.Contains(TestDataConstants.Order_Quantity))
                                    {

                                        if (!String.IsNullOrEmpty(dr[TestDataConstants.Order_Quantity].ToString()))
                                        {
                                            string[] arr = LblQuantity.MsaaObject.Name.Split(' ');
                                            if (!dr[TestDataConstants.Order_Quantity].ToString().Equals(arr[3]))
                                            {
                                                throw new Exception("Col Order Quantity contains " + dr[TestDataConstants.Order_Quantity].ToString() + " and ui is showing " + arr[3]);
                                            }

                                        }

                                        if (!String.IsNullOrEmpty(dr[TestDataConstants.Order_Count].ToString()))
                                        {
                                            string[] arr = LblCount.MsaaObject.Name.Split(' ');
                                            if (!dr[TestDataConstants.Order_Count].ToString().Equals(arr[3]))
                                            {
                                                throw new Exception("Col Order Count contains " + dr[TestDataConstants.Order_Count].ToString() + " and ui is showing " + arr[3]);
                                            }

                                        }
                                    }
                                    UltraButton1.WaitForVisible();
                                    UltraButton1.Click(MouseButtons.Left);

                                }
                                else if (ImportPositionsDisplayForm1.IsVisible)
                                {
                                    ImportPositionsDisplayForm1.BringToFront();
                                    if (ValidationStatus1.IsVisible)
                                    {
                                        ValidationStatus1.Click(MouseButtons.Left);
                                    }
                                    var mssaObj = GrdImportData1.MsaaObject.FindDescendantByName("Column Headers", 3000);
                                    mssaObj.FindDescendantByName("", 5000).Click(MouseButtons.Left);
                                    /*if (ColumnHeader2.IsVisible)
                                    {
                                        ColumnHeader2.Click(MouseButtons.Left);
                                    }*/
                                    if (testData.Tables[0].Columns.Contains(TestDataConstants.Order_Quantity))
                                    {

                                        if (!String.IsNullOrEmpty(dr[TestDataConstants.Order_Quantity].ToString()))
                                        {
                                            string[] arr = LblQuantity1.MsaaObject.Name.Split(' ');
                                            if (!dr[TestDataConstants.Order_Quantity].ToString().Equals(arr[3]))
                                            {
                                                throw new Exception("Col Order Quantity contains " + dr[TestDataConstants.Order_Quantity].ToString() + " and ui is showing " + arr[3]);
                                            }

                                        }

                                        if (!String.IsNullOrEmpty(dr[TestDataConstants.Order_Count].ToString()))
                                        {
                                            string[] arr = LblCount1.MsaaObject.Name.Split(' ');
                                            if (!dr[TestDataConstants.Order_Count].ToString().Equals(arr[3]))
                                            {
                                                throw new Exception("Col Order Count contains " + dr[TestDataConstants.Order_Count].ToString() + " and ui is showing " + arr[3]);
                                            }

                                        }
                                    }
                                    UltraButton11.WaitForVisible();
                                    UltraButton11.Click(MouseButtons.Left);
                                }
                            }
                            catch (Exception ex)
                            { Console.WriteLine(ex.Message);
                            throw;
                            }
                            
                            Wait(3000);
                        }


                    }
                    else if (dr[TestDataConstants.COL_ACTION].ToString().ToUpper().Equals("CANCEL"))
                    {
                        Cancel1.Click(MouseButtons.Left);
                    }

                }

                
            }
            catch (Exception)
            {
                throw;
            }
            //
            return null;
            
        }
    }
}
