CREATE TABLE [dbo].[T_ThirdPartyFtp] (
    [FtpId]				INT            IDENTITY (1, 1) NOT NULL,
    [FtpName]			NVARCHAR (50)  NOT NULL,
    [Host]				NVARCHAR (254) NULL,
    [Port]				INT            NULL,
    [UsePassive]		BIT            NULL,
    [Encryption] [nvarchar](50) NULL,
    [UserName]			NVARCHAR (255)  NULL,
    [Password]			NVARCHAR (50)  NULL,
    [FtpType]			NVARCHAR (50)  NULL,
    [KeyFingerPrint]	NVARCHAR (255) NULL,
	[SshPrivateKeyPath] NVARCHAR (MAX) NULL,
    [PassPhrase]		NVARCHAR (255) NULL,
    [FtpFolderPath]		NVARCHAR (255) DEFAULT ('/') NULL,
    CONSTRAINT [PK_ThirdPartyFtp] PRIMARY KEY CLUSTERED ([FtpId] ASC)
);

