#region Using namespaces
using System;
using System.Data;
using System.Data.SqlClient;
using Nirvana.BusinessObjects;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Nirvana.Global;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
#endregion

namespace Nirvana.PNL
{
	/// <summary>
	/// Summary description for PNLDataManager.
	/// </summary>
	public class PNLDataManager
	{
		private static readonly PNLDataManager instance = new PNLDataManager();
		
		private PNLDataManager()
		{
		}

		public static PNLDataManager GetInstance()
		{
			
			return instance;
		}

		#region Get PNL Main Order Detail by Asset ClassCommented

        //private Order FillOrderAUECDetails(object[] row, int offset)
        //{
        //    /// <summary>
        //    /// Fills the row of Order AUEC Detail to <see cref="Strategy"/> object.
        //    /// </summary>
        //    /// <param name="row">Datarow to be filled.</param>
        //    /// <param name="offset">offset</param>
        //    /// <returns>Object of <see cref="Strategy"/></returns>
        //    if (offset < 0)
        //    {
        //        offset = 0;
        //    }
        //    Order _order = null;
        //    try
        //    {
        //        if (row != null)
        //        {
        //            _order  = new Order();
        //            int  ASSET_NAME = offset + 0; 
        //            int  ASSET_ID = offset + 1; 
        //            int  UNDERLYING_NAME = offset + 2; 
        //            int  UNDERLYING_ID = offset + 3;
        //            //int  EXCHANGE_NAME = offset + 4;
        //            //int  EXCHANGE_ID = offset + 5;
        //            //int  CLORDER_ID = offset + 6; 
        //            int  AUEC_ID = offset + 4; 
				
        //            _order.AssetName			= row[ASSET_NAME].ToString();
        //            _order.AssetID				= Int32.Parse(row[ASSET_ID].ToString());
        //            _order.UnderlyingName		= row[UNDERLYING_NAME].ToString();
        //            _order.UnderlyingID			= Int32.Parse(row[UNDERLYING_ID].ToString());
        //            //_order.ExchangeName			= row[EXCHANGE_NAME].ToString();
        //            //_order.ExchangeID			= Int32.Parse(row[EXCHANGE_ID].ToString());
        //            //_order.ClOrderID			= row[CLORDER_ID].ToString();
        //            _order.AUECID				= Int32.Parse(row[AUEC_ID].ToString());
					
						
        //        }
        //    }
        //        #region Catch
        //    catch(Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, Common.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;			
        //        }
        //    }				
        //    #endregion
        //    return _order;	
        //}
        //public OrderCollection GetOrderAUECDetails()
        //{
        //    //TODO: Write SP to get OrderCollection for a user according to his permission.
        //    OrderCollection _orderCollection = new OrderCollection();
			
        //    Database db = DatabaseFactory.CreateDatabase();
        //    try
        //    {
        //        using(SqlDataReader reader = (SqlDataReader) db.ExecuteReader("P_GetPNLOrderAUECDetails"))
        //        {
        //            while(reader.Read())
        //            {
        //                object[] row = new object[reader.FieldCount];
        //                reader.GetValues(row);
        //                _orderCollection.Add(FillOrderAUECDetails(row, 0));		
        //            }
        //        }
        //    }
        //        #region Catch
        //    catch(Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, Common.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;			
        //        }
        //    }				
        //    #endregion
        //    return _orderCollection;
        //}	
		#endregion
		
		#region Get PNL Main Order Detail by Asset Class Commented

        //private Order FillViewByAssetClassOrderDetails(object[] row, int offset)
        //{
        //    /// <summary>
        //    /// Fills the row of Order AUEC Detail to <see cref="Strategy"/> object.
        //    /// </summary>
        //    /// <param name="row">Datarow to be filled.</param>
        //    /// <param name="offset">offset</param>
        //    /// <returns>Object of <see cref="Strategy"/></returns>
        //    if (offset < 0)
        //    {
        //        offset = 0;
        //    }
        //    Order _order = null;
        //    try
        //    {
        //        if (row != null)
        //        {
        //            _order  = new Order();
        //            int  CLORDER_ID = offset + 0; 
        //            int  AUEC_ID = offset + 1; 
        //            int  Asset_ID = offset + 2; 
        //            int  Asset_Name = offset + 3; 
        //            int  Underlying_ID = offset + 4; 
        //            int  Underlying_Name = offset + 5; 
        //            int  EXCHANGE_NAME = offset + 6;
        //            int  EXCHANGE_ID = offset + 7;
        //            int  CURRENCY_NAME = offset + 8;
        //            int  CURRENCY_ID = offset + 9;
        //            int  SUBORDER_ID = offset + 10; 
        //            int  EXE_OTY_ID = offset + 11; 
        //            int  QTY_ID = offset + 12; 
        //            int  AVG_PRICE = offset + 13; 
        //            int  SIDE = offset + 14; 
        //            int  SYMBOL = offset + 15;
					
        //            _order.ClOrderID			= row[CLORDER_ID].ToString();
        //            _order.AUECID				= Int32.Parse(row[AUEC_ID].ToString());
        //            _order.AssetID				= Int32.Parse(row[Asset_ID].ToString());
        //            _order.AssetName			= row[Asset_Name].ToString();
        //            _order.UnderlyingID			= Int32.Parse(row[Underlying_ID].ToString());
        //            _order.UnderlyingName		= row[Underlying_Name].ToString();
        //            _order.ExchangeName			= row[EXCHANGE_NAME].ToString();
        //            _order.ExchangeID			= Int32.Parse(row[EXCHANGE_ID].ToString());
        //            _order.CurrencyName			= row[CURRENCY_NAME].ToString();
        //            _order.CurrencyID			= Int32.Parse(row[CURRENCY_ID].ToString());
        //            //_order.					= row[SUBORDER_ID].ToString();
        //            _order.CumQty				= double.Parse( row[EXE_OTY_ID].ToString(), System.Globalization.NumberStyles.Float) ;// row[EXE_OTY_ID].ToString();
        //            _order.Quantity				=  double.Parse( row[QTY_ID].ToString(), System.Globalization.NumberStyles.Float) ;//Convert.ToDouble(row[QTY_ID]);
        //            _order.AvgPrice				=  double.Parse( row[AVG_PRICE].ToString(), System.Globalization.NumberStyles.Float) ;//Convert.ToDouble((row[AVG_PRICE]));
        //            _order.OrderSide			= row[SIDE].ToString();
        //            _order.Symbol				= row[SYMBOL].ToString();
					
						
        //        }
        //    }
        //        #region Catch
        //    catch(Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, Common.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;			
        //        }
        //    }				
        //    #endregion
        //    return _order;	
        //}

        //public OrderCollection GetViewByAssetClassOrderDetails(int UserID)
        //{
        //    //TODO: Write SP to get OrderCollection for a user according to his permission.
        //    OrderCollection _orderCollection = new OrderCollection();
			
        //    Database db = DatabaseFactory.CreateDatabase();
        //    try
        //    {
        //        object[] parameter = new object[1];
        //        parameter[0] = UserID;
        //        using(SqlDataReader reader = (SqlDataReader) db.ExecuteReader("P_GetPNLViewByAsset", parameter))
        //        {
        //            while(reader.Read())
        //            {
        //                object[] row = new object[reader.FieldCount];
        //                reader.GetValues(row);
        //                _orderCollection.Add(FillViewByAssetClassOrderDetails(row, 0));		
        //            }
        //        }
        //    }
        //        #region Catch
        //    catch(Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, Common.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;			
        //        }
        //    }				
        //    #endregion
        //    return _orderCollection;
        //}	
		#endregion

		#region Get PNL Data by Symbol Commented

        //private Order FillViewBySymbolOrderDetails(object[] row, int offset)
        //{
        //    /// <summary>
        //    /// Fills the row of Order AUEC Detail to <see cref="Strategy"/> object.
        //    /// </summary>
        //    /// <param name="row">Datarow to be filled.</param>
        //    /// <param name="offset">offset</param>
        //    /// <returns>Object of <see cref="Strategy"/></returns>
        //    if (offset < 0)
        //    {
        //        offset = 0;
        //    }
        //    Order _order = null;
        //    try
        //    {
        //        if (row != null)
        //        {
        //            _order  = new Order();

        //            int  SYMBOL = offset + 0;
        //            int  SIDE_ID = offset + 1;
        //            int  SIDE = offset + 2;
        //            int  EXE_OTY_ID = offset + 3; 
        //            int  AVG_PRICE = offset + 4; 
        //            int  CURRENCY_ID = offset + 5; 
					
        //            _order.Symbol				= row[SYMBOL].ToString();
        //            _order.OrderSide			= row[SIDE].ToString();
        //            _order.CumQty				=  double.Parse( row[EXE_OTY_ID].ToString(), System.Globalization.NumberStyles.Float) ;//row[EXE_OTY_ID].ToString();
        //            _order.AvgPrice				=  double.Parse( row[AVG_PRICE].ToString(), System.Globalization.NumberStyles.Float) ;//Convert.ToDouble((row[AVG_PRICE]));
        //            _order.CurrencyID			=  Convert.ToInt32(row[CURRENCY_ID]);
						
        //        }
        //    }
        //        #region Catch
        //    catch(Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, Common.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;			
        //        }
        //    }				
        //    #endregion
        //    return _order;	
        //}
		
        //public OrderCollection GetViewBySymbolDetails(int UserID)
        //{
        //    //TODO: Write SP to get OrderCollection for a user according to his permission.
        //    OrderCollection _orderCollection = new OrderCollection();
			
        //    Database db = DatabaseFactory.CreateDatabase();
        //    try
        //    {
        //        object[] parameter = new object[1];
        //        parameter[0] = UserID;
        //        using(SqlDataReader reader = (SqlDataReader) db.ExecuteReader("P_GetPNLViewBySymbol", parameter))
        //        {
        //            while(reader.Read())
        //            {
        //                object[] row = new object[reader.FieldCount];
        //                reader.GetValues(row);
        //                _orderCollection.Add(FillViewBySymbolOrderDetails(row, 0));		
        //            }
        //        }
        //    }
        //    #region Catch
        //    catch(Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, Common.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;			
        //        }
        //    }				
        //    #endregion
        //    return _orderCollection;
        //}	
		#endregion

		#region Get PNL Data by Trading Account Commented

        //private Order FillViewByTradingAccountOrderDetails(object[] row, int offset)
        //{
        //    /// <summary>
        //    /// Fills the row of Order AUEC Detail to <see cref="Strategy"/> object.
        //    /// </summary>
        //    /// <param name="row">Datarow to be filled.</param>
        //    /// <param name="offset">offset</param>
        //    /// <returns>Object of <see cref="Strategy"/></returns>
        //    if (offset < 0)
        //    {
        //        offset = 0;
        //    }
        //    Order _order = null;
        //    try
        //    {
        //        if (row != null)
        //        {
        //            _order  = new Order();

        //            int  TRADING_ACCOUNT_ID = offset + 0;
        //            int  TRADING_ACCOUNT_NAME = offset + 1;
        //            int  SYMBOL = offset + 2;
        //            int  SIDE_ID = offset + 3;
        //            int  SIDE = offset + 4;
        //            int  EXE_OTY_ID = offset + 5; 
        //            int  AVG_PRICE = offset + 6; 
        //            int  CURRENCY_ID = offset + 7; 

					
        //            _order.TradingAccountName	= row[TRADING_ACCOUNT_NAME].ToString();
        //            _order.Symbol				= row[SYMBOL].ToString();
        //            //_order.or					= row[SIDE_ID].ToString();
        //            _order.OrderSide			= row[SIDE].ToString();
        //            _order.CumQty				=  double.Parse( row[EXE_OTY_ID].ToString(), System.Globalization.NumberStyles.Float) ;//row[EXE_OTY_ID].ToString();
        //            _order.AvgPrice				= double.Parse( row[AVG_PRICE].ToString(), System.Globalization.NumberStyles.Float) ;//Convert.ToDouble((row[AVG_PRICE]));
        //            _order.CurrencyID			= Convert.ToInt32(row[CURRENCY_ID]);	
        //        }
        //    }
        //        #region Catch
        //    catch(Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, Common.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;			
        //        }
        //    }				
        //    #endregion
        //    return _order;
        //}

        //public OrderCollection GetViewByTradingAccountDetails(int UserID)
        //{
        //    TODO: Write SP to get OrderCollection for a user according to his permission.
        //    OrderCollection _orderCollection = new OrderCollection();
			
        //    Database db = DatabaseFactory.CreateDatabase();
        //    try
        //    {
        //        object[] parameter = new object[1];
        //        parameter[0] = UserID;
        //        using(SqlDataReader reader = (SqlDataReader) db.ExecuteReader("P_GetPNLViewByTradingAccount", parameter))
        //        {
        //            while(reader.Read())
        //            {
        //                object[] row = new object[reader.FieldCount];
        //                reader.GetValues(row);
        //                _orderCollection.Add(FillViewByTradingAccountOrderDetails(row, 0));		
        //            }
        //        }
        //    }
        //        #region Catch
        //    catch(Exception ex)
        //    {
        //         Invoke our policy that is responsible for making sure no secure information
        //         gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, Common.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;			
        //        }
        //    }				
        //    #endregion
        //    return _orderCollection;
        //}
        #endregion


        #region Get PNL Main Order Details

        private Order FillOrderDetails(object[] row, int offset)
		{
			/// <summary>
			/// Fills the row of Order AUEC Detail to <see cref="Strategy"/> object.
			/// </summary>
			/// <param name="row">Datarow to be filled.</param>
			/// <param name="offset">offset</param>
			/// <returns>Object of <see cref="Strategy"/></returns>
			if (offset < 0)
			{
				offset = 0;
			}
			Order _order = null;
			try
			{
				if (row != null)
				{
					_order  = new Order();

					int  CLORDER_ID = offset + 0; 
					int  TRADING_ACCOUNT_ID = offset + 1;
					int  TRADING_ACCOUNT_NAME = offset + 2;
					int  AUEC_ID = offset + 3; 
					int  ASSET_ID = offset + 4; 
					int  ASSET_NAME = offset + 5; 
					int  UNDERLYING_ID = offset + 6; 
					int  UNDERLYING_NAME = offset + 7; 
					int  EXCHANGE_ID = offset + 8;
					int  EXCHANGE_NAME = offset + 9;
					int  CURRENCY_ID = offset + 10;
					int  CURRENCY_NAME = offset + 11;
					int  EXE_OTY_ID = offset + 12; 
					//int  QUANTITY = offset + 13; 
					int  AVG_PRICE = offset + 13; 
					int  SYMBOL = offset + 14;
					int  SIDE = offset + 15; 
					int  SIDE_ID = offset + 16; 
					
					_order.ClOrderID			= row[CLORDER_ID].ToString();
					_order.TradingAccountID     = Int32.Parse(row[TRADING_ACCOUNT_ID].ToString());
					_order.TradingAccountName   = row[TRADING_ACCOUNT_NAME].ToString();
					_order.AUECID				= Int32.Parse(row[AUEC_ID].ToString());
					_order.AssetID				= Int32.Parse(row[ASSET_ID].ToString());
					_order.AssetName			= row[ASSET_NAME].ToString();
					_order.UnderlyingID			= Int32.Parse(row[UNDERLYING_ID].ToString());
					_order.UnderlyingName		= row[UNDERLYING_NAME].ToString();
					_order.ExchangeID			= Int32.Parse(row[EXCHANGE_ID].ToString());
					_order.ExchangeName			= row[EXCHANGE_NAME].ToString();
					_order.CurrencyID			= Int32.Parse(row[CURRENCY_ID].ToString());	
					_order.CurrencyName			= row[CURRENCY_NAME].ToString();
					_order.CumQty				=  double.Parse( row[EXE_OTY_ID].ToString(), System.Globalization.NumberStyles.Float) ;//row[EXE_OTY_ID].ToString();
					//_order.Quantity				= row[QUANTITY].ToString();
					_order.AvgPrice				=  double.Parse( row[AVG_PRICE].ToString(), System.Globalization.NumberStyles.Float) ;//Convert.ToDouble((row[AVG_PRICE]));
					_order.Symbol				= row[SYMBOL].ToString();
					_order.OrderSide			= row[SIDE].ToString();
				}
			}
				#region Catch
			catch(Exception ex)
			{
				// Invoke our policy that is responsible for making sure no secure information
				// gets out of our layer.
				bool rethrow = ExceptionPolicy.HandleException(ex, Common.POLICY_LOGANDTHROW);

				if (rethrow)
				{
					throw;			
				}
			}				
			#endregion
			return _order;	
		}

        public OrderCollection GetPNLOrderDetails(int UserID, string OrderByColumnName, long lastOrderSeqNumber)
		{
			//TODO: Write SP to get OrderCollection for a user according to his permission.
			OrderCollection _orderCollection = new OrderCollection();
			
			Database db = DatabaseFactory.CreateDatabase();
			try
			{
				object[] parameter = new object[3];
				parameter[0] = UserID;
				parameter[1] = OrderByColumnName;
                parameter[2] = lastOrderSeqNumber;

                System.Data.Common.DbCommand command = db.GetStoredProcCommand("P_GetOrderData", parameter);
                command.CommandTimeout = 100;
				using(SqlDataReader reader = (SqlDataReader) db.ExecuteReader(command)) //"P_GetOrderData", parameter))
				{
					while(reader.Read())
					{
						object[] row = new object[reader.FieldCount];
						reader.GetValues(row);
						_orderCollection.Add(FillOrderDetails(row, 0));		
					}
				}
			}
				#region Catch
			catch(Exception ex)
			{
				// Invoke our policy that is responsible for making sure no secure information
				// gets out of our layer.
				bool rethrow = ExceptionPolicy.HandleException(ex, Common.POLICY_LOGANDTHROW);

				if (rethrow)
				{
					throw;			
				}
			}				
			#endregion
			return _orderCollection;
		}	
		#endregion
	}
}
