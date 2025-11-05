CREATE TABLE [dbo].[T_DA_Filters](
	[FilterId] [int] IDENTITY(1,1) NOT NULL,
	[Assets] [varchar](MAX) NULL,
	[Funds] [varchar](MAX) NULL,
	[Exchanges] [varchar](MAX) NULL,
	[AUECs] [varchar](MAX) NULL,
PRIMARY KEY CLUSTERED 
(
	[FilterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
