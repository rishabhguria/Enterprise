CREATE TABLE [dbo].[T_TempSecMasterData] (
    [ID]               INT           IDENTITY (1, 1) NOT NULL,
    [Asset]            VARCHAR (200) NULL,
    [Symbol]           VARCHAR (100) NULL,
    [UnderlyingSymbol] VARCHAR (200) NULL,
    [CUSIP]            VARCHAR (200) NULL,
    [CompanyName]      VARCHAR (200) NULL,
    [OSISymbol]        VARCHAR (200) NULL,
    [Exchange]         VARCHAR (100) NULL
);

