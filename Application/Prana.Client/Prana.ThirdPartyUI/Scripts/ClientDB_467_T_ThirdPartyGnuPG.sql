
/****** Object:  Table [dbo].[T_ThirdPartyGnuPG]    Script Date: 05/05/2013 20:20:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T_ThirdPartyGnuPG]') AND type in (N'U'))
DROP TABLE [dbo].[T_ThirdPartyGnuPG]
GO


/****** Object:  Table [dbo].[T_ThirdPartyGnuPG]    Script Date: 05/05/2013 20:20:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[T_ThirdPartyGnuPG](
	[GnuPGId] [int] IDENTITY(1,1) NOT NULL,
	[GnuPGName] [nvarchar](50) NOT NULL,
	[HomeDirectory] [nvarchar](255) NOT NULL,
	[Command] [int] NULL,
	[UseCmdBatch] [bit] NOT NULL,
	[UseCmdYes] [bit] NOT NULL,
	[UseCmdArmor] [bit] NOT NULL,
	[VerboseLevel] [int] NOT NULL,
	[Recipient] [nvarchar](255) NOT NULL,
	[Originator] [nvarchar](255) NOT NULL,
	[PassPhrase] [nvarchar](255) NULL,
	[PassPhraseDescriptor] [nvarchar](255) NULL,
	[Timeout] [int] NOT NULL,
	[Enabled] [bit] NOT NULL
 CONSTRAINT [PK_T_ThirdPartyGnuPG] PRIMARY KEY CLUSTERED 
(
	[GnuPGId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

