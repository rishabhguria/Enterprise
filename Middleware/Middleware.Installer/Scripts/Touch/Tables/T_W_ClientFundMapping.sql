/****** Object:  Table [dbo].[T_W_ClientFundMapping]    Script Date: 02/27/2014 10:04:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_W_ClientFundMapping](
	[MappingID] [bigint] IDENTITY(1,1) NOT NULL,
	[ClientID] [int] NOT NULL,
	[TouchFundID] [int] NOT NULL,
 CONSTRAINT [PK_T_W_ClientFundMapping] PRIMARY KEY CLUSTERED 
(
	[MappingID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO