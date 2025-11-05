
CREATE Procedure [dbo].[P_SaveThirdPartyBatch]
(
	@ThirdPartyBatchId nvarchar(50),
	@Description nvarchar(50),
	@ThirdPartyTypeId int,
	@ThirdPartyId int,
	@ThirdPartyFormatId int,
	@IsLevel2Data bit,
	@Active bit ,
    @IncludeSent bit,
    @AllowedFixTransmission bit,
	@ThirdPartyCompanyId int,
	@GnuPGId int,
	@FtpId int,
	@EmailDataId int,
	@EmailLogId int
)
AS

Declare @result int


If (@ThirdPartyCompanyId=0 And @ThirdPartyTypeId In (3,4))
Begin
Set @ThirdPartyCompanyId = @ThirdPartyId
End

If (@ThirdPartyCompanyId=0 And @ThirdPartyTypeId In (1) )
Begin
Set @ThirdPartyCompanyId = (Select CompanyThirdPartyID from T_CompanyThirdParty
Where ThirdPartyID = @ThirdPartyId)
End

Declare @Batchcount int
Select @Batchcount=count(*) from T_ThirdPartyBatch where ThirdPartyBatchId=@ThirdPartyBatchId

if (@ThirdPartyBatchId > 0 AND @Batchcount>0)
begin
	Update T_ThirdPartyBatch Set		
		[Description]		= @Description,
		ThirdPartyTypeId	= @ThirdPartyTypeId,
		ThirdPartyId		= @ThirdPartyId,
		ThirdPartyFormatId	= @ThirdPartyFormatId,
		IsLevel2Data		= @IsLevel2Data,
		Active				= @Active,
        IncludeSent = @IncludeSent,
		AllowedFixTransmission = @AllowedFixTransmission,
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
        IncludeSent, 
		AllowedFixTransmission,
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
        @IncludeSent,  
		@AllowedFixTransmission,
		@ThirdPartyCompanyId,
		@GnuPGId,
		@FtpId,
		@EmailDataId,
		@EmailLogId
	)
	Set @result = @@Identity
end

select @result


