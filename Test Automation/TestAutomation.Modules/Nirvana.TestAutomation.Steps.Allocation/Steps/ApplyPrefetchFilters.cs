using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.UI;
using TestAutomationFX.Core;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    public class ApplyPrefetchFilters : AllocationUIMap, ITestStep
    {
        /// <summary>
        /// Begins the test execution
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenAllocation();
                AllocationGrids1.Click(MouseButtons.Left);
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_X);
                string filterTabName = testData.Tables[sheetIndexToName[0]].Rows[0]["FilterTabName"].ToString();
                Filters1.Click(MouseButtons.Left);
                if (filterTabName.Equals("All", StringComparison.InvariantCultureIgnoreCase))
                {
                    All.Click(MouseButtons.Left);
                }
                else
                {
                 
                    AllocatedDivideUnallocated.Click(MouseButtons.Left);
                    if (filterTabName.Equals("Allocated", StringComparison.InvariantCultureIgnoreCase))
                    {
                        Cmbboxsymbol1.Click(MouseButtons.Left);
                        Wait(100);
                        int count =0;
                        while(count<36)
                        {
                            IncreaseBtn1.Click(MouseButtons.Left);
                            count++;
                        }
                    }
                }
                SetFilters(testData, sheetIndexToName);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "ApplyPrefetchFilters");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                MinimizeAllocation();
            }
            return _res;
        }

        /// <summary>
        /// common method to set filters in appropriate section of the application
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        private void SetFilters(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                string filterTabName = testData.Tables[sheetIndexToName[0]].Rows[0]["FilterTabName"].ToString();
                Dictionary<string, UIAutomationElement> map = new Dictionary<string, UIAutomationElement>();
                if (filterTabName.Equals("All", StringComparison.InvariantCultureIgnoreCase))
                    map = GetAllControlMap();
                else if (filterTabName.Equals("Unallocated", StringComparison.InvariantCultureIgnoreCase))
                    map = GetUnallocatedControlMap();
                else if (filterTabName.Equals("Allocated", StringComparison.InvariantCultureIgnoreCase))
                    map = GetAllocatedControlMap();
                DataTable test = testData.Tables[sheetIndexToName[0]];
                foreach (DataColumn col in test.Columns)
                {
                    if (test.Rows[0][col.ColumnName].ToString() != string.Empty && map.ContainsKey(col.ColumnName))
                    {
                        map[col.ColumnName].Click(MouseButtons.Left);
                        var x = XamComboEditorAAsset;
                        Wait(50);
                        map[col.ColumnName].Click(MouseButtons.Left);
                        string[] commaSeparatedComboValues = test.Rows[0][col.ColumnName].ToString().Split(',');
                        if (col.ColumnName.ToString() == "Symbol")
                        {
                            foreach (string combovalue in commaSeparatedComboValues)
                            {
                                string str = "," ;
                                Keyboard.SendKeys(combovalue);
                                Wait(1000);
                                Keyboard.SendKeys(str);
                                Wait(1000);
                               
                            }
                                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                                Wait(1000);
                        }
                        else
                        {
                            foreach (string combovalue in commaSeparatedComboValues)
                            {
                                Keyboard.SendKeys(combovalue);
                                Wait(1000);
                                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                                Wait(1000);
                            }
                        }
                    }
                }
                Apply1.Click(MouseButtons.Left);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// returns columnName to control map for Unallocated filter group
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, UIAutomationElement> GetUnallocatedControlMap()
        {
            try
            {
                Dictionary<string, UIAutomationElement> controlMap = new Dictionary<string, UIAutomationElement>();
                controlMap.Add("Symbol", Cmbboxsymbol5);
                controlMap.Add("Asset", XamComboEditor28);
                controlMap.Add("Side", XamComboEditor29);
                controlMap.Add("Broker", XamComboEditor30);
                controlMap.Add("Currency", XamComboEditor31);
                controlMap.Add("Exchange", XamComboEditor32);
                controlMap.Add("Underlying", XamComboEditor33);
                controlMap.Add("Venue", XamComboEditor34);
                controlMap.Add("TradingAccount", XamComboEditor35);
                controlMap.Add("PreAllocated", XamComboEditor36);
                controlMap.Add("ManualGroup", XamComboEditor37);
                return controlMap;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// <summary>
        /// returns columnName to control map for allocated filter group
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, UIAutomationElement> GetAllocatedControlMap()
        {
            try
            {
                Dictionary<string, UIAutomationElement> controlMap = new Dictionary<string, UIAutomationElement>();
                controlMap.Add("MasterFund", XamComboEditor15);
                controlMap.Add("Symbol", Cmbboxsymbol4);
                controlMap.Add("Asset", XamComboEditor16);
                controlMap.Add("Side", XamComboEditor17);
                controlMap.Add("Account", XamComboEditor18);
                controlMap.Add("Broker", XamComboEditor19);
                controlMap.Add("Currency", XamComboEditor20);
                controlMap.Add("Exchange", XamComboEditor21);
                controlMap.Add("StrategyName", XamComboEditor22);
                controlMap.Add("Underlying", XamComboEditor23);
                controlMap.Add("Venue", XamComboEditor24);
                controlMap.Add("TradingAccount", XamComboEditor25);
                controlMap.Add("PreAllocated", XamComboEditor26);
                controlMap.Add("ManualGroup", XamComboEditor27);
                return controlMap;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// <summary>
        /// returns columnName to control map for All filter group
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, UIAutomationElement> GetAllControlMap()
        {
            try
            {
                Dictionary<string, UIAutomationElement> controlMap = new Dictionary<string, UIAutomationElement>();
                controlMap.Add("MasterFund",XamComboEditor);
                controlMap.Add("Symbol",Cmbboxsymbol3 );
                controlMap.Add("Asset",XamComboEditor3 );
                controlMap.Add("Side",XamComboEditor4 );
                controlMap.Add("Account", XamComboEditor5);
                controlMap.Add("Broker", XamComboEditor6);
                controlMap.Add("Currency",XamComboEditor7);
                controlMap.Add("Exchange",XamComboEditor8);
                controlMap.Add("StrategyName",XamComboEditor9 );
                controlMap.Add("Underlying",XamComboEditor10 );
                controlMap.Add("Venue",XamComboEditor11);
                controlMap.Add("TradingAccount",XamComboEditor12 );
                controlMap.Add("PreAllocated", XamComboEditor13);
                controlMap.Add("ManualGroup", XamComboEditor14);
                return controlMap;
            }
            catch (Exception)
            {
                
                throw;
            }
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