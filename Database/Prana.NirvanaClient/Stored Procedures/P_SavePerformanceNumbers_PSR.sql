/*************************************************                                                                    
Author : Pankaj Sharma                                              
Creation Date : 26 January, 2016                                             
Description :                                               
Script for Performance Section in the report                        
                                              
Please refer doc attached in the JIRA for detailed description                                          
http://jira.nirvanasolutions.com:8080/browse/PRANA-11769                                          
                                          
Execution Statement:                                        
exec  P_SavePerformanceNumbers_PSR      
@StartDate='2015-9-25 00:00:00:000',      
@EndDate='2015-9-25 00:00:00:000',      
@fund=N'1354,1355,1356,1357,1358,1359,1360,1361,1362,1363,1364,1365,1366,1369,1370,1371,1373,1374,1375,1376,1378,1379,1380'      
*************************************************/                                        
                                                                  
CREATE PROCEDURE [dbo].[P_SavePerformanceNumbers_PSR]      
(            
@StartDate DATETIME      
,@EndDate DATETIME                                          
,@fund VARCHAR(MAX)                                          
)                                                
As                                            
/*--------------------------------------------------------------------------------------------------                                            
Extract funds out in temp table                                          
---------------------------------------------------------------------------------------------------*/                                              
SELECT * INTO #Funds                                          
FROM dbo.Split(@fund, ',')            
          
          
--Fields For Net Income/PNL          
          
declare @tmpPNL varchar(MAX)                          
SET @tmpPNL = ''                          
Select @tmpPNL = @tmpPNL + Cast(FieldID as varchar(50)) +',' from T_FieldsPSR FLD             
Inner Join T_PositionsPSR POS on FLD.PositionID = POS.PostionID                                    
where Pos.PositionInPSR = 'Change In Equity' and  IncludeInNetIncome = 'True'                               
Set @tmpPNL = SUBSTRING(@tmpPNL, 0, LEN(@tmpPNL))             
print @tmpPNL          
          
          
--Fields For Net Opening EQUITY          
          
declare @tmpOpening varchar(MAX)                          
SET @tmpOpening = ''                          
Select @tmpOpening = @tmpOpening + Cast(FieldID as varchar(50)) +',' from T_FieldsPSR FLD             
Inner Join T_PositionsPSR POS on FLD.PositionID = POS.PostionID                                    
where Pos.PositionInPSR = 'PortFolio Composition'           
Set @tmpOpening = SUBSTRING(@tmpOpening, 0, LEN(@tmpOpening))             
print @tmpOpening          
          
          
--Fields For Cash in kind          
          
declare @tmpAddRed varchar(MAX)                          
SET @tmpAddRed = ''                          
Select @tmpAddRed = @tmpAddRed + Cast(FieldID as varchar(50)) +',' from T_FieldsPSR FLD             
Inner Join T_PositionsPSR POS on FLD.PositionID = POS.PostionID                                    
where Pos.PositionInPSR = 'Change In Equity' and  (IncludeInNetIncome = 'False' or IncludeInNetIncome IS NULL)          
Set @tmpAddRed = SUBSTRING(@tmpAddRed, 0, LEN(@tmpAddRed))             
print @tmpAddRed          
          
      
      
Select DateValue as Date,FundID  into #TempActivities  from [T_FundWiseFieldActivites]      
group by DateValue,FundID      
      
Alter table #TempActivities add PNL float, AddRed float, OpeningValue float      
      
      
--------------------------------------------------------------------------      
--For PNL      
--------------------------------------------------------------------------      
Update #TempActivities Set PNL = IsNull(Temp.PNL,0)      
from #TempActivities Activities      
Inner JOIN      
(      
Select DateValue AS date,FundID,Sum(ActivityBase) as PNL from [T_FundWiseFieldActivites]      
where FieldID in (SELECT items FROM Split(@tmpPNL,','))  
group by DateValue,FundID  
)as Temp      
on Temp.Date = Activities.Date AND Temp.FundID = Activities.FundID      
      
      
--------------------------------------------------------------------------      
--For AddRed      
--------------------------------------------------------------------------      
Update #TempActivities Set AddRed = IsNull(Temp.AddRed,0)      
from #TempActivities Activities      
Inner JOIN      
(      
Select DateValue as Date,FundID,Sum(ActivityBase) as AddRed from [T_FundWiseFieldActivites]      
where FieldID in (SELECT items FROM Split(@tmpAddRed,','))      
group by DateValue,FundID  
  
)as Temp      
on Temp.Date = Activities.Date AND Temp.FundID = Activities.FundID      
      
      
--------------------------------------------------------------------------      
--For Opeing Equity      
--------------------------------------------------------------------------      
Update #TempActivities Set OpeningValue = IsNull(Temp.OpeningValue,0)      
from #TempActivities Activities      
INNER JOIN          
(          
Select Sum(CloseDRBalBase - CloseCRBalBase) as OpeningValue,TransactionDate,FundID          
 FROM T_SubAccountBalances tempSubAccBal WITH(NOLOCK)                
WHERE tempSubAccBal.SubAccountID in                
(                    
 SELECT DISTINCT SubAccountID FROM T_SubAccountMappingPSR INNER JOIN T_FieldsPSR On T_FieldsPSR.FieldID = T_SubAccountMappingPSR.FieldID                     
 WHERE T_FieldsPSR.FieldID IN (SELECT items FROM Split(@tmpOpening,','))                  
)          
group by FundID,TransactionDate          
)as temp          
on Activities.Date-1=temp.TransactionDate and Activities.FundID=temp.FundID      
      
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[T_SavedPerformanceNumbers]'))      
BEGIN                
Drop table T_SavedPerformanceNumbers      
END          
      
      
Select * into T_SavedPerformanceNumbers from #TempActivities      
          
                        
/***************                                          
Drop Temporary TABLES                           
******************/                         
Drop table #Funds,#TempActivities