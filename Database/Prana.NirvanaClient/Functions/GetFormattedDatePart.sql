-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date, ,>  
-- Description: <Description, ,>  
-- =============================================  
CREATE FUNCTION GetFormattedDatePart  
(  
 @date datetime  
)  
RETURNS varchar(10)  
AS  
BEGIN  
 --declare @TodayDate varchar(10),@YesterdayDate varchar(10)     
 --set @TodayDate = CONVERT(VARCHAR(10), GETDATE(), 120)     
 RETURN CONVERT(VARCHAR(10), @date, 120)     
  
  
END  