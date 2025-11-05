                                                              
CREATE PROCEDURE [dbo].[P_MW_GetFundNAVWPSAAndIndex]                                                              
(                                                              
 @EndDate datetime,                                                              
 @FundID varchar(max),                                                          
 @paramNAVbyMWorPM int,                                                      
 @ITD dateTime,                                                    
 @Indexes varchar(max),                                  
 @ReportID Varchar(100),                                  
 @IncludeAccured bit,                                
 @IncludeNetGross bit,                              
 @IncludeITD bit                                                 
)                                                              
As                                                              
Begin                         
--------------------------------------------------adjust end date                           
                        
declare @ITDFromDate Datetime                         
declare @MTDFromdateNAV datetime,@DTDFromdateNAV datetime,@YTDFromdateNAV datetime,@QTDFromdateNAV DateTime                        
declare @MTDFromdateWPSA datetime,@DTDFromdateWPSA datetime,@YTDFromdateWPSA datetime,@QTDFromdateWPSA datetime                       
declare @MTDFromdateSP500 datetime,@DTDFromdateSP500 datetime,@YTDFromdateSP500 datetime,@QTDFromdateSP500 datetime                        
                        
                                                      
Create Table #Temp                                                      
(                                                      
 ITDdateNAV DateTime,DTDdateNAV Datetime, MTDdateNAV datetime, YTDdateNAV DateTime,QTDdateNAV DateTime,                        
 ITDNav Float,                                                      
 DTDNav Float,                                                      
 MTDNav Float,                                                      
 YTDNav Float,      
 QTDNav Float,                                                      
 ITDdateWPSA DateTime,DTDdateWPSA Datetime, MTDdateWPSA datetime, YTDdateWPSA DateTime, QTDdateWPSA DateTime,                       
 ITDWPSA Float,                                                      
 DTDWPSA Float,                                                      
 MTDWPSA Float,                                                      
 YTDWPSA Float,      
 QTDWPSA Float,                                                      
 ITDdateSP500 DateTime,DTDdateSP500 Datetime, MTDdateSP500 datetime, YTDdateSP500 DateTime,QTDdateSP500 DateTime,                        
 ITDSP500 Float,                                                      
 DTDSP500 Float,                                                      
 MTDSP500 Float,                                                      
 YTDSP500 Float,      
 QTDSP500 float,                                                      
 Funds varchar(max),                                                      
 ID int identity(1,1)                                                         
)                                                      
                        
--------------------------------------------------all the funds added in the Temp output table                                                        
                        
insert into #Temp(Funds) values(@FundID)                                                        
                        
--------------------------------------------------get fund names in a local variable                         
                        
Select * Into #Funds                                                                                                          
from dbo.Split(@FundID, ',')                                                      
                                                     
DECLARE @FNameList varchar(max)                                                    
SELECT @FNameList= coalesce(@FNameList + ',', '') + FName.Fundname                                                     
FROM (select Fundname FROM T_CompanyFunds inner join #Funds t on CompanyFundID=t.items) FName                                       
                        
--------------------------------------------------check if ITD is to be taken from DB or Report                        
                        
IF @IncludeITD = 1          ----Include ITD from DB                              
begin                           
 select @ITD = dbo.AdjustBusinessDays(min(CashMgmtStartDate),1,1) from dbo.T_CashPreferences where ID in (select items from #Funds)                            
end                              
                        
---------------------------------------------------Insert ITDdateNAV, ITDdateWPSA and ITDdateSP500                           
set @ITDFromdate= @ITD                    
declare @ITDFromdateNAV DateTime                  
set @ITDFromdateNAV = dbo.AdjustBusinessDays(@ITDFromdate,-1,1)                  
 Update #Temp set                         
 ITDdateNAV= @ITDFromdateNAV,                        
 ITDdateWPSA= @ITDFromdate,                        
 ITDdateSP500= @ITDFromdate                        
                                                     
---------------------------------------------------Insert DTDdateNAV, DTDdateWPSA and DTDdateSP500                        
                        
set @DTDFromdateNAV = dbo.AdjustBusinessDays(@EndDate,-1,11)                        
set @DTDFromdateWPSA = dbo.AdjustBusinessDays(dbo.AdjustBusinessDays(@EndDate,-1,11),1,11)                        
set @DTDFromdateSP500 = dbo.AdjustBusinessDays(dbo.AdjustBusinessDays(@EndDate,-1,11),1,11)                        
                        
                                  
-- Select @DTDFromdate=  dbo.AdjustBusinessDays(@EndDate,-1,11)                        
 update #Temp SET                         
 DTDdateNAV= @DTDFromdateNAV,                        
 DTDdateWPSA= @DTDFromdateWPSA,                        
 DTDdateSP500= @DTDFromdateSP500                        
                            
---------------------------------------------------Insert MTDdateNAV, MTDdateWPSA and MTDdateSP500                        
                        
set @MTDFromdateNAV = dbo.AdjustBusinessDays(dbo.F_MW_GetWeekendAdjusted (@EndDate,1,3),-1,11)                        
set @MTDFromdateWPSA = dbo.AdjustBusinessDays(dbo.AdjustBusinessDays(dbo.F_MW_GetWeekendAdjusted (@EndDate,1,3),-1,11),1,11)                        
set @MTDFromdateSP500 = dbo.AdjustBusinessDays(dbo.AdjustBusinessDays(dbo.F_MW_GetWeekendAdjusted (@EndDate,1,3),-1,11),1,11)                        
                         
 update #Temp SET                         
 MTDdateNAV= @MTDFromdateNAV,                        
 MTDdateWPSA= @MTDFromdateWPSA,                        
 MTDdateSP500= @MTDFromdateSP500                        
                            
---------------------------------------------------Insert YTDdateNAV, YTDdateWPSA and YTDdateSP500                        
                        
set @YTDFromdateNAV = dbo.F_MW_GetWeekendAdjusted (@EndDate,1,10)                        
set @YTDFromdateWPSA = dbo.AdjustBusinessDays(dbo.AdjustBusinessDays(dbo.F_MW_GetWeekendAdjusted (@EndDate,1,7),-1,11),1,11)                        
set @YTDFromdateSP500 = dbo.AdjustBusinessDays(dbo.AdjustBusinessDays(dbo.F_MW_GetWeekendAdjusted (@EndDate,1,7),-1,11),1,11)                        
                        
                        
 update #Temp SET                         
 YTDdateNAV= @YTDFromdateNAV,                        
 YTDdateWPSA= @YTDFromdateWPSA,                        
 YTDdateSP500= @YTDFromdateSP500          
      
      
---------------------------------------------------Insert QTDdateNAV, QTDdateWPSA and QTDdateSP500                        
                        
set @QTDFromdateNAV = dbo.AdjustBusinessDays(dbo.F_MW_GetWeekendAdjusted (@EndDate,1,5),-1,11)                         
set @QTDFromdateWPSA = dbo.AdjustBusinessDays(dbo.AdjustBusinessDays(dbo.F_MW_GetWeekendAdjusted (@EndDate,1,5),-1,11),1,11)                        
set @QTDFromdateSP500 = dbo.AdjustBusinessDays(dbo.AdjustBusinessDays(dbo.F_MW_GetWeekendAdjusted (@EndDate,1,5),-1,11),1,11)                        
                        
                        
 update #Temp SET                         
 QTDdateNAV= @QTDFromdateNAV,             
 QTDdateWPSA= @QTDFromdateWPSA,                        
 QTDdateSP500= @QTDFromdateSP500                        
                    
                                                  
----------------------------------------NAV---------------------------------------------------                                                     
declare @IsITD bit                                                    
set @IsITD=1                                                    
Update #Temp                                                     
set ITDNav=COALESCE(dbo.F_MW_GetNAV_WPS(@ITDFromdateNAV,@paramNAVbyMWorPM, @FNameList,@IsITD,@ReportID,@IncludeAccured),0)                                                    
                                                    
set @IsITD=0                                                    
update #Temp                                                     
set DTDNav=COALESCE(dbo.F_MW_GetNAV_WPS(@DTDFromdateNAV,@paramNAVbyMWorPM, @FNameList,@IsITD,@ReportID,@IncludeAccured),0)                                                     
                                                      
update #Temp                                                     
set MTDNav=          
CASE           
 WHEN datediff(d,@MTDFromdateWPSA,@ITDFromdateNAV)<0          
then COALESCE(dbo.F_MW_GetNAV_WPS(@MTDFromdateNAV,@paramNAVbyMWorPM, @FNameList,@IsITD,@ReportID,@IncludeAccured),0)                             
 else COALESCE(dbo.F_MW_GetNAV_WPS(@ITDFromdateNAV,@paramNAVbyMWorPM, @FNameList,@IsITD,@ReportID,@IncludeAccured),0)                             
End          
                                                       
update #Temp                                                     
set YTDNav=          
CASE           
 WHEN datediff(d,@YTDFromdateNAV,@ITDFromdateNAV)<0          
 then COALESCE(dbo.F_MW_GetNAV_WPS(@YTDFromdateNAV,@paramNAVbyMWorPM, @FNameList,@IsITD,@ReportID,@IncludeAccured),0)             
 else COALESCE(dbo.F_MW_GetNAV_WPS(@ITDFromdateNAV,@paramNAVbyMWorPM, @FNameList,@IsITD,@ReportID,@IncludeAccured),0)             
  end          
      
      
update #Temp                                                     
set QTDNav=          
CASE           
 WHEN datediff(d,@QTDFromdateNAV,@ITDFromdateNAV)<0          
 then COALESCE(dbo.F_MW_GetNAV_WPS(@QTDFromdateNAV,@paramNAVbyMWorPM, @FNameList,@IsITD,@ReportID,@IncludeAccured),0)             
 else COALESCE(dbo.F_MW_GetNAV_WPS(@ITDFromdateNAV,@paramNAVbyMWorPM, @FNameList,@IsITD,@ReportID,@IncludeAccured),0)             
  end          
       
                                                    
----------------------------------------------Index Calculation -------------------------------                                                    
CREATE table #Index                                               
(                                                    
rSymbol varchar(200),                                                  
Name varchar(200),                                                  
toDatePrice float,                                                  
toDateDate Datetime,                                    
[day] float,                                                  
dayPrice float,                                                  
dayDate datetime,                                                  
MTD float,                               
MTDPrice float,                                                  
MTDDate Datetime,                                                  
ITD float,                    
ITDPrice float,                                                  
ITDDate datetime,                                                  
YTD float,                                                 
YTDPrice float,                                                  
YTDDate Datetime,      
QTD float,                                                 
QTDPrice float,                                                  
QTDDate Datetime                                                    
                                                    
)                                                    
                                                     
--Insert into #Index exec P_EODBenchmarkWPSPerformance @EndDate,@ITD,@Indexes                                          
Insert into #Index exec P_EODBenchmarkPerformanceWPS @EndDate,@ITDFromdate,@Indexes                
                                            
update #Temp SET ITDSP500 =COALESCE(#Index.ITD,0)*100,                                                    
DTDSP500=COALESCE(#Index.[day],0)*100,                                                    
MTDSP500=COALESCE(#Index.MTD,0)*100,                                                    
YTDSP500=COALESCE(#Index.YTD,0)*100,      
QTDSP500=COALESCE(#Index.QTD,0)*100                                                    
                                                    
                                                    
from #Index                                                    
                                                    
          
CREATE TABLE #PerformanceTable          
   (                                
 FundId INT,          
 InceptionDate DATETIME,          
 YearDate DATETIME,          
 QuarterDate DATETIME,          
 MonthDate DATETIME,          
 DailyDate DATETIME,          
 ITDPerformance FLOAT,          
 YTDPerformance FLOAT,          
 QTDPerformance FLOAT,          
 MTDPerformance FLOAT,          
 DailyPerformance FLOAT           
)  
                                                    
Insert into #PerformanceTable exec P_GetPerformanceCalculation @FundID,@EndDate
                                                      
----------------------------------------------WPSA-----------------------------------------------------------------------                                                       
------------------------------- Update WPSA Performance Numbers --------------------------------------------------------- 
         
update #Temp SET
ITDdateWPSA = PT.InceptionDate,
DTDdateWPSA = PT.DailyDate,
MTDdateWPSA = PT.MonthDate,
QTDdateWPSA = PT.QuarterDate,
YTDdateWPSA = PT.YearDate,
ITDWPSA = ISNULL(PT.ITDPerformance,0),
DTDWPSA = ISNULL(PT.DailyPerformance,0),
MTDWPSA = ISNULL(PT.MTDPerformance,0),
QTDWPSA = ISNULL(PT.QTDPerformance,0),
YTDWPSA = ISNULL(PT.YTDPerformance,0)
FROM #PerformanceTable PT  
WHERE PT.FundID = -1                                                                                                                                                               
                        
select * from #Temp                                 
                                      
Drop table #Temp                                                        
Drop table #Funds                                                      
Drop table #Index                                 
Drop TABLE #PerformanceTable                                 
                                                                        
                                           
End 