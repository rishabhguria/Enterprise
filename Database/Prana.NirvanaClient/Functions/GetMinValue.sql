-- =============================================
-- Author:		Rajat
-- Create date: 28 Oct 2007
-- Description:	Returns the min of the passed 2 values. It was created so that we can use it. 
--		e.g - From the case statement as it recognize function calling as expression
-- =============================================
CREATE FUNCTION [dbo].[GetMinValue] (
	-- Add the parameters for the function here
	@firstValue float,
	@secondValue float
)
RETURNS float
AS
BEGIN
	-- Declare the return variable here
	declare @minValue float

	if @firstValue < @secondValue
		set @minValue = @firstValue
	else if @firstValue > @secondValue
		set @minValue = @secondValue
	else 
		set @minValue = @secondValue -- In case of equal values, return the second value.

return @minValue

END
