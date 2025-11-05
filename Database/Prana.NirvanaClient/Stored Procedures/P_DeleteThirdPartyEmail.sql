
create Procedure [dbo].[P_DeleteThirdPartyEmail]
(
	@EmailId int	
)

AS
begin

	Declare @InUse int
	Select @InUse = Count(*) from T_ThirdPartyBatch Where EmailLogId = @EmailId or EmailDataId = @EmailId

	if @InUse = 0
	begin
		Delete from T_ThirdPartyEmail where EmailId = @EmailId
	end

	select -@InUse
end
