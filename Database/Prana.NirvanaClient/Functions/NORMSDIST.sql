  
-- =============================================  
-- Author:  <Alan Hau>  
-- Create date: <3/25/2011>  
-- Description: <this is equivalent to the NORMSDIST(x) function in Excel, which is the cumulative standard normal distribution funtion integrated from minus infinity to @X>  
-- Sample:  SELECT dbo.NORMSDIST(1.333333)  
--    SELECT dbo.NORMSDIST(-1.333333)  
-- =============================================  
CREATE FUNCTION [dbo].[NORMSDIST]  
(  
 -- Add the parameters for the function here  
 @x float  
)  
RETURNS float  
AS  
BEGIN  
 -- Declare the return variable here  
 DECLARE @Result float  
  
 -- Add the T-SQL statements to compute the return value here  
   
   
 if @x < 0    
 begin  
 Select @result = 1.00 - dbo.NORMSDIST(-1.00 * @x)  
 end  
   
 else  
 begin  
   
         Declare @Zx float  
  
            Declare @p float   
            Declare @t float   
            Declare @b1 float   
            Declare @b2 float   
            Declare @b3 float   
            Declare @b4 float   
            Declare @b5 float   
   
 select   
 @Zx = (1.00 / (Sqrt(2.00 * PI())) * Exp(-1.00* power(@x , 2.00) / 2.00)) ,  
 @p = 0.2316419,  
 @t = 1.00 / (1.00 + 0.2316419 * @x),  
 @b1 = 0.31938153,  
 @b2 = -0.356563782,  
 @b3 = 1.781477937,  
 @b4 = -1.821255978,  
 @b5 = 1.330274429  
   
   
   
   
 Select @result = 1 - @Zx * (@b1 * @t + @b2 * power(@t , 2) + @b3 * power(@t , 3) + @b4 * power(@t , 4) + @b5 * power(@t , 5))  
   
 end  
   
   
   
   
  
 -- Return the result of the function  
 RETURN @result  
  
END  
  