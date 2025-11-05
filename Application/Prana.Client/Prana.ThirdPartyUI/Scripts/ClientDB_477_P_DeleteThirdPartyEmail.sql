/****** Object:  StoredProcedure [dbo].[P_DeleteThirdPartyEmail]    Script Date: 05/09/2013 14:10:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[P_DeleteThirdPartyEmail]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[P_DeleteThirdPartyEmail]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

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
