CREATE TABLE [dbo].[T_Currency] (
    [CurrencyID]     INT           IDENTITY (1, 1) NOT NULL,
    [CurrencyName]   NVARCHAR (50) NOT NULL,
    [CurrencySymbol] NVARCHAR (4)  NOT NULL,
    CONSTRAINT [PK_T_Currency] PRIMARY KEY CLUSTERED ([CurrencyID] ASC)
);

