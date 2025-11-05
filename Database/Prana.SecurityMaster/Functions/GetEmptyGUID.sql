
-- Author		: Rajat
-- Description	: Returns a empty GUID

CREATE FUNCTION dbo.GetEmptyGUID()
 RETURNS uniqueidentifier
 AS
 BEGIN
       DECLARE @EmptyGUID uniqueidentifier
      
       select @EmptyGUID = cast(cast(0 as binary) as uniqueidentifier)

       RETURN(@EmptyGUID)
 END