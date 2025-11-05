using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Nirvana.TestAutomation.BussinessObjects;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System.IO;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Steps.Rebalancer;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    class FetchAccountPositions:RebalancerUIMap,ITestStep
    {
        /// <summary>
        /// Run Test
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenRebalancer();
              //  Wait(4000);
                RebalancerTabButton.Click(MouseButtons.Left);

                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                    DataRow row = testData.Tables[sheetIndexToName[0]].Rows[0];
                    InputDataRebalancerToFetch(row);
                }

                Fetch1.Click(MouseButtons.Left);
                Fetch1.Click(MouseButtons.Left);
               
                Wait(4000);

                //Minimize Rebalancer
                KeyboardUtilities.MinimizeWindow(ref RebalanceTab);
            
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
        /// Add data to Rebalancer UI
        /// </summary>
        private void InputDataRebalancerToFetch(DataRow dr)
        {  
            try
            {
                string value=string.Empty;

                String VisibilityCheck = AccountVisibilityCheck();                
                String Check = VisibilityCheck.Trim('}');
                String[] Check2 = Check.Split(',');
                List<string> VisibilityValue = new List<string>();
                foreach (string i in Check2)
                    VisibilityValue.Add(i.Substring(i.IndexOf(':') + 1));

                if (!dr[TestDataConstants.COL_ACCOUNTGROUPS].ToString().Equals(String.Empty))
                {

                    KeyboardUtilities.MaximizeWindow(ref RebalanceTab);
                    while (TextBoxPresenter.Text.Length > 0)
                    {
                        TextBoxPresenter.Click(MouseButtons.Left);
                        DataUtilities.clearTextData();
                    }
                    Keyboard.SendKeys(dr[TestDataConstants.COL_ACCOUNTGROUPS].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                    Console.WriteLine("Fetch Account Position Done");
                }

               //Select Account Postions 
                if (!dr[TestDataConstants.COL_ACCOUNTPOSITIONS].ToString().Equals(String.Empty))
                {
                    CmbAccountPositions.Click(MouseButtons.Left);
                    Wait(4000);
                    value = dr[TestDataConstants.COL_ACCOUNTPOSITIONS].ToString();
                 
                    if (value.Equals("Real-Time Positions")) 
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);

                    if (value.Equals("Previous Day's Positions"))
                    {
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }
                }

                //select Calculation level
                if (!dr[TestDataConstants.COL_CALCULATIONLEVEL].ToString().Equals(String.Empty))
                {

                    value = dr[TestDataConstants.COL_CALCULATIONLEVEL].ToString();

                    CmbCalculationLevel.Click(MouseButtons.Left);
                    Wait(4000);

                    if (value.Equals("Account"))
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);

                    if (value.Equals("Master Fund"))
                    {
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }
                }

            
                //Select Refresh
                if (!dr[TestDataConstants.COL_REFRESHSIDE].ToString().Equals(String.Empty))
                {
                    value = dr[TestDataConstants.COL_REFRESHSIDE].ToString();

                    CmbRefresh.Click(MouseButtons.Left);
                    Wait(4000);

                    if (value.Equals("Positions"))
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);

                    if (value.Equals("Prices"))
                    {
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }

                    if (value.Equals("Positions and Prices"))
                    {
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);

                    }
                }
                Wait(2000);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        public static String AccountVisibilityCheck()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["PranaConnectionString"].ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Select preferenceValue from T_RebalPreferences where preferencekey = 'RebalAccountGroupVisibilityPref'", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            //String Data = "";
                            reader.Read();
                            String Data = reader.GetValue(0).ToString();
                            Console.WriteLine(Data);
                            return Data;

                            //return Data;
                        }
                    }
                }
            }
            catch (Exception) { throw; }
            
        }
       
    }
}
