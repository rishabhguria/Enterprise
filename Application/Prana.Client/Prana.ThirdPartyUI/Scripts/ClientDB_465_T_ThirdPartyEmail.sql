
/****** Object:  Table [dbo].[T_ThirdPartyEmail]    Script Date: 05/10/2013 17:24:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T_ThirdPartyEmail]') AND type in (N'U'))
DROP TABLE [dbo].[T_ThirdPartyEmail]
GO

/****** Object:  Table [dbo].[T_ThirdPartyEmail]    Script Date: 05/17/2013 11:13:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[T_ThirdPartyEmail](
	[EmailId] [int] IDENTITY(1,1) NOT NULL,
	[EmailName] [nvarchar](50) NOT NULL,
	[MailFrom] [nvarchar](50) NOT NULL,
	[MailTo] [nvarchar](255) NOT NULL,
	[CcTo] [nvarchar](255) NULL,
	[Smtp] [nvarchar](255) NOT NULL,
	[Port] [int] NOT NULL,
	[UserName] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NULL,
	[Enabled] [bit] NULL,
	[Subject] [nvarchar](50) NULL,
	[Body] [nvarchar](255) NULL,
	[Priority] [int] NULL,
	[MailType] [int] NULL,
 CONSTRAINT [PK_ThirdPartyEmail] PRIMARY KEY CLUSTERED 
(
	[EmailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[T_ThirdPartyEmail] ADD  CONSTRAINT [DF_ThirdPartyEmail_Enabled]  DEFAULT ((-1)) FOR [Enabled]
GO

