CREATE TABLE [dbo].[T_PMDataDump] (
	[Account] [nvarchar](100) NULL
	,[Symbol] [nvarchar](500) NULL
	,[Security Name] [nvarchar](1000) NULL
	,[Asset Class] [nvarchar](50) NULL
	,[NAV (Touch)] [nvarchar](50) NULL
	,[Position] [nvarchar](50) NULL
	,[Beta Adj. Exposure (Base)] [nvarchar](50) NULL
	,[Day P&L (Base)] [nvarchar](50) NULL
	,[Closing Mark] [nvarchar](50) NULL
	,[Px Last] [nvarchar](50) NULL
	,[Underlying Symbol] [nvarchar](200) NULL
	,[Underlying Price] [nvarchar](50) NULL
	,[Cost Basis P&L (Base)] [nvarchar](50) NULL
	,[Market Value (Base)] [nvarchar](50) NULL
	,[Net Exposure (Base)] [nvarchar](50) NULL
	,[Sector] [nvarchar](200) NULL
	,[Sub Sector] [nvarchar](200) NULL
	,[% Change] [nvarchar](50) NULL
	,[CreatedOn] [datetime] NULL
	,[Cost Basis] [nvarchar](50) NULL
	,[Country] [nvarchar](100) NULL
	,[Trade Currency] NVARCHAR(50) DEFAULT((0)) NOT NULL
	,[Delta Adj. Position] [nvarchar](100) NOT NULL DEFAULT((0))
	,[Px Selected Feed (Local)] [nvarchar](100) NOT NULL DEFAULT((0))
	,[Px Selected Feed (Base)] [nvarchar](100) NOT NULL DEFAULT((0))
	,[Cash Impact (Base)] NVARCHAR(100) NULL
	,[Earned Dividend (Base)] NVARCHAR(100) NULL
	,[Risk Currency] NVARCHAR(100) NULL
	,[Issuer] NVARCHAR(100) NULL
	,[Country Of Risk] NVARCHAR(100) NULL
	,[Region] NVARCHAR(100) NULL
	,[Analyst] NVARCHAR(100) NULL
	,[Market Cap] NVARCHAR(100) NULL
	,[Custom UDA1] NVARCHAR(100) NULL
	,[Custom UDA2] NVARCHAR(100) NULL
	,[Custom UDA3] NVARCHAR(100) NULL
	,[Custom UDA4] NVARCHAR(100) NULL
	,[Custom UDA5] NVARCHAR(100) NULL
	,[Custom UDA6] NVARCHAR(100) NULL
	,[Custom UDA7] NVARCHAR(100) NULL
    ,[User Asset] NVARCHAR(100) NULL 
    ,[Security Type] NVARCHAR(100) NULL
	,[Pricing Source] NVARCHAR(100) NULL, 
    [ISIN] NVARCHAR(50) NULL, 
    [BloombergSymbol] NVARCHAR(100) NULL, 
    [CUSIP] NVARCHAR(50) NULL, 
    [SEDOL] NVARCHAR(50) NULL, 
    [Multiplier] NVARCHAR(50) NULL, 
    [ContractType] NVARCHAR(50) NULL, 
    [FxRate] NVARCHAR(100) NULL,
	[Delta] [nvarchar](100) NULL,
	[Beta] [nvarchar](100) NULL,
	[ItmOtm] NVARCHAR (50) NULL,
	[PercentOfITMOTM] NVARCHAR (50) NULL,
	[IntrinsicValue] NVARCHAR (50) NULL,
	[DaysToExpiry] NVARCHAR (50) NULL,
	[GainLossIfExerciseAssign] NVARCHAR (50) NULL,
	[Gross Exposure (Local)] NVARCHAR (50) NULL,
    [Gross Exposure (Base)] NVARCHAR (50) NULL,
    [Net Exposure (Local)] NVARCHAR (50) NULL,
	[Exposure (Local)] NVARCHAR (50) NULL,
    [Exposure (Base)] NVARCHAR (50) NULL,
    [Beta Adj. Exposure (Local)] NVARCHAR (50) NULL,
    [Strategy] NVARCHAR(200) NULL,
    [Master Strategy] NVARCHAR(200) NULL,
    [Trade Attribute 1] NVARCHAR(200) NULL,
	[Trade Attribute 2] NVARCHAR(200) NULL,
	[Trade Attribute 3] NVARCHAR(200) NULL,
	[Trade Attribute 4] NVARCHAR(200) NULL,
	[Trade Attribute 5] NVARCHAR(200) NULL,
	[Trade Attribute 6] NVARCHAR(200) NULL,
    [Expiration Date] [datetime] NULL,
    [Expiration Month] NVARCHAR (50) NULL,
	[Custom UDA8] NVARCHAR(100) NULL,
	[Custom UDA9] NVARCHAR(100) NULL,
	[Custom UDA10] NVARCHAR(100) NULL,
	[Custom UDA11] NVARCHAR(100) NULL,
	[Custom UDA12] NVARCHAR(100) NULL,
	[BloombergSymbolWithExchangeCode] NVARCHAR(100) NULL,
	AdditionalTradeAttributes NVARCHAR(MAX) NULL
	);
GO

CREATE NONCLUSTERED INDEX [IDX_CreatedOn] ON [dbo].[T_PMDataDump] ([CreatedOn] ASC);
