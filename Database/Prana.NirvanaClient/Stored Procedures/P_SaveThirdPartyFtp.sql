CREATE PROCEDURE [dbo].[P_SaveThirdPartyFtp]
(
	@FtpId				INT,
	@FtpName			VARCHAR(50),
	@Host				VARCHAR(255),
	@Port				INT,
	@UsePassive			BIT,
	@Encryption			VARCHAR(255),
	@UserName			VARCHAR(255),
	@Password			VARCHAR(255),
	@FtpType			VARCHAR(25),
	@KeyFingerPrint		VARCHAR(255),
	@SshPrivateKeyPath	NVARCHAR(MAX),
	@PassPhrase			VARCHAR(255),
	@FtpFolderPath		VARCHAR(255)
)

AS

DECLARE @result INT
IF @FtpId > 0
BEGIN
	UPDATE T_ThirdPartyFtp 
	SET 
		[Host]				= @Host,
		[FtpName]			= @FtpName,
		[Port]				= @Port,
		[UsePassive]		= @UsePassive,
		[Encryption]	    = @Encryption,
		[UserName]			= @UserName,
		[Password]			= @Password,
		[FtpType]			= @FtpType,
		[KeyFingerPrint]	= @KeyFingerPrint,
		[SshPrivateKeyPath]	= @SshPrivateKeyPath,
		[PassPhrase]		= @PassPhrase,	
		[ftpFolderPath]		= @ftpFolderPath	
	WHERE FtpId = @FtpId	
	
	SET @result = @FtpId
END
ELSE
BEGIN
	INSERT INTO T_ThirdPartyFtp 
	(
		 [Host]				
		,[FtpName]			
		,[Port]				
		,[UsePassive]		
		,[Encryption]			
		,[UserName]			
		,[Password]			
		,[FtpType]			
		,[KeyFingerPrint]	
		,[SshPrivateKeyPath]	
		,[PassPhrase]		
		,[ftpFolderPath]		
	)
	VALUES
	(
		@Host, 
		@FtpName, 
		@Port, 
		@UsePassive, 
		@Encryption, 
		@UserName, 
		@Password, 
		@FtpType, 
		@KeyFingerPrint,
		@SshPrivateKeyPath, 
		@PassPhrase,
		@ftpFolderPath
	)
	SET @result = @@Identity
END

SELECT @result