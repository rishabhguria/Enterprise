  
-- =============================================  
-- Author:  <Alan Hau>  
-- Create date: <12/29/2010>  
-- Description: <Returns the Annualized risk-free-rate which is assummed to be .0023 for now>  
-- Sample :  Select dbo.F_getRiskFreeRate('12/10/2010', '12/10/2010', default)  
--    Select dbo.F_getRiskFreeRate('12/10/2010','12/10/2010',  0)  
-- =============================================  
CREATE FUNCTION [dbo].[F_getRiskFreeRate]  
(  
 @FromDate Datetime,  
 @ToDate Datetime,  
 @indexID int = 0  
   
)  
RETURNS float  
AS  
BEGIN  
  
Declare @result float  
  
select @result = .0023  
  
return @result  
  
END  
  