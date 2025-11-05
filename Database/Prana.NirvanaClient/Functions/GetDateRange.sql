
/*  
  
Author : Rajat  
Created: 02 Nov 2007  
Usage   
declare @date datetime  
set @date = getutcdate()  
select * from dbo.GetDateRange(GetUTCDate(),NULL) 
  
*/  
CREATE FUNCTION [dbo].[GetDateRange] (  
 @startDate datetime,   
 @endDate datetime  
)  
RETURNS @dateRange TABLE (Items datetime) 
AS  
  
    BEGIN  
  
    DECLARE @tempDate datetime  
  
 set @tempDate = @startDate  
 If @endDate is NULL   
 BEGIN  
  Set @endDate = @startDate  
 END  
    WHILE dbo.GetFormattedDatePart(@tempDate) <= dbo.GetFormattedDatePart(@endDate)  
  BEGIN   
         INSERT INTO @dateRange(Items) VALUES(@tempDate)  
   SET @tempDate = dateadd(dd,1,@tempDate)  
     END  
  
    RETURN   
END
