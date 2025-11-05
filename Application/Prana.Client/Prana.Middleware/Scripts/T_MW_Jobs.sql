/****** Object:  Table [dbo].[T_MW_Jobs]    Script Date: 06/13/2013 11:04:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_MW_Jobs](
	[JobName] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](50) NULL,
	[Enabled] [bit] NOT NULL CONSTRAINT [DF_T_MW_Jobs_Enabled]  DEFAULT ((-1)),
	[GroupId] [int] NOT NULL CONSTRAINT [DF_T_MW_Jobs_GroupId]  DEFAULT ((1)),
 CONSTRAINT [PK_T_MW_Jobs] PRIMARY KEY CLUSTERED 
(
	[JobName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]