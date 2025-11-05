-- =============================================  
-- Author:  Pooja
-- Create date: <5/06/2015>  
-- Description: <Modify the dates>  
-- Sample:  select dbo.F_MW_GetLinkedMDReturnNOF('7/1/2010','7/2/2011')    
--    select fromdate from  dbo.F_MW_GetLinkedMDReturnNOFWPS('7/1/2010','7/2/2010')     
-- =============================================  
create FUNCTION [dbo].[F_MW_DatesWPS]   
(  
 @fromDate datetime,    
 @toDate datetime  
)  
-- note that the script is just re-using F_MW_GetMDReturn but joining the correct dates  
RETURNS @D table (  
fromdate Datetime,  
toDate datetime  
)  
AS
BEGIN

declare @isLongerThanMonth bit  
  
-- given the input dates, determine the list of dates which we will store in @D that needs to be run from and to:  
  
if @fromDate <> DATEADD(DD, 1 - DAY(@fromDate), @fromDate)  
Declare @LtoDate Datetime  
BEGIN  
 if DATEADD(DD,-1,DATEADD(mm, DATEDIFF(m,0,@fromDate)+1,0)) >= @toDate  
 Begin  
  Select @LtoDate = @toDate  
  select @isLongerThanMonth = 0  
 End  
 Else  
 Begin  
  Select @LtoDate = DATEADD(DD,-1,DATEADD(mm, DATEDIFF(m,0,@fromDate)+1,0))  
  select @isLongerThanMonth = 1  
 End  
  
insert into @D   
Values (@fromDate, @LtoDate )  
END  
  
--After the left tail is taken care of, process the dates in the middle and right hand side, insert a new record to @D for each complete month, insert the tail to the right  
if @isLongerThanMonth = 1  
Begin  
Declare @MfromDate Datetime  
Declare @MtoDate Datetime  
select @MfromDate = DATEADD(DD, 1, @LtoDate)  
select @MtoDate = DATEADD(DD,-1,DATEADD(mm, DATEDIFF(m,0,@MfromDate)+1,0))  
  
 While @MfromDate <= @toDate   
  
 begin  
  if @MtoDate > @toDate  
  Begin  
   Select @MtoDate = @toDate  
   insert into @D   
   Values (@MfromDate, @MtoDate )  
   Break  
  End  
 insert into @D   
 Values (@MfromDate, @MtoDate )  
 select @MfromDate = DATEADD(DD, 1, @MtoDate)  
 select @MtoDate = DATEADD(DD,-1,DATEADD(mm, DATEDIFF(m,0,@MfromDate)+1,0))  
 End  
  
  
End  
  
--select @result = exp(sum(log(1 + dbo.F_MW_GetMDReturnNOF_WPS(fromdate,toDate, @groupfield, @Entity))))-1 from @D  
  
return  
END  
  
  