using System.Data;
using System.Linq;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    public class ReAllocateFromBlotterByAllocate : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                PranaApplication.BringToFrontOnAttach = false;
               // Wait(6000);
                 string message=string.Empty;
                 if (testData.Tables[0].Rows.Count > 0)
                 {
                     if (!string.IsNullOrEmpty(testData.Tables[0].Rows[0][TestDataConstants.COL_LOWERSTRIPMESSAGE].ToString()))
                     {
                         message = testData.Tables[0].Rows[0][TestDataConstants.COL_LOWERSTRIPMESSAGE].ToString();

                     }
                 }
                
                testData.Tables[0].Columns.Remove(TestDataConstants.COL_LOWERSTRIPMESSAGE);

                if (testData != null)
                {
                    try
                    {
                        DgBlotter2.MsaaObject.FindDescendantByName("OrderBindingList row 1", 4000).Click(MouseButtons.Right);
                    }
                    catch { }

                     bool isClicked = false;
                    //Wait(3000);

                    try
                    {
                        isClicked = pickFromMenuItem(PopupMenuContext, TestDataConstants.Allocate);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    if (isClicked == false)
                    {
                        DgBlotter2.MsaaObject.FindDescendantByName("OrderBindingList row 1", 4000).Click(MouseButtons.Right);

                        if (Allocate.IsVisible)
                            Allocate.Click();
                        //Wait(2000);
                        else
                        {
                            Console.WriteLine("Menu Item (Allocate) is not visible");
                        }
                    }
                    if (NirvanaAlert.IsVisible)
                    {
                        ButtonOK3.Click(MouseButtons.Left);
                    }
                    else if (ViewAllocationDetails3.IsVisible && testData.Tables[0].Rows.Count == 0)
                    {
                        KeyboardUtilities.CloseWindow(ref DockTop2);

                    }
                    else if (testData.Tables[0].Columns.Contains(TestDataConstants.COL_AllocateButtonNotPresent) && testData.Tables[0].Rows[0][TestDataConstants.COL_AllocateButtonNotPresent].ToString().ToUpper().Equals("TRUE"))
                    {
                        if (Button1.IsVisible)
                        {
                            throw new Exception("Allocate Button visible");
                        }
                        else
                        {
                            Console.WriteLine("Allocate Button Not Visible");
                        }

                        if (DockTop2.IsVisible)
                        {
                            KeyboardUtilities.CloseWindow(ref DockTop2);
                        }
                    }
                    else if (testData.Tables[0].Rows[0][TestDataConstants.COL_PREFERENCE_NAME].ToString() != String.Empty)
                    {
                        DataRow dr = testData.Tables[0].Rows[0];
                        UltraComboEditorSymbology.Click(MouseButtons.Left);
                        UltraComboEditorSymbology.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_PREFERENCE_NAME].ToString();
                        //Keyboard.SendKeys(dr[TestDataConstants.COL_TARGET_ALLOCATION_PERCENTAGE].ToString());
                        Button1.Click(MouseButtons.Left);
                       // Wait(2000);
                        if (DockTop2.IsVisible)
                        {
                            KeyboardUtilities.CloseWindow(ref DockTop2);
                        }
                    }
                    else
                    {

                        InputEnter(testData, sheetIndexToName, message);
                        Button1.Click(MouseButtons.Left);
                        bool stripmessage = VerifyStripMessage(message);

                      //  Wait(5000);

                        if (DockTop2.IsVisible)
                        {
                            KeyboardUtilities.CloseWindow(ref DockTop2);
                        }

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
            finally
            {
                
                CloseBlotter();
            }
            return _res;
        }


        private void InputEnter(DataSet testData, Dictionary<int, string> sheetIndexToName,string message)
        {
            try
            {
                List<string> AvailableAccounts = ReturnAvailableAccounts();
                int rowcount = GridViewAllocation1.RowCount;
                //delete allocated accounts first
                if (rowcount > 0)
                {
                    DeleteAlreadyAllocated(GridViewAllocation1, rowcount);
                    //Wait(6000);
                }

              //  int index = 0; 
                var colheaders = GridViewAllocation1.MsaaObject.FindDescendantByName("Column Headers", 4000);
                
                int columncount = GridViewAllocation1.ColumnCount;
                
                Dictionary<string, int> indexToColumnMapDictionary = new Dictionary<string, int>();
               // Wait(2000);
                for (int colIndex = 0; colIndex < columncount; colIndex++)
                {
                    if (indexToColumnMapDictionary.ContainsKey(colheaders.CachedChildren[colIndex].Name))
                    {
                        indexToColumnMapDictionary.Add(colheaders.CachedChildren[colIndex].Name + '2', colIndex);
                    }
                    else
                    {
                        indexToColumnMapDictionary.Add(colheaders.CachedChildren[colIndex].Name, colIndex);
                    }
                }


                if (testData.Tables[0].Rows.Count > 0)
                {

                    var RowAdder = GridViewAllocation1.MsaaObject.FindDescendantByName("Template Add Row", 8000);
                     int rowsize = 0;
                     bool validaccount = true; 
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        var RowAdder1 = GridViewAllocation1.MsaaObject;
                        var table = GridViewAllocation1.MsaaObject.FindDescendantByName("Table", 4000);
                        
                       // var account = RowAdder.FindDescendantByName("Account", 4000);
                       // var target_percent = RowAdder.FindDescendantByName("Target Allocation %", 4000);
                        //var target_quantity = RowAdder.FindDescendantByName("Target Allocation Quantity", 4000);
                        var account = table;
                        if (rowsize == 0)
                        {
                            account = RowAdder.FindDescendantByName("Account", 4000);

                        }
                        else if (rowsize >0)
                        {
                            if (validaccount == false)
                            {
                                account = table.FindDescendantByName("Template Add Row", 4000).FindDescendantByName("Account", 4000);
                                if (AvailableAccounts.Contains(dr[TestDataConstants.COL_Account].ToString()))
                                {
                                    validaccount = true;
                                }
 
                            }

                            else if (AvailableAccounts.Contains(dr[TestDataConstants.COL_Account].ToString()))
                            {
                                account = table.FindDescendantByName("Add Row", 4000).FindDescendantByName("Account", 4000);
                            }
                            else
                            {
                                account = table.FindDescendantByName("Add Row", 4000).FindDescendantByName("Account", 4000);
                                validaccount=false;
 
                            }
                       
                        }
                       // var target_percent = RowAdder.FindDescendantByName("Target Allocation %", 4000);
                        //var target_quantity = RowAdder.FindDescendantByName("Target Allocation Quantity", 4000);


                        if (!String.IsNullOrEmpty(dr[TestDataConstants.COL_Account].ToString()))
                        {

                            //RowAdder.CachedChildren[indexToColumnMapDictionary[TestDataConstants.COL_Account]].Click(MouseButtons.Left);              
                            account.Click(MouseButtons.Left);
                            Keyboard.SendKeys(dr[TestDataConstants.COL_Account].ToString());
                            KeyboardUtilities.PressKey(3, KeyboardConstants.DELETE_KEY);
                            if (AvailableAccounts.Contains(dr[TestDataConstants.COL_Account].ToString()))
                            {
                                if (!String.IsNullOrEmpty(dr[TestDataConstants.COL_TARGET_ALLOCATION_PERCENTAGE].ToString()) && String.IsNullOrEmpty(dr[TestDataConstants.COL_TARGETALLOCATIONQUANTITY].ToString())) //not empty allocation percentage && empty allocation quantity
                                {
                                    KeyboardUtilities.PressKey(4, KeyboardConstants.TABKEY);
                                    Keyboard.SendKeys(dr[TestDataConstants.COL_TARGET_ALLOCATION_PERCENTAGE].ToString());
                                    KeyboardUtilities.PressKey(2, KeyboardConstants.TABKEY);

                                }

                                if (!String.IsNullOrEmpty(dr[TestDataConstants.COL_TARGETALLOCATIONQUANTITY].ToString()) && String.IsNullOrEmpty(dr[TestDataConstants.COL_TARGET_ALLOCATION_PERCENTAGE].ToString()))// empty allocation percentage && not empty allocation quantity
                                {
                                    KeyboardUtilities.PressKey(5, KeyboardConstants.TABKEY);
                                    Keyboard.SendKeys(dr[TestDataConstants.COL_TARGETALLOCATIONQUANTITY].ToString());
                                    KeyboardUtilities.PressKey(1, KeyboardConstants.TABKEY);
                                }
                                if (!String.IsNullOrEmpty(dr[TestDataConstants.COL_TARGETALLOCATIONQUANTITY].ToString()) && !String.IsNullOrEmpty(dr[TestDataConstants.COL_TARGET_ALLOCATION_PERCENTAGE].ToString()))// empty allocation percentage && empty allocation quantity
                                {
                                    KeyboardUtilities.PressKey(6, KeyboardConstants.TABKEY);
                                    Wait(2000);
                                }
                                if (String.IsNullOrEmpty(dr[TestDataConstants.COL_TARGETALLOCATIONQUANTITY].ToString()) && String.IsNullOrEmpty(dr[TestDataConstants.COL_TARGET_ALLOCATION_PERCENTAGE].ToString()))// not empty allocation percentage && not empty allocation quantity
                                {
                                    KeyboardUtilities.PressKey(4, KeyboardConstants.TABKEY);
                                    Keyboard.SendKeys(dr[TestDataConstants.COL_TARGET_ALLOCATION_PERCENTAGE].ToString());
                                    Wait(2000);
                                    KeyboardUtilities.PressKey(1, KeyboardConstants.TABKEY);
                                    Keyboard.SendKeys(dr[TestDataConstants.COL_TARGETALLOCATIONQUANTITY].ToString());
                                    KeyboardUtilities.PressKey(1, KeyboardConstants.TABKEY);

                                }



                            }
                            else//incorrect account
                            {
                                RowAdder.CachedChildren[indexToColumnMapDictionary[TestDataConstants.COL_Account]].Click(MouseButtons.Left);
                                Keyboard.SendKeys(dr[TestDataConstants.COL_Account].ToString());
                                KeyboardUtilities.PressKey(4, KeyboardConstants.TABKEY);
                            }


                        }
                        else
                        {
                            //code can changed if certain scenario needs account section empty...however not a ideal scenario
                        }
                        rowsize++;

                    }
                }
                



            }
            catch (Exception)
            {
                throw;
            }
        
        }

        private List<string> ReturnAvailableAccounts()
        {
            List<string> PossibleAccounts = new List<string>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["PranaConnectionString"].ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT FundName FROM T_CompanyFunds", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                PossibleAccounts.Add(reader.GetString(0));

                            }

                        }
                    }
                }
              return PossibleAccounts;
     
            }
        
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return PossibleAccounts;
        }

        private void DeleteAlreadyAllocated(UIUltraGrid GridViewAllocation1, int rowcount)
        {

            try
            {
                
                var maingridbefore = GridViewAllocation1.MsaaObject.FindDescendantByName("Table", 4000);

                while (rowcount > 0)
                {
                    var maingrid = GridViewAllocation1.MsaaObject.FindDescendantByName("Table row "+rowcount, 4000);
                    maingrid.Click(MouseButtons.Right);
                    Wait(3000);
                    Delete.Click(MouseButtons.Left);
                    rowcount--;
                }
                var maingridafter = GridViewAllocation1.MsaaObject.FindDescendantByName("Table", 4000);
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }

        }

        private bool  VerifyStripMessage(string message)
        {
            var msaaObj = StatusStrip12.MsaaObject;
            string data;
           bool flag = false;
                int child = msaaObj.ChildCount;
            try
            {
                if (!String.IsNullOrEmpty(message))
                {
                    if(child>0)
                    {
                     data = msaaObj.CachedChildren[0].Name.ToString();
                        if(message.ToUpper().Equals("YES"))
                        {
                            Console.WriteLine("Yes strip contains message");
                            flag = true;
                        }

                        else if (data.Contains(message))
                        {
                            Console.WriteLine("Yes strip contains exact message");
                            flag = true;
 
                        }

                    }
                    else if(child==0)
                    {
                        if(message.ToUpper().Equals("NO"))
                        {
                            Console.WriteLine("Yes strip doesnot contains message");
                        }

                    }

                    else if (!String.IsNullOrEmpty(message) && child > 0)
                    {
                        throw new Exception("Strip Doesn't contains any message");
                    }

                }
                
            }
            catch (Exception ex)
            {
              
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return flag;
        }
        /// <summary>
        /// Disposes resources
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
    
}
