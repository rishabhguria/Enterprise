CREATE TABLE [dbo].[T_CompanyMarketDataProvider]
(
	[CompanyID] [INT] NOT NULL,
	[MarketDataProvider] [INT] NOT NULL,
	[SecondaryMarketDataProvider] [INT] NOT NULL DEFAULT 0,
	[IsMarketDataBlocked] [BIT] NOT NULL DEFAULT 1,
	[FactSetContractType] [INT] NOT NULL DEFAULT 0,
	CONSTRAINT [PK_T_CompanyMarketDataProvider] PRIMARY KEY CLUSTERED ([CompanyID] ASC),
	CONSTRAINT [FK_T_CompanyMarketDataProvider_T_Company] FOREIGN KEY([CompanyID])
		REFERENCES [dbo].[T_Company] ([CompanyID])
		ON UPDATE CASCADE
		ON DELETE CASCADE
);