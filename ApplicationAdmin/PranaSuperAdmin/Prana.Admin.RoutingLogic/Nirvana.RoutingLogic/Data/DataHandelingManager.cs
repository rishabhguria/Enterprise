using Prana.Admin.RoutingLogic.MisclFunctions;
using Prana.LogManager;
using System;
using System.Data;

namespace Prana.Admin.RoutingLogic.BLL
{

    /// <summary>
    /// Summary description for DataHandelingManager.
    /// </summary>
    public class DataHandelingManager
    {
        //		private static SqlDataAdapter sqlDataAdapterRL;

        private DataHandelingManager()
        {

            //
            // TODO: Add constructor logic here
            //
        }
        //		#region Dataloading
        //		public static bool  DataLoad(out System.Data.DataSet _dsData)
        //		{
        //			try
        //			{
        //				_dsData=new System.Data.DataSet("RL Data");
        //
        //				string  _strConStr= @"workstation id=Abhimanyu;packet size=4096;user id=sa;pwd=sa;data source=Prana\Prana;persist security info=False;initial catalog=PranaAdmin";
        //
        //				//				string  _strSelect=	@"Select * From T_RoutingInstruction;
        //				//									  Select * From T_RoutingLogic;
        //				//									  Select * From T_RoutingLogicClientGroup;
        //				//									  Select * From T_RoutingLogicCondition;
        //				//									  Select * From T_RoutingLogicGroupClient;
        //				//									  Select * From T_RoutingLogicGroupRL;
        //				//									  Select * From T_RoutingLogicParamater;
        //				//									  Select * From T_RoutingLogicVenues;
        //				//									  Select * From T_AUEC;
        //				//									  Select * From T_CompanyClient;
        //				//									  Select * From T_CompanyTradingAccounts";
        //				//Select * From T_RoutingInstruction; Select * From T_RoutingLogic; Select * From T_RoutingLogicClientGroup; Select * From T_RoutingLogicCondition; Select * From T_RoutingLogicGroupClient; Select * From T_RoutingLogicGroupRL; Select * From T_RoutingLogicParamater; Select * From T_RoutingLogicVenues; Select * From T_AUEC; Select * From T_CompanyClient; Select * From T_CompanyTradingAccounts;Select * From T_ExecutionInstructions; Select * From T_HandlingInstructions; Select * From T_OrderType; Select * From T_Side; Select * From T_Symbol; Select * From T_Exchange; Select * From T_CompanyClientVenue; Select * From T_RoutingLogicClientGroupRL; Select * From T_AUECExchange; Select * From T_AUEC; Select * From T_CompanyUnderLying; Select * From T_UnderLying; Select * From T_Asset; Select * From T_CounterPartyVenue; Select * From T_CompanyCounterPartyVenues; Select * From T_Venue; Select * From T_CounterParty
        //
        //				//string  _strSelect=	"Select * From T_RoutingInstruction; Select * From T_RoutingLogic; Select * From T_RoutingLogicClientGroup; Select * From T_RoutingLogicCondition; Select * From T_RoutingLogicGroupClient; Select * From T_RoutingLogicGroupRL; Select * From T_RoutingLogicParamater; Select * From T_RoutingLogicVenues; Select * From T_AUEC; Select * From T_CompanyClient; Select * From T_CompanyTradingAccounts;Select * From T_ExecutionInstructions; Select * From T_HandlingInstructions; Select * From T_OrderType; Select * From T_Side; Select * From T_Symbol; Select * From T_Exchange; Select * From T_CompanyClientVenue; Select * From T_RoutingLogicClientGroupRL; Select * From T_AUECExchange; Select * From T_AUEC; Select * From T_CompanyUnderLying; Select * From T_UnderLying; Select * From T_Asset; Select * From T_CounterPartyVenue; Select * From T_CompanyCounterPartyVenues; Select * From T_Venue; Select * From T_CounterParty ";
        //				string  _strSelect=	"Select * From T_RoutingLogic; Select * From T_RoutingLogicClientGroup; Select * From T_RoutingLogicCondition; Select * From T_RoutingLogicGroupClient; Select * From T_RoutingLogicGroupRL; Select * From T_RoutingLogicParameter; Select * From T_RoutingLogicVenues; Select * From T_AUEC; Select * From T_CompanyClient; Select * From T_CompanyTradingAccounts;Select * From T_ExecutionInstructions; Select * From T_HandlingInstructions; Select * From T_OrderType; Select * From T_Side; Select * From T_Symbol; Select * From T_Exchange; Select * From T_CompanyClientVenue; Select * From T_RoutingLogicClientGroupRL; Select * From T_AUECExchange;  Select * From T_UnderLying; Select * From T_Asset; Select * From T_CounterPartyVenue; Select * From T_CompanyCounterPartyVenues; Select * From T_Venue; Select * From T_CounterParty; Select * From T_RoutingLogicCompanyClient; Select * From T_RoutingLogicMemoryRL; Select * From T_CompanyClientAUEC; Select * From T_CompanyVenue ";
        //				SqlConnection _sqlCon = new SqlConnection(_strConStr);
        //
        //				sqlDataAdapterRL =  new SqlDataAdapter(_strSelect,_sqlCon);
        //				new SqlCommandBuilder(sqlDataAdapterRL);
        //					
        //				int _iItr=0; 
        //				string _strTableID = "Table";
        //
        //				foreach(string _strQuery in _strSelect.Split(';'))
        //				{
        //					foreach(string _strTableName in _strQuery.Split(' '))
        //					{
        //						if(_strTableName.StartsWith("T_"))
        //						{
        //							if(_iItr != 0)
        //							{
        //								_strTableID="Table"+ _iItr.ToString();
        //							}
        //
        //							sqlDataAdapterRL.TableMappings.Add(  _strTableID,_strTableName);
        //							_iItr++;
        //						}
        //					}	
        //				}
        //			
        //					
        //				sqlDataAdapterRL.Fill(_dsData);
        //										
        //				return true;
        //			}
        //			catch(Exception ex)
        //			{
        //				string s = ex.Message + ex.Source + ex.StackTrace;
        //
        //				_dsData = null;
        //				return false ;
        //			}
        //
        //		}
        //		#endregion

        //		#region Datasaving for all data
        //		public static bool DataSave( ref System.Data.DataSet _dsData, ref BLL.DataRoutingLogicObjects _dataRL)
        //		{
        //			try
        //			{
        //				sqlDataAdapterRL.Update(_dsData);
        //
        //				return true;
        //			}
        //			catch(Exception ex)
        //			{
        //				string s = ex.Message + ex.Source + ex.StackTrace;
        //				return false ;
        //			}
        //		}
        //		#endregion

        //		#region Datasaving for particular tabel
        //		public static bool DataSave( ref System.Data.DataSet _dsData, ref BLL.DataRoutingLogicObjects _dataRL, string _strTableName)
        //		{
        //			try
        //			{
        //				sqlDataAdapterRL.Update(_dsData, _strTableName);
        //
        //				return true;
        //			}
        //			catch(Exception ex)
        //			{
        //				string s = ex.Message + ex.Source + ex.StackTrace;
        //				return false ;
        //			}
        //		}
        //		#endregion

        //		#region Create memeory & default table in dataset
        //
        //		public static void FillDefaultTable(ref System.Data.DataSet _dsData, ref BLL.DataRoutingLogicObjects _dataRL)
        //        {
        //			foreach( System.Data.DataColumn _dtcol in _dsData.Tables["T_RoutingLogicMemoryRL"].Columns)
        //			{
        //				(_dsData.Tables["T_RoutingLogicMemoryRL"].Rows[1])[_dtcol.ColumnName]=_dsData.Tables["T_RoutingLogicMemoryRL"].Rows[0][_dtcol.ColumnName];
        //				
        //			}
        //
        //		}
        //
        //		#endregion

        #region Data-SPs
        //public static System.Data.DataSet  DataStoredProcedure1(string _strSPName, System.Data.SqlClient.SqlParameter[] _sqlSPParam)
        //{
        //    try
        //    {
        //        System.Data.DataSet _dsResult= new DataSet("Result");

        //        string  _strConStr= @"workstation id=Abhimanyu;packet size=4096;user id=sa;pwd=sa;data source=Prana\Prana;persist security info=False;initial catalog=PranaAdmin";

        //        //string  _strSelect=	"Select * From T_RoutingLogic; Select * From T_RoutingLogicClientGroup; Select * From T_RoutingLogicCondition; Select * From T_RoutingLogicGroupClient; Select * From T_RoutingLogicGroupRL; Select * From T_RoutingLogicParameter; Select * From T_RoutingLogicVenues; Select * From T_AUEC; Select * From T_CompanyClient; Select * From T_CompanyTradingAccounts;Select * From T_ExecutionInstructions; Select * From T_HandlingInstructions; Select * From T_OrderType; Select * From T_Side; Select * From T_Symbol; Select * From T_Exchange; Select * From T_CompanyClientVenue; Select * From T_RoutingLogicClientGroupRL; Select * From T_AUECExchange;  Select * From T_UnderLying; Select * From T_Asset; Select * From T_CounterPartyVenue; Select * From T_CompanyCounterPartyVenues; Select * From T_Venue; Select * From T_CounterParty; Select * From T_RoutingLogicCompanyClient; Select * From T_RoutingLogicMemoryRL; Select * From T_CompanyClientAUEC; Select * From T_CompanyVenue ";
        //        SqlConnection _sqlCon = new SqlConnection(_strConStr);

        //        SqlCommand _sqlCommand = new SqlCommand(_strSPName, _sqlCon);
        //        _sqlCommand.CommandType = CommandType.StoredProcedure;

        //        if(_sqlSPParam != null)
        //        {
        //            foreach(System.Data.SqlClient.SqlParameter _sqlSPParameter in _sqlSPParam)
        //            {
        //                _sqlCommand.Parameters.Add(_sqlSPParameter);
        //            }
        //        }

        //        //_sqlCommand.Parameters.Add("@CategoryID", SqlDbType.Int).Value = 1;

        //        SqlDataAdapter _sqlDataAdapterRL = new SqlDataAdapter(_sqlCommand);

        //        new SqlCommandBuilder(_sqlDataAdapterRL);

        //        _sqlDataAdapterRL.Fill(_dsResult);

        //         return _dsResult ;
        //    }
        //    catch(Exception ex)
        //    {
        //        string s = ex.Message + ex.Source + ex.StackTrace;

        //        MessageBox.Show("DataBase Connection error");
        //        return null;
        //    }

        //}


        public static System.Data.DataSet DataStoredProcedure(string _strSPName, System.Data.SqlClient.SqlParameter[] _sqlSPParam)
        {
            System.Data.DataSet _dsResult = new DataSet("Result");
            //			object[] parameter = new object[_sqlSPParam.Length];

            try
            {
                if (!Functions.IsNull(_sqlSPParam))
                {
                    object[] parameter = new object[_sqlSPParam.Length];

                    for (int i = 0; i < _sqlSPParam.Length; i++)
                    {
                        parameter[i] = _sqlSPParam[i].Value;

                    }

                    _dsResult = DatabaseManager.DatabaseManager.ExecuteDataSet(_strSPName, parameter);
                }
                else
                {
                    object[] parameter = new object[1];
                    parameter[0] = Functions.MinValue;

                    _dsResult = DatabaseManager.DatabaseManager.ExecuteDataSet(_strSPName, parameter);

                }




                //
                //			try
                //			{
                //				_dsResult = db.ExecuteDataSet("P_GetCounterPartyVenuesForCounterParty", parameter) ;
                ////				u4sing(SqlDataAdapter _sqlDataAdapterRL = (SqlDataAdapter) db.ExecuteDataSet("P_GetCounterPartyVenuesForCounterParty", parameter))
                ////				{
                ////					_sqlDataAdapterRL.Fill(_dsResult);
                ////
                //////					_dsResult = _s
                //////					object[] row = new object[reader.FieldCount];
                //////					reader.GetValues(row);
                //////					venues.Add(FillVenues(row, 0));		
                ////				}
                //
                ////				SqlDataAdapter _sqlDataAdapterRL = new SqlDataAdapter(_sqlCommand);
                ////				
                ////				new SqlCommandBuilder(_sqlDataAdapterRL);
                ////										
                ////				_sqlDataAdapterRL.Fill(_dsResult);
                ////								
                //				
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, "Notify Policy");

                if (rethrow)
                {
                    throw;
                }
                return null;
            }
            #endregion
            return _dsResult;
        }

        #endregion


    }
}
