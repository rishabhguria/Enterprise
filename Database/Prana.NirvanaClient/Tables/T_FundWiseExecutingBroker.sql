CREATE TABLE [dbo].[T_FundWiseExecutingBroker]
(
	[FundId] INT NOT NULL,
	[BrokerId] INT NOT NULL,
	[CompanyId] INT NOT NULL,
	CONSTRAINT [PK_T_FundWiseExecutingBroker] PRIMARY KEY CLUSTERED ([FundId] ASC)
)
