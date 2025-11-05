-- =============================================  
-- Author:        <Alan Hau>  
-- Create date: <3/25/2011>  
-- Description:    <This is equivalent to the dOne function used in the Black-Scholes model>  
-- sample:        select dbo.F_BS_dOne(214.08, 165.00, 19.00, .0023, .2378762, .23)  
-- =============================================  
CREATE FUNCTION [dbo].[F_BS_dOne] (
	-- Add the parameters for the function here  
	@UnderlyingPrice FLOAT
	,@ExercisePrice FLOAT
	,@time FLOAT
	,@Interest FLOAT
	,@Volatility FLOAT
	,@Dividend FLOAT
	)
RETURNS FLOAT
AS
BEGIN
	-- Declare the return variable here  
	DECLARE @Result FLOAT

	IF (@Volatility <> 0)
		-- Add the T-SQL statements to compute the return value here  
		SELECT @Result = (Log(@UnderlyingPrice / @ExercisePrice) + (@Interest - @Dividend + 0.5 * power(@Volatility, 2)) * @time) / (@Volatility * (Sqrt(@time)))

	-- Return the result of the function  
	RETURN @Result
END