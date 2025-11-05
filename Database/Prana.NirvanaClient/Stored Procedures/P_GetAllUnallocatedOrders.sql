                      
                              
                              
                                                                   
CREATE PROCEDURE [dbo].[P_GetAllUnallocatedOrders] (                                                                                          
                                                          
@AllAUECDatesString varchar(max)                                                                                          
)                                                                                          
as                                                                                           
                                                          
                                           
Declare @MarkPriceForDate   Table(Finalmarkprice float ,MarkPriceIndicator int, Symbol varchar(50) ,Date Datetime )                                            
                                            
INSERT INTO @MarkPriceForDate Select * From dbo.GetMarkPriceForDate(@AllAUECDatesString)                                           
                                          
Declare @AUECDatesTable Table(AUECID int,CurrentAUECDate DateTime)                                                                                  
                                                              
Insert Into @AUECDatesTable Select * From dbo.GetAllAUECDatesFromString(@AllAUECDatesString)                                                                                  
                                                              
                                          
                                          
Declare @UTCDate datetime                                                                                  
Select @UTCDate = CurrentAUECDate                                                                                   
  from @AUECDatesTable                                                                                   
  Where AUECID = 0                                                                                  
                                                             
                                                      
                                                    
Declare @FXConversionRatesDayBeforeMark Table                                                                                              
(                                                                     
 FromCurrencyID int,                                                                 
 ToCurrencyID int,                                                                   
 RateValue float,                                                                                              
 ConversionMethod int,                                                                                              
 Date DateTime,                                                                                              
 eSignalSymbol varchar(max),                                                                  
 InputDatePriceIndicator int ,  
TickerSymbol    varchar(50)                                                                                                 
)                                                      
                                                              
 Declare @FXConversionRatesDayMark Table                                                                            
(                                                                            
 FromCurrencyID int,                                                                   
 ToCurrencyID int,                                                         
 RateValue float,                                                                            
 ConversionMethod int,                                                
 Date DateTime,                                                                            
 eSignalSymbol varchar(max),                         
 InputDatePriceIndicator int,  
 TickerSymbol    varchar(50)                                                                          
)                                                                
                                            
Declare @DayBeforeUTCDate Datetime                                                         
Set @DayBeforeUTCDate = DateAdd(d,-1,@UTCDate)                            
                                 
INSERT INTO @FXConversionRatesDayMark
SELECT FromCurrencyID
	,ToCurrencyID
	,RateValue
	,ConversionMethod
	,Date
	,eSignalSymbol
	,InputDatePriceIndicator
	,TickerSymbol
FROM GetFXConversionRatesForDate(@UTCDate, 1) AS TempFx
WHERE FundID = 0
  
  
                                                                
INSERT INTO @FXConversionRatesDayBeforeMark
SELECT FromCurrencyID
	,ToCurrencyID
	,RateValue
	,ConversionMethod
	,Date
	,eSignalSymbol
	,InputDatePriceIndicator
	,TickerSymbol
FROM GetFXConversionRatesForDate(@DayBeforeUTCDate, 1)
WHERE FundID = 0
    
select                                                                                        
G.GroupID, --It will be used as ID in cache for consol view     --1                                                                                   
G.OrderSideTagValue,                                               --2                                         
G.TradingAccountID,                                                  --3                                      
G.Symbol,                                                  --4                                      
G.CounterPartyID,                                             --5                                           
G.VenueID,                                                  --6                                      
G.OrderTypeTagValue,                                           --7                                             
G.AssetID,                                                  --8                                      
G.UnderLyingID,                               --9                                      
G.ExchangeID,                                    --10                                                    
G.AUECID,                                      
G.Quantity as GrQuantity,                                            --11                                            
G.CumQty as GrCumQty, -- (G.CumQty - G.AllocatedQty) as UnAllocatedQty,                                               --13                                      
G.AvgPrice,                     --14                                      
Convert(varchar(50), G.AUECLocalDate) As TransactTime,            --15                                                                            
G.UserID,    --16                                      
CASE  G.AssetID When 5 then                                            
Case  When datediff(d,G.AllocationDate,@UTCDate) = 0 Then IsNull(G.AvgPrice,0)                                                                                           
Else IsNull(FXDayBeforeMark.RateValue,0)                                                                                                  
  End                                                                                   
ELSE                                                                                  
CASE datediff(day, G.AllocationDate, AUECDates.CurrentAUECDate)                                                                                 
 when  0                                                                                 
  THEN IsNull(G.AvgPrice,0)                                                                                
 else IsNull(MP.Finalmarkprice, 0)                                                                            
end                                                                         
END AS MarkPriceForDatePrice                                                                         
,Secdata.LeadCurrencyID                                                            
,Secdata.VsCurrencyID                                           
, G.SettlementDate                                                                              
, G.Description                                                                      
, Case  G.AssetID                                                             
 When 5 then ISNULL(FXDayBeforeMark.InputDatePriceIndicator,1)                                                                 
 Else ISNULL(MP.MarkPriceIndicator,1)                                   
End as MarkPriceForDatePriceIndicator,                                          
G.CurrencyID ,                                
CASE G.AssetID                      
WHEN 5                                                             
THEN FXDayMark.RateValue                                                              
ELSE                                                                
 DMP.FinalMarkPrice                                                               
END As TodayMark ,                            
G.IsSwapped,                          
Swap.NotionalValue,                          
 Swap.BenchMarkRate,                          
Swap.Differential,                          
Swap.OrigCostBasis,                          
Swap.DayCount,                          
Swap.SwapDescription,                          
Swap.FirstResetDate,                          
Swap.OrigTransDate,                          
Swap.ResetFrequency,                          
Swap.ClosingPrice,                          
Swap.ClosingDate,                          
Swap.TransDate,                        
  G.FXRate,                        
G.FXConversionMethodOperator,                      
MP.Date As YesterdayMarkPriceDate,                      
DMP.Date As TodayMarkPriceDate,                      
Secdata.CompanyName,                      
Secdata.UnderLyingSymbol,                      
Secdata.Delta,                    
Secdata.Multiplier,                
Secdata.PutOrCall,            
G.IsPreAllocated as IsGrPreAllocated,            
G.AllocatedQty as GrAllocatedQty            
from                                                                                              
T_Group as G                                       
                                                                                             
INNER JOIN @AUECDatesTable As AUECDates on AUECDates.AUECID = G.AUECID                                           
LEFT OUTER JOIN @MarkPriceForDate MP on MP.Symbol= G.Symbol                         
LEFT OUTER JOIN V_SecMasterData Secdata on Secdata.TickerSymbol=G.Symbol        
LEFT OUTER JOIN @FXConversionRatesDayBeforeMark FXDayBeforeMark on                                
FXDayBeforeMark.TickerSymbol = G.Symbol                                
and datediff(d,FXDayBeforeMark.Date,DateAdd(d,-1,@UTCDate)) >= 0                                          
LEFT OUTER JOIN @FXConversionRatesDayMark FXDayMark on FXDayMark.TickerSymbol = G.Symbol and (datediff(d,FXDayMark.Date,@UTCDate) >= 0)                        
     
     
        
          
LEFT OUTER JOIN  PM_DayMarkPrice DMP on  (DMP.Symbol=G.Symbol and  datediff(d,DMP.Date,AUECDates.CurrentAUECDate) = 0  )                                                                            
left outer join T_SwapParameters as Swap on G.GroupID = Swap.GroupID                       
where G.StateID=1 -- Represents that the group is fully unallocated  --  and IsmanualGroup=1                       
or (G.CumQty - G.AllocatedQty) > 0 -- Which means that the group is partially filled.              
      
order by transactTime                      
--select * from V_SecmasterData where tickerSymbol='TY H9'

