CREATE TABLE [dbo].[T_BTExchangeMapping] (
    [ExchangeMappingID]   INT           IDENTITY (1, 1) NOT NULL,
    [ExchangeIDList]      VARCHAR (50)  NOT NULL,
    [ExchangeMappingList] VARCHAR (100) NOT NULL,
    [TemplateID]          VARCHAR (200) NOT NULL,
    CONSTRAINT [PK_T_BTTemplateExchange] PRIMARY KEY CLUSTERED ([ExchangeMappingID] ASC),
    FOREIGN KEY ([TemplateID]) REFERENCES [dbo].[T_BTTemplateList] ([TemplateID])
);

