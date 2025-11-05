      
CREATE proc [dbo].[P_GetHistOrOpenTradedSymbols]  
(                                                                                                                          
 @isOpenTradedSymbols bit                                                                                                    
)                                                                                                                          
As
  
IF(@isOpenTradedSymbols = 'False')
BEGIN 

 Select distinct symbol from T_Group
 INNER JOIN V_SecmasterData Secmaster ON Secmaster.TickerSymbol = T_Group.Symbol
END

else IF(@isOpenTradedSymbols = 'True')
BEGIN
CREATE TABLE #RequestSybolTable(                                        
 [Symbol] [varchar](100) ,                                        
)                  
                                      
INSERT INTO #RequestSybolTable                                                      
 Select distinct symbol from T_Group
 INNER JOIN V_SecmasterData Secmaster ON Secmaster.TickerSymbol = T_Group.Symbol
where StateID =1

INSERT INTO #RequestSybolTable 
Select DISTINCT symbol from PM_Taxlots where TaxLotOpenQty<>0 and
TaxLot_PK 
in (SELECT MAX(TaxLot_PK) from PM_Taxlots where TaxLotOpenQty>=0 GROUP BY TaxLotID)


select DISTINCT Symbol from #RequestSybolTable
drop table #RequestSybolTable  

END


