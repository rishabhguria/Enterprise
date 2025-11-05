/* Kuldeep Agrawal: 2014-03-04
   cacheupdationrequired parameter is used for deciding whether updated PI data is to be returned back or not.
   As this SP is called from several places like on closing, bulk closing or deletion of un-allocated trades.
   For closing it is required to publish updated data (PI cache update), so we need to set this parameter to 1
   But in other cases we don't require any cache update so we send this parameter as 0. */

CREATE PROCEDURE [dbo].[P_UpdateOptionModelUserData] 
(
@cacheupdationrequired int
)                    
                                                                                                                                                                             
AS                       
                             
BEGIN                  

Create Table #OpenSymbols (symbol varchar(100))

Insert into #OpenSymbols  
Select distinct Symbol from PM_taxlots WITH (NOLOCK) WHERE  PM_taxlots.TaxLotOpenQty >0 and PM_taxlots.Taxlot_pk in( select max (Taxlot_pk) from PM_taxlots  WITH (NOLOCK) group by taxlotID)  
union
SELECT SM.UnderLyingSymbol from V_SecMasterData_WithUnderlying SM inner JOIN #OpenSymbols OS
on SM.TickerSymbol=OS.symbol
where SM.UnderLyingSymbol not in(SELECT OS.symbol from #OpenSymbols)
union 
SELECT SM.TickerSymbol from V_SecMasterData_WithUnderlying SM  
where SM.AssetId=7  
UNION
SELECT G.Symbol from T_Group G
where G.StateID=1 and G.CumQty>0

Delete FROM T_UserOptionModelInput where T_UserOptionModelInput.Symbol NOT IN (SELECT distinct symbol FROM #OpenSymbols) AND IsManuallyAdded = 0                        

IF(@cacheupdationrequired=1)
EXEC P_GetOptionModelUserData  NULL, 0
                   
DROP TABLE #OpenSymbols                 
                     
END

