using Prana.Admin.RoutingLogic.MisclFunctions;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Prana.Admin.RoutingLogic.BLL
{
    /// <summary>
    /// Summary description for DataCallFunctionsManager.
    /// </summary>
    public class DataCallFunctionsManager
    {
        private static System.Data.DataSet dsData; private static Prana.Admin.RoutingLogic.BLL.DataRoutingLogicObjects dataRL;
        private static string strMemoryID = "RL";
        private static string strTabID = "RL";
        private static int ipkCompanyID = -1;
        private static string strSeperator = "#"; /////instead of C in csv file, using this,   

        private DataCallFunctionsManager()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static void InitializeDataCall(ref System.Data.DataSet _dsData, ref Prana.Admin.RoutingLogic.BLL.DataRoutingLogicObjects _dataRL, int _iCompanyID)
        {
            dsData = _dsData;
            dataRL = _dataRL;
            ipkCompanyID = _iCompanyID;
        }

        public static System.Data.DataTable SaveRLGroup(string _strMemoryID, string _strTabID)
        {
            strMemoryID = _strMemoryID;
            strTabID = _strTabID;

            System.Data.SqlClient.SqlParameter[] _sqlParam = new System.Data.SqlClient.SqlParameter[6];

            int iGroupID = dataRL.ID(strTabID);
            string strName = dataRL.Name(strTabID);
            int iApplyRL = dataRL.ApplyRL(strTabID);
            //hash seprated values  //hsv
            string strClientIDHSV = "";
            string strApplyRLHSV = "";
            string strRLID = "";

            for (int i = 0; i < dataRL.ClientCount(strTabID); i++)
            {
                if (dataRL.ClientID(strTabID, i) >= 0)
                {
                    strClientIDHSV += dataRL.ClientID(strTabID, i).ToString() + strSeperator;
                    strApplyRLHSV += dataRL.ClientApplyRL(strTabID, i).ToString() + strSeperator;
                }
            }

            for (int i = 0; i < dataRL.RoutingPathCount(strTabID); i++)
            {
                if (dataRL.RoutingPathID(strTabID, i) >= 0)
                {
                    strRLID += dataRL.RoutingPathID(strTabID, i).ToString() + strSeperator;

                }
            }

            _sqlParam[0] = new System.Data.SqlClient.SqlParameter("@GroupID", iGroupID);
            _sqlParam[1] = new System.Data.SqlClient.SqlParameter("@Name", strName);
            _sqlParam[2] = new System.Data.SqlClient.SqlParameter("@ApplyRLGroup", iApplyRL);
            _sqlParam[3] = new System.Data.SqlClient.SqlParameter("@ClientIDHSV", strClientIDHSV);
            _sqlParam[4] = new System.Data.SqlClient.SqlParameter("@ApplyRLHSV", strApplyRLHSV);
            _sqlParam[5] = new System.Data.SqlClient.SqlParameter("@RLIDHSV", strRLID);

            //actula saving call
            System.Data.DataSet _dsTemp = BLL.DataHandelingManager.DataStoredProcedure("P_SaveRLGroup", _sqlParam);

            if (Functions.IsNull(_dsTemp) || Functions.IsNull(_dsTemp.Tables[0]))
            {
                return null;
            }


            return _dsTemp.Tables[0].Copy();



        }

        public static System.Data.DataTable SaveRLClient(string _strMemoryID, string _strTabID)
        {
            strMemoryID = _strMemoryID;
            strTabID = _strTabID;

            System.Data.SqlClient.SqlParameter[] _sqlParam = new System.Data.SqlClient.SqlParameter[5];

            int iClientID = dataRL.ID(strTabID);
            string strRLID = "";

            int iApplyRL = dataRL.ApplyRL(strTabID);

            int iAUECID = dataRL.AUECID(strTabID);

            for (int i = 0; i < dataRL.RoutingPathCount(strTabID); i++)
            {
                if (dataRL.RoutingPathID(strTabID, i) >= 0)
                {
                    strRLID += dataRL.RoutingPathID(strTabID, i).ToString() + strSeperator;

                }
            }

            _sqlParam[0] = new System.Data.SqlClient.SqlParameter("@ClientID", iClientID);
            _sqlParam[1] = new System.Data.SqlClient.SqlParameter("@ApplyRL", iApplyRL);
            _sqlParam[2] = new System.Data.SqlClient.SqlParameter("@RLIDHSV", strRLID);
            _sqlParam[3] = new SqlParameter("@AUECID", iAUECID);
            _sqlParam[4] = new SqlParameter("@CompanyID", ipkCompanyID);

            //actula saving call
            System.Data.DataSet _dsTemp = BLL.DataHandelingManager.DataStoredProcedure("P_SaveRLClient", _sqlParam);

            if (Functions.IsNull(_dsTemp.Tables[0]))
            {
                return null;
            }


            return _dsTemp.Tables[0].Copy();



        }


        public static System.Data.DataTable SaveRLogic(string _strMemoryID, string _strTabID, int iRoutingPathIndex)
        {
            strMemoryID = _strMemoryID;
            strTabID = _strTabID;
            //			int iRoutingPathIndex = _iRoutingPathIndex;
            System.Data.SqlClient.SqlParameter[] _sqlParam = new System.Data.SqlClient.SqlParameter[10];


            int iRLID = dataRL.RoutingPathID(strTabID, iRoutingPathIndex);
            string strRLName = dataRL.RoutingPathName(strTabID, iRoutingPathIndex);

            int iTradeAccDefaultID = dataRL.TradingAccountIDDefault(strTabID, iRoutingPathIndex);
            int iAUECID = dataRL.AUECID(strTabID);

            string strParameterID = "";
            string strParameterValue = "";
            string strJoinCondition = "";
            string strOperatorID = "";

            string strTradingAccID = "";
            string strCounterPartyVenueID = "";

            for (int i = 0; i < dataRL.ConditionsCount(strTabID, iRoutingPathIndex); i++)
            {
                if (dataRL.ParameterID(strTabID, iRoutingPathIndex, i) >= 0)
                {
                    strParameterID += dataRL.ParameterID(strTabID, iRoutingPathIndex, i).ToString() + strSeperator;
                    strParameterValue += dataRL.ParameterValue(strTabID, iRoutingPathIndex, i) + strSeperator;
                    strJoinCondition += dataRL.JoinConditonID(strTabID, iRoutingPathIndex, i).ToString() + strSeperator;
                    strOperatorID += dataRL.OperatorID(strTabID, iRoutingPathIndex, i).ToString() + strSeperator;
                }
            }

            for (int i = 0; i < dataRL.TradingAccountCount(strTabID, iRoutingPathIndex); i++)
            {
                if (dataRL.TradingAccountID(strTabID, iRoutingPathIndex, i) >= 0 || dataRL.CounterPartyVenueID(strTabID, iRoutingPathIndex, i) >= 0)
                {
                    strTradingAccID += dataRL.TradingAccountID(strTabID, iRoutingPathIndex, i).ToString() + strSeperator;

                    strCounterPartyVenueID += dataRL.CounterPartyVenueID(strTabID, iRoutingPathIndex, i).ToString() + strSeperator;
                }
            }


            _sqlParam[0] = new System.Data.SqlClient.SqlParameter("@RLID", iRLID);
            _sqlParam[1] = new System.Data.SqlClient.SqlParameter("@RLName", strRLName);
            _sqlParam[2] = new System.Data.SqlClient.SqlParameter("@AUECID", iAUECID);
            _sqlParam[3] = new System.Data.SqlClient.SqlParameter("@TradingAccountDefaultID", iTradeAccDefaultID);
            _sqlParam[4] = new System.Data.SqlClient.SqlParameter("@ParameterIDHSV", strParameterID);
            _sqlParam[5] = new System.Data.SqlClient.SqlParameter("@ParameterValueHSV", strParameterValue);
            _sqlParam[6] = new System.Data.SqlClient.SqlParameter("@JoinConditionHSV", strJoinCondition);
            _sqlParam[7] = new System.Data.SqlClient.SqlParameter("@OperatorIDHSV", strOperatorID);
            _sqlParam[8] = new System.Data.SqlClient.SqlParameter("@TradingAccIDHSV", strTradingAccID);
            _sqlParam[9] = new System.Data.SqlClient.SqlParameter("@CounterPartyVenueIDHSV", strCounterPartyVenueID);



            //actula saving call
            System.Data.DataSet _dsTemp = BLL.DataHandelingManager.DataStoredProcedure("P_SaveRLogic", _sqlParam);

            if (Functions.IsNull(_dsTemp) || Functions.IsNull(_dsTemp.Tables[0]))
            {
                return null;
            }

            dataRL.RoutingPathID(strTabID, iRoutingPathIndex, Convert.ToInt32(_dsTemp.Tables[0].Rows[0][0].ToString()));
            return _dsTemp.Tables[0].Copy();


            //
            //			System.Data.SqlClient.SqlParameter[] _sqlParam = new System.Data.SqlClient.SqlParameter[dsData.Tables["dtMemoryRL"].Columns.Count];
            //				
            //			System.Data.DataRow _row = dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID);
            //				
            //			string _strValue="" ;
            //			int _iValue=0;
            //
            //			for(int i=0;i<dsData.Tables["dtMemoryRL"].Columns.Count; i++)
            //			{
            //				if(_row[i].GetType().Equals( _strValue.GetType()))
            //				{
            //					_strValue = (Functions.IsNull(_row[i]))?"":_row[i].ToString();
            //					_sqlParam[i] = new System.Data.SqlClient.SqlParameter("@"+dsData.Tables["dtMemoryRL"].Columns[i].ColumnName.Trim(),_strValue);
            //				}
            //				else if(_row[i].GetType().Equals( _iValue.GetType()))
            //				{
            //					_iValue = (Functions.IsNull(_row[i]))?(Functions.MinValue):Convert.ToInt32(_row[i].ToString());
            //					_sqlParam[i] = new System.Data.SqlClient.SqlParameter("@"+dsData.Tables["dtMemoryRL"].Columns[i].ColumnName.Trim(),_iValue);
            //				}
            //				else   // null  system.dbnull
            //				{
            //					_sqlParam[i] = new System.Data.SqlClient.SqlParameter("@"+dsData.Tables["dtMemoryRL"].Columns[i].ColumnName.Trim(),Functions.MinValue);
            //				}
            //			}
            //
            //			//actula saving call
            //			System.Data.DataSet _dsTemp = BLL.DataHandelingManager.DataStoredProcedure("P_SaveRL",_sqlParam);
            //			
            //			if(Functions.IsNull(_dsTemp))
            //			{
            //				return null;
            //			}
            //			return _dsTemp.Tables[0].Copy();
        }



        //		public static bool LoadMemoryRL(string _strMemoryID, string _strTabID, out DataTable dtMemoryRL)
        //		{
        //
        //			strMemoryID = _strMemoryID;
        //			strTabID = _strTabID;
        //
        //
        //			System.Data.SqlClient.SqlParameter[] _sqlParam ;//= new System.Data.SqlClient.SqlParameter[];
        ////			DataColumn[] _dcPrimaryKey ;//= new DataColumn[];
        ////			System.Data.DataTable _dtResult ;
        //			int _ipkRLID = (Functions.IsNull(dsData.Tables["dtMemoryGroupClient"].Rows[0]["RLID0"]))?Functions.MinValue:int.Parse(dsData.Tables["dtMemoryGroupClient"].Rows[0]["RLID0"].ToString());
        //			_sqlParam = new System.Data.SqlClient.SqlParameter[] {new System.Data.SqlClient.SqlParameter("@RLID",_ipkRLID), new System.Data.SqlClient.SqlParameter("@MemoryID",strMemoryID)};
        //			dtMemoryRL = (BLL.DataHandelingManager.DataStoredProcedure("P_GetRL",_sqlParam)).Tables[0].Copy();
        //				DataColumn[] _dcPrimaryKey ;
        //			dtMemoryRL.TableName="dtMemoryRL";
        //			_dcPrimaryKey=  new DataColumn[] {dtMemoryRL.Columns["MemoryID"]};
        //			dtMemoryRL.PrimaryKey = _dcPrimaryKey;
        ////
        ////			return _dtResult;
        //
        //			if(Functions.IsNull(dtMemoryRL))
        //			{
        //				return false;
        //			}
        //
        //			//					dtMemoryRL.TableName="dtMemoryRL";
        //			
        //	
        //			if(dsData.Tables.Contains("dtMemoryRL"))
        //			{
        //				dsData.Tables["dtMemoryRL"].Dispose();
        //				dsData.Tables.Remove("dtMemoryRL");
        //			}
        //			dsData.Tables.Add(dtMemoryRL) ;
        //			
        //					
        //			//			_dcPrimaryKey=  new DataColumn[] {dtMemoryRL.Columns["MemoryID"]};
        //			//			this.dsData.Tables["dtMemoryRL"].PrimaryKey = _dcPrimaryKey;
        //		
        //			
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //			return true;
        //
        //		}



        //public static System.Data.DataTable SaveGroupClient(string _strMemoryID, string _strTabID)
        //{

        //    ////////  to be modified
        //    strMemoryID = _strMemoryID;
        //    strTabID = _strTabID;


        //    System.Data.SqlClient.SqlParameter[] _sqlParam = new System.Data.SqlClient.SqlParameter[dsData.Tables["dtMemoryRL"].Columns.Count];

        //    System.Data.DataRow _row = dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID);

        //    string _strValue="" ;
        //    int _iValue=0;

        //    for(int i=0;i<dsData.Tables["dtMemoryRL"].Columns.Count; i++)
        //    {
        //        if(_row[i].GetType().Equals( _strValue.GetType()))
        //        {
        //            _strValue = (Functions.IsNull(_row[i]))?"":_row[i].ToString();
        //            _sqlParam[i] = new System.Data.SqlClient.SqlParameter("@"+dsData.Tables["dtMemoryRL"].Columns[i].ColumnName.Trim(),_strValue);
        //        }
        //        else if(_row[i].GetType().Equals( _iValue.GetType()))
        //        {
        //            _iValue = (Functions.IsNull(_row[i]))?(Functions.MinValue):Convert.ToInt32(_row[i].ToString());
        //            _sqlParam[i] = new System.Data.SqlClient.SqlParameter("@"+dsData.Tables["dtMemoryRL"].Columns[i].ColumnName.Trim(),_iValue);
        //        }
        //        else   // null  system.dbnull
        //        {
        //            _sqlParam[i] = new System.Data.SqlClient.SqlParameter("@"+dsData.Tables["dtMemoryRL"].Columns[i].ColumnName.Trim(),Functions.MinValue);
        //        }
        //    }

        //    //actula saving call
        //    System.Data.DataSet _dsTemp = BLL.DataHandelingManager.DataStoredProcedure("P_SaveRL",_sqlParam);
        //    if(Functions.IsNull(_dsTemp))
        //    {
        //        return null;
        //    }

        //    return _dsTemp.Tables[0].Copy();
        //}



        public static void LoadDataTables(int _ipkCompanyID, out DataTable dtTree, out DataTable dtAUEC, out DataTable dtParameters, out DataTable dtCounterPartyVenue, out DataTable dtTradingAccount)
        {
            ipkCompanyID = _ipkCompanyID;

            System.Data.SqlClient.SqlParameter[][] _sqlParam = new System.Data.SqlClient.SqlParameter[8][];
            for (int i = 0; i < 8; i++)
            {
                _sqlParam[i] = new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@CompanyID", ipkCompanyID) };
            }

            //dsData.Tables.Add(
            dtTree = (BLL.DataHandelingManager.DataStoredProcedure("P_GetRLTree", _sqlParam[4])).Tables[0].Copy();
            dtTree.TableName = "dtTree";

            string[] _strKeyArray;
            char _cSeperatorKey = ':';
            string _strKeyTemp = "";
            foreach (System.Data.DataRow _row in dtTree.Rows)
            {
                _strKeyArray = _row["KeyID"].ToString().Split(_cSeperatorKey);
                _strKeyTemp = "";
                foreach (string _str in _strKeyArray)
                {
                    _strKeyTemp += _str.Trim() + _cSeperatorKey.ToString();
                }
                _row["KeyID"] = _strKeyTemp.Remove(_strKeyTemp.Length - 1, 1);
            }


            dtAUEC = (BLL.DataHandelingManager.DataStoredProcedure("P_GetRLAllAUECAssetUnderLyingExchange", _sqlParam[0])).Tables[0].Copy();
            dtAUEC.TableName = "dtAUEC";


            dtParameters = (BLL.DataHandelingManager.DataStoredProcedure("P_GetRLAllParameters", _sqlParam[1])).Tables[0].Copy();
            dtParameters.TableName = "dtParameters";


            dtCounterPartyVenue = (BLL.DataHandelingManager.DataStoredProcedure("P_GetRLAllCounterPartyVenues", _sqlParam[2])).Tables[0].Copy();
            dtCounterPartyVenue.TableName = "dtCounterPartyVenue";


            dtTradingAccount = (BLL.DataHandelingManager.DataStoredProcedure("P_GetRLAllTradingAccount", _sqlParam[3])).Tables[0].Copy();
            dtTradingAccount.TableName = "dtTradingAccount";



        }



        public static void LoadRLList(out DataTable dtRLList)
        {
            DataColumn[] _dcPrimaryKey = new DataColumn[1];

            dtRLList = (BLL.DataHandelingManager.DataStoredProcedure("P_GetRLList", null)).Tables[0].Copy();
            dtRLList.TableName = "dtRLList";

            _dcPrimaryKey[0] = dtRLList.Columns["RLID"];
            dtRLList.PrimaryKey = _dcPrimaryKey;


        }


        //		public static void LoadGroupClient( int _iClientID, int _iGroupID, out DataTable dtMemoryGroupClient)
        //		{
        //			System.Data.SqlClient.SqlParameter[] _sqlParam ;//= new System.Data.SqlClient.SqlParameter[];
        //			DataColumn[] _dcPrimaryKey ;
        //
        //			_sqlParam = new System.Data.SqlClient.SqlParameter[] {new System.Data.SqlClient.SqlParameter("@ClientID",_iClientID), new System.Data.SqlClient.SqlParameter("@GroupID",_iGroupID) };
        //			dtMemoryGroupClient = (BLL.DataHandelingManager.DataStoredProcedure("P_GetRLMemoryGroupClient",_sqlParam)).Tables[0].Copy();
        //			dtMemoryGroupClient.TableName="dtMemoryGroupClient";
        //					
        //			_dcPrimaryKey= new DataColumn[] {dtMemoryGroupClient.Columns["ID"]};
        //			dtMemoryGroupClient.PrimaryKey = _dcPrimaryKey;
        //
        //
        //
        //		}


        public static void LoadGroupClient(int _iClientID, int _iGroupID, string _strTabID, int _iAUECID)
        {
            const int iTableIndexIDNameApplyRL = 0;
            const int iTableIndexRLIDs = 1;
            int _iRank = 0;

            System.Data.SqlClient.SqlParameter[] _sqlParam;//= new System.Data.SqlClient.SqlParameter[];


            _sqlParam = new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@ClientID", _iClientID), new System.Data.SqlClient.SqlParameter("@GroupID", _iGroupID), new System.Data.SqlClient.SqlParameter("@AUECID", _iAUECID) };
            System.Data.DataSet _dsData = BLL.DataHandelingManager.DataStoredProcedure("P_GetRLGroupClient", _sqlParam);

            if (_dsData.Tables[0].Rows.Count == 0)
            {
                dataRL.ID(_strTabID, Functions.MinValue);
                dataRL.Name(_strTabID, "New");
                dataRL.ApplyRL(_strTabID, 0);
                int _iCountRLDefault = 1;
                dataRL.RoutingPathCount(_strTabID, _iCountRLDefault);

                _iRank = _iCountRLDefault - 1;

                LoadRL(_strTabID, Functions.MinValue, _iRank);

                return;
            }

            dataRL.ID(_strTabID, int.Parse(_dsData.Tables[iTableIndexIDNameApplyRL].Rows[0]["ID"].ToString()));
            dataRL.Name(_strTabID, _dsData.Tables[iTableIndexIDNameApplyRL].Rows[0]["Name"].ToString().Trim());
            dataRL.ApplyRL(_strTabID, int.Parse(_dsData.Tables[iTableIndexIDNameApplyRL].Rows[0]["ApplyRL"].ToString()));

            dataRL.RoutingPathCount(_strTabID, int.Parse(_dsData.Tables[iTableIndexRLIDs].Rows.Count.ToString()));

            int _iRLID = -1;

            foreach (System.Data.DataRow _row in _dsData.Tables[iTableIndexRLIDs].Rows)
            {
                _iRank = int.Parse(_row["Rank"].ToString()) - 1;
                _iRLID = int.Parse(_row["RLID"].ToString());
                LoadRL(_strTabID, _iRLID, _iRank);
            }
        }


        //		public static void SaveMemory(string _strMemoryID )
        //		{
        //			//			object[] parameter = new object[dsData.Tables["dtMemoryRL"].Columns.Count];
        //			System.Data.SqlClient.SqlParameter[] _sqlParam = new System.Data.SqlClient.SqlParameter[dsData.Tables["dtMemoryRL"].Columns.Count];
        //				
        //			System.Data.DataRow _row = dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryID);
        //				
        //			string _strValue="" ;
        //			int _iValue=0;
        //
        //			for(int i=0;i<dsData.Tables["dtMemoryRL"].Columns.Count; i++)
        //			{
        //				if(_row[i].GetType().Equals( _strValue.GetType()))
        //				{
        //					_strValue = (Functions.IsNull(_row[i]))?"":_row[i].ToString();
        //					_sqlParam[i] = new System.Data.SqlClient.SqlParameter("@"+dsData.Tables["dtMemoryRL"].Columns[i].ColumnName.Trim(),_strValue);
        //				}
        //				else if(_row[i].GetType().Equals( _iValue.GetType()))
        //				{
        //
        //					_iValue = (Functions.IsNull(_row[i]))?(Functions.MinValue):Convert.ToInt32(_row[i].ToString());
        //					_sqlParam[i] = new System.Data.SqlClient.SqlParameter("@"+dsData.Tables["dtMemoryRL"].Columns[i].ColumnName.Trim(),_iValue);
        //				}
        //				else   // null  system.dbnull
        //				{
        //					_sqlParam[i] = new System.Data.SqlClient.SqlParameter("@"+dsData.Tables["dtMemoryRL"].Columns[i].ColumnName.Trim(),Functions.MinValue);
        //				}
        //			}
        //
        //			//actula saving call
        //			BLL.DataHandelingManager.DataStoredProcedure("P_SaveRLMemory",_sqlParam);
        //
        //
        //
        //		}



        public static bool Delete(string _strSelectedKey, int iDeleteForceFully)//, int _iValue, int iDeleteForceFully)
        {
            const string _cPrefixRL = "r";
            const string _cPrefixClient = "c";
            const string _cPrefixGroup = "g";
            bool _bResult = false;
            //			DataTable _dtResult = null;
            DataSet _dsResult = null;

            System.Data.SqlClient.SqlParameter[] _sqlParam;
            string[] _strKeySelectedArray = _strSelectedKey.Split(':');
            int _iValue = Convert.ToInt32(_strKeySelectedArray[1]);
            string _strSelectedPrefix = _strKeySelectedArray[0];


            switch (_strSelectedPrefix)
            {
                case _cPrefixRL:

                    _sqlParam = new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@RLID", _iValue) };//, new System.Data.SqlClient.SqlParameter("@DeleteForceFully", iDeleteForceFully ) };
                    _dsResult = (BLL.DataHandelingManager.DataStoredProcedure("P_DeleteRLogic", _sqlParam));


                    break;
                case _cPrefixClient:

                    _sqlParam = new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@ClientID", _iValue), new System.Data.SqlClient.SqlParameter("@AUECID", Convert.ToInt32(_strKeySelectedArray["c:1:4".Split(':').Length - 1])), new System.Data.SqlClient.SqlParameter("@CompanyID", ipkCompanyID) };
                    _dsResult = (BLL.DataHandelingManager.DataStoredProcedure("P_DeleteRLCompanyClient", _sqlParam));



                    break;
                case _cPrefixGroup:
                    _sqlParam = new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@GroupID", _iValue) };
                    _dsResult = (BLL.DataHandelingManager.DataStoredProcedure("P_DeleteRLGroup", _sqlParam));



                    break;
                default:

                    break;
            }

            BLL.DataHandelingManager.DataStoredProcedure("P_DeleteRLClean", null);

            if ((_dsResult.Tables.Count <= 00))
            {
                _bResult = false;
            }
            else
            {
                _bResult = true;
            }

            return _bResult;
        }


        //		public static void LoadRL(string _strMemoryIDRL, int _iValue, out DataTable dtMemoryRL)
        //		{
        //
        //			System.Data.SqlClient.SqlParameter[] _sqlParam ;
        //
        //			
        //			//System.Data.SqlClient.SqlParameter[] _sqlParam ;//= new System.Data.SqlClient.SqlParameter[];
        //			DataColumn[] _dcPrimaryKey ;//= new DataColumn[];
        //
        //			_sqlParam = new System.Data.SqlClient.SqlParameter[] {new System.Data.SqlClient.SqlParameter("@RLID",_iValue), new System.Data.SqlClient.SqlParameter("@MemoryID",_strMemoryIDRL)};
        //			dtMemoryRL= (BLL.DataHandelingManager.DataStoredProcedure("P_GetRL",_sqlParam)).Tables[0].Copy();
        //			dtMemoryRL.TableName="dtMemoryRL";
        //					
        //			_dcPrimaryKey=  new DataColumn[] {dtMemoryRL.Columns["MemoryID"]};
        //			dtMemoryRL.PrimaryKey = _dcPrimaryKey;
        //
        //			if(dsData.Tables.Contains("dtMemoryRL"))
        //			{
        //				dsData.Tables["dtMemoryRL"].Dispose();
        //				dsData.Tables.Remove("dtMemoryRL");
        //			}
        //			dsData.Tables.Add(dtMemoryRL) ;
        //			/*********
        //			  new
        //			 * ******/
        //		}

        public static void LoadRL(string _strTabID, int _iValueRLID, int _RoutingPathIndex)
        {
            const int iTableIndexRoutingLogic = 0;
            const int iTableIndexVenues = 2;
            const int iTableIndexConditons = 1;
            System.Data.SqlClient.SqlParameter[] _sqlParam;
            int _iRank = 0;


            _sqlParam = new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@RLID", _iValueRLID) };
            System.Data.DataSet _dsData = BLL.DataHandelingManager.DataStoredProcedure("P_GetRLRLogic", _sqlParam);

            if (_dsData.Tables[0].Rows.Count == 0)
            {
                dataRL.RoutingPathID(_strTabID, _RoutingPathIndex, Functions.MinValue);
                dataRL.RoutingPathName(_strTabID, _RoutingPathIndex, "New");
                dataRL.AUECID(_strTabID, Functions.MinValue);
                dataRL.AssetID(_strTabID, 0);
                dataRL.UnderLyingID(_strTabID, 0);
                dataRL.ExchangeID(_strTabID, 0);

                dataRL.TradingAccountIDDefault(_strTabID, _RoutingPathIndex, Functions.MinValue);

                dataRL.ConditionsCount(_strTabID, _RoutingPathIndex, 1);

                _iRank = 0;

                dataRL.ParameterID(_strTabID, _RoutingPathIndex, _iRank, Functions.MinValue);
                dataRL.ParameterValue(_strTabID, _RoutingPathIndex, _iRank, "");
                dataRL.JoinConditonID(_strTabID, _RoutingPathIndex, _iRank, 0);
                dataRL.OperatorID(_strTabID, _RoutingPathIndex, _iRank, 0);

                dataRL.TradingAccountCount(_strTabID, _RoutingPathIndex, 0);

                return;
            }

            System.Data.DataRow _rowRL = _dsData.Tables[iTableIndexRoutingLogic].Rows[0];
            dataRL.RoutingPathID(_strTabID, _RoutingPathIndex, int.Parse(_rowRL["ID"].ToString()));
            dataRL.RoutingPathName(_strTabID, _RoutingPathIndex, _rowRL["Name"].ToString().Trim());
            dataRL.AUECID(_strTabID, int.Parse(_rowRL["AUECID"].ToString()));
            dataRL.AssetID(_strTabID, int.Parse(_rowRL["AssetID"].ToString()));
            dataRL.UnderLyingID(_strTabID, int.Parse(_rowRL["UnderLyingID"].ToString()));
            dataRL.ExchangeID(_strTabID, int.Parse(_rowRL["ExchangeID"].ToString()));

            dataRL.TradingAccountIDDefault(_strTabID, _RoutingPathIndex, int.Parse(_rowRL["TradingAccountIDDefault"].ToString()));

            System.Data.DataRowCollection _rowConditionCollection = _dsData.Tables[iTableIndexConditons].Rows;
            dataRL.ConditionsCount(_strTabID, _RoutingPathIndex, _rowConditionCollection.Count);


            object _obj;
            foreach (System.Data.DataRow _row in _rowConditionCollection)
            {
                _iRank = int.Parse(_row["Rank"].ToString()) - 1;

                dataRL.ParameterID(_strTabID, _RoutingPathIndex, _iRank, int.Parse(_row["ParameterID"].ToString()));
                dataRL.ParameterValue(_strTabID, _RoutingPathIndex, _iRank, _row["ParameterValue"].ToString().Trim());
                _obj = _row["JoinConditionID"];
                if (Functions.IsNull(_obj))
                {
                    _obj = -1;
                }
                dataRL.JoinConditonID(_strTabID, _RoutingPathIndex, _iRank, int.Parse(_obj.ToString()));
                _obj = _row["OperatorID"];
                if (Functions.IsNull(_obj))
                {
                    _obj = -1;
                }
                dataRL.OperatorID(_strTabID, _RoutingPathIndex, _iRank, int.Parse(_obj.ToString()));

            }

            dataRL.TradingAccountCount(_strTabID, _RoutingPathIndex, int.Parse(_dsData.Tables[iTableIndexVenues].Rows.Count.ToString()));
            foreach (System.Data.DataRow _row in _dsData.Tables[iTableIndexVenues].Rows)
            {
                _iRank = int.Parse(_row["Rank"].ToString()) - 1;
                if (_iRank > dataRL.TradingAccountCount(_strTabID, _RoutingPathIndex) - 1)
                {
                    dataRL.TradingAccountCount(_strTabID, _RoutingPathIndex, _iRank + 1);
                }

                _obj = _row["TradingAccountID"];
                if (Functions.IsNull(_obj))
                {
                    _obj = -1;
                }
                dataRL.TradingAccountID(_strTabID, _RoutingPathIndex, _iRank, Convert.ToInt32(_obj.ToString()));

                _obj = _row["CounterPartyVenueID"];
                if (Functions.IsNull(_obj))
                {
                    _obj = -1;
                }
                dataRL.CounterPartyVenueID(_strTabID, _RoutingPathIndex, _iRank, int.Parse(_obj.ToString()));

                _obj = _row["CounterPartyID"];
                if (Functions.IsNull(_obj))
                {
                    _obj = -1;
                }
                dataRL.CounterPartyID(_strTabID, _RoutingPathIndex, _iRank, int.Parse(_obj.ToString()));

                _obj = _row["VenueID"];
                if (Functions.IsNull(_obj))
                {
                    _obj = -1;
                }
                dataRL.VenueID(_strTabID, _RoutingPathIndex, _iRank, int.Parse(_obj.ToString()));

            }

        }




        public static void LoadClientList(int ipkCompanyID, int _iGroupID, out DataTable dtClientList)
        {
            System.Data.SqlClient.SqlParameter[] _sqlParam;//= new System.Data.SqlClient.SqlParameter[];


            _sqlParam = new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@CompanyID", ipkCompanyID), new System.Data.SqlClient.SqlParameter("@GroupID", _iGroupID) };
            dtClientList = (BLL.DataHandelingManager.DataStoredProcedure("P_GetRLClientList", _sqlParam)).Tables[0].Copy();
            dtClientList.TableName = "dtClientList";

            strTabID = "group";
            string _sSelect = "AUECID = " + dataRL.AUECID(strTabID).ToString() + " AND Checked = 1 ";
            bool _bGroupApplyTrue = true;
            bool _bGroupApplyFalse = true;
            int _iApplyRLClient = 0;
            foreach (System.Data.DataRow _row in dtClientList.Select(_sSelect))
            {
                _iApplyRLClient = Convert.ToInt32(_row["ApplyRL"].ToString());
                if (_iApplyRLClient == 0)
                {
                    _bGroupApplyTrue = false;
                }
                if (_iApplyRLClient == 1)
                {
                    _bGroupApplyFalse = false;
                }
                if (!(_bGroupApplyFalse) && !(_bGroupApplyTrue))
                {
                    break;
                }
            }

            if (_bGroupApplyFalse)
            {
                dataRL.ApplyRL(strTabID, 0);
            }
            else if (_bGroupApplyTrue)
            {
                dataRL.ApplyRL(strTabID, 1);
            }
            else
            {
                dataRL.ApplyRL(strTabID, 2);
            }
        }
























    }
}
