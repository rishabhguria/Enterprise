using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAutomationFX.Core;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    public static class BlotterHelper
    {
        public static DataRow[] GetGridMatchedRowFromTestResults(DataTable dtGridDataTable, string matchcolumns, DataRow testDataRow, bool isVerifyData)
        {
            try
            {
                if (isVerifyData)
                {
                    return dtGridDataTable.Select(String.Format(matchcolumns, testDataRow[TestDataConstants.COL_SYMBOL], testDataRow[TestDataConstants.COL_QUANTITY],
                            testDataRow[TestDataConstants.COL_SIDE], testDataRow[TestDataConstants.COL_STATUS], testDataRow[TestDataConstants.COL_WORKING_QUANTITY], testDataRow[TestDataConstants.COL_EXECUTED_QUANTITY]));
                }
                else
                {
                    return dtGridDataTable.Select(String.Format(matchcolumns, testDataRow[TestDataConstants.COL_SYMBOL], testDataRow[TestDataConstants.COL_QUANTITY], testDataRow[TestDataConstants.COL_SIDE], testDataRow[TestDataConstants.COL_STATUS]));
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return null;
        }

    }
}

