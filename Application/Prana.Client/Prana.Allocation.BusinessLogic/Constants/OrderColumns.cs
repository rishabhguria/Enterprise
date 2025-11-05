using System;
using Prana.Global;
using System.Collections;
namespace Prana.Allocation.BLL
{
	/// <summary>
	/// Summary description for OrderColumns.
	/// </summary>
	/// 
	public class Constants
	{
	public static  string MULTIPLE="MUL";
        public static string MULTIPLEID = "0";  
        public static int MULTIPLEID_INT = 0;
        
	}
	
    //public enum UnAllocaedDisplayColumns
    //{
	    
    //    ClOrderID,
    //    OrderSide,
    //    Symbol,
    //    Venue,
    //    CounterPartyName,
    //    TradingAccountName,
    //    CumQty,
    //    AvgPrice,			
    //    Quantity,		
    //    OrderType,
    //    ExchangeName,
    //    AssetName,
    //    UnderlyingName,
		
		
		
		
    //}
    //public enum GroupedDisplayColumns
    //{
	    
		
    //    OrderSide,
    //    Symbol,
    //    Venue,
    //    CounterPartyName,
    //    TradingAccountName,
    //    CumQty,
    //    AvgPrice,			
    //    Quantity,		
    //    OrderType,
		
		
		
		
    //}

    //public enum AllocatedDisplayColumns
    //{
	    
		
    //    OrderSide,
    //    Symbol,
    //    Venue,
    //    CounterPartyName,
    //    TradingAccountName,
    //    CumQty,
    //    AvgPrice,			
    //    Quantity,		
    //    OrderType,
    //    AllocatedQty,
		
		
		
    //}


    

    public enum UnallocatedBasketColumns
    { 
        BasketName,
        BasketID,
        TradedBasketID,
        TradedTime,
        Asset,
        Underlying,
        TradingAccount,
        Quantity,
        CumQty,
        ExeValue,
        User

    }
    public enum BasketGroupColumns
    {
        
        
        Quantity,
        CumQty,
        ExeValue,
        Asset,
        Underlying,
        TradingAccount,
        User

    }
    public enum AllocatedBasketColumns
    {
        Quantity,
        CumQty,
        ExeValue,
        Asset,
        Underlying,
        AllocatedQty,
        BasketName,
        ListID,
        Commission,
        Fees      

    }

    
}

