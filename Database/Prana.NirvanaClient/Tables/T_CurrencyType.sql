CREATE TABLE [dbo].[T_CurrencyType] (
    [CurrencyTypeID] INT          IDENTITY (1, 1) NOT NULL,
    [CurrencyType]   VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_CurrencyType] PRIMARY KEY CLUSTERED ([CurrencyTypeID] ASC)
);

