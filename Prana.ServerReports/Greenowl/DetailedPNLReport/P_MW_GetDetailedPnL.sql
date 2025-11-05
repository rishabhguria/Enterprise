/*************************************************                          
Author : Ankit Misra                          
Creation Date : 17th April , 2015                            
Description : Brings the Data for DetailedPnL Report which includes DailyPNL,MTD PNL,YTD PNL with a capability of removing closed position of previous month          
Execution Statement:                         
exec P_MW_GetDetailedPnL @EndDate='2015-04-22 00:00:00:000',@Fund=N'1245,1213,1214,1238,1239,1240,1241,1242,1244,1243,1246',@Asset=N'6,1,2',@ShowClosing=1,@ReportId=N'MTM_V3',@SearchString=N'',@SearchBy=N'Symbol'                     
*************************************************/          
CREATE Procedure P_MW_GetDetailedPnL                      
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
                      
Select * InTo #Symbol                                 
From dbo.split(@SearchString , ',')                
                     
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
                      
SELECT                      
Rundate,                      
TradeDate,                      
------------------                      
--Pick All Symbols                      
------------------                      
Symbol,                      
CUSIPSymbol,                                       
ISINSymbol ,                                      
SEDOLSymbol ,                                       
BloombergSymbol ,                                       
ReutersSymbol ,                                       
IDCOSymbol ,                                       
OSISymbol,                             
UnderlyingSymbol ,                      
---------------                      
--Filter Fields                      
---------------                                  
Fund,                      
TA.AssetId As AssetID,                                      
Asset,                                        
TradeCurrency,                                       
Side,                          
SecurityName,                                       
MasterFund,                                      
Strategy,                                      
UDASector,                                       
UDACountry,                                      
UDASecurityType,                                      
UDAAssetClass,                                      
UDASubSector ,                      
---------------------------------------------------                      
--Other Basic Fields required for report generation                      
---------------------------------------------------                                  
UnitCostLocal,                                       
OpeningFXRate,                                       
UnitCostBase,                                       
EndingFXRate,                      
                      
Case                      
When Open_CloseTag = 'O' And DateDiff(Day,RunDate,@PreviousBusinessDay) = 0                      
Then BeginningQuantity                      
Else 0.0                      
End As BegQty,                      
                      
Case                      
When Open_CloseTag = 'O' And DateDiff(Day,RunDate,@EndDate) = 0                      
Then BeginningQuantity                      
Else 0.0                      
End As EndQty,                      
                      
0 AS TradedQty,                      
    
Case                      
When Open_CloseTag = 'O' And (IsNull(BeginningQuantity,0) * IsNull(Multiplier,0)) <> 0    
Then (TotalCost_Local * SideMultiplier)/(BeginningQuantity * Multiplier)    
Else 0.0    
End As CostBasis,                   
                      
Case                      
When Open_CloseTag = 'O' And DateDiff(Day,RunDate,@EndDate) = 0                      
Then BeginningPriceLocal                      
Else 0.0                      
End As BegPrice,                      
                      
Case                      
When Open_CloseTag = 'O' And DateDiff(Day,RunDate,@EndDate) = 0                      
Then EndingPriceLocal                      
Else 0.0                      
End As EndPrice,                      
                      
Case                      
When Open_CloseTag = 'O' And DateDiff(Day,RunDate,@EndDate) = 0                      
Then EndingmarketValueBase                      
Else 0.0                      
End As EndingmarketValueBase,                      
IsNull(DeltaExposureBase,0) As DeltaExposureBase,                      
                      
--Case                      
--When DateDiff(Day,@EndDate,TradeDate) = 0 Or DateDiff(Day,@EndDate,ClosingDate) = 0                       
--Then TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend                      
--Else 0.0                      
--End As TodayPNL,                  
                  
 Case                      
When DateDiff(Day,@EndDate,Rundate) = 0                      
Then TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend                      
Else 0.0                      
End As TodayPNL,                  
                   
                      
Case                       
When Datediff (day ,@MTDFromdate , Rundate) >= 0 And Datediff(day ,Rundate ,@EndDate) >= 0                       
Then TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend                      
Else 0.0                   
End As  MTDPNL,                      
                      
TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend  As YTDPNL ,  
Open_CloseTag,  
TotalCost_Local,  
Multiplier  
                    
-------------------------------                      
--Insert Data Into Temp Table                      
-------------------------------                      
INTO #TempMTMCustomized                      
                      
From T_MW_GenericPNL PNL                   
Inner Join #T_CompanyFunds ON #T_CompanyFunds.FundName = PNL.Fund                      
Inner Join #T_Assets TA ON TA.AssetName = PNL.Asset                                     
Where                                             
datediff (day , @YTDFromdate , Rundate) >= 0 and                                     
datediff(day , Rundate , @EndDate) >= 0  And Open_CloseTag <> 'C'                      
OR                      
(                      
datediff (day ,Case when @ShowClosing = 1 Then @YTDFromdate Else @MTDFromdate End, Rundate) >= 0 and                       
Datediff(day , Rundate , @EndDate) >= 0  And Open_CloseTag = 'C'                      
)                      
And Open_CloseTag <> 'Accruals'                      
order by Symbol                      
-------------------------------------                      
--Update Current Days Traded Quantity                      
-------------------------------------                      
Update #TempMTMCustomized                      
Set TradedQty=EndQty-BegQty                
                
If(@SearchString <> '')                                               
 Begin                                        
  if (@searchby='Symbol')                                    
  begin                                    
  SELECT * FROM #TempMTMCustomized                                    
  Inner Join #Symbol on #Symbol.items = #TempMTMCustomized.Symbol                                    
  Order by symbol                                    
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
    
                  
Drop Table #TempMTMCustomized,#T_CompanyFunds,#T_Assets,#Symbol                      
                      
END