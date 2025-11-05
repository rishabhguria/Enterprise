using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using Prana.Global;

namespace Prana.BasketTrading
{
    public class BasketTradingConstants
    {

        public static string GreaterThan = ">";
        public static string GreaterThanEqual = ">=";
        public static string lessThan = "<";
        public static string LessThanEqual = "<=";

        //Price Related
        public static string BchMark_PrvDayClose = "PreviousDayClose";
        public static string BchMark_StrikePrice = "StrikePrice";
        public static string BchMark_VWAP = "VWAP";


        //Volume  Relataed
        public static string BchMark_PrvDayVolumne = "PreviousDayVolumne";
        public static string BchMark_ADV = "ADV";

        //public static readonly string BorrowerID = System.Configuration.ConfigurationManager.AppSettings["BorrowerID"];
        public static readonly string BorrowerID = ConfigurationHelper.Instance.GetAppSettingValueByKey("BorrowerID");

        public delegate void BasketUpLoadHandler(object sender, UpLoadedBasketArgs e);
        public delegate void BasketUpLoadIDSHandler(object sender, UpLoadedBasketIDSArgs e);

        public delegate void NewBasketUpLoadHandler(object sender, NewBasketUploadArgs e);
       

        public enum GridType
        { 
            BasketTrading,
            Wave,
            GroupTkt,
            GroupTktReplace,
            BasketUpLoad,
            PostTrade
        }



        public enum TradeFilterTypes
        {
            Volume ,
            Value ,
            RoundLot,
            BidOfferSpread 
        }
        public enum FilterOperators
        {          
        
        
        }

        public enum PriceType
        {
            Bid ,
            Ask 
        }
        //TBC
        public enum FilterBenchMarks
        {
            //Price Related
            PreviousDayClose,
            StrikePrice,
            VWAP,


            //Volume  Relataed
            PreviousDayVolume,
            ADV
        }
        public enum PriceBenchMark
        {
            //Price Related
            DayOpen,
            //DayClose, 
            //PrevDayOpen,
            PrevDayClose

        }


        public enum ReportType
        {
            //Summary,
            Detail,
            Residual
        }
        public enum FormatType
        {
            Select,
            CSV,
            TSV,
            Excel,
            Manual

        }
        public enum UpLoadParameters
        {
            Select ,
            Percentage,
            NumberOfShares
        }
        public enum CalculationCriteria
        {
            Select,
            UserUpload,
            PriceWeighted,
            EqualWeighted
            
        }
       //TBD
        public enum BasketNonEditableColumns
        {
            CumQty,
            ExecType,
            OrderStatus,
            ExecTransType,
            LastMarket,
            LastPrice,
            LeavesQty,
            LastShares,
            ClOrderID,
            UnsentQty,
            SendQty,
            AvgPrice,         
            TransactionTime,
            Text
        }
       
      
      //TBD
        public enum PostTradingExtraColumns
        { 
            DayOpen,
            DayOpenChange,
            PrevDayOpen,
            PrevDayOpenChange,
            //VWAP,
            //VWAPChange
        }
       public  class UpLoadedBasketArgs : EventArgs
        {
            private BasketDetail basketDetail = new BasketDetail();

            //For saving the Saved state of the newly created basket
            private bool _isBasketNameSaved = false;

            public bool IsBasketNameSaved
            {
                get { return _isBasketNameSaved; }
                set { _isBasketNameSaved= value; }
            }
	

           public UpLoadedBasketArgs()
            { }
            public BasketDetail BasketDetail
            {
                get { return basketDetail; }
                set { basketDetail = value; }
            }

        }
        public class NewBasketUploadArgs : EventArgs
        {
           public  List<BasketDetail> NewBaskets = new List<BasketDetail>();

            public NewBasketUploadArgs()
            { }
           

        }
        public class UpLoadedBasketIDSArgs : EventArgs
        {
            private string basketIDS;
            public string basketNames;
            public UpLoadedBasketIDSArgs()
            { }
            public string UpLoadedBasketIDS
            {
                get { return basketIDS; }
                set { basketIDS = value; }
            }

        }    
    }

}