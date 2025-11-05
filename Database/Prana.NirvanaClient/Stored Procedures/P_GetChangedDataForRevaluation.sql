
/*        
Created By: Kuldeep Agrawal        
Creation Date: 02 Feb 2016        
Purpose:- For revaluation, now we are just considering the changes done in the system after the last reval run, so this SP        
returns the changed symbols with a specific date range in which we need to run the revaluation batch again.    
Execution Method : exec [P_GetChangedDataForRevaluation] '2016-02-10','1182,1183,1184,1185,1190',1    
*/        
        
CREATE Procedure P_GetChangedDataForRevaluation        
(        
@EndDate DATETIME,        
@fundIDs VARCHAR(max),        
@isincludecommission bit = 0        
)        
        
AS        
        
DECLARE @BaseCurrecyId INT
SELECT @BaseCurrecyId = BaseCurrencyID FROM T_Company WITH(NOLOCK)
     
-- Preliminary preparations        
-- 1. Creating Fund table based on Fund string passed.        
CREATE TABLE #Funds (FundID INT)        
INSERT INTO #Funds        
SELECT Items AS FundID        
FROM dbo.Split(@FundIDs, ',')        
        
--Step 1: Select non processed changes.        
INSERT INTO T_TempTradeAudit(AuditID, Symbol, Action, FundID, OriginalDate)
SELECT AuditID, Symbol, Action, FundID, OriginalDate
FROM T_TradeAudit WITH(NOLOCK)       
WHERE IsProcessed = 0        
AND Datediff(d,OriginalDate,@EndDate) >=0 
And FundID IN(SELECT FundID from #Funds)
  
--Step 1: Select non processed changes.        
SELECT * INTO #T_NonProcessedChanges
FROM T_TempTradeAudit  WITH(NOLOCK)     
WHERE Datediff(d,OriginalDate,@EndDate) >=0 
And FundID IN(SELECT FundID from #Funds)
  
-- Step 2 : Split Priority-1 and Priority-2 changes in 2 different tables.        
SELECT Symbol,FundID,OriginalDate INTO #T_P1Changes         
FROM #T_NonProcessedChanges        
WHERE Action IN(1,2,8,10,12,13,14,15,16,17,18,19,20,21,24,25,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,55,56,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,83,84,85,88,89,90,91,93,94)  

SELECT Symbol,FundID,OriginalDate, dateadd(day,1,OriginalDate) as nextdate        
INTO #T_PChanges         
FROM #T_NonProcessedChanges        
WHERE Action IN(49,86)        

CREATE TABLE #T_P2Changes
(
Symbol Nvarchar(50),
FundId Int,
OriginalDate DateTime,
nextdate DateTime
)   

-- Step 3. Fetching open Symbols Fund wise For Priority-2 changes.        
IF exists (SELECT top 1 * from #T_NonProcessedChanges where action in (50,87))    
BEGIN   

SELECT
 G.Symbol,
 PM.FundId, 
pm.AUECModifiedDate,
G.AUECLocalDate,				 
 G.GroupId,
 
 CurrencyID
 into #OpenPositionsGroups
 FROM T_Group G 
INNER JOIN PM_Taxlots PM ON G.GroupId = PM.GroupId
inner join  #Funds on #Funds.FundID= PM.fundid
where DateDiff(d, G.AUECLocalDate, @EndDate) >= 0   
 
INSERT INTO #T_PChanges     
SELECT PM.Symbol,PM.FundId as FundID,OriginalDate, dateadd(day,1,OriginalDate) as nextdate    
FROM #OpenPositionsGroups PM
left join T_CurrencyStandardPairs CSP ON PM.CurrencyID=CSP.FromCurrencyID    
INNER JOIN #T_NonProcessedChanges NPC ON CSP.eSignalSymbol=NPC.Symbol AND PM.FundId = NPC.FundId
Where NPC.action in (50,87)      
and  PM.CurrencyID <> @BaseCurrecyId  
and DateDiff(d, PM.AUECLocalDate, @EndDate) >= 0    
UNION    
SELECT G.Symbol,PM.FundId as FundID,OriginalDate, dateadd(day,1,OriginalDate) as nextdate    
FROM T_Group G 
INNER JOIN PM_Taxlots PM ON G.GroupId = PM.GroupId
left join  T_CurrencyStandardPairs CSP ON G.CurrencyID=CSP.ToCurrencyID    
INNER JOIN #T_NonProcessedChanges NPC ON CSP.eSignalSymbol=NPC.Symbol AND PM.FundId = NPC.FundId
Where NPC.action in (50,87)      
and  G.CurrencyID <> @BaseCurrecyId   
and DateDiff(d, G.AUECLocalDate, @EndDate) >= 0   
 
drop table #OpenPositionsGroups	
END    
     

-- Step 4: Replicating information of P-2 changes for every fund in which the Symbol exists as we         
-- have to run revaluation for every fund in case of P-2 changes.        
INSERT INTO #T_P2Changes        
SELECT Symbol,FundID,OriginalDate,nextdate         
FROM #T_PChanges        
--Inner JOIN #PM_Taxlots        
--ON #T_PChanges.Symbol = #PM_Taxlots.Symbol AND #T_PChanges.FundId = #PM_Taxlots.FundId         

Delete from #T_P2Changes where fundid = 0        

-- Step 5: Detect Symbol wise Minimum Date for P-1 Changes.        
Select Symbol,FundID, min(OriginalDate) as FromDate,@EndDate as ToDate  
INTO #T_ChangedSymbols         
FROM #T_P1Changes        
Group BY Symbol,FundID        
 
-- Step 6: Delete From P2 Changes Where Date falls in P1 changes date range.        
Delete #T_P2Changes        
From #T_P2Changes        
Inner JOIN #T_ChangedSymbols        
ON #T_ChangedSymbols.Symbol = #T_P2Changes.Symbol and #T_ChangedSymbols.FundID = #T_P2Changes.FundID        
Where #T_ChangedSymbols.FromDate <= #T_P2Changes.OriginalDate        
and #T_ChangedSymbols.ToDate >= #T_P2Changes.OriginalDate   

-- Step 7: Merge All Symbols Together.        
Insert Into #T_ChangedSymbols        
Select * from #T_P2Changes order by fundid,symbol,OriginalDate        

-- Step 8: Create date range for symbols        
        
--i.        
SELECT IDENTITY(int,1,1) as ID, Symbol, FUNDid, FromDate, Todate, 0 as Diff ,0 as diff2        
into #T_ChangedSymbols_Final from #T_ChangedSymbols order by fundid,symbol,FromDate        
      
--ii.        
Update A        
Set A.diff = ISNULL(DATEDIFF(d,B.Fromdate,A.ToDate),-10)        
From #T_ChangedSymbols_Final A        
left join #T_ChangedSymbols_Final B        
On A.ID+1 = B.ID and A.Symbol = B.Symbol And A.Fundid = B.Fundid    
      
--iii.        
update #T_ChangedSymbols_Final        
set diff2=1        
Where ID in(Select min(ID) from #T_ChangedSymbols_Final group by FUNDid,Symbol)    
      
--iv.        
Update A        
Set A.diff2 = 1        
From #T_ChangedSymbols_Final A        
left join #T_ChangedSymbols_Final B        
On A.ID = B.ID+1 and A.Symbol = B.Symbol And A.Fundid = B.Fundid        
Where B.Diff<-1       
   
--v.        
DELETE From #T_ChangedSymbols_Final        
Where Diff >= -1 and diff2 = 0   
       
--vi.        
Select Symbol,Fundid,CAST(FLOOR(CAST(FromDate AS FLOAT)) AS DATETIME) as FromDate,CAST(FLOOR(CAST(ToDate AS FLOAT)) AS DATETIME) as ToDate  into #T_DateRange     
from #T_ChangedSymbols_Final        
Where Diff<-1 and diff2 =1       
   
--vii.        
DELETE From #T_ChangedSymbols_Final        
Where Diff<-1 and diff2 =1    
      
--viii.        
alter table #T_ChangedSymbols_Final        
drop column id        
  
--ix.        
alter table #T_ChangedSymbols_Final        
add ID int identity(1,1)   
        
--x.        
Insert into #T_DateRange        
SELECT A.Symbol,a.fundid,CONVERT(DATETIME,CONVERT(VARCHAR(10),B.FromDate, 112)) as FromDate, CONVERT(DATETIME,CONVERT(VARCHAR(10),A.Todate, 112)) as ToDate     
From #T_ChangedSymbols_Final A        
left join #T_ChangedSymbols_Final B        
On A.ID = B.ID+1 and a.Symbol = b.Symbol and a.FUNDid = b.fundid        
WHERE a.Diff < -1 and b.Diff2 = 1        
  
--Step 9: Insert ALL in case of Last revaluation run was before the end date, so that it can run for all the symbols in that date range.        
Insert into #T_DateRange        
Select 'ALL_NIRVSYMBOL',LCDR.FundID, CONVERT(DATETIME,CONVERT(VARCHAR(10),isnull(LCDR.lastRevalRunDate,'1800-01-01'), 112)),CONVERT(DATETIME,CONVERT(VARCHAR(10),@Enddate, 112))    
From T_LastCalcDateRevaluation LCDR        
Inner Join #Funds         
ON LCDR.FundID = #Funds.Fundid          
Where isnull(LCDR.lastRevalRunDate,'1800-01-01') < @EndDate        
        
Delete DR    
From #T_DateRange DR    
Inner join T_LastCalcDateRevaluation LCDR            
ON DR.FundID = LCDR.FundID    
Where Symbol='ALL_NIRVSYMBOL'       
and Datediff(d,@EndDate,getdate()) > 0 and Datediff(d,@EndDate,isnull(LCDR.lastRevalRunDate,'1800-01-01')) >= 0    
          
Select *  into #AllSymbols from #T_DateRange where Symbol='ALL_NIRVSYMBOL'  
  
Delete Symbols  
from #T_DateRange Symbols  
Inner JOIN  #AllSymbols AllSymbols  
on Symbols.fundid=AllSymbols.fundid  
Where AllSymbols.FromDate<=Symbols.FromDate  
And Symbols.Symbol<>'ALL_NIRVSYMBOL'  
  
Update #T_DateRange              
set ToDate = dateadd(d,-1,todate)            
Where DATEDIFF(d,TODATE,GETDATE()) < 0  
  
Update  Symbols  
set Symbols.ToDate = dateadd(d,-1,Symbols.todate)  
from  #T_DateRange Symbols  
inner join #AllSymbols AllSymbols  
on Symbols.fundid=AllSymbols.fundid  
where symbols.symbol<>'ALL_NIRVSYMBOL'   
and AllSymbols.fromdate=Symbols.todate  
  
Update  Symbols  
set Symbols.ToDate = dateadd(d,-1,AllSymbols.fromdate)  
from  #T_DateRange Symbols  
inner join #AllSymbols AllSymbols  
on Symbols.fundid=AllSymbols.fundid  
where symbols.symbol<>'ALL_NIRVSYMBOL'   
and datediff(d,AllSymbols.fromdate,Symbols.todate) >= 0

Select * from #T_DateRange        
where Symbol IS NOT NULL  
        
--Deleteing temporary tables        
        
Drop table #T_ChangedSymbols_Final        
Drop table #Funds        
Drop table #T_NonProcessedChanges        
Drop table #T_P1Changes        
Drop table #T_P2Changes        
Drop table #T_ChangedSymbols        
Drop table #T_DateRange  
Drop Table #AllSymbols
DROP TABLE #T_PChanges