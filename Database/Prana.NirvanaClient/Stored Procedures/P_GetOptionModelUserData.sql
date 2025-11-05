CREATE PROCEDURE [dbo].[P_GetOptionModelUserData]                         
(                          
@listSymbols varchar(max) ,    
@fetchZeroPositionData int                 
)                                                                                                                                                                                     
AS                             
                            
BEGIN      
                        
CREATE TABLE #SecMasterTemp      
(      
Symbol varchar(100),      
SecurityName varchar(200),      
OSISymbol varchar(100),      
IDCOSymbol varchar(100),       
AssetID int,       
UnderLyingSymbol varchar(100),      
ExpirationDate datetime,      
StrikePrice float,      
PutorCall varchar(10),      
VsCurrencyID int,      
LeadCurrencyID int,      
ProxySymbol varchar(100),      
BloombergSymbol varchar(200),  
AuecID int,   
SharesOutstanding float,
BloombergSymbolWithExchangeCode varchar(200)
)       
      
Insert into #SecMasterTemp      
(      
Symbol ,      
SecurityName ,      
OSISymbol,      
IDCOSymbol,       
AssetID ,       
UnderLyingSymbol ,      
ExpirationDate ,      
StrikePrice ,      
PutorCall,      
VsCurrencyID ,      
LeadCurrencyID ,      
ProxySymbol ,      
BloombergSymbol,  
AuecID,
SharesOutstanding,
BloombergSymbolWithExchangeCode
)      
SELECT distinct SM.TickerSymbol, SM.CompanyName, SM.OSISymbol, SM.IDCOSymbol, SM.AssetID, SM.UnderLyingSymbol,SM.ExpirationDate,SM.StrikePrice,SM.PutorCall,SM.VSCurrencyID,SM.LeadCurrencyID,SM.ProxySymbol ,SM.BloombergSymbol, SM.AUECID,SM.SharesOutstanding, SM.BloombergSymbolWithExchangeCode                    
  
FROM V_SecMasterData_WithUnderlying SM WITH(NOLOCK)        
      
         
                       
CREATE TABLE #Temp                                                                                                     
(                        
Symbol varchar(100),                                                                                                                    
SecurityName varchar(200),                        
OSISymbol varchar(100),                        
IDCOSymbol varchar(100),                        
AssetID int,                        
UnderLyingSymbol varchar(100),                    
ExpirationDate datetime null,                    
StrikePrice float,                    
PutorCall varchar(10),                  
VsCurrencyID int,                  
LeadCurrencyID int,            
ProxySymbol varchar(100) ,      
BloombergSymbol varchar(200),      
IsHistorical bit,  
AuecID int,    
SharesOutstanding float,
IsManualInput BIT DEFAULT 0,
BloombergSymbolWithExchangeCode varchar(200)
 )                        
                                      
BEGIN                        
      
Create Table #Symbols (symbol varchar(100))      
Begin      
if ((@listSymbols is NULL or @listSymbols = '') and @fetchZeroPositionData=0)    
Insert into #Symbols       
Select distinct Symbol from PM_taxlots WHERE  PM_taxlots.TaxLotOpenQty >0 and PM_taxlots.Taxlot_pk in( select max (Taxlot_pk) from PM_taxlots group by taxlotID)        
group BY PM_Taxlots.FundID,PM_Taxlots.Symbol      
having SUM(PM_Taxlots.TaxLotOpenQty *      
CASE OrderSideTagValue        
When '1' then 1        
When '3' then 1        
When 'A' then 1        
When 'B' then 1        
else -1        
end       
) <>0      
union      
select distinct Symbol from T_group  where CumQty >0                    
and  StateID=1     
UNION     
SELECT DISTINCT symbol from #SecMasterTemp    
where AssetID=7    
UNION
SELECT DISTINCT symbol FROM T_UserOptionModelInput      
WHERE IsManuallyAdded = 1  
    
else if ((@listSymbols is NULL or @listSymbols = '') and @fetchZeroPositionData=1)    
Insert into #Symbols       
Select distinct Symbol from PM_taxlots WHERE  PM_taxlots.TaxLotOpenQty >0 and PM_taxlots.Taxlot_pk in( select max (Taxlot_pk) from PM_taxlots group by taxlotID)        
union      
select distinct Symbol from T_group  where CumQty >0                    
and  StateID=1      
UNION     
SELECT DISTINCT symbol from #SecMasterTemp    
where AssetID=7
UNION
SELECT DISTINCT symbol FROM T_UserOptionModelInput      
WHERE IsManuallyAdded = 1   
                               
else       
 Insert into #Symbols        
 Select Items as Symbol from dbo.Split(@listSymbols,',')       
End                         
          
INSERT INTO #Temp(Symbol, SecurityName, OSISymbol, IDCOSymbol, AssetID, UnderLyingSymbol,ExpirationDate,StrikePrice,PutorCall,VsCurrencyID,LeadCurrencyID,ProxySymbol,BloombergSymbol,IsHistorical, AuecID,SharesOutstanding, IsManualInput, BloombergSymbolWithExchangeCode)                        
SELECT     distinct SM.Symbol, SM.SecurityName, SM.OSISymbol, SM.IDCOSymbol, SM.AssetID, SM.UnderLyingSymbol,SM.ExpirationDate,SM.StrikePrice,SM.PutorCall,SM.VSCurrencyID,SM.LeadCurrencyID,SM.ProxySymbol ,SM.BloombergSymbol,'0', SM.AuecID,SM.SharesOutstanding,0,SM.BloombergSymbolWithExchangeCode                
     
FROM #SecMasterTemp SM  INNER JOIN #Symbols  on #Symbols.Symbol = SM.Symbol       
                       
--WHERE  PM_taxlots.TaxLotOpenQty >0 and PM_taxlots.Taxlot_pk in( select max (Taxlot_pk) from PM_taxlots group by taxlotID)                    
--                    
--INSERT INTO #Temp(Symbol, SecurityName, OSISymbol, IDCOSymbol, AssetID, UnderLyingSymbol,ExpirationDate,StrikePrice,PutorCall,VsCurrencyID,LeadCurrencyID,ProxySymbol)                        
--SELECT     distinct SM.TickerSymbol, SM.CompanyName, SM.OSISymbol, SM.IDCOSymbol, SM.AssetID, SM.UnderLyingSymbol,SM.ExpirationDate,SM.StrikePrice,SM.PutorCall,SM.VSCurrencyID,SM.LeadCurrencyID,SM.ProxySymbol                          
--FROM V_SecMasterData_WithUnderlying SM INNER JOIN  T_group G on G.Symbol=SM.TickerSymbol   where CumQty >0                    
--and  G.StateID=1                     
                                                           
INSERT INTO #Temp(Symbol, SecurityName, OSISymbol, IDCOSymbol, AssetID, UnderLyingSymbol,ExpirationDate,StrikePrice,PutorCall,VsCurrencyID,LeadCurrencyID,ProxySymbol ,BloombergSymbol,IsHistorical,AuecID,SharesOutstanding,IsManualInput,BloombergSymbolWithExchangeCode)                        
SELECT  distinct SM.Symbol, SM.SecurityName, SM.OSISymbol, SM.IDCOSymbol, SM.AssetID, SM.UnderLyingSymbol,SM.ExpirationDate,SM.StrikePrice,SM.PutorCall,SM.VSCurrencyID,SM.LeadCurrencyID,SM.ProxySymbol,SM.BloombergSymbol,'0', SM.AuecID,SM.SharesOutstanding,0,SM.BloombergSymbolWithExchangeCode                     
     
FROM #SecMasterTemp SM  inner join #Temp on SM.Symbol = #Temp.UnderlyingSymbol                  
WHERE SM.Symbol not in (select Symbol from #Temp)                     
       
 if ((@listSymbols IS NOT NULL AND @listSymbols <> ''))     
INSERT INTO #Temp(Symbol, SecurityName, OSISymbol, IDCOSymbol, AssetID, UnderLyingSymbol,ExpirationDate,StrikePrice,PutorCall,VsCurrencyID,LeadCurrencyID,ProxySymbol ,BloombergSymbol,IsHistorical,AuecID,SharesOutstanding,IsManualInput,BloombergSymbolWithExchangeCode)                        
SELECT  distinct SM.Symbol, SM.SecurityName, SM.OSISymbol, SM.IDCOSymbol, SM.AssetID, SM.UnderLyingSymbol,SM.ExpirationDate,SM.StrikePrice,SM.PutorCall,SM.VSCurrencyID,SM.LeadCurrencyID,SM.ProxySymbol,SM.BloombergSymbol,'1', SM.AuecID,SM.SharesOutstanding,0,SM.BloombergSymbolWithExchangeCode                     
     
FROM #SecMasterTemp SM  inner join T_UserOptionModelInput on SM.Symbol = T_UserOptionModelInput.Symbol                  
WHERE T_UserOptionModelInput.Symbol not in (select Symbol from #Temp)        
                    
END      
      
      
--Update #Temp                      
--set SecurityName = SM.CompanyName                      
--FROM V_SecMasterData_WithUnderlying SM                      
--where  #Temp.Symbol = SM.TickerSymbol                      
                  
SELECT                         
distinct TMP.Symbol as Symbol,                            
TMP.SecurityName as SecurityDescription,            
IsNull(OMI.HistoricalVolatility,0) as HistoricalVolatility,                          
IsNull(OMI.HistoricalVolatilityUsed, 0) as HistoricalVolatilityUsed,                            
IsNull(OMI.UserVolatility,0) as UserVolatility ,                         
IsNull(OMI.UserVolatilityUsed, 0) as UserVolatilityUsed,                            
IsNull(OMI.UserInterestRate,0) as   UserInterestRate,                        
IsNull(OMI.UserInterestRateUsed, 0) as UserInterestRateUsed,                            
IsNull(OMI.UserDividend,0)as UserDividend,                            
IsNull(OMI.UserDividendUsed, 0) as UserDividendUsed,                                      
IsNull(OMI.UserStockBorrowCost,0)as UserStockBorrowCost,                            
IsNull(OMI.UserStockBorrowCostUsed, 0) as UserStockBorrowCostUsed,                            
IsNull(OMI.UserDelta,0) as UserDelta,                            
IsNull(OMI.UserDeltaUsed, 0) as UserDeltaUsed,                            
IsNull(OMI.UserLastPrice,0) as UserLastPrice,                            
IsNull(OMI.UserLastPriceUsed, 0) as UserLastPriceUsed,                  
IsNull(OMI.UserForwardPoints, 0) as UserForwardPoints,                           
IsNull(OMI.UserForwardPointsUsed, 0) as UserForwardPointsUsed,                        
IsNull(OMI.UserTheoreticalPriceUsed, 0) as UserTheoreticalPriceUsed,            
IsNull(OMI.UserProxySymbolUsed, 0) as UserProxySymbolUsed,           
ISNULL(OMI.UserSharesOutstandingUsed,0) as UserSharesOutstandingUsed,    
ISNULL(OMI.UserSharesOutstanding,0) as UserSharesOutstanding,  
ISNULL(OMI.SMUserSharesOutstandingUsed,0) as SMUserSharesOutstandingUsed,    
ISNULL(TMP.SharesOutstanding,0) as SMUserSharesOutstanding,       
ISNULL(OMI.UserClosingMarkUsed,0) as UserClosingMarkUsed,        
TMP.OSISymbol,                            
TMP.IDCOSymbol,                            
'PS' as PSSymbol,                          
TMP.AssetID,                          
TMP.UnderLyingSymbol,                    
TMP.ExpirationDate,                    
TMP.StrikePrice,                    
TMP.PutorCall,                  
TMP.VsCurrencyID,                  
TMP.LeadCurrencyID,            
TMP.ProxySymbol,      
TMP.BloombergSymbol,      
TMP.IsHistorical,  
TMP.AuecID,
IsNull(omi.IsManuallyAdded,0) as ManualInput,    
TMP.BloombergSymbolWithExchangeCode
FROM #Temp as TMP                           
LEFT OUTER JOIN T_UserOptionModelInput as OMI on TMP.Symbol = OMI.Symbol                                    
                         
                      
DROP TABLE #Temp,#Symbols,#SecMasterTemp                        
                           
END
