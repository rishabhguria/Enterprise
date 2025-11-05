/*****************************************************************        
       
Author : Ankit             
Creation date: MAY 12 ,2014  
Description : Get NAV for the Start of The Day  
    
Execution Method:   
P_MW_GetStartOfDayNAV '2014/04/27','63','MTM_V0',1  
        
***********************************************************************/        
        
      
CREATE  proc P_MW_GetStartOfDayNAV  
(                
  @StartDate datetime,                          
  @fund varchar(max) ,      
  @ReportID Varchar(100),      
  @NAVfromPMorMW  int             
)                
as         
BEGIN                   
Declare @NAV float        
  
DECLARE @companyID int                        
SET @companyID = (select top 1 CompanyID from T_Company)                        
                        
DECLARE @auecID int                        
SET @auecID = (SELECT DefaultAUECID FROM T_Company WHERE CompanyID = @companyID)             
      
if(@NAVfromPMorMW = 1)        
 BEGIN      
  select * into #FundIDs            
  from dbo.Split(@fund, ',')          
          
  Select @StartDate =  CONVERT(VARCHAR(10),dbo.AdjustBusinessDays(@StartDate,-1,@auecID) ,110 )     
         
  select @NAV= SUM(NAVValue)  from            
  PM_NAVValue             
  where datediff(d,@StartDate,Date)=0              
  and             
  FundID in(select * from #FundIDs)         
 END      
      
ELSE      
 BEGIN      
  Declare @fundNames varchar(MAX)        
  select @StartDate = dbo.F_MW_GetWeekendAdjusted(@StartDate,0,1)        
  select @fundNames=COALESCE(@fundNames +',', '')+FundName from T_CompanyFunds where CompanyFundID in (select * from dbo.Split(@fund, ','))        
  exec P_MW_GetNAV_WithAccruedDividend @StartDate,@fundNames,@ReportID,@NAV output       
 END      
        
select @NAV as NAV                
END