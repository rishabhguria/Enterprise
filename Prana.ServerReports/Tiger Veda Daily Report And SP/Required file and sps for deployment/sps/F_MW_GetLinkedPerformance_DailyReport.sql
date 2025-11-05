 /*=============================================              
 Author:  Ankit Misra              
 Create date: 23 June 2015              
 Description: <returns the monthly linked modified dietz return for given funds>              
 Sample:  Select dbo.F_MW_GetLinkedPortfolioACB_DailyReport('7/3/2015','7/3/2015','1308')            
 =============================================*/              
ALTER FUNCTION [dbo].[F_MW_GetLinkedPerformance_DailyReport]              
(              
 @FromDate Datetime,                
 @ToDate Datetime,        
 @Fund Varchar(max),  
 @BloombergSymbol Varchar(100),  
 @PreviousMonthsReturn float             
)              
-- Note that the Script is just Re-Using F_MW_GetPortfolioACB_DailyReport but joining the Correct Dates              
RETURNS float              
AS              
BEGIN         
        
--Declare @FromDate Datetime                
--Declare @ToDate Datetime        
--Declare @Fund Varchar(max)  
--Declare @BloombergSymbol Varchar(100)    
--Declare @PreviousMonthsReturn float      
--        
--Set @FromDate = '1/1/2015'        
--Set @ToDate= '8/15/2015'           
--Set @Fund = '1286,1288,1290,1306'--'1270,1271,1298,1302,1304,1305,1306,1307,1308,1309,1310'    
--Set @BloombergSymbol = 'PortFolio'    
--Set @PreviousMonthsReturn = 
--              
Declare @MDreturn Float              
Declare @IsLongerThanMonth Bit   
  
Select @MDreturn= @PreviousMonthsReturn            
              
-- given the input dates, determine the list of dates which we will store in @Date that needs to be run from and to:              
Declare @Date Table             
(              
FromDate Datetime,              
ToDate Datetime              
)              
              
IF @FromDate <> DATEADD(DD, 1 - DAY(@FromDate), @FromDate)              
Declare @LtoDate Datetime              
BEGIN              
 if DATEADD(DD,-1,DATEADD(mm, DATEDIFF(m,0,@fromDate)+1,0)) >= @ToDate              
 Begin              
  Select @LtoDate = @ToDate            
  select @IsLongerThanMonth = 0              
 End              
 Else              
 Begin              
  Select @LtoDate = DATEADD(DD,-1,DATEADD(mm, DATEDIFF(m,0,@FromDate)+1,0))              
  select @IsLongerThanMonth = 1              
 End              
              
Insert into @Date               
Values (@FromDate, @LtoDate )              
END              
              
--After the left tail is taken care of, process the dates in the middle and right hand side, insert a new record to @Date for each complete month, insert the tail to the right              
IF @IsLongerThanMonth = 1              
Begin              
Declare @MfromDate Datetime              
Declare @MtoDate Datetime              
select @MfromDate = DATEADD(DD, 1, @LtoDate)              
select @MtoDate = DATEADD(DD,-1,DATEADD(mm, DATEDIFF(m,0,@MfromDate)+1,0))              
              
While @MfromDate <= @ToDate             
 Begin              
  If @MtoDate > @ToDate              
  Begin              
   Select @MtoDate = @toDate              
   Insert into @Date               
   Values (@MfromDate, @MtoDate )              
   Break              
  End              
 Insert into @Date               
 Values (@MfromDate, @MtoDate )              
 Select @MfromDate = DATEADD(DD, 1, @MtoDate)              
 Select @MtoDate = DATEADD(DD,-1,DATEADD(mm, DATEDIFF(m,0,@MfromDate)+1,0))              
 End             
End  

--Select * from @Date
                  
Select @MDreturn =@MDreturn+ SUM(LOG(1 + dbo.F_MW_GetPerformance_DailyReport(FromDate,ToDate,@Fund,@BloombergSymbol))) From @Date     
----@MDreturn needs to be EXP(@MDreturn)-1 to get correct result      
--Select ISNULL((EXP(@MDreturn)-1),0)     
             
Return (EXP(@MDreturn)-1)           
              
END 