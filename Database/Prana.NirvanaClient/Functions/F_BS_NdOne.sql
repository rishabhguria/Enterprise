  
-- =============================================  
-- Author:  <Alan Hau>  
-- Create date: <3/25/2011>  
-- Description: <This is equivalent to the NdOne function used in the Black-Scholes model, which is also same as the standard Normal Distribution function, but you replace @x with the contents of dOne>  
-- sample:  select dbo.F_BS_NdOne(214.08, 165.00, 19.00, .0023, .2378762, .23)  
-- =============================================  
CREATE FUNCTION [dbo].[F_BS_NdOne]  
(  
 -- Add the parameters for the function here  
 @UnderlyingPrice float,  
 @ExercisePrice float,  
 @time float,  
 @Interest float,  
 @Volatility float,  
 @Dividend float  
)  
RETURNS float  
AS  
BEGIN  
 -- Declare the return variable here  
 DECLARE @Result float  
  
 -- Add the T-SQL statements to compute the return value here  
    SELECT @Result = Exp(-(power(dbo.F_BS_dOne(@UnderlyingPrice, @ExercisePrice, @time, @Interest, @Volatility, @Dividend) , 2) / 2.00)) / (Sqrt(2.00 * 3.14159265358979))  
  
  
 -- Return the result of the function  
 RETURN @Result  
  
END  
  