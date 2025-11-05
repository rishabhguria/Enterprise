CREATE TABLE [dbo].[T_ThirdPartyAllocationMessages]
(
	[MsgId] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[BlockId] INT NOT NULL,
	[IndividualAllocID]  NVARCHAR(50) NULL, 
	[AllocAccount] VARCHAR(100) NULL,
	[AllocQty]  FLOAT (53)    NULL,
	[AllocAvgPx]  FLOAT (53)    NULL,
	[Commission]  FLOAT (53)    NULL,
	[Misc Fees]  FLOAT (53)    NULL,
	[NetMoney]  FLOAT (53)    NULL,
	[MatchStatus] VARCHAR(MAX) NULL,
	[AllocText] VARCHAR(MAX) NULL,
	[TradeDate] DATETIME NULL,
	[ConfirmID] VARCHAR(MAX) NULL,
	[ConfirmRefID] VARCHAR(MAX) NULL,
	[ConfirmTransType] VARCHAR(1) NULL,
	[ConfirmType] VARCHAR(1) NULL,
	[ConfirmStatus] VARCHAR(1) NULL,
	[AffirmStatus] VARCHAR(1) NULL,
	[ConfirmRejReason] VARCHAR(1) NULL,
	CONSTRAINT [FK_BlockId_T_ThirdPartyAllocationMessages] FOREIGN KEY ([BlockId]) REFERENCES [dbo].[T_ThirdPartyAllocationBlocks] ([BlockId])
)
