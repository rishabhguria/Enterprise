--USE [QANirvanaClient]
GO
/****** Object:  Table [dbo].[T_ReportGroupRowColors_New]    Script Date: 08/19/2015 11:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ReportGroupRowColors_New](
	[PKID] [int] NOT NULL,
	[Group1BGColor] [nvarchar](50) NULL,
	[Group2BGColor] [nvarchar](50) NULL,
	[Group3BGColor] [nvarchar](50) NULL,
	[Group4BGColor] [nvarchar](50) NULL,
	[DetailRowBGColor] [nvarchar](50) NULL,
 CONSTRAINT [PK_T_ReportGroupRowColors_New] PRIMARY KEY CLUSTERED 
(
	[PKID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
