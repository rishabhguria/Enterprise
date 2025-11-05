CREATE TABLE [dbo].[T_OTC_Templates](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](500) NOT NULL,
	[InstrumentType] [varchar](100) NOT NULL,
	[Description] [varchar](1000) NOT NULL,
	[UnderlyingAssetID] [int] NOT NULL,
	[ISDACounterParty] [int] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifieDate] [datetime] NULL,
	[ISDAContract] [ntext] NULL,
	[DaysToSettle] [int] NULL
 CONSTRAINT [PK_T_OTC_Templates] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
