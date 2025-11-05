CREATE TABLE [dbo].[T_Currency] (
    [CurrencyID]     INT           NOT NULL,
    [CurrencyName]   NVARCHAR (50) NOT NULL,
    [CurrencySymbol] NVARCHAR (4)  NOT NULL,
    CONSTRAINT [PK__T_Currency__114A936A] PRIMARY KEY CLUSTERED ([CurrencyID] ASC) WITH (FILLFACTOR = 100)
);

