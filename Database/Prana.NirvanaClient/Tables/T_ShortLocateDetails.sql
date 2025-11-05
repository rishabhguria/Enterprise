CREATE TABLE [dbo].[T_ShortLocateDetails]
(
	[NirvanaLocateID] INT NOT NULL PRIMARY KEY IDENTITY (1,1),
	[BorrowerId] VARCHAR(20) NOT NULL, 
    [ClientMasterfundId] INT NOT NULL, 
    [BrokerId] INT NOT NULL, 
	[Ticker] VARCHAR(20) NOT NULL, 
	[BorrowSharesAvailable]  INT NOT NULL, 
	[BorrowRate]  float NOT NULL, 
	[BorrowedShare]  INT NOT NULL, 
	[BorrowedRate]  float NOT NULL, 
	[SODBorrowShareAvailable]  INT NOT NULL,
	[SODBorrowRate]  float NOT NULL, 
	[StatusSource] VARCHAR(20) NOT NULL, 
	[SLImportDate] Date NOT NULL
)
