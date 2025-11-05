using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prana.BusinessObjects.LiveFeed;
using System.Collections.Concurrent;
using System.Data;
using Prana.Utilities.ImportExportUtilities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;

namespace Prana.BlpDLWSAdapter.BusinessObject.Mappings
{
    public sealed class HistoricalFields
    {
        private static volatile HistoricalFields instance;
   private static object syncRoot = new Object();

   private HistoricalFields() { }

   public static HistoricalFields Instance
   {
      get 
      {
         if (instance == null) 
         {
            lock (syncRoot) 
            {
                if (instance == null)
                {
                    instance = new HistoricalFields();
                    instance.FillData();
                }

            }
         }
         return instance;
      }
   }

   private void FillData()
   {
       try
       {
           _histPricingFieldsMap.TryAdd(PricingDataType.Ask.ToString(), "PX_ASK");
           _histPricingFieldsMap.TryAdd(PricingDataType.Bid.ToString(), "PX_BID");
           _histPricingFieldsMap.TryAdd(PricingDataType.Close.ToString(), "PX_OFFICIAL_CLOSE");
           _histPricingFieldsMap.TryAdd(PricingDataType.High.ToString(), "PX_HIGH");
           _histPricingFieldsMap.TryAdd(PricingDataType.Last.ToString(), "PX_LAST");
           _histPricingFieldsMap.TryAdd(PricingDataType.Low.ToString(), "PX_LOW");
           _histPricingFieldsMap.TryAdd(PricingDataType.Mid.ToString(), "PX_MID");
           _histPricingFieldsMap.TryAdd(PricingDataType.Open.ToString(), "PX_OPEN");
           FillFieldMappingDictionary();
       }
       catch (Exception ex)
       {
           // Invoke our policy that is responsible for making sure no secure information
           // gets out of our layer.
           bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
           if (rethrow)
           {
               throw;
           }
       }
   }
   public void ReloadCache()
   {
       try
       {
           lock (_histPricingFieldsMap)
           {
               _histPricingFieldsMap.Clear();
               FillData();
           }
       }
       catch (Exception ex)
       {
           // Invoke our policy that is responsible for making sure no secure information
           // gets out of our layer.
           bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
           if (rethrow)
           {
               throw;
           }
       }
   }
   void FillFieldMappingDictionary()
   {
       try
       {
           string pricingFieldMappingFilePath = AppDomain.CurrentDomain.BaseDirectory + @"xmls";
           DataTable dataSource = FileReaderFactory.GetDataTableFromDifferentFileFormats(pricingFieldMappingFilePath + @"\NirvanaFieldBloombergFieldMapping.csv");
           foreach (DataRow dr in dataSource.Rows)
           {
                   _histPricingFieldsMap.AddOrUpdate(dr["COL1"].ToString().Trim(' ', '"', '\''), dr["COL2"].ToString().Trim(' ', '"', '\''), (key, oldValue) => dr["COL2"].ToString().Trim(' ', '"', '\''));
               }
           }
       catch (Exception ex)
       {
           // Invoke our policy that is responsible for making sure no secure information
           // gets out of our layer.
           bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
           if (rethrow)
           {
               throw;
           }
       }
   }


   ConcurrentDictionary<string, string> _histPricingFieldsMap = new ConcurrentDictionary<string, string>();
        internal string GetBloombergFieldForNirvanaField(string nirvanaField)
        {
            if (_histPricingFieldsMap.ContainsKey(nirvanaField))
            {
                return _histPricingFieldsMap[nirvanaField];
            }
            else
                return nirvanaField;
        }

        internal string GetNirvanaFieldFromBloombergField(string bloombergField)
        {
            if (_histPricingFieldsMap.Values.Contains(bloombergField))
            {
                return _histPricingFieldsMap.Where(pair => bloombergField.Equals(pair.Value, StringComparison.InvariantCultureIgnoreCase))
                              .Select(pair => pair.Key).First();
            }
            else
                return bloombergField;
        }
    }

//Possible bloomberg historical fields.These can be mapped to system fields whenever the fields are created
//    Field Mnemonic Field Description
//12MO_CALL_IMP_VOL 12 Month Call Implied Volatility
//12MO_PUT_IMP_VOL 12 Month Put Implied Volatility
//18MO_CALL_IMP_VOL 18 Month Call Implied Volatility
//18MO_PUT_IMP_VOL 18 Month Put Implied Volatility
//24MO_CALL_IMP_VOL 24 Month Call Implied Volatility
//24MO_PUT_IMP_VOL 24 Month Put Implied Volatility
//3MO_CALL_IMP_VOL 3 Month Call Implied Volatility
//3MO_PUT_IMP_VOL 3 Month Put Implied Volatility
//6MO_CALL_IMP_VOL 6 Month Call Implied Volatility
//6MO_PUT_IMP_VOL 6 Month Put Implied Volatility
//ASSET_SWAP_SPD_MID Mid Asset Swap Spread
//BN_SURVEY_AVERAGE BN Survey Average
//BN_SURVEY_HIGH BN Survey High
//BN_SURVEY_LOW BN Survey Low
//BN_SURVEY_MEDIAN BN Survey Median
//BN_SURVEY_NUMBER_OBSERVATIONS BN Survey Number Of Observations
//BN_SURVEY_WEIGHTED_AVG BN Survey Weighted Average
//CALL_IMP_VOL_10D 10 Day Call Implied Volatility
//CALL_IMP_VOL_30D 30 Day Call Implied Volatility
//CALL_IMP_VOL_60D 60 Day Call Implied Volatility
//CHG_NET_1D Price Change 1 Day Net
//CHG_NET_1M Price Change 1 Month Net
//CHG_NET_2D Price Change 2 Day Net
//CHG_NET_5D Price Change 5 Day Net
//CHG_PCT_1D Price Change 1 Day Percent
//CHG_PCT_1M Price Change 1 Month Percent
//CHG_PCT_5D Price Change 5 Day Percent
//CNVX_OAS_BID Bid OAS Convexity
//CUR_MKT_CAP Current Market Cap
//DISC_MRGN_ASK Ask Discount Margin (Benchmark)
//DISC_MRGN_BID Bid Discount Margin (Benchmark)
//DISC_MRGN_MID Mid Discount Margin (Benchmark)
//DUR_ADJ_OAS_BID Bid OAS Effective Duration
//DVD_SH_12M Dividend Per Share 12 Month (Gross)
//DVD_SH_LAST Dividend Per Share Last Net
//EQY_DVD_SH_12M_NET Dividend Per Share 12 Month (Net)
//EQY_DVD_YLD_12M Dividend 12 Month Yld - Gross
//EQY_DVD_YLD_12M_NET Dividend 12 Month Yld - Net
//EQY_DVD_YLD_IND Dividend Indicated Yld - Gross
//EQY_SH_OUT Current Shares Outstanding
//EQY_TURNOVER Equity Turnover / Traded Value
//EQY_WEIGHTED_AVG_PX VWAP (Vol Weighted Average Price)
//FUND_CLASS_ASSETS Class Assets FUND_NET_ASSET_VAL
//Net Asset Value (NAV)
//FUND_TOTAL_ASSETS Fund Total Assets
//FUT_AGGTE_OPEN_INT Aggregate Open Interest
//FUT_AGGTE_VOL Aggregate Volume of Futures Contracts
//FUT_NORM_PX Normalized Future's Price
//FUT_PX Futures Trade Price
//HIST_CALL_IMP_VOL Hist. Call Implied Volatility
//HIST_PUT_IMP_VOL Hist. Put Implied Volatility
//INDX_DIVISOR Divisor
//LAST_DPS_GROSS Dividend Per Share Last (Gross)
//LAST_TRADE_ONLY The Last Actual Trade
//MMKT_7D_YIELD Money Market 7 Day Yield
//MOV_AVG_100D Moving Avg 100 Day
//MOV_AVG_10D Moving Avg 10 Day
//MOV_AVG_120D Moving Avg 120 Day
//MOV_AVG_180D Moving Avg 180 Day
//MOV_AVG_200D Moving Avg 200 Day
//MOV_AVG_20D Moving Avg 20 Day
//MOV_AVG_30D Moving Avg 30 Day
//MOV_AVG_40D Moving Avg 40 Day
//MOV_AVG_50D Moving Avg 50 Day
//MOV_AVG_5D Moving Avg 5 Day
//MOV_AVG_60D Moving Avg 60 Day
//OAS_SPREAD_BID Bid OAS Spread (bp)
//OAS_VOL_BID Bid OAS Volatility
//OFF_ON_EXCH_VOLUME Off And On Exchange Volume
//OPEN_INT Open Interest
//OPEN_INT_TOTAL_CALL Total Call Open Interest
//OPEN_INT_TOTAL_PUT Total Put Open Interest
//PE_RATIO Price Earnings Ratio (P/E)
//PUT_IMP_VOL_10D 10 Day Put Implied Volatility
//PUT_IMP_VOL_30D 30 Day Put Implied Volatility
//PUT_IMP_VOL_60D 60 Day Put Implied Volatility
//PX_ASK Ask Price PX_ASK_ALL_SESSION Ask Price
//All Session PX_ASK_POST_SESSION Ask Price Post-Session
//PX_ASK_PRE_SESSION Ask Price Pre-Session
//PX_AT_TRADE_VOLUME Intraday AT Trade Vol for London Set Stocks
//PX_BID Bid Price
//PX_BID_ALL_SESSION Bid Price All Session
//PX_BID_POST_SESSION Bid Price Post-Session
//PX_BID_PRE_SESSION Bid Price Pre-Session
//PX_CANCELLATION Cancellation Price
//PX_CLOSE_1D Closing Price 1 Day Ago
//PX_DISC_ASK Ask Discount Dollar Price
//PX_DISC_BID Bid Discount Dollar Price
//PX_DISC_MID Mid Discount Dollar Price
//PX_FIXING Fixing Price PX_HIGH
//High Price
//PX_HIGH_ALL_SESSION High Price All Session
//PX_HIGH_ASK High Ask Price
//PX_HIGH_BID High Bid Price
//PX_HIGH_POST_SESSION High Price Post-Session
//PX_HIGH_PRE_SESSION High Price Pre-Session
//PX_LAST Last Price
//PX_LAST_ALL_SESSIONS Last Price All Sessions
//PX_LAST_POST_SESSION Last Price Post-Session
//PX_LAST_PRE_SESSION Last Price Pre-Session
//PX_LONDON_MANUAL_VOLUME London Manual Trade Volume
//PX_LOW Low Price
//PX_LOW_ALL_SESSION Low Price All Session
//PX_LOW_ASK Low Ask Price
//PX_LOW_BID Low Bid Price
//PX_LOW_POST_SESSION Low Price Post-Session
//PX_LOW_PRE_SESSION Low Price Pre-Session
//PX_MID Mid Price
//PX_NASDAQ_CLOSE NASDAQ Official Closing Price
//PX_OFF_EXCH_VOLUME Off-Exchange Volume
//PX_OFFICIAL_AUCTION Official Auction Price
//PX_OFFICIAL_CLOSE Official Closing Price
//PX_OPEN Open Price
//PX_OPEN_POST_SESSION Open Price Post-Session
//PX_OPEN_PRE_SESSION Open Price Pre-Session
//PX_SETTLE Settlement Price
//PX_TO_BOOK_RATIO Price to Book Ratio
//PX_TO_CASH_FLOW Price/Cash Flow
//PX_TO_SALES_RATIO Price to Sales Ratio
//PX_VOLUME Volume
//PX_VOLUME_ALL_SESSION Volume All Session
//PX_VOLUME_POST_SESSION Volume Post-Session
//PX_VOLUME_PRE_SESSION Volume Pre-Session
//QUOTE_PRIOR_BID Bid Prior Close
//RISK_MID Mid Risk
//RSI_14D RSI 14 Day
//RSI_30D RSI 30 Day
//RSI_3D RSI 3 Day
//RSI_9D RSI 9 Day
//SHORT_INT Short Interest
//SHORT_INT_RATIO Short Interest Ratio
//SHORT_SELL_NUM_SHARES Short Sell Number Of Shares
//SHORT_SELL_TURNOVER Short Sell Turnover
//VOLATILITY_10D Volatility 10 Day
//VOLATILITY_120D Volatility 120 Day
//VOLATILITY_150D Volatility 150 Day
//VOLATILITY_180D Volatility 180 Day
//VOLATILITY_200D Volatility 200 Day
//VOLATILITY_20D Volatility 20 Day
//VOLATILITY_260D Volatility 260 Day
//VOLATILITY_30D Volatility 30 Day
//VOLATILITY_360D Volatility 360 Day
//VOLATILITY_60D Volatility 60 Day
//VOLATILITY_90D Volatility 90 Day
//VOLUME_TOTAL_CALL Total Call Volume
//VOLUME_TOTAL_PUT Total Put Volume
//VWAP_NUM_TRADES VWAP Number of Trades
//VWAP_TURNOVER VWAP (Turnover)
//VWAP_VOLUME VWAP Volume
//WRT_OUTSTANDING Warrants Outstanding
//YLD_ANNUAL_ASK Ask Annual Yield
//YLD_ANNUAL_BID Bid Annual Yield
//YLD_ANNUAL_MID Mid Annual Yield
//YLD_BLENDED_ASK Ask Blended Yield
//YLD_BLENDED_BID Bid Blended Yield
//YLD_BLENDED_MID Mid Blended Yield
//YLD_CHG_NET_1D_NO_BP Yield Change 1 Day Net (No Bp)
//YLD_CHG_NET_2D_NO_BP Yield Change 2 Day Net (No Bp)
//YLD_CHG_NET_5D_NO_BP Yield Change 5 Day Net (no bp)
//YLD_CNV_ASK Ask Yield To Convention
//YLD_CNV_BID Bid Yield To Convention
//YLD_CNV_FROM_HIGH Yield To Worst Convention From High Price
//YLD_CNV_FROM_LOW Yield To Worst Convention From Low Price
//YLD_CNV_MID Mid Yield To Convention
//YLD_CNV_OPEN Open Yield To Worst Convention
//YLD_CUR_ASK Ask Current Yield
//YLD_CUR_BID Bid Current Yield
//YLD_CUR_MID Mid Current Yield
//YLD_PFD_STR_ASK Ask Preferred Strip Yield
//YLD_PFD_STR_BID Bid Preferred Strip Yield
//YLD_PFD_STR_MID Mid Preferred Strip Yield
//YLD_SEMI_ANNUAL_ASK Ask Semi-annual Yield
//YLD_SEMI_ANNUAL_BID Bid Semi-annual Yield
//YLD_SEMI_ANNUAL_MID Mid Semi-annual Yield
//YLD_SOV_SPREAD_ASK Ask Sovereign Spread
//YLD_SOV_SPREAD_BID Bid Sovereign Spread
//YLD_SOV_SPREAD_MID Mid Sovereign Spread
//YLD_STR_ASK Ask Stripped Yield
//YLD_STR_BID Bid Stripped Yield
//YLD_STR_MID Mid Stripped Yield
//YLD_YTC_ASK Ask Yield To Next Call
//YLD_YTC_BID Bid Yield To Next Call
//YLD_YTC_MID Mid Yield To Next Call
//YLD_YTM_ASK Ask Yield To Maturity
//YLD_YTM_BID Bid Yield To Maturity
//YLD_YTM_MID Mid Yield To Maturity
}
