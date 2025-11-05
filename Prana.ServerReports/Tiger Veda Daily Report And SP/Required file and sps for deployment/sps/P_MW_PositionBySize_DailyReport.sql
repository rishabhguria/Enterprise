 /*************************************************                                                        
Author : Ankit Misra                                                       
Creation Date : 9th June , 2015                                                          
Description : Script for Position By Size part of Daily Report                                          
             
Modified By: Sandeep Singh    
Date: 10 August 2015    
Desc: http://jira.nirvanasolutions.com:8080/browse/PRANA-10294 (No FX Spot or Forward positions required in the Positions by Size area)                                      
                                   
Execution Statement:                                                       
P_MW_PositionBySize_DailyReport @EndDate='7/7/2015',@Fund=N'1270,1271,1298,1302,1304,1305,1306,1307,1308,1309,1310',@PTHFund='1270'                                   
--*************************************************/                                      
ALTER Procedure [dbo].[P_MW_PositionBySize_DailyReport]                                      
(                                      
 @EndDate datetime,                                      
 @Fund Varchar(max),              
 @PTHFund Varchar(Max)                                      
)                                      
AS                                   
BEGIN                          
                      
--Declare @EndDate datetime                                          
--Declare @Fund Varchar(2000)        
--Declare @PTHFund Varchar(2000)                                             
--                                          
--Set @EndDate = '7/31/2015'                                                                  
--Set @Fund = '1286,1306,1288,1290,1307,1300,1301,1311,1308,1310,1309,1282,1279,1280,1281,1293,1294,1265,1305,1266,1304,1263,1264,1277,1302,1268,1269,1267,1303,1295,1296,1297,1292'                             
--Set @PTHFund = '1400'                                
                                    
Declare @T_FundIDs Table                                                                                                                                                  
(                                                                                                                                                  
 FundId int                                                                                                                                                  
)                                                                                                                                                  
Insert Into @T_FundIDs Select * From dbo.Split(@Fund, ',')                                                                                      
                
---- PTH Funds              
Declare @T_PTHFundIDs Table                                                                                                                                                                        
(                                                                                                                                                                        
 FundId int                                                                                                                                                                        
)                                                                                                                                                                        
Insert Into @T_PTHFundIDs Select * From dbo.Split(@PTHFund, ',')                
                      
Declare @PTHFundCount Int              
Set @PTHFundCount = (Select Count(FundID) from @T_PTHFundIDs)              
                  
If ( @PTHFundCount > 0)              
Begin              
Delete From @T_FundIDs Where FundID In ( Select FUndID From @T_PTHFundIDs)              
              
End                 
                                                                                      
CREATE TABLE #T_CompanyFunds                                                                     
(                                                                                                                       
 CompanyFundID int,                                                                                                              
 FundName varchar(50)                                    
)                                                        
Insert Into #T_CompanyFunds                                                              
Select                                                                                     
CompanyFundID,                                                                     
FundName                                                                                        
From T_CompanyFunds INNER JOIN @T_FundIDs FundIDs ON T_CompanyFunds.CompanyFundID = FundIDs.FundID                                    
-------------------------------------------------------------------                                                
--Select Symbols which are OPEN                                                
-------------------------------------------------------------------                                                
Select   
PNL.Symbol AS OpenSymbol,  
(SUM(BeginningQuantity *UnitCostLocal)/NULLIF(Sum(BeginningQuantity),0)) AS WeightedAveragePrice                                                                 
Into #TempOpenPosSymbols                                                                
From T_MW_GenericPNL PNL                                                                                               
Inner Join #T_CompanyFunds ON #T_CompanyFunds.FundName = PNL.Fund                                                 
Where Datediff(day , Rundate , @EndDate) = 0  And Open_CloseTag In ('O') And Asset Not In ('Cash','FX','FXForward')  
Group By PNL.Symbol   
  
--Select * from #TempOpenPosSymbols                                          
-------------------------------------------------------------------                                                      
--Selection of Required Fields From Generic Middleware Table                                                      
-------------------------------------------------------------------                        
Select                     
PNL.Symbol,                      
UnderlyingSymbol,                      
UnderlyingSymbolPrice,                  
Case                                                                   
 When DateDiff(Day,@EndDate,Rundate) = 0  And Open_CloseTag = 'O' AND BeginningPriceLocal<>0                     
 Then BeginningPriceLocal      
 WHEN DateDiff(Day,@EndDate,Rundate) = 0  And Open_CloseTag = 'O' AND BeginningPriceLocal=0      
 Then ISNULL(TOPS.WeightedAveragePrice,0)                
End As BeginningPriceLocal,                  
Case                                                                   
 When DateDiff(Day,@EndDate,Rundate) = 0  And Open_CloseTag = 'O'                      
 Then EndingPriceLocal                  
End As EndingPriceLocal,                  
Case                                                                   
 When DateDiff(Day,@EndDate,Rundate) = 0  And Open_CloseTag = 'O' AND Asset NOT IN ('FX','FXForward')                     
 Then DeltaExposureBase                  
 Else 0.0                  
End As DeltaExposureBase,                     
TotalRealizedPNLOnCost,                      
ChangeInUNRealizedPNL,                      
Dividend,                      
SecurityName,              
Case              
 When CustomUDA5 <> 'Undefined' And CustomUDA5 <> ''              
 Then CustomUDA5                      
 Else ''              
End As NextReport,              
              
Case                                                                   
 When DateDiff(Day,@EndDate,Rundate) = 0  And Open_CloseTag = 'O' And TVol.TradingVolume <> 0 And EndingPriceLocal <> 0 AND Asset NOT IN ('FX','FXForward')                  
 Then Abs((DeltaExposureBase / TVol.TradingVolume) * EndingPriceLocal)                  
 Else 0.0                  
End As DaytoExit,
TVol.TradingVolume                
              
Into #TempGenericPNL                      
                       
From T_MW_GenericPNL PNL                              
INNER JOIN #T_CompanyFunds ON #T_CompanyFunds.FundName = PNL.Fund                  
Inner Join #TempOpenPosSymbols TOPS ON  TOPS.OpenSymbol = PNL.Symbol               
Left Outer Join PM_DailyTradingVol TVol On TVol.Symbol = PNL.Symbol And DateDiff(Day,TVol.Date,@EndDate) = 0                                         
WHERE DATEDIFF(Day,Rundate,@EndDate) = 0                     
AND Open_CloseTag <> 'Accruals'                      
                       
-------------------------------------------------------------------                                    
---- Selection of Required Fields From Generic Middleware Table                                    
-------------------------------------------------------------------                               
Select                          
UnderlyingSymbol,                      
MAX(Symbol) As SymbolMax,                      
MIN(Symbol) As SymbolMin,                              
Max(BeginningPriceLocal) As BeginningPriceLocal,                      
Max(EndingPriceLocal) As LocalPX,                        
Sum(ISNULL(DeltaExposureBase,0)) AS DeltaNetExposure,                                    
Sum(TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend) AS DayPnl,                              
Max(NextReport) As NextReport,              
Max(DaytoExit) As DaytoExit,
Max(TradingVolume) As TradingVolume                
InTo #TempPNL                             
From #TempGenericPNL                      
Group By UnderlyingSymbol 


Update #TempPNL
Set DaytoExit = 
Case
When TradingVolume <> 0 And LocalPX <> 0
Then Abs((DeltaNetExposure / (TradingVolume * LocalPX)))
Else 0
End
From #TempPNL
                          
                      
Alter Table #TempPNL                           
Add Side Varchar(20),                    
PercentChangePrice Float,                      
SecurityName Varchar(500)                      
                    
                      
Update #TempPNL                      
Set Side =                       
 Case                      
  When DeltaNetExposure >= 0                      
  Then 'Long'                      
  Else 'Short'                      
 End                      
                      
Update Temp                      
Set SecurityName = TempGeneric.SecurityName                      
From #TempPNL Temp                      
Inner Join #TempGenericPNL TempGeneric On TempGeneric.UnderlyingSymbol = Temp.UnderlyingSymbol                      
                     
Update #TempPNL                      
Set PercentChangePrice = ISNULL(((LocalPX - BeginningPriceLocal)/NULLIF(BeginningPriceLocal,0))*100,0)                      
                       
                      
Select                      
UnderlyingSymbol,                      
Side,                      
LocalPX,                      
DeltaNetExposure,                      
DayPnL,                      
PercentChangePrice,                      
SecurityName,                    
SymbolMax,                    
SymbolMin,              
Case              
When NextReport <> '' And ISDATE(NextReport) = 1              
Then Cast(NextReport as Datetime)              
Else ''              
End As NextReport,              
DaytoExit                      
From #TempPNL                      
--Where UnderlyingSymbol In ('A','B')                      
                         
Drop Table #T_CompanyFunds,#TempGenericPNL,#TempPNL,#TempOpenPosSymbols                              
END