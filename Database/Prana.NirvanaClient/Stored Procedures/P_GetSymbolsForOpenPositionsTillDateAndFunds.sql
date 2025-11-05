
--EXEC P_GetSymbolsForOpenPositionsTillDateAndFunds '2014-06-05 08:00:00.000',1182,1
CREATE Procedure [dbo].[P_GetSymbolsForOpenPositionsTillDateAndFunds]
(
@Date DateTime,
@FundIds nvarchar(200),
@SMBatchID int
)
As
Create Table #OpenSymbol
(
TickerSymbol nvarchar(200),
BloombergSymbol nvarchar(200),
ISINSymbol nvarchar(20),
SEDOLSymbol nvarchar(20),
CUSIPSymbol nvarchar(20),
AUECID int,
FundID int
)
SELECT * into #tempSplit from dbo.Split(@FundIds,',')as Items

------------------- To apply filter condition ------------------------------------
declare @filterClause varchar(max)
select @filterClause= FilterClause from T_SMBatchSetup where SMBatchID=@SMBatchID 
if @filterClause is not null and @filterClause <>''
set @filterClause = ' and (' + @filterClause +')'
else
set @filterClause=''
----------------------------------------------------------------------------------
declare @query varchar(max)

set @query=
'Insert Into #OpenSymbol
Select distinct
sec.TickerSymbol,
sec.BloombergSymbol,
sec.ISINSymbol,
sec.SEDOLSymbol,
sec.CUSIPSymbol,
sec.AUECID,
PT.FundID
from PM_Taxlots PT  
left Outer Join V_SecMasterData as sec on sec.TickerSymbol = PT.Symbol    
Where Taxlot_PK in                                                                                     
(                                                                
 Select Max(Taxlot_PK) from PM_Taxlots                                                                                     
 Where DateDiff(d, PM_Taxlots.AUECModifiedDate,'+''''+CONVERT(VARCHAR(100),@date, 101)+''''+') >= 0                                      
 Group By TaxlotId                                                   
)                                                                      
and TaxLotOpenQty<>0   
and PT.FundID in (select * from #tempSplit) 
and (sec.ExpirationDate >='+''''+CONVERT(VARCHAR(100),@date, 101)+'''' +
' OR ExpirationDate IN (SELECT MIN(ExpirationDate) from V_SecMasterData)) ' + @filterClause

--print(@query)
exec (@query)

Select 
OS.TickerSymbol,
OS.BloombergSymbol,
OS.ISINSymbol,
OS.SEDOLSymbol,
OS.CUSIPSymbol,
COALESCE(CPM.SourceID,1) as SourceID,
COALESCE(PS.SourceName,'') as SecondarySource
from #OpenSymbol OS
left JOIN T_AUEC A on A.AUECID= OS.AUECID
left JOIN T_CompanyPricingMaster CPM on CPM.CompanyFundID = OS.FundID and CPM.AssetClassID= A.AssetID  and CPM.ExchangeID = A.ExchangeID and CPM.RuleType=2 -- 2 for SM Batch Rules
left JOIN T_PricingSecondarySource PS on PS.SourceID = CPM.SecondarySourceID

drop TABLE #tempSplit
Drop Table #OpenSymbol

