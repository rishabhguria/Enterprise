CREATE Procedure [dbo].[P_OpenPositionDividendImport]  
As                                  
Begin  
  
SET NOCOUNT on   
CREATE TABLE [dbo].[#TempTableCSVData]  
(  
 COL1 [varchar](500) ,  
 COL2 [varchar](500),  
 COL3 [varchar](500) ,  
 COL4 [varchar](500),  
 COL5 [varchar](500) ,  
 COL6 [varchar](500),  
 COL7 [varchar](500) ,  
 COL8 [varchar](500),  
 COL9 [varchar](500) ,  
 COL10 [varchar](500),  
 COL11 [varchar](500) ,  
 COL12 [varchar](500),  
 COL13 [varchar](500) ,  
 COL14 [varchar](500),  
 COL15 [varchar](500) ,  
 COL16 [varchar](500),  
 COL17 [varchar](500) ,  
 COL18 [varchar](500) ,
 COL19 [varchar](500),
 COL20 [varchar](500),
 COL21 [varchar](500),
 COL22 [float] 
  
)  
  
Bulk Insert [dbo].[#TempTableCSVData]  
From 'C:\Users\Public\Desktop\Makalu\Dividend Import From BBG Files\Makalu\Prices.CSV'  
With (FirstRow = 2,Fieldterminator = ',', Rowterminator = '\n')  
 --select * from  #TempTableCSVData Where COL17 like '%2019-02-04%'

 Update [#TempTableCSVData]
 Set
 COL1 = RTRIM(LTRIM (COL1)) ,  
 COL2 = RTRIM(LTRIM (COL2)),  
 COL3 = RTRIM(LTRIM (COL3)) ,  
 COL4 = RTRIM(LTRIM (COL4)),  
 COL5 = RTRIM(LTRIM (COL5)) ,  
 COL6 = RTRIM(LTRIM (COL6)),  
 COL7 = RTRIM(LTRIM (COL7)) ,  
 COL8 = RTRIM(LTRIM (COL8)),  
 COL9 = RTRIM(LTRIM (COL9)) ,  
 COL10 = RTRIM(LTRIM (COL10)),  
 COL11 = RTRIM(LTRIM (COL11)) ,  
 COL12 = RTRIM(LTRIM (COL12)),  
 COL13 = RTRIM(LTRIM (COL13)) ,  
 COL14 = RTRIM(LTRIM (COL14)),  
 COL15 = RTRIM(LTRIM (COL15)) ,  
 COL16 = RTRIM(LTRIM (COL16)),  
 COL17 = RTRIM(LTRIM (COL17)) ,  
 COL18 = RTRIM(LTRIM (COL18)) ,
 COL19 = RTRIM(LTRIM (COL19)),
 COL20 = RTRIM(LTRIM (COL20)),
 COL21 = RTRIM(LTRIM (COL21)),
 COL22 = RTRIM(LTRIM (COL22))
  
 

Select   
Distinct PT.Symbol,
sum(TaxlotOpenQty) as TaxlotOpenQty,FundName ,   
Case dbo.GetSideMultiplier(PT.OrderSideTagValue)   
When 1   
 Then 'Long'   
Else 'Short'   
End As Side   
, G.Auecid  
Into #temptable   
from PM_Taxlots PT Inner Join T_Group G on G. GroupID = PT .GroupID  
Inner Join T_Companyfunds TC on TC.CompanyFundID = PT.FundID  
Where TaxLotOpenQty<> 0 and Taxlot_PK in  
(   
 Select Max (Taxlot_PK ) from PM_Taxlots Where DateDiff ( d, PM_Taxlots .AUECModifiedDate , getdate()) >= 0 Group by TaxlotID  
)  
  
Group by PT.OrderSideTagValue,PT.Symbol,FundName ,Auecid  
  

--select col13,* from #TempTableCSVData where col1 like '%QUAL%'  
  
Select  
temp.FundName as AccountName,  
temp.Symbol as Symbol,  
(Temp.TaxLotOpenQty) as TaxLotOpenQty,  
Temp.Side as side,  
tt.Col2 as BBG,  
tt.col17 as ExDate,  
tt.Col18 as PayoutDate,  
LEFT(CONVERT(VARCHAR,dbo.AdjustBusinessDays(tt.Col19,2,temp.Auecid), 120), 10) as RecordDate,  
VSMD.CompanyName as [Description],  
(temp.TaxLotOpenQty*tt.Col22) as Dividend,  
Case  
When Temp.Side = 'Long'  
Then 'Dividend Income'  
Else 'Dividend Expense'  
End as ActivitySummary,  
(Select CurrencySymbol from T_Currency where CurrencyID = VSMD.CurrencyID) as Currency  
Into #tempTableFinalResult  
from #temptable temp   
inner join V_SecMasterData VSMD on VSMD.TickerSymbol=temp.Symbol  
inner join [#TempTableCSVData] tt on tt.COL2=VSMD.BloombergSymbol  
  
Where tt.Col22 <>0 and tt.Col22 <> '' and tt.COL17 is NOT Null and TT.Col17 <> '' and datediff(d,TT.COL17,getdate())=0  
  
Select * from #tempTableFinalResult  
  
Drop Table [#TempTableCSVData]  
Drop Table #temptable,#tempTableFinalResult  
  
  
end 