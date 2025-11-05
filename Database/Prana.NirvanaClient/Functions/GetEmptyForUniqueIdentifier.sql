-- =============================================
-- Author:		Rajat Tandon
-- Create date: 16 Feb 2009
-- Description:	Returns the empty guid
-- =============================================
CREATE FUNCTION GetEmptyForUniqueIdentifier 
(
)
RETURNS UniqueIdentifier
AS
BEGIN
	-- Declare the return variable here
	DECLARE @emptyUniqueIdentifier uniqueidentifier
	set @emptyUniqueIdentifier = '00000000-0000-0000-0000-000000000000'
	
	-- Return the result of the function
	RETURN @emptyUniqueIdentifier

END
