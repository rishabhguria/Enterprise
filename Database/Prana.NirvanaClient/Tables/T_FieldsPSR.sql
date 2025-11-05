CREATE TABLE [dbo].[T_FieldsPSR](
	[FieldID] [int] IDENTITY(1,1) NOT NULL,
	[FieldName] [varchar](100) NULL,
	[PositionID] [int] NOT NULL,
    [IncludeInNetIncome] [bit]  NULL
) ON [PRIMARY]