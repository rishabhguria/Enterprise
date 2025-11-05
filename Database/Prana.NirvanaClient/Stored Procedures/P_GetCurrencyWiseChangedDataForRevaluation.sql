CREATE Procedure [dbo].[P_GetCurrencyWiseChangedDataForRevaluation]        
(        
@EndDate DATETIME,        
@fundIDs VARCHAR(max)       
)        
        
AS        
        
DECLARE @BaseCurrecyId INT
SELECT top 1 @BaseCurrecyId = BaseCurrencyID FROM T_Company WITH(NOLOCK) Where CompanyID > 0
     
-- Preliminary preparations        
-- 1. Creating Fund table based on Fund string passed.        
CREATE TABLE #Funds (FundID INT)        
INSERT INTO #Funds        
SELECT Items AS FundID        
FROM dbo.Split(@FundIDs, ',')        
        
--Step 1: Select non processed changes.        
SELECT * INTO #T_NonProcessedChanges         
FROM T_TempTradeAudit  WITH(NOLOCK)     
WHERE Datediff(d,OriginalDate,@EndDate) >=0 
And FundID IN(SELECT FundID from #Funds)  
  
-- Step 2 : Split Priority-1 and Priority-2 changes in 2 different tables.        
SELECT Symbol,FundID,OriginalDate,0 as CurrencyID INTO #T_P1Changes         
FROM #T_NonProcessedChanges        
WHERE Action IN(1,2,8,10,12,13,14,15,16,17,18,19,20,21,24,25,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,83,84,85,88,89,90,91,93,94)  

SELECT Symbol, FundID,OriginalDate, dateadd(day,1,OriginalDate) as nextdate,0 as CurrencyID        
INTO #T_PChanges         
FROM #T_NonProcessedChanges        
WHERE Action IN(49,86)        

CREATE TABLE #T_P2Changes
(
Symbol Nvarchar(50),
FundId Int,
OriginalDate DateTime,
nextdate DateTime,
CurrencyID INT
)    

-- Step 3. Fetching open Symbols Fund wise For Priority-2 changes.        
IF exists (SELECT top 1 * from #T_NonProcessedChanges where action in (50,87))    
BEGIN    
INSERT INTO #T_PChanges     
SELECT G.Symbol,PM.FundId as FundID,OriginalDate, dateadd(day,1,OriginalDate) as nextdate,0 as CurrencyID  
FROM T_Group G 
INNER JOIN PM_Taxlots PM ON G.GroupId = PM.GroupId
left join T_CurrencyStandardPairs CSP ON G.CurrencyID=CSP.FromCurrencyID    
INNER JOIN #T_NonProcessedChanges NPC ON CSP.eSignalSymbol=NPC.Symbol AND PM.FundId = NPC.FundId
Where NPC.action in (50,87)      
and  G.CurrencyID <> @BaseCurrecyId  
and DateDiff(d, G.AUECLocalDate, @EndDate) >= 0    
UNION    
SELECT G.Symbol,PM.FundId as FundID,OriginalDate, dateadd(day,1,OriginalDate) as nextdate,0 as CurrencyID    
FROM T_Group G 
INNER JOIN PM_Taxlots PM ON G.GroupId = PM.GroupId
left join T_CurrencyStandardPairs CSP ON G.CurrencyID=CSP.ToCurrencyID    
INNER JOIN #T_NonProcessedChanges NPC ON CSP.eSignalSymbol=NPC.Symbol AND PM.FundId = NPC.FundId
Where NPC.action in (50,87)      
and  G.CurrencyID <> @BaseCurrecyId   
and DateDiff(d, G.AUECLocalDate, @EndDate) >= 0    
END    

CREATE TABLE #PM_Taxlots (FundID int,Symbol VARCHAR(100))        
        
INSERT INTO #PM_Taxlots (FundID,Symbol)        
SELECT Distinct PM.FundID,PM.Symbol        
FROM PM_Taxlots PM
INNER JOIN #T_PChanges TP ON PM.FundId = TP.FundId AND PM.Symbol = TP.Symbol    
WHERE TaxLotOpenQty <> 0        
AND Taxlot_PK IN         
(        
SELECT max(Taxlot_PK)        
FROM PM_Taxlots        
WHERE DateDiff(d, PM_Taxlots.AUECModifiedDate, @EndDate) >= 0        
GROUP BY taxlotid        
)        
And PM.FundID IN(SELECT FundID from #Funds)      

-- Step 4: Replicating information of P-2 changes for every fund in which the Symbol exists as we         
-- have to run revaluation for every fund in case of P-2 changes.        
INSERT INTO #T_P2Changes        
SELECT #PM_Taxlots.Symbol,#PM_Taxlots.FundID,OriginalDate,nextdate,0 as CurrencyID         
FROM #T_PChanges        
Inner JOIN #PM_Taxlots        
ON #T_PChanges.Symbol = #PM_Taxlots.Symbol AND #T_PChanges.FundId = #PM_Taxlots.FundId     

Delete from #T_P2Changes where fundid = 0        

Select *  into #SecMasterData from V_SecMasterData
Where CurrencyID <> @BaseCurrecyId


Update P1
Set P1.CurrencyID = CSP.CurrencyID
FROM T_Currency CSP 
INNER JOIN #T_P1Changes P1 
ON CSP.CurrencySymbol = P1.Symbol 
and  CSP.CurrencyID <> @BaseCurrecyId   

Update P1
Set P1.CurrencyID = SM.CurrencyID
From #SecMasterData SM
inner join #T_P1Changes P1
ON SM.TickerSymbol = P1.Symbol

Update P2
Set P2.CurrencyID = SM.CurrencyID
From #SecMasterData SM
inner join #T_P2Changes P2
ON SM.TickerSymbol = P2.Symbol

delete from #T_P1Changes where CurrencyID = 0
delete from #T_P2Changes where CurrencyID = 0

Select TransactionDate, AccountID, Currency, Comments, IsProcessed into #T_ManualJournalEntries 
from T_CashJournalAudit
Where Isprocessed = 0 and Currency <> 'USD'
and Datediff(d,TransactionDate,@EndDate) >=0 
And AccountID IN(SELECT FundID from #Funds)

-- Step 5: Detect Symbol wise Minimum Date for P-1 Changes.        
Select CurrencyID,FundID, min(OriginalDate) as FromDate,@EndDate as ToDate  
INTO #T_ChangedSymbols         
FROM #T_P1Changes        
Group BY CurrencyID,FundID        

Insert into #T_ChangedSymbols
Select CurrencyID,AccountID,min(TransactionDate) as FromDate, @EndDate as ToDate 
FROM #T_ManualJournalEntries mje inner join T_Currency c on mje.Currency = c.CurrencySymbol
Group By AccountID, CurrencyID
 
Create TABLE #T_FxRateChangedData
(
	CurrencyID int,
	AccountID Int,
	FromDate DateTime,
	ToDate DateTime
)


--Fx Rate changed Data If There is no Symbol
IF exists (SELECT top 1 * from #T_NonProcessedChanges where action in (50,87))    
BEGIN    
INSERT INTO #T_FxRateChangedData     
SELECT ToCurrencyID as CurrencyID ,FundID As AccountID, OriginalDate as FromDate, dateadd(day,1,OriginalDate) as ToDate 
FROM T_CurrencyStandardPairs CSP Inner join #T_NonProcessedChanges NPC ON CSP.eSignalSymbol=NPC.Symbol 
Where NPC.action in (50,87)
and  CSP.ToCurrencyID <> @BaseCurrecyId   
UNION    
SELECT FromCurrencyID as CurrencyID ,FundID As AccountID, OriginalDate as FromDate, dateadd(day,1,OriginalDate) as ToDate 
FROM T_CurrencyStandardPairs CSP Inner join #T_NonProcessedChanges NPC ON CSP.eSignalSymbol=NPC.Symbol 
Where NPC.action in (50,87) 
and  CSP.FromCurrencyID <> @BaseCurrecyId 
END 

Insert into #T_ChangedSymbols
Select CurrencyID, AccountID, Min(FromDate),Max(ToDate) 
FROM #T_FxRateChangedData
Group By AccountID, CurrencyID

-- Step 6: Delete From P2 Changes Where Date falls in P1 changes date range.        
Delete #T_P2Changes        
From #T_P2Changes        
Inner JOIN #T_ChangedSymbols        
ON #T_ChangedSymbols.CurrencyID = #T_P2Changes.CurrencyID and #T_ChangedSymbols.FundID = #T_P2Changes.FundID        
Where #T_ChangedSymbols.FromDate <= #T_P2Changes.OriginalDate        
and #T_ChangedSymbols.ToDate >= #T_P2Changes.OriginalDate   

-- Step 7: Merge All Symbols Together.        
Insert Into #T_ChangedSymbols        
Select CurrencyID,FundID,OriginalDate,nextdate
from #T_P2Changes order by fundid,CurrencyID,OriginalDate    
    

-- Step 8: Create date range for symbols        
        
--i.        
SELECT IDENTITY(int,1,1) as ID, CurrencyID, FUNDid, FromDate, Todate, 0 as Diff ,0 as diff2        
into #T_ChangedSymbols_Final from #T_ChangedSymbols order by fundid,CurrencyID,FromDate        
      
--ii.        
Update A        
Set A.diff = ISNULL(DATEDIFF(d,B.Fromdate,A.ToDate),-10)        
From #T_ChangedSymbols_Final A        
left join #T_ChangedSymbols_Final B        
On A.ID+1 = B.ID and A.CurrencyID = B.CurrencyID And A.Fundid = B.Fundid    
      
--iii.        
update #T_ChangedSymbols_Final        
set diff2=1        
Where ID in(Select min(ID) from #T_ChangedSymbols_Final group by FUNDid,CurrencyID)    
      
--iv.        
Update A        
Set A.diff2 = 1        
From #T_ChangedSymbols_Final A        
left join #T_ChangedSymbols_Final B        
On A.ID = B.ID+1 and A.CurrencyID = B.CurrencyID And A.Fundid = B.Fundid        
Where B.Diff<-1       
   
--v.        
DELETE From #T_ChangedSymbols_Final        
Where Diff >= -1 and diff2 = 0   
       
--vi.   
CREATE TABLE #T_DateRange (CurrencyID int,Fundid int, FromDate DATETIME,ToDate DATETIME,  SingleOrAll VARCHAR(100))  
INSERT INTO #T_DateRange
Select CurrencyID,Fundid,CAST(FLOOR(CAST(FromDate AS FLOAT)) AS DATETIME) as FromDate,CAST(FLOOR(CAST(ToDate AS FLOAT)) AS DATETIME) as ToDate, 'Single' as SingleOrAll    
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
SELECT A.CurrencyID,a.fundid,CONVERT(DATETIME,CONVERT(VARCHAR(10),B.FromDate, 112)) as FromDate, CONVERT(DATETIME,CONVERT(VARCHAR(10),A.Todate, 112)) as ToDate, 'Single' as SingleOrAll     
From #T_ChangedSymbols_Final A        
left join #T_ChangedSymbols_Final B        
On A.ID = B.ID+1 and a.CurrencyID = b.CurrencyID and a.FUNDid = b.fundid        
WHERE a.Diff < -1 and b.Diff2 = 1        
  
--Step 9: Insert ALL in case of Last revaluation run was before the end date, so that it can run for all the currency in that date range.        
Insert into #T_DateRange        
Select DISTINCT cal.CurrencyID,LCDR.FundID, CONVERT(DATETIME,CONVERT(VARCHAR(10),isnull(LCDR.lastRevalRunDate,'1800-01-01'), 112)),CONVERT(DATETIME,CONVERT(VARCHAR(10),@Enddate, 112)), 'ALL_NIRVSYMBOL'    
From T_LastCalcDateRevaluation LCDR        
INNER JOIN #Funds ON LCDR.FundID = #Funds.Fundid  
INNER JOIN T_LastCalculatedBalanceDate cal WITH(NOLOCK) ON #Funds.FundID = cal.FundID
INNER JOIN T_SubAccounts sub WITH(NOLOCK) ON cal.SubAcID = sub.SubAccountID
INNER JOIN T_TransactionType ty WITH(NOLOCK) ON sub.TransactionTypeID = ty.TransactionTypeID
INNER JOIN T_CompanyFunds fund WITH(NOLOCK) ON cal.FundID = fund.CompanyFundID
Where ty.TransactionTypeAcronym IN ('Cash', 'ACB')  AND isnull(LCDR.lastRevalRunDate,'1800-01-01') < @EndDate  and cal.CurrencyID <> @BaseCurrecyId   
        
Delete DR    
From #T_DateRange DR    
Inner join T_LastCalcDateRevaluation LCDR            
ON DR.FundID = LCDR.FundID    
Where SingleOrAll='ALL_NIRVSYMBOL'       
and Datediff(d,@EndDate,getdate()) > 0 and Datediff(d,@EndDate,isnull(LCDR.lastRevalRunDate,'1800-01-01')) >= 0    
          
Select *  into #AllSymbols from #T_DateRange where SingleOrAll='ALL_NIRVSYMBOL'  
  
Delete Symbols  
from #T_DateRange Symbols  
Inner JOIN  #AllSymbols AllSymbols  
on Symbols.fundid=AllSymbols.fundid  
Where AllSymbols.FromDate<=Symbols.FromDate  
And Symbols.SingleOrAll<>'ALL_NIRVSYMBOL'  
  
Update #T_DateRange              
set ToDate = dateadd(d,-1,todate)            
Where DATEDIFF(d,TODATE,GETDATE()) < 0  
  
Update  Symbols  
set Symbols.ToDate = dateadd(d,-1,Symbols.todate)  
from  #T_DateRange Symbols  
inner join #AllSymbols AllSymbols  
on Symbols.fundid=AllSymbols.fundid  
where symbols.SingleOrAll<>'ALL_NIRVSYMBOL'   
and AllSymbols.fromdate=Symbols.todate  
  
Update  Symbols  
set Symbols.ToDate = dateadd(d,-1,AllSymbols.fromdate)  
from  #T_DateRange Symbols  
inner join #AllSymbols AllSymbols  
on Symbols.fundid=AllSymbols.fundid  
where symbols.SingleOrAll<>'ALL_NIRVSYMBOL'   
and datediff(d,AllSymbols.fromdate,Symbols.todate) >= 0

Select CurrencyID, FundID, FromDate, ToDate from #T_DateRange        
where CurrencyID IS NOT NULL and CurrencyID <> 0 and CurrencyID <> '-2147483648' 
Order by CurrencyID
        
--Deleteing temporary tables        
        
Drop table #T_ChangedSymbols_Final        
Drop table #Funds        
Drop table #T_NonProcessedChanges        
Drop table #T_P1Changes        
Drop table #T_P2Changes        
Drop table #PM_Taxlots        
Drop table #T_ChangedSymbols        
Drop table #T_DateRange  
Drop Table #SecMasterData
Drop Table #T_ManualJournalEntries
Drop Table #AllSymbols
DROP TABLE #T_FxRateChangedData
DROP TABLE #T_PChanges