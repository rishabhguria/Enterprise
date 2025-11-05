/****** Object:  Table [dbo].[T_ThirdPartyBatch]    Script Date: 05/10/2013 17:06:41 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[P_SaveThirdPartyBatch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[P_SaveThirdPartyBatch]
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[P_SaveThirdPartyBatch]
(
	@ThirdPartyBatchId nvarchar(50),
	@Description nvarchar(50),
	@ThirdPartyTypeId int,
	@ThirdPartyId int,
	@ThirdPartyFormatId int,
	@IsLevel2Data bit,
	@Active bit ,
	@ThirdPartyCompanyId int,
	@GnuPGId int,
	@FtpId int,
	@EmailDataId int,
	@EmailLogId int
)
AS

Declare @result int

if @ThirdPartyBatchId > 0
begin
	Update T_ThirdPartyBatch Set		
		[Description]		= @Description,
		ThirdPartyTypeId	= @ThirdPartyTypeId,
		ThirdPartyId		= @ThirdPartyId,
		ThirdPartyFormatId	= @ThirdPartyFormatId,
		IsLevel2Data		= @IsLevel2Data,
		Active				= @Active,
		ThirdPartyCompanyId	= @ThirdPartyCompanyId,
		GnuPGId				= @GnuPGId,
		FtpId				= @FtpId,	
		EmailDataId			= @EmailDataId,		
		EmailLogId			= @EmailLogId
		
	where ThirdPartyBatchId = @ThirdPartyBatchid

	Set @result = @ThirdPartyBatchId
	
end
else
begin
	Insert into T_ThirdPartyBatch
	(		
		[Description],
		ThirdPartyTypeId,
		ThirdPartyId,
		ThirdPartyFormatId,
		IsLevel2Data,
		Active,
		ThirdPartyCompanyId,
		GnuPGId,
		FtpId,
		EmailDataId,
		EMailLogId
	)
	Values
	(		
		@Description,
		@ThirdPartyTypeId,
		@ThirdPartyId,
		@ThirdPartyFormatId,
		@IsLevel2Data,
		@Active,
		@ThirdPartyCompanyId,
		@GnuPGId,
		@FtpId,
		@EmailDataId,
		@EmailLogId		
	)
	Set @result = @@Identity
end

select @result