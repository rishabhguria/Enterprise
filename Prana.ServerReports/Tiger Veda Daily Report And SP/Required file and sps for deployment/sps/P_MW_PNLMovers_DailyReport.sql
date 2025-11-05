/*************************************************                                                        
Author : Ankit Misra                                                       
Creation Date : 28th May , 2015                                                          
Description : Script for PNLMover part of Daily Report                                          
                                           
Execution Statement:                                                       
P_MW_PNLMovers_DailyReport @EndDate='7/2/2015',@Fund=N'1270,1271,1298,1302,1304,1305,1306,1307,1308,1309,1310',@PTHFund='1270'                                      
--*************************************************/                                      
CREATE Procedure [dbo].[P_MW_PNLMovers_DailyReport]                                      
(                                      
 @EndDate datetime,                                      
 @Fund Varchar(max),              
 @PTHFund Varchar(Max)                                      
)                                      
AS                  
--                
--Declare @EndDate datetime                                              
--Declare @Fund Varchar(2000)            
--Declare @PTHFund Varchar(2000)                                                 
--                                              
--Set @EndDate = '7/31/2015'                                                                      
--Set @Fund = '1286,1306,1288,1290,1307,1300,1301,1311,1308,1310,1309,1282,1279,1280,1281,1293,1294,1265,1305,1266,1304,1263,1264,1277,1302,1268,1269,1267,1303,1295,1296,1297,1292'                                 
--Set @PTHFund = '1400'                                      
BEGIN                                    
                                    
                                    
Declare @DefaultAUECID int                                      
Set @DefaultAUECID=(select top 1 DefaultAUECID  from T_Company where companyId <> -1)                                    
-------------------------------------------------------------------                                    
--                                    
-------------------------------------------------------------------                                    
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
            
-----------------------------------------------------------------------------------------------------------              
--REMOVING PTH FUNDS FROM FUND PARAMETER STRING FOR [F_MW_GetLinkedPortfolioACB_DailyReport] FUNCTION              
-----------------------------------------------------------------------------------------------------------              
DECLARE @FundsExcludingPTHFunds Varchar(max)              
SELECT  @FundsExcludingPTHFunds = ISNULL(@FundsExcludingPTHFunds,'')+ CONVERT(varchar(4), FundId)+',' FROM @T_FundIDs                
-----------------------------------------------------------------------------------------------------------                                                                                     
                                                                                      
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
Symbol AS OpenSymbol,        
(SUM(BeginningQuantity *UnitCostLocal)/NULLIF(Sum(BeginningQuantity),0)) AS WeightedAveragePrice        
Into #TempOpenPosSymbols                                                
From T_MW_GenericPNL                                                                                                                        
Inner Join #T_CompanyFunds ON #T_CompanyFunds.FundName = T_MW_GenericPNL.Fund         
Where Datediff(day , Rundate , @EndDate) = 0  And Open_CloseTag In ('O')         
Group By Symbol                                 
                
--Select * from #TempOpenPosSymbols      
-------------------------------------------------------------------                                    
--Selection of Required Fields From Generic Middleware Table                                    
-------------------------------------------------------------------                  
                
Select                  
Rundate,                  
Symbol,                  
UnderlyingSymbol,                  
UnderlyingSymbolPrice,        
CASE        
WHEN BeginningPriceLocal<>0                
THEN BeginningPriceLocal        
ELSE ISNULL(TOPS.WeightedAveragePrice,0)       
END AS BeginningPriceLocal,                  
EndingPriceLocal,      
CASE            
WHEN Asset NOT IN ('FX','FXForward')            
THEN DeltaExposureBase            
ELSE 0.0            
END AS DeltaExposureBase,                  
TotalRealizedPNLOnCost,                  
ChangeInUNRealizedPNL,                  
Dividend,             
SecurityName                  
Into #TempGenericPNL                
From T_MW_GenericPNL PNL                          
INNER JOIN #T_CompanyFunds ON #T_CompanyFunds.FundName = PNL.Fund      
Inner Join #TempOpenPosSymbols TOPS ON  TOPS.OpenSymbol = PNL.Symbol                   
Where Datediff(day , Rundate , @EndDate)= 0 and                                    
Open_CloseTag <> 'Accruals' and Asset Not In ('Cash','FX','FXForward')               
                             
-------------------------------------------------------------------                                
--Selection of Required Fields From Generic Middleware Table                                
-------------------------------------------------------------------                           
Select                      
UnderlyingSymbol,                
MAX(Symbol) As SymbolMax,                
MIN(Symbol) As SymbolMin,                         
Sum(ISNULL(DeltaExposureBase,0)) AS DeltaNetExposure,                                                                               
Sum(TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend) AS DayPnl,                
Max(BeginningPriceLocal) As BeginningPriceLocal,                
Max(EndingPriceLocal) As LocalPX                          
                  
InTo #TempPNL                         
From #TempGenericPNL                  
WHERE DATEDIFF(Day,Rundate,@EndDate) = 0                  
Group By UnderlyingSymbol                          
                
                  
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
Set                
SecurityName = TempGeneric.SecurityName                  
From #TempPNL Temp                  
Inner Join #TempGenericPNL TempGeneric On TempGeneric.UnderlyingSymbol = Temp.UnderlyingSymbol                  
                  
Update #TempPNL                  
Set PercentChangePrice = ISNULL(((LocalPX - BeginningPriceLocal)/NULLIF(BeginningPriceLocal,0))*100,0)                 
                
Alter Table #TempPNL                
Add PercentContribution Float                
                
Declare @DailyPortfolioACB Float                          
Set @DailyPortfolioACB = NULLIF(dbo.[F_MW_GetLinkedPortfolioACB_DailyReport](@EndDate,@EndDate,@FundsExcludingPTHFunds),0)                
                
Update #TempPNL                
Set PercentContribution = ISNULL((DayPnL/@DailyPortfolioACB),0)                
                  
Select                  
UnderlyingSymbol,                 
SymbolMax,                
SymbolMin,                 
Side,                  
LocalPX,                  
DeltaNetExposure As Exposure,                  
DayPnL,                  
PercentChangePrice As PercentageChange,                  
SecurityName,                
PercentContribution                 
From #TempPNL                 
--Where UnderlyingSymbol = 'AAPL'                
Order by UnderlyingSymbol                
                
                
Drop Table #TempPNL,#TempGenericPNL,#T_CompanyFunds,#TempOpenPosSymbols                                
                                    
End