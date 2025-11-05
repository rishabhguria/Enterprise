CREATE TABLE [dbo].[T_Volatility] (
    [SecurityID]       INT           IDENTITY (1, 1) NOT NULL,
    [SecurityName]     VARCHAR (10)  NOT NULL,
    [CallPut]          VARCHAR (5)   NULL,
    [StrikePrice]      FLOAT (53)    NULL,
    [UnderlyingSymbol] VARCHAR (100) NULL,
    [AutoVolatility]   FLOAT (53)    NULL,
    [UserVolatility]   FLOAT (53)    NULL,
    [UserID]           INT           NOT NULL,
    CONSTRAINT [PK_T_Volatility] PRIMARY KEY CLUSTERED ([SecurityID] ASC, [SecurityName] ASC, [UserID] ASC)
);

