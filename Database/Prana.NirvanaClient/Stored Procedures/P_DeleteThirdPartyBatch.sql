
CREATE Procedure [dbo].[P_DeleteThirdPartyBatch]
(
	@ThirdPartyBatchId int
)

AS
begin
Delete from T_ThirdPartyBatch where ThirdPartyBatchId = @ThirdPartyBatchId	
select @ThirdPartyBatchId
end

