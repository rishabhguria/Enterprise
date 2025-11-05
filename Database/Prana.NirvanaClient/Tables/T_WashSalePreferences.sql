CREATE TABLE [dbo].[T_WashSalePreferences]
(
	[WashSaleStartDate]    DATETIME     NOT NULL,
	[FundID]               INT          DEFAULT ((0)) NOT NULL,
	CONSTRAINT [PK_T_WashSalePreferences] PRIMARY KEY CLUSTERED ([FundID] ASC)
)
