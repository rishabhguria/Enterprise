CREATE PROCEDURE [dbo].[P_GetOptionModelTable_New]                                                          
                                                                                                                
AS               
              
BEGIN  



  CREATE TABLE #Temp_SecMasterView                                                                                       
(            
  TickerSymbol varchar(100),      
  UnderLyingSymbol varchar (100),      
  AssetID int                                                                                                  
)           
--Created temp table for secmaster view data
CREATE TABLE #Temp_Symbols                                                                                       
(          
  Symbol varchar(100),    
  UnderLyingSymbol varchar (100),    
  AssetID int                                                                                                
)    
             
Create Table #Temp  
(    
   taxlot_pkey int  
)  
--created table for taxlot primary key, which can be used directly.
insert into #Temp(taxlot_pkey)   
select max (Taxlot_pk) from PM_taxlots group by taxlotID    
    
            
INSERT INTO #Temp_SecMasterView(TickerSymbol,UnderlyingSymbol,AssetID)  
SELECT distinct TickerSymbol, UnderLyingSymbol,AssetID  
FROM V_SecMasterData_WithUnderlying   
    
INSERT INTO #Temp_Symbols(Symbol,UnderLyingSymbol,AssetID)          
--SELECT     distinct SM.TickerSymbol, SM.UnderLyingSymbol,SM.AssetID      
--FROM V_SecMasterData_WithUnderlying SM  
Select distinct TickerSymbol,UnderlyingSymbol,AssetID from #Temp_SecMasterView S INNER JOIN PM_taxlots  on PM_taxlots.Symbol = S.TickerSymbol            
WHERE  PM_taxlots.TaxLotOpenQty >0 and PM_taxlots.Taxlot_pk in( select taxlot_pkey from #Temp)        
      
INSERT INTO #Temp_Symbols(Symbol,UnderLyingSymbol,AssetID)          
Select distinct TickerSymbol,UnderlyingSymbol,#Temp_SecMasterView.AssetID from #Temp_SecMasterView INNER JOIN  T_group G on G.Symbol=#Temp_SecMasterView.TickerSymbol   where CumQty >0        
and  G.StateID=1 
--Used temp sec master view table      
      
          
        
Delete from #Temp_SecMasterView where TickerSymbol in (select Symbol from #Temp_Symbols)  
--Deleted rows form #TempSecMaster View Table with symbol that already exist in #Temp_Symbols 
          
INSERT INTO #Temp_Symbols(Symbol,UnderLyingSymbol, AssetID)          
Select distinct TickerSymbol,#Temp_SecMasterView.UnderLyingSymbol,#Temp_SecMasterView.AssetID 
from #Temp_SecMasterView  inner join #Temp_Symbols on #Temp_SecMasterView.TickerSymbol = #Temp_Symbols.UnderlyingSymbol      
--WHERE  SM.TickerSymbol not in (select Symbol from #Temp_Symbols)     
--no need of where clause as symbols present in #Temp_Symbols has already been deleted.

              
SELECT              
#Temp_Symbols.Symbol,     
#Temp_Symbols.UnderLyingSymbol,       
IsNull(HistoricalVolatility,0) as HistoricalVolatility,      
#Temp_Symbols.AssetID as AssetID,               
IsNull(HistoricalVolatilityUsed, 0) as HistoricalVolatilityUsed,              
IsNull(UserVolatility,0) as UserVolatility,              
IsNull(UserVolatilityUsed, 0) as UserVolatilityUsed,              
IsNull (UserInterestRate,0) as UserInterestRate ,              
IsNull(UserInterestRateUsed, 0) as UserInterestRateUsed,              
IsNull (UserDividend,0) as UserDividend,              
IsNull(UserDividendUsed, 0) as UserDividendUsed,                       
IsNull (UserStockBorrowCost,0) as UserStockBorrowCost,              
IsNull(UserStockBorrowCostUsed, 0) as UserStockBorrowCostUsed,              
Isnull (UserDelta,0) as UserDelta,              
IsNull(UserDeltaUsed, 0) as UserDeltaUsed,              
Isnull(UserLastPrice,0)as UserLastPrice,              
IsNull(UserLastPriceUsed, 0) as UserLastPriceUsed ,  
Isnull(UserForwardPoints,0)as UserForwardPoints,                 
IsNull(UserForwardPointsUsed, 0) as UserForwardPointsUsed            
FROM T_UserOptionModelInput        
right join #Temp_Symbols  on T_UserOptionModelInput.Symbol = #Temp_Symbols.Symbol      
    
Drop table #Temp_Symbols   
Drop table #Temp_SecMasterView   
Drop table #Temp   
 --join T_AUEC on SM.AUECID = T_AUEC.AUECID      
--where T_UserOptionModelInput.Symbol = Symbol        
                                                                                                                                                           
END 

