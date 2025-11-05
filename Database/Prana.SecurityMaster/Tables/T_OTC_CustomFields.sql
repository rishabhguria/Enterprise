
CREATE TABLE [dbo].[T_OTC_CustomFields](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[InstrumentType] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[DefaultValue] [sql_variant] NOT NULL,
	[DataType] [varchar](50) NOT NULL,
    [UIOrder] INT NULL DEFAULT 99, 
    CONSTRAINT [PK_T_OTC_CustomFields] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]