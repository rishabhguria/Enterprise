using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Steps.Admin.Scripts;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nirvana.TestAutomation.Steps.Admin
{
    class BasketPermission : IUIAutomationTestStep
    {
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                if (testData.Tables[0] == null || testData.Tables[0].Rows.Count <= 0)
                {
                    return _result;
                }
                List<string> UserID = new List<string>();

                DataTable companyIDUserIDList = null;
                try
                {
                    companyIDUserIDList = SqlUtilities.GetDataFromQuery("SELECT CompanyId,UserId FROM T_CA_OtherCompliancePermission", "Client");
                }
                catch
                {
                    _result.IsPassed = false;
                    throw new Exception("T_CA_OtherCompliancePermission failed.");
                }
                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    string CompanyWise = string.IsNullOrEmpty(dr["CompanyWise"].ToString()) ? "5" : dr["CompanyWise"].ToString();
                    UserID = string.IsNullOrEmpty(dr["UserID"].ToString()) ? new List<string>() : dr["UserID"].ToString().Split(',').ToList();
                    string disable = string.IsNullOrEmpty(dr["Disable"].ToString()) ? "" : "0";
                    string enable = string.IsNullOrEmpty(dr["Enable"].ToString()) ? "" : "1";
                    string isBasketComplianceEnabledCompany = !string.IsNullOrEmpty(dr["isBasketComplianceEnabledCompany"].ToString()) ? dr["isBasketComplianceEnabledCompany"].ToString() : "";
                   
                    if (UserID.Count > 0)
                    {
                    foreach (string id in UserID)
                    {
                        foreach (DataRow row in companyIDUserIDList.Rows)
                        {
                            string companyId = row["CompanyId"].ToString();
                            string userId = row["UserId"].ToString();

                            if (string.Equals(companyId, CompanyWise, StringComparison.OrdinalIgnoreCase) && string.Equals(userId, id, StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string statetoset = "";
                                    statetoset = string.IsNullOrEmpty(disable)?enable:disable;
                                    SQLQueriesConstants.DisableBCPermission(userId, companyId, companyIDUserIDList, statetoset, isBasketComplianceEnabledCompany);
                                }
                                catch { }
                            }
                        }
                    }
                    }
                    else
                    {
                        try
                        {
                            foreach (DataRow row in companyIDUserIDList.Rows)
                            {
                                string companyId = row["CompanyId"].ToString();

                                if (string.Equals(companyId, CompanyWise, StringComparison.OrdinalIgnoreCase))
                                {
                                    try
                                    {
                                        SQLQueriesConstants.DisableBCPermission("", companyId, companyIDUserIDList, string.IsNullOrEmpty(disable) ? enable:disable,"");
                                    }
                                    catch
                                    {
                                        _result.IsPassed = false;
                                        throw new Exception("T_CA_OtherCompliancePermission DisableBCPermission failed.");
                                    }
                                }

                            }
                        }
                        catch { }
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
            return _result;
        }
    }
}
