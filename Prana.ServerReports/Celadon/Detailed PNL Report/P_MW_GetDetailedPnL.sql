/*************************************************                                                          
Author : Ankit Misra                                                          
Creation Date : 17th April , 2015                                                            
Description : Brings the Data for DetailedPnL Report which includes DailyPNL,MTD PNL,YTD PNL with a capability of removing closed position of previous month                                          
Execution Statement:                                                         
exec P_MW_GetDetailedPnL_Test2 @EndDate='2015-09-02 00:00:00:000',@Fund=N'1245,1213,1214,1238,1239,1240,1241,1242,1244,1243,1246',@Asset=N'6,1,2',@ShowClosing=1,@ReportId=N'MTM_V3',@SearchString=N'',@SearchBy=N'Symbol'          
*************************************************/     
ALTER Procedure [dbo].[P_MW_GetDetailedPnL_Test]                                                      
(                                                      
@EndDate DateTime,                                                      
@Fund Varchar(max),                                                      
@Asset varchar(max),                                                      
@ShowClosing Bit,                                          
@ReportId Varchar(100),                                                
@SearchString Varchar(5000) ,                                                                      
@SearchBy Varchar(100)                                                      
)    
AS                                   
--Declare @EndDate DateTime                                                      
--Declare @Fund Varchar(max)                                                      
--Declare @Asset varchar(max)                                                      
--Declare @ShowClosing Bit                                          
--Declare @ReportId Varchar(100)                                                
--Declare @SearchString Varchar(5000)                                                                     
--Declare @SearchBy Varchar(100)                                     
--                                    
--Set @EndDate = '04-15-2015'                                    
--Set @Fund = '1213'                                    
--Set @Asset = '1,2'                                    
--Set @ShowClosing = 1                                    
--Set @ReportId = 'MTM_V3'                                    
--Set @SearchString =''                                    
--Set @SearchBy  = 'Symbol'    
BEGIN                                                      
SET NOCOUNT ON;    
    
Select * InTo #TempSymbol                       
From dbo.split(@SearchString , ',')    
    
Select Distinct * InTo #Symbol                       
From #TempSymbol    
    
Declare @T_FundIDs Table    
(    
 FundId int    
)    
Insert Into @T_FundIDs Select * From dbo.Split(@Fund, ',')    
    
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
    
Declare @T_AssetIDs Table                                                          
(                                 
 AssetId int                                                                                                                  
)                                                                               
Insert Into @T_AssetIDs Select * From dbo.Split(@Asset, ',')                                           
                                                      
CREATE TABLE #T_Assets                                                   
(                                                       
 AssetID int,              
 AssetName varchar(50)                                      
)                                                      
Insert Into #T_Assets                                                      
Select                                                                                               
T_Asset.AssetID,                                                        
T_Asset.AssetName                                                        
From T_Asset INNER JOIN @T_AssetIDs AssetIDs ON T_Asset.AssetID = AssetIDs.AssetID    
    
Declare                                                       
@PreviousBusinessDay DateTime,                                                      
@MTDFromdate datetime,                                                      
@QTDFromdate datetime,                                                      
@YTDFromdate datetime                                                      
                                                      
Declare @DefaultAUECID int                                                                                                              
Set @DefaultAUECID=(select top 1 DefaultAUECID  from T_Company where companyId <> -1)                                                      
                                                      
Set @PreviousBusinessDay =  dbo.AdjustBusinessDays(@EndDate,-1, @DefaultAUECID)                                                      
Select @MTDFromdate= dbo.F_MW_GetWeekendAdjusted (@EndDate,1,3)                                                               
Select @YTDFromdate= dbo.F_MW_GetWeekendAdjusted (@EndDate,1,7)                                                
                                                
IF (Datediff(Day,@EndDate,@MTDFromdate)=0 and Datediff(Day,@EndDate,@YTDFromdate)=0)                                                
BEGIN                                               
Set @MTDFromdate=dbo.F_MW_GetWeekendAdjusted (dbo.AdjustBusinessDays(@EndDate,-1, @DefaultAUECID),1,3)                                                
Set @YTDFromdate=dbo.F_MW_GetWeekendAdjusted (dbo.AdjustBusinessDays(@EndDate,-1, @DefaultAUECID),1,7)                                                
END    
    
Select     
Distinct PNL.Symbol                  
Into #TempOpenPosSymbols                  
From T_MW_GenericPNL PNL                                                 
Inner Join #T_CompanyFunds ON #T_CompanyFunds.FundName = PNL.Fund                                                    
Inner Join #T_Assets TA ON TA.AssetName = PNL.Asset                                                        
Where Datediff(day , Rundate , @EndDate) = 0  And Open_CloseTag In ('O')

Select Distinct PNL.Symbol     
Into #TempClosedPosSymbols              
From T_MW_GenericPNL PNL                                             
Inner Join #T_CompanyFunds ON #T_CompanyFunds.FundName = PNL.Fund            
Inner Join #T_Assets TA ON TA.AssetName = PNL.Asset                                                               
Where                                                                       
Datediff (day ,@MTDFromdate, Rundate) >= 0 and                                                               
Datediff(day , Rundate , @EndDate) >= 0  And Open_CloseTag In ('C','D')    

Select Symbol              
Into #TempAllSymbol              
From #TempOpenPosSymbols              
Insert Into #TempAllSymbol              
Select Symbol From #TempClosedPosSymbols       
    
Create Table #TempMTMCustomized                    
(                    
--Rundate Datetime,                                              
--TradeDate Datetime,                                                      
Symbol Varchar(200),                                                      
CUSIPSymbol Varchar(200),                                                                        
ISINSymbol Varchar(200),                                                                       
SEDOLSymbol Varchar(200),                                                                        
BloombergSymbol Varchar(200),                                                                        
ReutersSymbol Varchar(200),                                                                        
IDCOSymbol Varchar(200),                                                                        
OSISymbol Varchar(200),                             
UnderlyingSymbol Varchar(200),                                                         
--Fund Varchar(200),                                                      
--AssetID Int,                                                                      
Asset Varchar(200),   
TradeCurrency Varchar(20),                                                                       
Side Varchar(20),                                                          
SecurityName Varchar(200),                                                                       
--MasterFund Varchar(200),                                                                      
Strategy Varchar(200),                                                                      
UDASector Varchar(200),                                                              
UDACountry Varchar(200),                                                                      
UDASecurityType Varchar(200),                                                                  
UDAAssetClass Varchar(200),                                                                      
UDASubSector Varchar(200),                                                      
--UnitCostLocal Float,                                     
--OpeningFXRate Float,                                                                       
--UnitCostBase Float,                                                                       
--EndingFXRate Float,                
BegQty Float,                
EndQty Float,                
TradedQty Float,                                                      
--CostBasis Float,                
BegPrice Float,                
EndPrice Float,                
EndingmarketValueBase Float,                
DeltaExposureBase Float,                
TodayPNL Float,                
MTDPNL Float,                               
YTDPNL Float,                                  
--Open_CloseTag Varchar(20),                               
TotalCost_Local_ForOpenPos Float,                
Multiplier Float,                              
SideMultiplier Int,                              
Multiplier_ForOpenPos Float,                                
SideMultiplier_ForOpenPos Int,                  
--TaxlotID Varchar(50),                
UnderlyingBloombergSymbol Varchar(200),            
PrevSideMultiplier Int                
)    
  
Insert Into #TempMTMCustomized    
SELECT                                                      
--Rundate,                                      
--TradeDate,                                                      
------------------                                                      
--Pick All Symbols                                                      
------------------                                                      
Symbol,                              
MAX(PNL.CUSIPSymbol) AS CUSIPSymbol,                                                                       
MAX(PNL.ISINSymbol) AS ISINSymbol,                                                                      
MAX(PNL.SEDOLSymbol) AS SEDOLSymbol,                                                                       
MAX(Case                
When PNL.BloombergSymbol IS Null Or PNL.BloombergSymbol = ''                
Then Symbol              
Else PNL.BloombergSymbol                
End) As BloombergSymbol,                                                                       
MAX(PNL.ReutersSymbol) AS ReutersSymbol,                                                                       
MAX(PNL.IDCOSymbol) AS IDCOSymbol,                                                                       
MAX(PNL.OSISymbol) AS OSISymbol,                             
MAX(Case                        
When SubString(PNL.UnderlyingSymbol,1,1) = '$'                        
Then SubString(PNL.UnderlyingSymbol,2,Len(PNL.UnderlyingSymbol))                        
Else PNL.UnderlyingSymbol                        
End) As UnderlyingSymbol,                                                      
---------------                               
--Filter Fields                                                      
---------------                                                                  
--Fund,                                           
--TA.AssetId As AssetID,                                           
Asset,                                                                        
MAX(TradeCurrency) AS TradeCurrency,                                                                       
Side,                                                          
MAX(SecurityName) AS SecurityName,                                                                     
MAX(Strategy) AS Strategy,                                                                      
MAX(UDASector) AS UDASector,                                                              
MAX(UDACountry) AS UDACountry,                                                                      
MAX(UDASecurityType) AS UDASecurityType,                                                                      
MAX(UDAAssetClass) AS UDAAssetClass,                                                                      
MAX(UDASubSector) AS UDASubSector,                                                      
---------------------------------------------------                                                      
--Other Basic Fields required for report generation                                                      
---------------------------------------------------                                                                  
--UnitCostLocal,                                     
--OpeningFXRate,                                                                       
--UnitCostBase,                                                                       
--EndingFXRate,                                                     
                                                      
SUM(Case                                                      
When Open_CloseTag = 'O' And DateDiff(Day,RunDate,@PreviousBusinessDay) = 0                                                      
Then BeginningQuantity                                                      
Else 0.0                                                      
End) As BegQty,                                                      
                                                   
SUM(Case                                                      
When Open_CloseTag = 'O' And DateDiff(Day,RunDate,@EndDate) = 0                                                      
Then BeginningQuantity                                                     
Else 0.0                                                      
End) As EndQty,                                           
                                                      
0 AS TradedQty,                                                      
                                    
--Case                                                      
--When Open_CloseTag = 'O' And DateDiff(Day,RunDate,@EndDate) = 0 And (IsNull(BeginningQuantity,0) * IsNull(PNL.Multiplier,0)) <> 0                                    
--Then (TotalCost_Local * SideMultiplier)/(BeginningQuantity * PNL.Multiplier)                                    
--Else 0.0                                    
--End As CostBasis,                                                   
                                                      
MAX(Case                                                      
When Open_CloseTag = 'O' And DateDiff(Day,RunDate,@EndDate) = 0                                                      
Then BeginningPriceLocal                                                      
Else 0.0                                                      
End) As BegPrice,                                                      
                                                      
MAX(Case                                                      
When Open_CloseTag = 'O' And DateDiff(Day,RunDate,@EndDate) = 0              
Then EndingPriceLocal                                                      
Else 0.0                                                      
End) As EndPrice,                                                      
                                                      
SUM(Case                                                      
When Open_CloseTag = 'O' And DateDiff(Day,RunDate,@EndDate) = 0                                                      
Then EndingmarketValueBase                                                      
Else 0.0                              
End) As EndingmarketValueBase,                                   
                                    
SUM(Case                                                      
When Open_CloseTag = 'O' And DateDiff(Day,RunDate,@EndDate) = 0                                                        
Then IsNull(DeltaExposureBase,0)                                                      
Else 0.0                                           
End) As DeltaExposureBase,                                                
                                                      
SUM(Case                 
 When DateDiff(Day,@EndDate,Rundate) = 0                                                      
 Then TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend                                                      
 Else 0.0                                                      
End) As TodayPNL,                                                  
                                                      
SUM(Case                                                       
 When Datediff (day ,@MTDFromdate , Rundate) >= 0 And Datediff(day ,Rundate ,@EndDate) >= 0                                                       
 Then TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend                                                      
 Else 0.0                                                   
End) As  MTDPNL,                               
                              
SUM(TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend)  As YTDPNL ,                                  
--Open_CloseTag,                               
Sum(Case                                                      
 When (Open_CloseTag = 'O' And DateDiff(Day,RunDate,@EndDate) = 0 )                                   
 Then TotalCost_Local                                    
 Else 0.0                                    
End) As TotalCost_Local_ForOpenPos,                               
                                 
MAX(PNL.Multiplier) AS Multiplier,                              
MAX(  
CASE   
WHEN SideMultiplier=0 AND Side='Long'  
THEN 1  
WHEN SideMultiplier=0 AND Side='Short'  
THEN -1  
ELSE SideMultiplier  
END  
) AS SideMultiplier,                              
MAX(Case                                                      
 When (Open_CloseTag = 'O' And DateDiff(Day,RunDate,@EndDate) = 0 )                                   
 Then PNL.Multiplier                                    
 Else 0.0                                    
End) As Multiplier_ForOpenPos,                                
MAX(Case                                                      
 When (Open_CloseTag = 'O' And DateDiff(Day,RunDate,@EndDate) = 0 )                                   
 Then SideMultiplier                                    
 Else 1                                    
End) As SideMultiplier_ForOpenPos,                  
--TaxlotID,                
MAX(IsNull(VS.UnderlyingSymbol,PNL.UnderlyingSymbol)) As UnderlyingBloombergSymbol,            
MAX(Case                                                      
When Open_CloseTag = 'O' And DateDiff(Day,RunDate,@PreviousBusinessDay) = 0                                                      
Then SideMultiplier                                                      
Else 0                                                      
End) AS PrevSideMultiplier                         
                                                 
From T_MW_GenericPNL PNL                                                   
--Inner Join #T_CompanyFunds ON #T_CompanyFunds.FundName = PNL.Fund                                                      
--Inner Join #T_Assets TA ON TA.AssetName = PNL.Asset                   
Left Outer join V_SecMasterData_WithUnderlying VS ON (PNL.UnderlyingSymbol=VS.TickerSymbol)                                                                  
Where                                                                             
(    
Datediff (day , @YTDFromdate , Rundate) >= 0 and                                                                     
Datediff(day , Rundate , @EndDate) >= 0  And Open_CloseTag <> 'C'                                                    
OR                                                      
(                                                      
Datediff (day ,Case when @ShowClosing = 1 Then @YTDFromdate Else @MTDFromdate End, Rundate) >= 0 and                                                       
Datediff(day , Rundate , @EndDate) >= 0  And Open_CloseTag = 'C'                                                      
)    
)                                                      
And Open_CloseTag <> 'Accruals'    
Group BY Symbol,Asset,Side    
  
-------------------------------------                                                    
--Update Current Days Traded Quantity                                                    
-------------------------------------                                                    
Update #TempMTMCustomized                                                    
Set TradedQty = ((EndQty*SideMultiplier) - (BegQty*PrevSideMultiplier))   
  
If(@ShowClosing = 0)                        
Begin             
DELETE #TempMTMCustomized                        
 Where Symbol Not In (Select Distinct Symbol From #TempAllSymbol)                   
End  
If(@SearchString <> '')                                                                           
 Begin                                                                    
  if (@searchby='Symbol')                                                                
  begin                                      
  SELECT * FROM #TempMTMCustomized                                                                
  Inner Join #Symbol on #Symbol.items = #TempMTMCustomized.Symbol                                                                
  Order by TradeDate                                                                
  end                                                                
  else if (@searchby='underlyingSymbol')                                                               
  begin                                                            
  SELECT * FROM #TempMTMCustomized                                     
  Inner Join #Symbol on #Symbol.items = #TempMTMCustomized.underlyingSymbol                                                                
  Order by symbol                                                                
  end                                                                  
  else if (@searchby='BloombergSymbol')                                                                
  begin                                                                
  SELECT * FROM #TempMTMCustomized                                               
  Inner Join #Symbol on #Symbol.items = #TempMTMCustomized.BloombergSymbol                                                                
  Order by symbol                                                    
  end                                                     
  else if (@searchby='SedolSymbol')                                                                
  begin                                                                
  SELECT * FROM #TempMTMCustomized                                 
  Inner Join #Symbol on #Symbol.items = #TempMTMCustomized.SedolSymbol                                                                
  Order by symbol                                                                
  end                                                                    
  else if (@searchby='OSISymbol')                                                              
  begin                                                                
  SELECT * FROM #TempMTMCustomized                                                                
  Inner Join #Symbol on #Symbol.items = #TempMTMCustomized.OSISymbol                                                                
  Order by symbol                                                                
  end                                                                    
  else if (@searchby='IDCOSymbol')                                                                
  begin                                                                
  SELECT * FROM #TempMTMCustomized                                                                
  Inner Join #Symbol on #Symbol.items = #TempMTMCustomized.IDCOSymbol                                                                
  Order by symbol                                                                
  end                                                                    
  else if (@searchby='ISINSymbol')                                                             
  begin                                                                
 SELECT * FROM #TempMTMCustomized                                                                
  Inner Join #Symbol on #Symbol.items = #TempMTMCustomized.ISINSymbol                                                                
  Order by symbol                                                                
  end                                                                   
  else if (@searchby='CUSIPSymbol')                                                                
  begin                                                                
  SELECT * FROM #TempMTMCustomized                                          
  Inner Join #Symbol on #Symbol.items = #TempMTMCustomized.CUSIPSymbol                                                                
  Order by symbol                                                                
  end                                                                             
 End                                                                            
Else                                                                            
 Begin                                                                            
  Select * from #TempMTMCustomized Order By symbol                                                                  
 End

Drop Table #TempMTMCustomized,#T_CompanyFunds,#T_Assets,#Symbol,#TempSymbol             
Drop Table #TempClosedPosSymbols,#TempOpenPosSymbols,#TempAllSymbol 
    
END 