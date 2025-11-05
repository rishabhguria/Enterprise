Create TABLE [dbo].[PM_CollateralInterest]
(
    [CollateralInterestID] INT IDENTITY(1,1)  NOT NULL,
    [Date] DATETIME NOT NULL,
	[FundID] INT  NOT NULL, 
    [BenchmarkName] VARCHAR(50) NOT NULL, 
    [BenchmarkRate] DECIMAL NULL, 
    [Spread] INT NULL, 
	PRIMARY KEY CLUSTERED ([CollateralInterestID] ASC)
    
)
