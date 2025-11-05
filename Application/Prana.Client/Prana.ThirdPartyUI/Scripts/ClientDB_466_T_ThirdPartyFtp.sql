
/****** Object:  Table [dbo].[T_ThirdPartyFtp]    Script Date: 05/03/2013 11:00:12 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T_ThirdPartyFtp]') AND type in (N'U'))
DROP TABLE [dbo].[T_ThirdPartyFtp]
GO

/****** Object:  Table [dbo].[T_ThirdPartyFtp]    Script Date: 05/03/2013 11:00:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[T_ThirdPartyFtp](
	[FtpId] [int] IDENTITY(1,1) NOT NULL,
	[FtpName] [nvarchar](50) NOT NULL,
	[Host] [nvarchar](254) NULL,
	[Port] [int] NULL,
	[UsePassive] [bit] NULL,
	[Encryption] [bit] NULL,
	[UserName] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NULL,
	[FtpType] [nvarchar](50) NULL,
	[KeyFile] [nvarchar](255) NULL,
	[PassPhrase] [nvarchar](255) NULL,
	[FtpFolderPath] [nvarchar](255) NULL
 CONSTRAINT [PK_ThirdPartyFtp] PRIMARY KEY CLUSTERED 
(
	[FtpId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


