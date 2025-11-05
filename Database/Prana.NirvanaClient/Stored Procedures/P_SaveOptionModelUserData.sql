
CREATE PROC [dbo].[P_SaveOptionModelUserData]          
(                                                                                              
 @Xml varchar(max),                                                                                                                              
 @ErrorMessage varchar(500) output,                                                                                                                                       
 @ErrorNumber int output                                                                                       
)                                                                                              
As  
SET @ErrorNumber = 0                                                                                                                      
SET @ErrorMessage = 'Success'                                                                                                    
                                                                                                              
BEGIN TRY                                                                                                       
                                                                                              
 BEGIN TRAN TRAN1               
            
-- SAVES PSSYMBOL AND MULTIPLIERS                                                                          
DECLARE @handle int                                                                           
exec sp_xml_preparedocument @handle OUTPUT,@Xml    

Create Table  #TempUserOptionModelInput  
(
Symbol varchar(100),      
HistoricalVolatility float,          
HistoricalVolatilityUsed bit,          
UserVolatility float,          
UserVolatilityUsed bit,          
UserInterestRate float,          
UserInterestRateUsed bit,          
UserDividend float,          
UserDividendUsed bit, 
UserStockBorrowCost float,
UserStockBorrowCostUsed bit,         
UserDelta float,          
UserDeltaUsed bit,          
UserLastPrice float,          
UserLastPriceUsed bit,   
UserForwardPoints float,  
UserForwardPointsUsed bit,
UserTheoreticalPriceUsed bit,
UserProxySymbolUsed bit,
UserSharesOutstandingUsed bit,
UserSharesOutstanding float,
SMUserSharesOutstandingUsed bit,
SMUserSharesOutstanding float,
UserClosingMarkUsed bit,
IsManualInput bit 
)       
INSERT INTO #TempUserOptionModelInput          
(          
Symbol,      
HistoricalVolatility,          
HistoricalVolatilityUsed,          
UserVolatility,          
UserVolatilityUsed,          
UserInterestRate,          
UserInterestRateUsed,          
UserDividend,          
UserDividendUsed,   
UserStockBorrowCost,
UserStockBorrowCostUsed,       
UserDelta,          
UserDeltaUsed,          
UserLastPrice,          
UserLastPriceUsed,  
UserForwardPoints,  
UserForwardPointsUsed,
UserTheoreticalPriceUsed,
UserProxySymbolUsed,
UserSharesOutstandingUsed,
UserSharesOutstanding,
SMUserSharesOutstandingUsed,
SMUserSharesOutstanding,
UserClosingMarkUsed,
IsManualInput             
)          
SELECT               
Symbol,      
Isnull(HistoricalVol,0),          
Isnull(HistoricalVolatilityUsed,0),          
IsNull(Volatility, 0),          
Isnull(VolatilityUsed,0),          
IsNull(IntRate, 0),          
Isnull(IntRateUsed,0),          
IsNull(Dividend, 0),          
Isnull(DividendUsed,0),       
IsNull(StockBorrowCost, 0),          
Isnull(StockBorrowCostUsed,0),          
IsNull(Delta, 0),          
Isnull(DeltaUsed,0),         
IsNull(LastPrice, 0),          
Isnull(LastPriceUsed,0),  
IsNull(ForwardPoints,0),  
Isnull(ForwardPointsUsed,0),
Isnull(TheoreticalPriceUsed,0),
IsNull(ProxySymbolUsed,0),  
IsNull(SharesOutstandingUsed,0),
IsNull(SharesOutstanding,0),
IsNull(SMSharesOutstandingUsed,0),
IsNull(SMSharesOutstanding,0),
ISNULL(ClosingMarkUsed,0),
IsNull(ManualInput,0)
               
FROM  OPENXML(@handle,'//ArrayOfUserOptModelInput/UserOptModelInput',3)                                                                  
WITH                                                                                          
(                 
Symbol varchar(100),      
HistoricalVol float,          
HistoricalVolatilityUsed bit,          
Volatility float,          
VolatilityUsed bit,          
IntRate float,          
IntRateUsed bit,          
Dividend float,          
DividendUsed bit,       
StockBorrowCost float,          
StockBorrowCostUsed bit,          
Delta float,          
DeltaUsed bit,          
LastPrice float,          
LastPriceUsed bit,   
ForwardPoints float,  
ForwardPointsUsed bit,
TheoreticalPriceUsed bit,
ProxySymbolUsed bit,
SharesOutstandingUsed bit,
SharesOutstanding FLOAT,
SMSharesOutstandingUsed bit,
SMSharesOutstanding FLOAT,
ClosingMarkUsed bit,
ManualInput bit          
)  

--select * from T_UserOptionModelInput
-- Prior to this all the expired options were being deleted from OMI table but this was causing problem when any expired options 
-- was open and UserPrice was checked off for that as everytime PI is reopened it used to get un-checked.
Select distinct Symbol as symbol
into #OpenSymbols
From PM_Taxlots WITH (NOLOCK)
Where TaxLotOpenQty<>0 and
Taxlot_PK in                                                                                           
 (                                                                                                 
    Select max(Taxlot_PK) from PM_Taxlots WITH (NOLOCK)                                                                                                                                                             
    where DateDiff(d,PM_Taxlots.AUECModifiedDate,GETDATE()) >=0                                                                                                                                    
    group by taxlotid                                                 
  )  

Select DISTINCT UnderlyingSymbol as Symbol
into #UnderlyingSymbols
From V_SecMasterData_WithUnderlying
where TickerSymbol in(SELECT Symbol from #OpenSymbols UNION SELECT symbol from T_Group where StateID=1 UNION SELECT symbol from T_UserOptionModelInput where IsManuallyAdded = 1)

delete from T_UserOptionModelInput where Symbol not in(SELECT symbol FROM #OpenSymbols UNION SELECT tickersymbol from V_SecMasterData where AssetId=7 UNION SELECT symbol from T_Group where StateID=1 UNION SELECT symbol from #UnderlyingSymbols UNION SELECT symbol from T_UserOptionModelInput where IsManuallyAdded = 1)  


delete UPI
from T_UserOptionModelInput UPI
inner join #TempUserOptionModelInput TUPI
on UPI.symbol=TUPI.symbol

--Update T_UserOptionModelInput     
--set             
-- Symbol   = #TempUserOptionModelInput.Symbol,      
--HistoricalVolatility = #TempUserOptionModelInput.HistoricalVolatility,          
--HistoricalVolatilityUsed = #TempUserOptionModelInput.HistoricalVolatilityUsed,          
--UserVolatility = #TempUserOptionModelInput.UserVolatility,          
--UserVolatilityUsed = #TempUserOptionModelInput.UserVolatilityUsed,          
--UserInterestRate = #TempUserOptionModelInput.UserInterestRate,          
--UserInterestRateUsed = #TempUserOptionModelInput.UserInterestRateUsed,          
--UserDividend = #TempUserOptionModelInput.UserDividend,          
--UserDividendUsed = #TempUserOptionModelInput.UserDividendUsed,          
--UserDelta = #TempUserOptionModelInput.UserDelta,          
--UserDeltaUsed = #TempUserOptionModelInput.UserDeltaUsed,          
--UserLastPrice = #TempUserOptionModelInput.UserLastPrice,          
--UserLastPriceUsed = #TempUserOptionModelInput.UserLastPriceUsed,  
--UserForwardPoints = #TempUserOptionModelInput.UserForwardPoints,  
--UserForwardPointsUsed = #TempUserOptionModelInput.UserForwardPointsUsed,
--UserTheoreticalPriceUsed = #TempUserOptionModelInput.UserTheoreticalPriceUsed,
--UserProxySymbolUsed = #TempUserOptionModelInput.UserProxySymbolUsed   
--
--from T_UserOptionModelInput,#TempUserOptionModelInput  
--
--where #TempUserOptionModelInput.Symbol = T_UserOptionModelInput.Symbol

Insert into T_UserOptionModelInput
(          
Symbol,      
HistoricalVolatility,          
HistoricalVolatilityUsed,          
UserVolatility,          
UserVolatilityUsed,          
UserInterestRate,          
UserInterestRateUsed,          
UserDividend,          
UserDividendUsed,               
UserStockBorrowCost,          
UserStockBorrowCostUsed,          
UserDelta,          
UserDeltaUsed,          
UserLastPrice,          
UserLastPriceUsed,  
UserForwardPoints,  
UserForwardPointsUsed,
UserTheoreticalPriceUsed,
UserProxySymbolUsed,
UserSharesOutstandingUsed,
UserSharesOutstanding,
SMUserSharesOutstandingUsed,
SMUserSharesOutstanding,
UserClosingMarkUsed,
IsManuallyAdded           
)     
select  
Symbol,      
Isnull(HistoricalVolatility,0),          
HistoricalVolatilityUsed,          
IsNull(UserVolatility, 0),          
UserVolatilityUsed,          
IsNull(UserInterestRate, 0),          
UserInterestRateUsed,          
IsNull(UserDividend, 0),          
UserDividendUsed,                
IsNull(UserStockBorrowCost, 0),          
UserStockBorrowCostUsed,          
IsNull(UserDelta, 0),          
UserDeltaUsed,          
IsNull(UserLastPrice, 0),          
UserLastPriceUsed,  
IsNull(UserForwardPoints,0),  
UserForwardPointsUsed,
UserTheoreticalPriceUsed,
UserProxySymbolUsed,
UserSharesOutstandingUsed,
UserSharesOutstanding,
SMUserSharesOutstandingUsed,
SMUserSharesOutstanding,
UserClosingMarkUsed,
IsManualInput from #TempUserOptionModelInput  

DELETE FROM T_UserOptionModelInput        
WHERE        
(UserVolatility + HistoricalVolatility + UserInterestRate + UserDividend + UserStockBorrowCost + UserDelta + UserLastPrice + UserForwardPoints + UserSharesOutstanding +SMUserSharesOutstanding) = 0          
AND         
(HistoricalVolatilityUsed | UserVolatilityUsed | UserInterestRateUsed | UserDividendUsed | UserStockBorrowCostUsed | UserDeltaUsed | UserLastPriceUsed | UserForwardPointsUsed | UserTheoreticalPriceUsed |UserProxySymbolUsed | UserSharesOutstandingUsed | UserClosingMarkUsed | SMUserSharesOutstandingUsed) = 0         
AND      
IsManuallyAdded =0
--
--
--Select opt.symbol as Symbol into  #tempSymbols from
--T_UserOptionModelInput opt
--inner join V_Secmasterdata on (opt.Symbol = V_Secmasterdata.TickerSymbol and V_Secmasterdata.ExpirationDate < GetDate() and V_Secmasterdata.ExpirationDate <> '1800-01-01 00:00:00.000')
--
--DELETE FROM T_UserOptionModelInput where symbol in (Select Symbol from #tempSymbols)

Drop Table #openSymbols,#TempUserOptionModelInput  

COMMIT TRANSACTION TRAN1                                                                              
                                                                                                     
exec sp_xml_removedocument @handle                                                                                         
END TRY                                                                                                      
BEGIN CATCH                                 
SET @ErrorMessage = ERROR_MESSAGE();                                                                                                      
SET @ErrorNumber = Error_number();                        
ROLLBACK TRANSACTION TRAN1                                          
END CATCH;  

