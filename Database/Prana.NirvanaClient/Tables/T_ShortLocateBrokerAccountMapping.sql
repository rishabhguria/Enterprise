CREATE TABLE [dbo].[T_ShortLocateBrokerAccountMapping]
(
	[MappingID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[BrokerID] INT NOT NULL ,
	[AccountID] INT NOT NULL
)
