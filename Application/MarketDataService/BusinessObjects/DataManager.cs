using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Collections.Specialized;

namespace BusinessObjects
{
    /// <summary>
    /// Class to fetch data from NirvanaDA service
    /// </summary>
    public static class DataManager
    {
        private static DbCommand _cmd = null;

        /// <summary>
        /// The database
        /// </summary>
        private static Database _db = DatabaseFactory.CreateDatabase();

        /// <summary>
        /// Pick up the queries while initialization from config.
        /// TODO : Move it to stored procedure later on
        /// </summary>
        /// <param name="sqlQuery"></param>
        static DataManager()
        {
            try
            {
                string sqlQueryUpdated = ConfigurationManager.AppSettings["SqlQuery"].Replace("\r\n", " "); //.Replace("&lt;", "<").Replace("&gt;", ">");
                _cmd = new SqlCommand(sqlQueryUpdated);
                _cmd.CommandTimeout = 60;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        //        -- TODO :	1) Ideally exchange should come already in T_PMDataDump. Until then, applied the logic to pick up any international exchange
        //        --				coming after the - [eSignal notation]
        //        --			2) Need to include the currency as well in the similar manner 
        //        --			It returns, symbol, Asset Class, Exchange
        //static string openSymbolsQuery = "Select distinct CASE WHEN ISNULL(OMI.UserProxySymbolUsed, 0) = 1 THEN [dump].ProxySymbol ELSE [dump].Symbol END AS Symbol,[dump].BloombergSymbol,REPLACE([dump].[Asset Class],'EquitySwap','Equity') as Asset, [dump].Exchange from dbo.T_PMDataDump [dump] with(nolock) Left Join T_UserOptionModelInput OMI ON OMI.Symbol = [dump].Symbol WHERE [dump].CreatedOn = (SELECT MAX (DD.CreatedOn) FROM T_PMDataDump DD where ISNULL([dump].CreatedOn,0) <> 0) union Select distinct [Underlying Symbol] as Symbol,[Underlying Symbol]+' US Equity' as BloombergSymbol, 'Equity' as Asset,'US' as Exchange from dbo.T_PMDataDump with(nolock) where [Asset Class] = 'EquityOption' and Isnull([Underlying Symbol],'') not in (Select distinct Symbol from T_PMDataDump) and CreatedOn = (SELECT MAX (DD.CreatedOn) FROM T_PMDataDump DD where ISNULL(DD.CreatedOn,0) <> 0)";
        
        public static DataTable GetOpenSymbolsData()
        {
            DataTable openSymbolsTable = null;
            try
            {
                //symbol  BloombergSymbol  Asset   Exchange
                //6502 - TSE    Equity TSE
                //AAPL AAPL US Equity Equity  

                DataSet openSymbolDataSet = _db.ExecuteDataSet(_cmd);
                if (openSymbolDataSet != null && openSymbolDataSet.Tables.Count > 0)
                {
                    openSymbolsTable = openSymbolDataSet.Tables[0];
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            return openSymbolsTable;
        }

    }
}
