/****** Object:  StoredProcedure [dbo].[P_DeleteThirdPartyBatch]    Script Date: 05/09/2013 14:10:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[P_DeleteThirdPartyBatch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[P_DeleteThirdPartyBatch]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[P_DeleteThirdPartyBatch]
(
	@ThirdPartyBatchId int
)

AS
begin
Delete from T_ThirdPartyBatch where ThirdPartyBatchId = @ThirdPartyBatchId	
select @ThirdPartyBatchId
end

