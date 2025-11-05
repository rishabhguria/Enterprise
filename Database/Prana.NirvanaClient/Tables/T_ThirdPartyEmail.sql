CREATE TABLE [dbo].[T_ThirdPartyEmail] (
    [EmailId]   INT            IDENTITY (1, 1) NOT NULL,
    [EmailName] NVARCHAR (50)  NOT NULL,
    [MailFrom]  NVARCHAR (50)  NOT NULL,
    [MailTo]    NVARCHAR (255) NOT NULL,
    [CcTo]      NVARCHAR (255) NULL,
    [Smtp]      NVARCHAR (255) NOT NULL,
    [Port]      INT            NOT NULL,
    [UserName]  NVARCHAR (50)  NULL,
    [Password]  NVARCHAR (200)  NULL,
    [Enabled]   BIT            CONSTRAINT [DF_ThirdPartyEmail_Enabled] DEFAULT ((-1)) NULL,
    [Subject]   NVARCHAR (50)  NULL,
    [Body]      NVARCHAR (255) NULL,
    [Priority]  INT            NULL,
    [MailType]  INT            NULL,
	[BccTo] 	NVARCHAR(255) NULL,
    [SSLEnabled] BIT CONSTRAINT [DF_T_ThirdPartyEmail_SSLEnabled] DEFAULT ((-1)) NULL, 
    CONSTRAINT [PK_ThirdPartyEmail] PRIMARY KEY CLUSTERED ([EmailId] ASC)
);

