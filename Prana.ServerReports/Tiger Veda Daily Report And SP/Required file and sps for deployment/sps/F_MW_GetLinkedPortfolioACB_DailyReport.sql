 /*=============================================          
 Author:  Ankit Misra          
 Create date: 23 June 2015          
 Description: <returns the monthly linked modified dietz return for given funds>          
 Sample:  Select dbo.F_MW_GetLinkedPortfolioACB_DailyReport('7/3/2015','7/3/2015','1308')        
 =============================================*/          
CREATE FUNCTION [dbo].[F_MW_GetLinkedPortfolioACB_DailyReport]          
(          
 @FromDate Datetime,            
 @ToDate Datetime,    
 @Fund Varchar(max)         
)          
-- Note that the Script is just Re-Using F_MW_GetPortfolioACB_DailyReport but joining the Correct Dates          
RETURNS float          
AS          
BEGIN     
    
--Declare @FromDate Datetime            
--Declare @ToDate Datetime    
--Declare @Fund Varchar(max)    
--    
--Set @FromDate = '7/3/2015'    
--Set @ToDate= '7/3/2015'     
--Set @Fund = '1308'--'1270,1271,1298,1302,1304,1305,1306,1307,1308,1309,1310'    
--          
Declare @PortfolioACB Float          
Declare @IsLongerThanMonth Bit          
          
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
          
insert into @Date           
Values (@FromDate, @LtoDate )          
END          
          
--After the left tail is taken care of, process the dates in the middle and right hand side, insert a new record to @Date for each complete month, insert the tail to the right          
IF @IsLongerThanMonth = 1          
Begin          
Declare @MfromDate Datetime          
Declare @MtoDate Datetime          
select @MfromDate = DATEADD(DD, 1, @LtoDate)          
select @MtoDate = DATEADD(DD,-1,DATEADD(mm, DATEDIFF(m,0,@MfromDate)+1,0))          
          
 While @MfromDate <= @toDate         
 Begin          
  If @MtoDate > @toDate          
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
       
Select @PortfolioACB = exp(sum(log(1 + dbo.F_MW_GetPortfolioACB_DailyReport(FromDate,ToDate,@Fund))))-1 From @Date   
--Select @PortfolioACB  
--         
Return @PortfolioACB        
          
END 