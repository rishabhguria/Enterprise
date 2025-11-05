/****** Object:  Table [dbo].[T_ThirdPartyBatch]    Script Date: 05/10/2013 17:24:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T_ThirdPartyBatch]') AND type in (N'U'))
DROP TABLE [dbo].[T_ThirdPartyBatch]
GO


/****** Object:  Table [dbo].[T_ThirdPartyBatch]    Script Date: 05/10/2013 17:24:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[T_ThirdPartyBatch](
	[ThirdPartyBatchId] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
	[ThirdPartyTypeId] [int] NULL,
	[ThirdPartyId] [int] NULL,
	[ThirdPartyFormatId] [int] NULL,
	[IsLevel2Data] [bit] NULL,
	[Active] [bit] NULL,
	[ThirdPartyCompanyId] [int] NULL,
	[GnuPGId] [int] NULL,
	[FtpId] [int] NULL,
	[EmailDataId] [int] NULL,
	[EmailLogId] [int] NULL
 CONSTRAINT [PK_T_ThirdPartyBatch] PRIMARY KEY CLUSTERED 
(
	[ThirdPartyBatchId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


/****** Object:  Index [IX_T_ThirdPartyBatch]    Script Date: 05/10/2013 17:24:32 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_T_ThirdPartyBatch] ON [dbo].[T_ThirdPartyBatch] 
(
	[ThirdPartyFormatId] ASC,
	[ThirdPartyCompanyId] ASC,
	[ThirdPartyId] ASC,
	[ThirdPartyTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unique Index' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_ThirdPartyBatch', @level2type=N'INDEX',@level2name=N'IX_T_ThirdPartyBatch'
GO


