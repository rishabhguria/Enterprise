                          
/*************************************************                                                                                                            
Author : Ankit Misra                                                                                                           
Creation Date : 28th May , 2015                                                                                                              
Description : Script for DashBoard PNL part of Daily Report                                                                                              
                           
Modified By: Sandeep Singh              
Date: 10 AUGUST 2015              
Desc: http://jira.nirvanasolutions.com:8080/browse/PRANA-10296              
Daily report: Instead of AUEC Asset, can UDA Asset be the other level of grouping              
                                                                                  
Modified By: Sandeep Singh                
Date: 26 AUGUST 2015                
Desc: http://jira.nirvanasolutions.com:8080/browse/PRANA-10559                
Daily report: FX Forwards Incorrect P&L Calculation 

Execution Statement:                                                                                                           
P_MW_AssetSideWisePNL_DailyReport @EndDate='7/31/2015',@Fund=N'1286,1306,1288,1290,1307,1300,1301,1311,1308,1310,1309,1282,1279,1280,1281,1293,1294,1265,1305,1266,1304,1263,1264,1277,1302,1268,1269,1267,1303,1295,1296,1297,1292',@IncludeHistoricalPNL = 1,
  
@PTHFund=N''                                                                                         
*************************************************/                                                                                          
CREATE Procedure [dbo].[P_MW_AssetSideWisePNL_DailyReport]                                                                                        
(                                                                                        
 @EndDate datetime,                                                                                        
 @Fund Varchar(max),                                        
 @IncludeHistoricalPNL Bit,                                        
 @PTHFund Varchar(Max)                                                                                        
)                                                                                        
AS                                                                                        
BEGIN                                                                                   
                                                                                
--Declare @EndDate datetime                                                                              
--Declare @Fund Varchar(2000)                                        
--Declare @IncludeHistoricalPNL Bit                                                                                  
--Declare @PTHFund Varchar(max)                                        
--                                                                             
--Set @EndDate = '7/31/2015'                                                                          
--Set @Fund = '1286,1288,1290,1306'                   
--Set @IncludeHistoricalPNL=1                                        
--Set @PTHFund = '1307'                                                                         
                                                                                        
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
--------------------------------------------------------------------------------                                             
---- PTH Funds Removal Section                            
--------------------------------------------------------------------------------                                          
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
----------------------------------------------------------------------------                                                                                        
--Adjusting Dates AUECwise PreviousBusinessDay, MonthStartDate,YearStartDate                                                                         
----------------------------------------------------------------------------                                                                          
                                                                                        
Declare                                                        
@PreviousBusinessDay DateTime,                                                                        
@MTDFromdate datetime,                                                                    
@YTDFromdate datetime                                                                                        
                                                            
Set @PreviousBusinessDay =  dbo.AdjustBusinessDays(@EndDate,-1, @DefaultAUECID)                                                                         
Select @MTDFromdate= dbo.F_MW_GetWeekendAdjusted (@EndDate,1,3)                                                                                      
Select @YTDFromdate= dbo.F_MW_GetWeekendAdjusted (@EndDate,1,7)                                                                                                                 
                                                                                   
IF (Datediff(Day,@EndDate,@MTDFromdate)=0 and Datediff(Day,@EndDate,@YTDFromdate)=0)                                                                                     
BEGIN                                                                  
Set @MTDFromdate=dbo.F_MW_GetWeekendAdjusted (dbo.AdjustBusinessDays(@EndDate,-1, @DefaultAUECID),1,3)                                                                                                                                    
Set @YTDFromdate=dbo.F_MW_GetWeekendAdjusted (dbo.AdjustBusinessDays(@EndDate,-1, @DefaultAUECID),1,7)                                                                                                                                    
END                                                                                     
 ----------------------------------------------------------------------------------      
--THIS PART IS HARD CODED BECAUSE TIGERVEDA USES HISTORICAL PNL BEFORE '7/31/2015'      
----------------------------------------------------------------------------------      
Declare @ITDDate Datetime      
Set @ITDDate='08/01/2015'                                                                                      
----------------------------------------------------------------------------------                                                                                    
--Select Symbols which are OPEN                                                                                  
-------------------------------------------------------------------                                                                                  
Select             
Symbol AS OpenSymbol,            
((Max(EndingPriceLocal)- (SUM(BeginningQuantity *UnitCostLocal)/NULLIF(Sum(BeginningQuantity),0)))/NULLIF((SUM(BeginningQuantity *UnitCostLocal)/Sum(BeginningQuantity)),0))*100 AS PercentageChange,            
Side AS OpenSymbolSide             
Into #TempOpenPosSymbols                                                    
From T_MW_GenericPNL      
Inner Join #T_CompanyFunds ON #T_CompanyFunds.FundName = T_MW_GenericPNL.Fund             
Where Datediff(day , Rundate , @EndDate) = 0  And Open_CloseTag In ('O') And Asset <> 'Cash'            
Group By Symbol,Side   
  
-------------------------------------------------------------------          
/* Grouping is required on he basis of UDA asset class. Client changes symbol UDA asset freguently and   
 as per Gennaro no one wants to re-run historical middle ware. This report show historical P&L as well so   
historical symbols will be picked. For same symbol, there may be multiple UDA asset class. So we are taking   
latest symbol wise UDA asset class and updating the historical as well.  
*/  
-------------------------------------------------------------------  
Select Distinct   
Symbol,UDAAssetClass   
InTo #TempSymbolUDAAssset  
From T_MW_GenericPNL      
Where Datediff(day , Rundate , @EndDate) = 0 And Open_CloseTag In ('O') And Asset Not In ('Cash','FX','FXForward')   
                                                                             
-------------------------------------------------------------------                                                                                        
--Selection of Required Fields From Generic Middleware Table                                                                                        
-------------------------------------------------------------------                                                                                        
SELECT                                                                                         
Max(                 
Case                                                                          
When (Asset = 'FX' Or Asset = 'FXForward') And SM.LeadCurrency <> 'USD'                                                                                
Then IsNull(SM.LeadCurrency,PNL.TradeCurrency)                                   
When (Asset = 'FX' Or Asset = 'FXForward') And SM.LeadCurrency = 'USD'                                                                                
Then IsNull(SM.VSCurrency,PNL.TradeCurrency)                                                                  
Else SecurityName                                                                          
End) As SecurityName,                                                                                
Symbol,                                                     
Max(PNL.BloombergSymbol) As BloombergSymbol,                                                                                
Max(PNL.UnderlyingSymbol) As UnderlyingSymbol,                                     
                                                 
--Sum(                                                                                  
-- Case                                             
-- When DateDiff(Day,@EndDate,Rundate) = 0 And Open_CloseTag = 'O'                                                   
-- Then BeginningQuantity * SideMultiplier                                                                                    
-- Else 0                                                                                    
-- End) As Position,                          
                          
Sum(                                              
Case                                                      
 When DateDiff(Day,@EndDate,Rundate) = 0 And Open_CloseTag = 'O' And Asset <> 'FX' And Asset <> 'FXForward'                                                                                 
 Then BeginningQuantity * SideMultiplier                                           
 When DateDiff(Day,@EndDate,Rundate) = 0 And Open_CloseTag = 'O' And (Asset = 'FX' Or Asset = 'FXForward')                                                                                
 Then                                   
  Case                                  
--		When PNL.BaseCurrency = SM.LeadCurrency                                      
--		Then (BeginningQuantity * SideMultiplier * EndingPriceLocal) * (-1)                                   
--		Else (BeginningQuantity * SideMultiplier)   
   When PNL.BaseCurrency = SM.LeadCurrency                                  
		Then (BeginningQuantity * SideMultiplier * UnitCostLocal) * (-1)                                   
   Else (BeginningQuantity * SideMultiplier)                                  
  End                                    
 Else 0.0                                                                                    
End) As Position,                                                      
                                                                                 
Max(SideMultiplier) As SideMultiplier,                                                                                
 Side,                                                                                      
Max(Asset) As Asset,                                                         
              
Max(                                
 Case                                                                        
 When (Asset = 'FX' Or Asset = 'FXForward')                                                                   
 Then 'FX Hedge'                                                                        
 Else UDAAssetClass +' '+ Side                   
 End ) as AssetSideCategory,                                                                                    
                                                                                 
Sum(                                                                                   
Case                                                      
 When DateDiff(Day,@EndDate,Rundate) = 0 And Open_CloseTag = 'O' And Asset <> 'FX' And Asset <> 'FXForward'                                                           
 Then ISNULL(DeltaExposureBase,0)                
 When DateDiff(Day,@EndDate,Rundate) = 0 And Open_CloseTag = 'O' And (Asset = 'FX' Or Asset = 'FXForward')                                                                                
 Then                                   
--	Case                                      
--		When PNL.BaseCurrency = SM.LeadCurrency                                      
--		Then (((BeginningQuantity * SideMultiplier * EndingPriceLocal) * (-1)) * EndingFXRate)                                      
--		Else ((BeginningQuantity * SideMultiplier) * EndingPriceLocal)                                    
--	End      
  Case                                  
   When PNL.BaseCurrency = SM.LeadCurrency                                  
		Then 
			Case 
				When EndingPriceLocal <> 0
				Then (((BeginningQuantity * SideMultiplier * UnitCostLocal) * (-1)) / EndingPriceLocal)
				Else 0
			End                                      
   Else ((BeginningQuantity * SideMultiplier) * EndingPriceLocal)                                
  End                                    
 Else 0.0                                                                                    
End                          
) As ExposureValue,                                                                                    
                                                                                     
Max(                                                                                     
 Case                         
 When DateDiff(Day,@EndDate,Rundate) = 0  And Open_CloseTag = 'O'                                                                                     
 Then EndingPriceLocal                                                                                      
 Else 0.0                                                                                      
 End) As LocalPX,                                                                                 
                                                                                   
Max(                                                                                  
 Case                                                                                                     
  When DateDiff(Day,@EndDate,Rundate) = 0  And Open_CloseTag = 'O'                
  Then                 
   Case                
    When BaseCurrency = TradeCurrency                                                                                     
    Then EndingPriceBase                 
    When BaseCurrency <> TradeCurrency And EndingPriceLocal <> 0 And TradeCurrency <> 'IDR'                                                                                    
    Then 1/EndingPriceLocal               
  When BaseCurrency <> TradeCurrency And EndingPriceLocal <> 0 And TradeCurrency = 'IDR'                                                                                  
    Then (1/EndingPriceLocal) * 1000                
   Else 0.0                
   End                                                                                    
 Else 0.0                                                                                      
 End                
) As BasePX,                                                                                   
                                                                                  
--Max(                                                                                     
-- Case                                                                                                     
-- When DateDiff(Day,@EndDate,Rundate) = 0  And Open_CloseTag = 'O' AND  BeginningPriceLocal<>0                                   
-- Then ISNULL(((EndingPriceLocal - BeginningPriceLocal)/NULLIF(BeginningPriceLocal,0))*100,0)                     
-- When DateDiff(Day,@EndDate,Rundate) = 0  And Open_CloseTag = 'O' AND  BeginningPriceLocal=0                                                                                   
-- Then ISNULL(((EndingPriceLocal - UnitCostLocal)/NULLIF(UnitCostLocal,0))*100,0)                                                                   
---- Else 0.0                                                                         
-- End) as PercentageChange,            
            
Max(                                                                                 
 Case                                                                                                 
 When DateDiff(Day,@EndDate,Rundate) = 0  And Open_CloseTag = 'O' AND  BeginningPriceLocal<>0                                                                               
 Then ISNULL(((EndingPriceLocal - BeginningPriceLocal)/NULLIF(BeginningPriceLocal,0))*100,0)                 
 When DateDiff(Day,@EndDate,Rundate) = 0  And Open_CloseTag = 'O' AND  BeginningPriceLocal=0                                                                               
 Then   ISNULL(TOPS.PercentageChange,0)            
-- Else 0.0                                                                     
 End) as PercentageChange,                                                                                    
                                                                     
Sum(                                                                                    
 Case                                                                                                     
 When DateDiff(Day,@EndDate,Rundate) = 0 -- And Open_CloseTag = 'O'                                                        
 Then TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend                                                                       
 Else 0.0                                                                                                  
 End) As TodayPNL,                                                                                                                          
                                                                                  
Sum(                                                                                     
 Case                                                         
 When Datediff (day ,@MTDFromdate , Rundate) >= 0 And Datediff(day ,Rundate ,@EndDate) >= 0 AND Datediff(day ,@ITDDate ,Rundate) >= 0                                                                                                                        
                       
 Then TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend                                                                                                                           
 Else 0.0                          
 End) As  MTDPNL,                                                                                    
                     
 Sum(      
Case      
WHEN Datediff(day ,@ITDDate ,Rundate) >= 0      
THEN (TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend)       
ELSE 0.0      
END) As YTDPNL                                                                                
                                                   
Into #TempBodyPNL                                                                                
                                                                                        
From T_MW_GenericPNL PNL                                                                                                                      
Inner Join #T_CompanyFunds ON #T_CompanyFunds.FundName = PNL.Fund                                                                                  
Inner Join #TempOpenPosSymbols TOPS ON  TOPS.OpenSymbol = PNL.Symbol AND TOPS.OpenSymbolSide =PNL.Side                                                             
Left Outer Join V_SecMasterData SM On SM.TickerSymbol = PNL.Symbol                                                            
Where                                                                                                                                                                 
Datediff (day , @YTDFromdate , Rundate) >= 0 And                                                                                    
Datediff(day , Rundate , @EndDate) >= 0 And                                                                                         
Open_CloseTag <> 'Accruals'                                                                              
Group By Symbol,Side                           
                                
  
---- Update AssetSideCategory as stated above where #TempSymbolUDAAssset table is created  
Update BodyPNL  
Set BodyPNL.AssetSideCategory = Temp.UDAAssetClass +' '+ BodyPNL.Side   
From #TempBodyPNL BodyPNL  
Inner Join #TempSymbolUDAAssset Temp On Temp.Symbol = BodyPNL.Symbol           
                        
------- Update FX and FX Forward                                
SELECT                                                                                         
SecurityName,                                                                                
Max(Symbol) As Symbol,                                                                                
Max(BloombergSymbol) As BloombergSymbol,                  
Max(UnderlyingSymbol) As UnderlyingSymbol,                                                                                  
Sum(Position) As Position,                                                     
Max(SideMultiplier) As SideMultiplier,                                                                                
Max(Side) As Side,                                                                                      
Max(Asset) As Asset,                                                           
AssetSideCategory,                                                                                    
Sum(ExposureValue) As ExposureValue,                                                                                    
Max(LocalPX) As LocalPX,                                                                                     
Max(BasePX) As BasePX,                                                                                    
Max(PercentageChange) as PercentageChange,                                                                                    
Sum(TodayPNL) As TodayPNL,                                                                                                                          
Sum(MTDPNL) As  MTDPNL,                                                                                    
Sum(YTDPNL) As YTDPNL                                
                                
Into #TempFXHedge   
From #TempBodyPNL                                
Where AssetSideCategory = 'FX Hedge'                                
Group by AssetSideCategory ,SecurityName                                 
                                
                                
Delete #TempBodyPNL                                
From #TempBodyPNL                                
Where AssetSideCategory = 'FX Hedge'                                
                                
Insert Into #TempBodyPNL                                
Select * From #TempFXHedge                               
                              
--------------------------------------                              
--Add MTD and YTD Performance Columns                               
--------------------------------------                              
                              
Alter Table #TempBodyPNL                                                                            
Add MTDPercent Float                                                                 
              
Alter Table #TempBodyPNL                                                                     
Add YTDPercent Float                              
                              
Update #TempBodyPNL                              
Set MTDPercent =0.0,                         
 YTDPercent =0.0                              
                              
--Select * from  #TempBodyPNL                                 
 ----------------------------------------------------------------------------------      
--THIS PART IS HARD CODED BECAUSE TIGERVEDA USES HISTORICAL PNL BEFORE '7/31/2015'      
----------------------------------------------------------------------------------      
IF(DATEDIFF(Day,@YTDFromdate,@ITDDate)>0)       
BEGIN      
SET @YTDFromdate= @ITDDate      
END      
IF(DATEDIFF(Day,@MTDFromdate,@ITDDate)>0)       
BEGIN      
SET @MTDFromdate= @ITDDate      
END                         
---------------------------------------------------------------------------------------------------------------------------                              
--This Part Includes Historical PNL which is not detailed and only used for calculating YTD PNL of JAN,FEB,MAR,APR of 2015                                              
---------------------------------------------------------------------------------------------------------------------------                                            
                                              
IF(@IncludeHistoricalPNL=1 AND DATEDIFF(Day,'2015/01/01',@EndDate)>=0 AND DATEDIFF(Day,@EndDate,'2015/12/31')>=0)                                              
BEGIN                              
                                             
SELECT                              
MAX(InvestmentDescription) AS SecurityName,                              
InvestmentCode AS Symbol,                                               
InvestmentCode AS BloombergSymbol,                              
'-' AS UnderlyingSymbol,                              
0 AS Position,                              
0 AS SideMultiplier,                              
MAX(Direction) AS Side,                              
MAX(Asset) AS Asset,                                              
AssetSideCategory,                              
0 As ExposureValue,                                                                                    
0 As LocalPX,                                             
0 As BasePX,                               
0 as PercentageChange,                              
0 AS TodayPNL,                                    
                                    
CASE                              
WHEN DATEDIFF(Day,'2015/07/31',@EndDate)=0                                    
THEN ISNULL(SUM(JulPNL),0)                                    
WHEN DATEDIFF(Day,'2015/06/30',@EndDate)=0                                    
THEN ISNULL(SUM(JunPNL),0)                                    
WHEN DATEDIFF(Day,'2015/05/31',@EndDate)=0                                    
THEN ISNULL(SUM(MayPNL),0)                                    
WHEN DATEDIFF(Day,'2015/04/30',@EndDate)=0                                    
THEN ISNULL(SUM(AprPNL),0)                                    
WHEN DATEDIFF(Day,'2015/03/31',@EndDate)=0                                    
THEN ISNULL(SUM(MarPNL),0)                                    
WHEN DATEDIFF(Day,'2015/02/28',@EndDate)=0                                    
THEN ISNULL(SUM(FebPNL),0)                                    
WHEN DATEDIFF(Day,'2015/01/31',@EndDate)=0                                    
THEN ISNULL(SUM(JanPNL),0)                                    
ELSE 0.0                                    
END AS MTDPNL,                                    
                                    
CASE                              
WHEN DATEDIFF(Day,'2015/07/31',@EndDate)>=0                                    
THEN ISNULL(SUM(JanPNL+FebPNL+MarPNL+AprPNL+MayPNL+JunPNL+JulPNL),0)                                     
WHEN DATEDIFF(Day,'2015/06/30',@EndDate)=0                                    
THEN ISNULL(SUM(JanPNL+FebPNL+MarPNL+AprPNL+MayPNL+JunPNL),0)                                    
WHEN DATEDIFF(Day,'2015/05/31',@EndDate)=0                                    
THEN ISNULL(SUM(JanPNL+FebPNL+MarPNL+AprPNL+MayPNL),0)                                    
WHEN DATEDIFF(Day,'2015/04/30',@EndDate)=0                                    
THEN ISNULL(SUM(JanPNL+FebPNL+MarPNL+AprPNL),0)                                    
WHEN DATEDIFF(Day,'2015/03/31',@EndDate)=0                                    
THEN ISNULL(SUM(JanPNL+FebPNL+MarPNL),0)                                    
WHEN DATEDIFF(Day,'2015/02/28',@EndDate)=0                             
THEN ISNULL(SUM(JanPNL+FebPNL),0)                                    
WHEN DATEDIFF(Day,'2015/01/31',@EndDate)=0                                    
THEN ISNULL(SUM(JanPNL),0)                                    
ELSE 0.0                                    
END AS YTDPNL,                              
                              
CASE                              
WHEN DATEDIFF(Day,'2015/07/31',@EndDate)=0                                    
THEN log(1 + SUM(JulPNLPercent/100))      
WHEN DATEDIFF(Day,'2015/06/30',@EndDate)=0                                    
THEN log(1 + SUM(JunPNLPercent/100))                                     
WHEN DATEDIFF(Day,'2015/05/31',@EndDate)=0                        
THEN log(1 + SUM(MayPNLPercent/100))                                      
WHEN DATEDIFF(Day,'2015/04/30',@EndDate)=0                                    
THEN log(1 + SUM(AprPNLPercent/100))                                     
WHEN DATEDIFF(Day,'2015/03/31',@EndDate)=0                                    
THEN log(1 + SUM(MarPNLPercent/100))                                      
WHEN DATEDIFF(Day,'2015/02/28',@EndDate)=0                                    
THEN log(1 + SUM(FebPNLPercent/100))                                    
WHEN DATEDIFF(Day,'2015/01/31',@EndDate)=0                                    
THEN log(1 + SUM(JanPNLPercent/100))                                
ELSE 0.0                                    
END AS MTDPercent,                              
                              
CASE                              
WHEN DATEDIFF(Day,'2015/07/31',@EndDate)>=0                                    
THEN (log(1 + SUM(JulPNLPercent/100))+log(1 + SUM(JunPNLPercent/100))+log(1 + SUM(MayPNLPercent/100))+log(1 + SUM(AprPNLPercent/100))+ log(1 + SUM(MarPNLPercent/100))+ log(1 + SUM(FebPNLPercent/100))+log(1 + SUM(JanPNLPercent/100)))      
WHEN DATEDIFF(Day,'2015/06/30',@EndDate)=0                 
THEN (log(1 + SUM(JunPNLPercent/100))+log(1 + SUM(MayPNLPercent/100))+log(1 + SUM(AprPNLPercent/100))+ log(1 + SUM(MarPNLPercent/100))+ log(1 + SUM(FebPNLPercent/100))+log(1 + SUM(JanPNLPercent/100)))      
WHEN DATEDIFF(Day,'2015/05/31',@EndDate)=0                                    
THEN (log(1 + SUM(MayPNLPercent/100))+log(1 + SUM(AprPNLPercent/100))+ log(1 + SUM(MarPNLPercent/100))+ log(1 + SUM(FebPNLPercent/100))+log(1 + SUM(JanPNLPercent/100)))      
WHEN DATEDIFF(Day,'2015/04/30',@EndDate)=0                                    
THEN (log(1 + SUM(AprPNLPercent/100))+ log(1 + SUM(MarPNLPercent/100))+ log(1 + SUM(FebPNLPercent/100))+log(1 + SUM(JanPNLPercent/100)))                                     
WHEN DATEDIFF(Day,'2015/03/31',@EndDate)=0                                    
THEN (log(1 + SUM(MarPNLPercent/100))+ log(1 + SUM(FebPNLPercent/100))+log(1 + SUM(JanPNLPercent/100)))      
WHEN DATEDIFF(Day,'2015/02/28',@EndDate)=0                                    
THEN (log(1 + SUM(FebPNLPercent/100))+log(1 + SUM(JanPNLPercent/100)))                                    
WHEN DATEDIFF(Day,'2015/01/31',@EndDate)=0                                    
THEN (log(1 + SUM(FebPNLPercent/100)))                                
ELSE 0.0                                    
END AS YTDPercent                              
                              
INTO #TempTigerVedaHistoricalPNL                                                
FROM T_TigerVedaMonthWiseHistoricalPNL TVPS                                            
Inner Join #T_CompanyFunds ON #T_CompanyFunds.FundName = TVPS.Account                                               
WHERE Asset<>'Cash' AND AssetSideCategory IS NOT NULL                                              
GROUP BY                                              
AssetSideCategory,InvestmentCode                              
                              
Update #TempTigerVedaHistoricalPNL    
Set     
#TempTigerVedaHistoricalPNL.AssetSideCategory= BodyPNL.AssetSideCategory    
From #TempTigerVedaHistoricalPNL    
Inner Join #TempBodyPNL BodyPNL ON (BodyPNL.Symbol=#TempTigerVedaHistoricalPNL.Symbol)                                  
                              
--SELECT * FROM #TempTigerVedaHistoricalPNL                          
--SELECT * FROM #TempBodyPNL                              
                              
Update #TempBodyPNL                              
Set                              
#TempBodyPNL.MTDPNL=ISNULL(#TempBodyPNL.MTDPNL,0)+ISNULL(TVHP.MTDPNL,0),   --To Be Confirmed                               
#TempBodyPNL.YTDPNL=ISNULL(#TempBodyPNL.YTDPNL,0)+ISNULL(TVHP.YTDPNL,0),                              
#TempBodyPNL.MTDPercent =ISNULL(#TempBodyPNL.MTDPercent,0)+ISNULL(TVHP.MTDPercent,0), --To Be Confirmed                               
#TempBodyPNL.YTDPercent =ISNULL(#TempBodyPNL.YTDPercent,0)+ISNULL(TVHP.YTDPercent,0)                  
FROM                               
#TempBodyPNL                              
INNER JOIN #TempTigerVedaHistoricalPNL TVHP ON (#TempBodyPNL.Symbol=TVHP.Symbol )--AND #TempBodyPNL.AssetSideCategory = TVHP.AssetSideCategory)                 
            
--Select * from  #TempBodyPNL                          
                              
                              
IF(                              
--DATEDIFF(Day,'2015/08/31',@EndDate)=0 OR             
DATEDIFF(Day,'2015/07/31',@EndDate)=0 OR DATEDIFF(Day,'2015/06/30',@EndDate)=0 OR                              
DATEDIFF(Day,'2015/05/31',@EndDate)=0 OR DATEDIFF(Day,'2015/04/30',@EndDate)=0 OR DATEDIFF(Day,'2015/03/31',@EndDate)=0 OR                              
DATEDIFF(Day,'2015/02/28',@EndDate)=0 OR DATEDIFF(Day,'2015/01/31',@EndDate)=0                               
 )                              
 BEGIN                              
  Insert Into #TempBodyPNL                              
  (SecurityName,                              
  Symbol,                              
  BloombergSymbol,                              
  UnderlyingSymbol,                              
  Position,                              
  SideMultiplier,                              
  Side,                              
  Asset,                              
  AssetSideCategory,                              
  ExposureValue,                              
  LocalPX,                              
  BasePX,                              
  PercentageChange,                              
  TodayPNL,                           
  MTDPNL,                              
  YTDPNL,                              
  MTDPercent,                              
  YTDPercent)                              
  Select                              
  TVHP.SecurityName,                              
  TVHP.Symbol,                              
  TVHP.BloombergSymbol,                              
  TVHP.UnderlyingSymbol,                              
  TVHP.Position,                              
  TVHP.SideMultiplier,                              
  TVHP.Side,                              
  TVHP.Asset,                              
  TVHP.AssetSideCategory,                              
  TVHP.ExposureValue,                              
  TVHP.LocalPX,                
  TVHP.BasePX,                              
  TVHP.PercentageChange,                              
  TVHP.TodayPNL,                              
  TVHP.MTDPNL,                              
  TVHP.YTDPNL,                              
  TVHP.MTDPercent,                              
  TVHP.YTDPercent                               
  From #TempTigerVedaHistoricalPNL TVHP                              
  Left Outer Join #TempBodyPNL ON (#TempBodyPNL.Symbol=TVHP.Symbol AND #TempBodyPNL.AssetSideCategory = TVHP.AssetSideCategory)                              
  Where #TempBodyPNL.BloombergSymbol IS NULL                              
 END            
Drop Table #TempTigerVedaHistoricalPNL                                     
END             
            
                               
                              
--Select * from  #TempBodyPNL                                           
-----------------------------------------------------------------------------------------------------------------------------                                                                              
                                                            
--Select * from #TempBodyPNL                                                                                
--Where Symbol ='HFC'                                                                                
/*                       
#Temp_Boxed table is used to keep the boxed symbol with position and side                                                                                
1) Collect Symbol                                                                                
2) Check symbol(s) exists more that one i.e. with Long and Short                                                             
3) If yes then keep else remove from _Boxed table because if any symbol has only long or short that will not come under boxed category                                                                                
*/                                                                                
Select                                          
Asset,         
Symbol,                                                                                
Position,                                                                                
Side                                                                      
Into #Temp_Boxed                                                                                
From #TempBodyPNL                                                                                
                                                          
---- #Temp1 is used to delete symbols which do not come under boxed positions category                                                                                
Select                                                                                
Symbol                                                                                
Into #Temp1            
From #Temp_Boxed                                                                                
Group by Symbol                                                                                
Having Count(Symbol) = 1                                                                               
                                                        
Delete #Temp_Boxed                                              
Where Symbol In (Select Symbol From #Temp1)                                                                              
                                                        
--Select * from #Temp_Boxed                                                                                
--Where Symbol ='HFC'                                                                                
                                                                                
/*                                                                       
Now we have boxed symbols list in #Temp_Boxed table                                                                                
1) Add a new column say Category and set its value to Boxed                                                                               
2) How much positions will be boxed i.e. if 100 Long and 80 Short, then 80 will be boxed and 20 will be short                                                       
   this we do in the below query and keep data in #Temp_FinalBoxedPositions table                                                                                
*/                                                     
                             
Select                                   
#TempBodyPNL.Asset,                                                                                
#TempBodyPNL.Symbol,                                                                                
Sum(#TempBodyPNL.Position) As Qty,                                                                                
Sum(#TempBodyPNL.ExposureValue) As ExposureValue,                                                                                
Sum(#TempBodyPNL.TodayPNL) As TodayPNL,                                                                                
Sum(#TempBodyPNL.MTDPNL) As MTDPNL,                                                                                
Sum(#TempBodyPNL.YTDPNL) As YTDPNL,                     
'Boxed' As Category                                                                                
Into #Temp_FinalBoxedPositions                                                 
From #TempBodyPNL                                                                                
Where Symbol In (Select Symbol From #Temp_Boxed)                  
Group By #TempBodyPNL.Asset,#TempBodyPNL.Symbol                                                                                
                                                                                
/*                                                                                
  As we have boxed symbol and qty, add side column in the temp table on the basis of qty                                                                                
  If qty >= 0 then long else short                                                                                
*/                                
                                                                                
Alter Table #Temp_FinalBoxedPositions                                                                                
Add Side Varchar(10)                             
                                                                                
---- Update side in the temp table                                                                                
Update #Temp_FinalBoxedPositions                                                                                
Set Side  =                                                                                 
Case                                                                                
 When Qty >= 0                                                                                
 Then 'Long'                                                                                
 Else 'Short'                                                       
End                                                                                
                                                                                
----Select * From #Temp_FinalBoxedPositions                                                                                
----Where Symbol ='HFC'                                                                                
/*                                                                                
 We alter main temp table and add new column IsBoxed and set its value true                                                          
 i.e. assume all positions are boxed initially                                                                             
 Usage of IsBoxed: is used to identify the remaining qty after calculating the boxed postion.                                                                                
 Example: Symbol: HFC, Buy 1000 and Short 250, then IsBoxed will be false for the balance positions say 750                                                                                
 Also based on IsBoxed field, we update AssetSideCategory for boxed positions                                                                                 
*/                                                                                
                                                   
----Select * from #TempBodyPNL                                                                                
----Where Symbol ='HFC'                                                                                
                                                                                
Alter Table #TempBodyPNL                                                                                
Add IsBoxed Bit                                                                                
                                                                                
----Initially set IsBoxed = 1 for all                                                                                 
Update #TempBodyPNL                                  
Set IsBoxed = 1                                                                                
                                                   
---- Now set IsBoxed = 0 which are boxed (#Temp_FinalBoxedPositions has boxed position symbols)                                                                             
Update #TempBodyPNL                                                                                
Set                               
 Position = TempFinal.Qty,                                                                                 
 TodayPNL = TempFinal.TodayPNL,                                          
 MTDPNL = TempFinal.MTDPNL,                                                              
 YTDPNL = TempFinal.YTDPNL,                                                                               
 ExposureValue = TempFinal.ExposureValue,                                                      
 IsBoxed = 0                                                                                
From #TempBodyPNL                  
Inner Join #Temp_FinalBoxedPositions TempFinal On TempFinal.Symbol = #TempBodyPNL.Symbol And TempFinal.Side = #TempBodyPNL.Side                                                                                
                                                      
--Select * from #TempBodyPNL                                                                                
--Where Symbol ='HFC'                                   
--Select                                                                                
--Symbol                                                                                
--Into #Temp2                                                         
--From #TempBodyPNL                                                                                
--Group by Symbol           
--Having Count(Symbol) > 1                                                                    
                                                                                
--Select *                             
--Into #Temp3                                                                                
--from #TempBodyPNL                                                                                
--Where Symbol In (Select Distinct Symbol From #Temp_Boxed)                                                  
--And IsBoxed = 1                                                                                
                                                                                
---- AssetSideCategory updated for boxed symbols and also considered IsBoxed field with value = 1                                                    
Update #TempBodyPNL                                                                                
Set AssetSideCategory = 'Boxed'                             
Where Symbol In (Select Distinct Symbol From #Temp_Boxed)                                                                                
And IsBoxed = 1                                                           
                                                                                
/*                                                                                
Insert a duplicate entry in the main table for boxed positions                                                                                
Example as above                                                                                
Symbol: HFC, Long 1000 and Short 250, 250 Long and 250 Short will be under boxed category and                                                                                 
remaining balanced 750 will be long                                                                                
So we insert here 250 short                                                                                
*/                                                                                
             
Insert into #TempBodyPNL                                                                                
Select                                                                                
SecurityName,                                                                                
Symbol,                                                                                
BloombergSymbol,                                                                                
UnderlyingSymbol,                                                                                
(Position*-1),                                                                                
(SideMultiplier*-1),                                                                                
Case                                                             
 When Side='Short'                                                                                
 Then 'Long'                                   
 Else 'Short'                                                                                
End As Side,                                                                                
Asset,                                                                                
AssetSideCategory,                                                                                
--MonthsStartingMarketValue,                                             
(ExposureValue * -1),                                                    
--(ExposureValuePercentage * - 1),                                                                                
LocalPX,                                                                                
BasePX,                                                              
PercentageChange,                                                                                
0 As TodayPNL,--(TodayPNL*-1),                                                 
0 As MTDPNL,--(MTDPNL*-1),                          
0 As YTDPNL,                              
0 AS MTDPercent,                              
0 AS YTDPercent, --(YTDPNL*-1),                                                                                
IsBoxed                                                                                
From                                                                                
#TempBodyPNL Where AssetSideCategory = 'Boxed'                                     
                                                                                
Update #TempBodyPNL                                                            
Set TodayPNL = 0,                                                                                
MTDPNL = 0,                                                                                
YTDPNL = 0                                                                                
Where AssetSideCategory = 'Boxed'                                                        
                                                      
---- Long Short will be based on Exposure                                                      
Update #TempBodyPNL                                                      
Set                                                       
AssetSideCategory =                                                       
Case                                                      
 When (Side = 'Long' And ExposureValue < 0)                                                      
 Then Replace(AssetSideCategory,'Long','Short')                                    
 When (Side = 'Short' And ExposureValue > 0)                                                      
 Then Replace(AssetSideCategory,'Short','Long')                                                      
 Else AssetSideCategory                                                      
End                                                      
Where AssetSideCategory <> 'Boxed'                                                       
                                                      
                   
                                    
                                                                  
--------------------------------------------------------------------------------------------------------------                                                                    
--Calculation of MTD Contribution Percentage And YTD Contribution Percentage                                                           
--------------------------------------------------------------------------------------------------------------                                
                                                                  
Update #TempBodyPNL                                                                    
Set             
MTDPercent = dbo.[F_MW_GetLinkedPerformance_DailyReport](@MTDFromdate,@EndDate,@FundsExcludingPTHFunds,#TempBodyPNL.BloombergSymbol,ISNULL(#TempBodyPNL.MTDPercent,0)),                                                                    
YTDPercent = dbo.[F_MW_GetLinkedPerformance_DailyReport](@YTDFromdate,@EndDate,@FundsExcludingPTHFunds,#TempBodyPNL.BloombergSymbol,ISNULL(#TempBodyPNL.YTDPercent,0))                                                                    
--                                                                    
--------------------------------------------------------------------------------------------------------------                                                                    
--Adding Customized Sorting Asset and Side Wise                                                                    
--------------------------------------------------------------------------------------------------------------                                                                   
                                                                                
Alter Table #TempBodyPNL                                                                            
Add CustomizedSorting Int Not Null DEFAULT ((0))                                                                                                                                                 
Update #TempBodyPNL                          
Set CustomizedSorting =                                                                             
Case                                                                            
When AssetSideCategory = 'Equity Long'                                                                        
Then 1                                                                            
When AssetSideCategory = 'Equity Short'                                                                            
Then 2                                                                            
When AssetSideCategory = 'EquityOption Long'                                                                   
Then 3                                                                            
When AssetSideCategory = 'EquityOption Short'                                                                            
Then 4                                                                            
When AssetSideCategory = 'Future Long'                                                                            
Then 5                                                                   
When AssetSideCategory = 'Future Short'                                                                            
Then 6                              
When AssetSideCategory = 'FutureOption Long'                                                                            
Then 7                                                                            
When AssetSideCategory = 'FutureOption Short'                                                                            
Then 8                                                                            
--When AssetSideCategory = 'FX Hedge Long'                                                                            
--Then 9                                                                            
--When AssetSideCategory = 'FX Hedge Short'                                               
--Then 10                                    
When AssetSideCategory = 'FX Hedge'                                                                            
Then 9                                                                            
When AssetSideCategory = 'FixedIncome Long'                                                                 
Then 10                                                                            
When AssetSideCategory = 'FixedIncome Short'                                                                            
Then 11                                                                            
When AssetSideCategory = 'PrivateEquity Long'                                                
Then 12                                 
When AssetSideCategory = 'PrivateEquity Short'                                                                            
Then 13                                                                            
When AssetSideCategory = 'Boxed'                                                        
Then 14                                                                            
Else 15                                      
End                                                                            
                                                                            
Select * from #TempBodyPNL                                                                                
Where Position <> 0                                                                              
Order by CustomizedSorting,SecurityName                                                                               
--Order by CustomizedSorting,Side,SecurityName               
                                                                                
Drop Table #T_CompanyFunds ,#TempOpenPosSymbols,#TempBodyPNL,#Temp_Boxed,#Temp_FinalBoxedPositions,#Temp1,#TempFXHedge,#TempSymbolUDAAssset                                                                                
                                                                        
END