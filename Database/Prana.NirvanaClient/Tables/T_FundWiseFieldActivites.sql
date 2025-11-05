CREATE TABLE [dbo].[T_FundWiseFieldActivites](
	[FieldID] [int] NOT NULL,
	[FieldName] [varchar](100) NULL,
	[FundID] [int] NOT NULL,
	[DateValue] [datetime] NULL,
	[ActivityBase] [float] NULL
) ON [PRIMARY]