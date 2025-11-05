 /*************************************************                                            
Author : Ankit Misra                                           
Creation Date : 10th June , 2015                                              
Description : Script for Lower DashBoard Exposure part of Daily Report    
  
Modified By: Sandeep Singh  
Date: 10 August 2015  
Desc: http://jira.nirvanasolutions.com:8080/browse/PRANA-10294 (No FX Spot or Forward positions required in the Positions by Size area)                                    
                                
Execution Statement:                                           
P_MW_LowerDashBoardExposure_DailyReport @EndDate= '7/1/2015' ,@Fund=N'1270,1271,1298,1302,1304,1305,1306,1307,1308,1309,1310', @PTHFund = '1270'                    
*************************************************/                          
ALTER Procedure [dbo].[P_MW_LowerDashBoardExposure_DailyReport]                          
(                          
 @EndDate datetime,                          
 @Fund Varchar(max),      
 @PTHFund Varchar(Max)                          
)                          
AS                    
                    
--Declare @EndDate datetime                                        
--Declare @Fund Varchar(2000)      
--Declare @PTHFund Varchar(Max)                                            
--                                        
--Set @EndDate = '7/2/2015'                                    
--Set @Fund = '1309'--'1270,1271,1298,1302,1304,1305,1306,1307,1308,1309,1310'      
--Set @PTHFund = '1270'                   
                          
BEGIN                          
                          
Declare @DefaultAUECID int                          
Set @DefaultAUECID=(select top 1 DefaultAUECID  from T_Company where companyId <> -1)                          
                          
Declare @PreviousBusinessDay DateTime                          
Set @PreviousBusinessDay = dbo.AdjustBusinessDays(@EndDate,-1, @DefaultAUECID)                          
                          
                          
------------------------------------------------------------------------------------------                          
--Pick Selected Funds Based on their ID                          
------------------------------------------------------------------------------------------                          
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
------------------------------------------------------------------------------------------                          
--Pick Required Fields from T_MW_GenericPNL                          
------------------------------------------------------------------------------------------                          
SELECT            
          
UnderlyingSymbol,            
               
Sum            
(                        
 CASE                            
  WHEN DATEDIFF(d,rundate,@EndDate)=0                            
  THEN ISNULL(EndingmarketValueBase,0)                            
  ELSE 0.0                            
 END            
) AS EndOfDay,              
                          
            
Sum            
(                           
 CASE                            
  WHEN DATEDIFF(d,rundate,@EndDate)=0 AND Asset <> 'Cash' AND Asset NOT IN ('FX','FXForward')                     
  THEN ISNULL(DeltaExposureBase,0)                            
  ELSE 0.0                            
 END            
) AS DeltaExposureBase          
            
INTO #PNL                             
FROM T_MW_GenericPNL PNL                          
INNER JOIN #T_CompanyFunds ON #T_CompanyFunds.FundName = PNL.Fund                               
WHERE DATEDIFF(Day,Rundate,@EndDate)=0                        
AND Open_CloseTag = 'O' And Asset Not In ('Cash','FX','FXForward')          
Group By UnderlyingSymbol          
          
Alter Table #PNL            
Add Side Varchar(10) Null            
-----------------------------------------------------------------------------------------------            
-- Update side based on DeltaNetExposure            
-----------------------------------------------------------------------------------------------          
Update #PNL            
Set Side =             
 Case            
  When DeltaExposureBase >= 0            
  Then 'Long'            
  Else 'Short'            
 End                   
-----------------------------------------------------------------------------------------------                          
--Declare Required Table and Fill with Appropriate Fields                          
-----------------------------------------------------------------------------------------------                          
DECLARE @table TABLE                              
(                              
 Item  VARCHAR(MAX),                              
 Quantity FLOAT                              
)                    
                     
INSERT INTO @table(Item) values('L/S Ratio')                         
INSERT INTO @table(Item) values('Longs')                    
INSERT INTO @table(Item) values('Short')                    
INSERT INTO @table(Item) values('Gross')                          
INSERT INTO @table(Item) values('Net')                    
                    
                    
----------------------------------------------------------------------------------------------------------------------------                    
-- Since End of the day NAV is used at multiple places for calculation of percentage therefore stored in a seperate variable                    
----------------------------------------------------------------------------------------------------------------------------                    
--                    
--Declare @EndingNAV float                    
--Set  @EndingNAV = (Select NULLIF(Sum(EndOfDay),0) from #PNL) 

Declare @EndingNAV float
                    
Create Table #EODNAV
(
EODNAV Float
)

Insert Into #EODNAV
Exec P_MW_GetEndOfTheDayNAV_DailyReport @EndDate,@Fund,@PTHFund,'DailyReport_MW' 

Select @EndingNAV = SUM(EODNAV) from #EODNAV          
          
Declare @longDeltaExposure Float, @shortDeltaExposure Float            
          
Select @longDeltaExposure = Sum(DeltaExposureBase) From #PNL Where Side  = 'Long'           
Select @shortDeltaExposure = Sum(DeltaExposureBase) From #PNL Where Side  = 'Short'                    
                          
-----------------------------------------------------------------------------------------------                          
--Update Table With Required Calculated Fields                          
-----------------------------------------------------------------------------------------------                        
Update @table                          
Set Quantity = Abs(ISNULL(@longDeltaExposure/NULLIF(@shortDeltaExposure,0),0)) Where Item = 'L/S Ratio'                    
                    
Update @table                          
Set Quantity= (ISNULL(@longDeltaExposure/@EndingNAV,0)*100) where Item = 'Longs'                    
                    
Update @table                          
Set Quantity= Abs(ISNULL(@shortDeltaExposure/@EndingNAV,0)*100) where Item = 'Short'                     
                          
Update @table                            
Set Quantity = (Select ISNULL(Sum(Abs(DeltaExposureBase))/@EndingNAV,0) from #PNL)*100 Where Item = 'Gross'                            
                            
Update @table                            
--Set Quantity = Abs((Select ISNULL(Sum(DeltaExposureBase)/@EndingNAV,0) from #PNL)) * 100 Where Item = 'Net'                   
Set Quantity = (Select ISNULL(Sum(DeltaExposureBase)/@EndingNAV,0) from #PNL) * 100 Where Item = 'Net'                   
                          
Select * from @table                          
                          
Drop Table #PNL,#T_CompanyFunds,#EODNAV                        
END