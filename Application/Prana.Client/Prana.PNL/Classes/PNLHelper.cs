using System;
using Nirvana.BusinessObjects;

namespace Nirvana.PNL
{
	/// <summary>
	/// Summary description for PNLHelper.
	/// </summary>
	public class PNLHelper
	{
		public PNLHelper()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// to find the conversion factor between two different currency
		/// </summary>
		/// <param name="fromCurrencyID"></param>
		/// <param name="toCurrencyID"></param>
		/// <returns></returns>
		public static double findConversionFactor(CurrencyConversionCollection currencyConversionList, int fromCurrencyID, int toCurrencyID)
		{
			double factor = 1.0;
			int conversionCount = currencyConversionList.Count;
	
			for(int i=0;i<conversionCount;i++)
			{
				CurrencyConversion currencyConversion = (CurrencyConversion)currencyConversionList[i];

				if(currencyConversion.FromCurrencyID == fromCurrencyID && currencyConversion.ToCurrencyID == toCurrencyID)
				{
					factor = currencyConversion.CurrencyConversionFactor;
					break;
				}
				else if(currencyConversion.FromCurrencyID == toCurrencyID && currencyConversion.ToCurrencyID == fromCurrencyID)
				{
					factor = (1/currencyConversion.CurrencyConversionFactor);
					break;
				}

			}

			return factor;
		}
			
		/// <summary>
		/// Returns the column name of order by
		/// </summary>
		/// <param name="L2Column"></param>
		/// <returns></returns>
		public static string GetOrderByColumnName(int columnIndex)
		{
			string coulmnName = string.Empty;

			switch(columnIndex)
			{
				case (int)PNLColumns.Asset:
                    coulmnName = "AssetName" + "," + "UnderlyingName"; //"AUECID"; // Modified Rajat - 17-08-2006
					break;
		
				case (int)PNLColumns.Client:
					coulmnName = "ClientID"; //to be decided
					break;

				case (int)PNLColumns.User:
					coulmnName = "UserID"; //to be decided
					break;

				case (int)PNLColumns.Exchange:
					coulmnName = "ExchangeID";
					break;

				case (int)PNLColumns.Side:
					coulmnName = "OrderSideID";
					break;

				case (int)PNLColumns.Symbol:
					coulmnName = "Symbol";
					break;

				case (int)PNLColumns.TradingAccount:
					coulmnName = "TradingAccountID";
					break;
			}

			return coulmnName;
		}

		/// <summary>
		/// Return the value of L2 column 
		/// </summary>
		/// <param name="L2Column"></param>
		/// <param name="order"></param>
		/// <returns></returns>
		public static string GetColumnValue(int L2Column, Order order)
		{
			string coulmnValue = string.Empty;

			switch(L2Column)
			{
				case (int)PNLColumns.Asset:
                    coulmnValue = order.AssetName + " - " + order.UnderlyingName; //order.AUECID.ToString(); // Modified Rajat 17-08-2006
					break;
		
				case (int)PNLColumns.Client:
					coulmnValue = "1"; //to be decided
					break;

				case (int)PNLColumns.User:
					coulmnValue = "2"; //to be decided
					break;

				case (int)PNLColumns.Exchange:
					coulmnValue = order.ExchangeID.ToString();
					break;

				case (int)PNLColumns.Side:
					coulmnValue = order.OrderSide;
					break;

				case (int)PNLColumns.Symbol:
					coulmnValue = order.Symbol;
					break;

				case (int)PNLColumns.TradingAccount:
					coulmnValue = order.TradingAccountID.ToString();
					break;
			}

			return coulmnValue;
		}

		public static string GetDisplayColumnValue(int L2Column, Order order)
		{
			string coulmnValue = string.Empty;

			switch(L2Column)
			{
				case (int)PNLColumns.Asset:
					coulmnValue = order.AssetName+"-"+order.UnderlyingName;
					break;
		
				case (int)PNLColumns.Client:
					coulmnValue = "1"; //to be decided
					break;

				case (int)PNLColumns.User:
					coulmnValue = "2"; //to be decided
					break;

				case (int)PNLColumns.Exchange:
					coulmnValue = order.ExchangeName;
					break;

				case (int)PNLColumns.Side:
					coulmnValue = order.OrderSide;
					break;

				case (int)PNLColumns.Symbol:
					coulmnValue = order.Symbol;
					break;

				case (int)PNLColumns.TradingAccount:
					coulmnValue = order.TradingAccountName;
					break;
			}

			return coulmnValue;
		}

		/// <summary>
		/// Return the value of L2 column 
		/// </summary>
		/// <param name="L2Column"></param>
		/// <param name="order"></param>
		/// <returns></returns>
		public static string GetLevel2ColumnValue(int L2Column, PNLLevel2Data pnlLevel2Data)
		{
			string coulmnValue = string.Empty;

			switch(L2Column)
			{
				case (int)PNLColumns.Asset:
					coulmnValue = pnlLevel2Data.Asset+"-"+pnlLevel2Data.UnderLying;
					break;
		
				case (int)PNLColumns.Client:
					coulmnValue = "1"; //to be decided
					break;

				case (int)PNLColumns.User:
					coulmnValue = "2"; //to be decided
					break;

				case (int)PNLColumns.Exchange:
					coulmnValue = pnlLevel2Data.Exchange;
					break;

				case (int)PNLColumns.Side:
					coulmnValue = pnlLevel2Data.Side;
					break;

				case (int)PNLColumns.Symbol:
					coulmnValue = pnlLevel2Data.Symbol;
					break;

				case (int)PNLColumns.TradingAccount:
					coulmnValue = pnlLevel2Data.TradingAccount;
					break;
			}

			return coulmnValue;
		}

		/// <summary>
		/// Return the value of L2 column 
		/// </summary>
		/// <param name="L2Column"></param>
		/// <param name="order"></param>
		/// <returns></returns>
		public static string GetLevel1ColumnValue(int L1Column, PNLLevel1Data pnlLevel1Data)
		{
			string coulmnValue = string.Empty;

			switch(L1Column)
			{
				case (int)PNLColumns.Asset:
					coulmnValue = pnlLevel1Data.Asset+"-"+pnlLevel1Data.UnderLying;
					break;
		
				case (int)PNLColumns.Client:
					coulmnValue = "1"; //to be decided
					break;

				case (int)PNLColumns.User:
					coulmnValue = "2"; //to be decided
					break;

				case (int)PNLColumns.Exchange:
					coulmnValue = pnlLevel1Data.Exchange;
					break;

				case (int)PNLColumns.Side:
					coulmnValue = pnlLevel1Data.Side;
					break;

				case (int)PNLColumns.Symbol:
					coulmnValue = pnlLevel1Data.Symbol;
					break;

				case (int)PNLColumns.TradingAccount:
					coulmnValue = pnlLevel1Data.TradingAccount;
					break;
			}

			return coulmnValue;
		}

		/// <summary>
		/// Return the value of L2 column 
		/// </summary>
		/// <param name="L2Column"></param>
		/// <param name="order"></param>
		/// <returns></returns>
		public static string GetColumnDisplayValue(int L2Column, Order order)
		{
			string coulmnValue = string.Empty;

			switch(L2Column)
			{
				case (int)PNLColumns.Asset:
					coulmnValue = order.AssetName+"-"+order.UnderlyingName;
					break;
		
				case (int)PNLColumns.Client:
					coulmnValue = "1"; //to be decided
					break;

				case (int)PNLColumns.User:
					coulmnValue = "2"; //to be decided
					break;

				case (int)PNLColumns.Exchange:
					coulmnValue = order.ExchangeName.ToString();
					break;

				case (int)PNLColumns.Side:
					coulmnValue = order.OrderSide;
					break;

				case (int)PNLColumns.Symbol:
					coulmnValue = order.Symbol;
					break;

				case (int)PNLColumns.TradingAccount:
					coulmnValue = order.TradingAccountName.ToString();
					break;
			}

			return coulmnValue;
		}

		/// <summary>
		/// Return the value of L2 column 
		/// </summary>
		/// <param name="L2Column"></param>
		/// <param name="order"></param>
		/// <returns></returns>
		public static string GetChartDisplayValue(int L2Column, Order order)
		{
			string coulmnValue = string.Empty;

			switch(L2Column)
			{
				case (int)PNLColumns.Asset:
					coulmnValue = order.AssetName + " - " + order.UnderlyingName;
					break;
		
				case (int)PNLColumns.Client:
					coulmnValue = "1"; //to be decided
					break;

				case (int)PNLColumns.User:
					coulmnValue = "2"; //to be decided
					break;

				case (int)PNLColumns.Exchange:
					coulmnValue = order.ExchangeName.ToString();
					break;

				case (int)PNLColumns.Side:
					coulmnValue = order.OrderSide;
					break;

				case (int)PNLColumns.Symbol:
					coulmnValue = order.Symbol;
					break;

				case (int)PNLColumns.TradingAccount:
					coulmnValue = order.TradingAccountName.ToString();
					break;
			}

			return coulmnValue;
		}

		public static double GetFeedValue(SymbolL1Data symbolL1Data, int calculcationColumnIndex)
		{
			double returnValue = double.MinValue;

			try
			{
				switch((PNLCalcuationColumn)calculcationColumnIndex)
				{
					case PNLCalcuationColumn.Ask:
						returnValue = symbolL1Data.Ask;
						break;

					case PNLCalcuationColumn.Bid:
						returnValue = symbolL1Data.Bid;
						break;

					case PNLCalcuationColumn.Last:
						returnValue = symbolL1Data.Last;
						break;

					case PNLCalcuationColumn.Previous:
						returnValue = symbolL1Data.Previous;
						break;
				}
			}
			catch(Exception ex)
			{
				int i=0;
			}


			return returnValue;

		}



}
}
