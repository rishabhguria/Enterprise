using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using Microsoft.SqlServer.Management.Smo;
using System.Collections.Specialized;
using System.Data;
using System.Text.RegularExpressions;

namespace Middleware.Reference.Check
{
   
    class Program
    {                 

        static void Main(string[] args)
        {

            string[] Current = 
            {
                    "P_MW_ChangeInEquity", 
                    "P_MW_GetActivitySummary",
                    "P_MW_GetGenericPNL",
                    "P_MW_GetMTM",
                    "P_MW_GetRealizedPNL",
                    "P_MW_GetTradedCash",
                    "P_MW_GetTransactions",
                    "P_MW_GetTSR",
                    "P_MW_GetUnrealizedPNL",
                    "P_MW_OptRiskByUnderlying",
                    "P_MW_RunDailyPnL", 
                    "P_MW_RunDailyTransactions", 
                    "P_MW_SetMTMprefs",
                    "P_MW_SetRealizedPrefs",
                    "P_MW_SetUnrealizedPrefs", 
                    "P_MW_SetValuationPrefs"
            };

            string[] Ignore = 
            {
                "P_GetGenericPNL_UpdatedV4",
                "P_GetGenericPNL_UpdatedV7",
                "P_GetDerivedData_Dailybatch_test"

            };

            string[] Other = 
            {
                "F_getFundCorrelation_KZ",
                "P_W_getVAMIDaily_KZ",
                "F_getMDReturnTEST",
                "F_D_GetExposureSeries"
            };

            Search.Exclude.AddRange(Current);
            Search.Exclude.AddRange(Ignore);

            //Search.Exclude.Add("P_TS_batch");

            //Search.Exclude.Add("P_GetGenericPNL_DailyBatch");
            //Search.Exclude.Add("P_GetDerivedData_Dailybatch");
            //Search.Exclude.Add("P_GetDerivedData_Dailybatch_test");

            //Search.Exclude.Add("P_GetDerivedData_ReRunDates");
            //Search.Exclude.Add("P_FillDailyReturns_ReRunDates");

            //Search.Exclude.Add("P_FillGreeksAndExposure");

            //Search.Exclude.Add("P_GetGenericPNL_UpdatedV4");
            //Search.Exclude.Add("P_GetGenericPNL_UpdatedV7");

            //Search.Exclude.Add("P_MergeWithBob");

            //Search.Exclude.Add("P_getDataFromVeda");
            //Search.Exclude.Add("P_GetTransactions_DailyBatch");
            //Search.Exclude.Add("P_MergeEODMapping");
            //Search.Exclude.Add("P_MergeMarkDataResults");
            
            //Search.Exclude.Add("P_MW_GetGenericPNL");
            //Search.Exclude.Add("P_MW_RunDailyPnL");
            
            Search search = new Search(false);
        }


    

    }
}
