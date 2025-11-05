
CREATE PROCEDURE [dbo].[P_SaveThirdPartyGnuPG]
(
	@GnuPGId int,
	@GnuPGName varchar(50),
	@HomeDirectory varchar(255),
	@Command int,
	@UseCmdBatch bit,
	@UseCmdYes bit,
	@UseCmdArmor bit,
	@VerboseLevel int,
	@Recipient varchar(255),
	@Originator varchar(255),
	@PassPhrase varchar(255),
	@PassPhraseDescriptor varchar(255),
	@Timeout int,	
	@Enabled bit
	,@ExtensionToAdd varchar(20)
	
)
AS

Declare @result int

if @GnuPGId > 0
begin
	Update T_ThirdPartyGnuPG
	Set
		HomeDirectory			= @HomeDirectory,
		GnuPGName				= @GnuPGName,
		[Command]				= @Command,
		UseCmdBatch				= @UseCmdBatch,
		UseCmdYes				= @UseCmdYes,
		UseCmdArmor				= @UseCmdArmor,
		VerboseLevel			= @VerboseLevel,
		Recipient				= @Recipient,
		Originator				= @Originator,
		PassPhrase				= @PassPhrase,
		PassPhraseDescriptor	= @PassPhraseDescriptor,
		[Timeout]				= @Timeout,		
		[Enabled]				= @Enabled
		,ExtensionToAdd			= @ExtensionToAdd		
	where GnuPGId = @GnuPGId
		
	Set @result = @GnuPGid
end
else
begin
	Insert into T_ThirdPartyGnuPG
	(
		GnuPGName, 
		HomeDirectory,
		UseCmdBatch,
		UseCmdYes,
		UseCmdArmor,
		VerboseLevel,
		Recipient, 
		Originator, 
		PassPhrase,
		PassPhraseDescriptor,
		[Timeout],
		[Command], 
		[Enabled]
		,ExtensionToAdd
	)
	Values
	(
		@GnuPGName,
		@HomeDirectory,
		@UseCmdBatch,
		@UseCmdYes,
		@UseCmdArmor,
		@VerboseLevel,
		@Recipient, 
		@Originator, 
		@PassPhrase,
		@PassPhraseDescriptor,
		@Timeout,
		@Command, 
		@Enabled
		,@ExtensionToAdd
	)
	
	Set @result = @@Identity	
end

Select @result
