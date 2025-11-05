
/**********************************************************
Modified by- omshiv
Description - add underlyingSymbols to symbol list for traded derivative securities
http://jira.nirvanasolutions.com:8080/browse/PRANA-3257

Date: Jan 2014

Exec cmd : GetAllTradedSymbols
********************************************************/      
      
CREATE proc [dbo].[GetAllTradedSymbols]  
as  

CREATE TABLE #RequestSybolTable(                                        
 [Symbol] [varchar](100) ,                                        
)

Select * into #SecmasterData from V_SecMasterData  
where (V_SecMasterData.ExpirationDate >= getdate() AND V_SecMasterData.AssetID in(2,4,8,11,3)) or (V_SecMasterData.AssetID not in (2,4,8,11,3)) 

insert INTO #RequestSybolTable
 Select distinct symbol from T_Group
 INNER JOIN #SecmasterData Secmaster ON Secmaster.TickerSymbol = T_Group.Symbol
 WHERE (Secmaster.ExpirationDate >= getdate() AND Secmaster.AssetID in(2,4,8,11,3)) or (Secmaster.AssetID not in (2,4,8,11,3))


insert INTO #RequestSybolTable
 Select distinct Secmaster.UnderLyingSymbol from T_Group
 INNER JOIN #SecmasterData Secmaster ON Secmaster.TickerSymbol = T_Group.Symbol
 WHERE (Secmaster.ExpirationDate >= getdate() AND Secmaster.AssetID in(2,4,8,11,3))

select DISTINCT Symbol from #RequestSybolTable
drop table #RequestSybolTable 
drop TABLE #SecmasterData  

