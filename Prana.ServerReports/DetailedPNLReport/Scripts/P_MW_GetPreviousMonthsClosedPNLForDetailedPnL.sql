/*************************************************                              
Author : Ankit Misra                              
Creation Date : 17th April , 2015                                
Description : Pick Realized PNL Date From Begining of the Year till Previous Month EndDate JIRA: PRANA-6554              
Execution Statement:                             
 P_MW_GetPreviousMonthsClosedPNLForDetailedPnL '4/15/2015','1245,1213,1214,1238,1239,1240,1241,1242,1244,1243,1246','6,1,2',0,'FLML','Symbol'              
*************************************************/              
CREATE Procedure P_MW_GetPreviousMonthsClosedPNLForDetailedPnL                           
(                                     
 @EndDate DateTime,                            
 @Fund varchar(max),                      
 @Asset varchar(max),                            
 @ShowPreviousMonthClosedPNL bit,                  
 @SearchString Varchar(5000) ,                                        
 @SearchBy Varchar(100)                           
)                            
AS                   
SET NOCOUNT ON;                  
                       
                  
                      
--Check for Closed Details is False No need to Execute Full SP                           
IF(@ShowPreviousMonthClosedPNL=0)                        
BEGIN                        
Select * InTo #Symbol                                   
From dbo.split(@SearchString , ',')                 
                    
Declare @DefaultAUECID int                                                                                
Set @DefaultAUECID=(select top 1 DefaultAUECID  from T_Company where companyId <> -1)                 
                           
Declare @YTDFromdate datetime,@PreviousMonthEndDate datetime                            
                            
Select @YTDFromdate= dbo.F_MW_GetWeekendAdjusted (@EndDate,1,7)                            
Select @PreviousMonthEndDate= dbo.F_MW_GetWeekendAdjusted (@EndDate,1,9)                
                
IF (Datediff(Day,@EndDate,@YTDFromdate)=0)                  
BEGIN                
Set @EndDate=dbo.AdjustBusinessDays(@EndDate,-1, @DefaultAUECID)                           
Set @YTDFromdate= dbo.F_MW_GetWeekendAdjusted (@EndDate,1,7)                            
Set @PreviousMonthEndDate= dbo.F_MW_GetWeekendAdjusted (@EndDate,1,9)                
END              
              
IF(DateDiff(Month,@PreviousMonthEndDate,@EndDate)=0)              
BEGIN              
Set @PreviousMonthEndDate=dbo.AdjustBusinessDays(@PreviousMonthEndDate,-1,@DefaultAUECID)              
END                  
                  
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
From                       
T_Asset           
INNER JOIN @T_AssetIDs AssetIDs ON T_Asset.AssetID = AssetIDs.AssetID                      
                      
Select    
TaxlotID,      
Open_CloseTag ,                
Symbol,        
TA.AssetId As AssetID,                        
CUSIPSymbol,               
ISINSymbol ,                                        
SEDOLSymbol ,                                         
BloombergSymbol ,                                         
ReutersSymbol ,                                         
IDCOSymbol ,                                         
OSISymbol,                                         
UnderlyingSymbol ,                      
Strategy,                                        
UDASector,                                         
UDACountry,                                        
UDASecurityType,                                        
UDAAssetClass,                                        
UDASubSector,                            
ISNULL(TotalRealizedPNLOnCost,0) + ISNULL(ChangeInUNRealizedPNL,0) + ISNULL(Dividend,0) As PreviousMonthsPNL    
                 
Into #TempPreviousMonthsPNL                          
From T_MW_GenericPNL PNL                                   
Inner Join #T_CompanyFunds ON #T_CompanyFunds.FundName = PNL.Fund                                      
Inner Join #T_Assets TA ON TA.AssetName = PNL.Asset                                                     
Where                                                             
Datediff (day , @YTDFromdate , Rundate) >= 0 and                                                     
Datediff(day , Rundate , @EndDate) >= 0  And Open_CloseTag <> 'C'                                    
OR                                      
(                                      
Datediff (day ,@YTDFromdate, Rundate) >= 0 and                                       
Datediff(day , Rundate , @EndDate) >= 0  And Open_CloseTag = 'C'                                      
)                                     
And Open_CloseTag <> 'Accruals'             
                  
If(@SearchString <> '')                                                 
 Begin                                               
  if (@searchby='Symbol')                                      
  begin                                      
  SELECT * FROM #TempPreviousMonthsPNL                                        
Inner Join #Symbol on #Symbol.items = #TempPreviousMonthsPNL.Symbol                                     
  end                                      
  else if (@searchby='underlyingSymbol')                                      
  begin                                  
  SELECT * FROM #TempPreviousMonthsPNL                                      
  Inner Join #Symbol on #Symbol.items = #TempPreviousMonthsPNL.underlyingSymbol                                      
  end                                        
  else if (@searchby='BloombergSymbol')                                      
  begin                                      
  SELECT * FROM #TempPreviousMonthsPNL                                      
  Inner Join #Symbol on #Symbol.items = #TempPreviousMonthsPNL.BloombergSymbol                                      
  end                                          
  else if (@searchby='SedolSymbol')                                      
  begin                                      
  SELECT * FROM #TempPreviousMonthsPNL                                      
  Inner Join #Symbol on #Symbol.items = #TempPreviousMonthsPNL.SedolSymbol                                      
  end                                          
  else if (@searchby='OSISymbol')                                      
  begin                                      
  SELECT * FROM #TempPreviousMonthsPNL                                      
  Inner Join #Symbol on #Symbol.items = #TempPreviousMonthsPNL.OSISymbol                                      
  end                                          
  else if (@searchby='IDCOSymbol')                                      
  begin                                      
  SELECT * FROM #TempPreviousMonthsPNL                                      
  Inner Join #Symbol on #Symbol.items = #TempPreviousMonthsPNL.IDCOSymbol                                       
  end                                          
  else if (@searchby='ISINSymbol')                                      
  begin                                      
  SELECT * FROM #TempPreviousMonthsPNL           
  Inner Join #Symbol on #Symbol.items = #TempPreviousMonthsPNL.ISINSymbol                                      
  end                                         
  else if (@searchby='CUSIPSymbol')                                      
  begin                                      
  SELECT * FROM #TempPreviousMonthsPNL                                      
  Inner Join #Symbol on #Symbol.items = #TempPreviousMonthsPNL.CUSIPSymbol                                      
  end                                                   
 End                                                  
Else                                                 
 Begin                                                  
  Select * from #TempPreviousMonthsPNL Order By TaxlotID,Open_CloseTag                                        
 End                       
                            
Drop Table #T_CompanyFunds,#TempPreviousMonthsPNL,#T_Assets,#Symbol                           
END                            
ELSE                            
BEGIN                    
Select 0 as PreviousMonthsPNL                            
END