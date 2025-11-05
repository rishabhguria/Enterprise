CREATE TABLE [dbo].[T_ThirdPartyToleranceProfile](
	[ThirdPartyToleranceProfileId] [int] IDENTITY(1,1) NOT NULL,
	[ThirdPartyBatchId] [int] NOT NULL,
	[LastModified] [DATETIME] NOT NULL,
	[MatchingField] [int] NOT NULL,
	[AvgPrice] [float] NOT NULL,
	[Commission] [float] NOT NULL,
	[MiscFees] [float] NOT NULL,
	[NetMoney] [float] NOT NULL,
	CONSTRAINT [PK_T_ThirdPartyToleranceProfile] PRIMARY KEY ([ThirdPartyToleranceProfileId])
);
