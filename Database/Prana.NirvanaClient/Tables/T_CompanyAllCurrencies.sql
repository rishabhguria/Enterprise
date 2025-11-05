CREATE TABLE [dbo].[T_CompanyAllCurrencies] (
    [CompanyAllCurrencyID] INT IDENTITY (1, 1) NOT NULL,
    [CurrencyID]           INT NOT NULL,
    [CompanyID]            INT NOT NULL,
    CONSTRAINT [PK_T_CompanyAllCurrencies] PRIMARY KEY CLUSTERED ([CompanyAllCurrencyID] ASC)
);

