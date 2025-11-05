CREATE PROCEDURE [dbo].[P_GetAllocIDFromBlockID]
	@BlockID INT
AS
BEGIN
	SELECT AllocID FROM T_ThirdPartyAllocationBlocks WHERE BlockId = @BlockID
END
