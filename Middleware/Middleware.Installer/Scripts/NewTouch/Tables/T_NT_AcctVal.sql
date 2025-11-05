
GO
CREATE TABLE [dbo].[T_NT_AcctVal](
	[AcctId] [int] NOT NULL,
	[UpdateDate] [smalldatetime] NOT NULL,
	[Val_TradingLevel] [decimal](38, 8) NOT NULL,
	[Val_InvestedCash] [decimal](38, 8) NOT NULL,
	[Approved] [bit] NULL,
 CONSTRAINT [PK_T_NT_AcctVal] PRIMARY KEY CLUSTERED 
(
	[AcctId] ASC,
	[UpdateDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO