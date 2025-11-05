CREATE PROCEDURE [dbo].[P_GetThirdPartyBatchEventData] @blockID INT
AS
SELECT FM.TransmissionTime
	,FM.Description
	,FM.Direction
	,FM.AllocationID
	,FM.FIXMsg
FROM T_ThirdPartyAllocationBlocks AB
INNER JOIN T_ThirdPartyJobFixMessages FM ON AB.BlockId = FM.BlockId
WHERE AB.BlockId = @blockID