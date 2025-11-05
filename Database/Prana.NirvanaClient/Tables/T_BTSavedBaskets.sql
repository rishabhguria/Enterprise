CREATE TABLE [dbo].[T_BTSavedBaskets] (
    [SavedBasketID]         VARCHAR (50)  NOT NULL,
    [SavedBasketName]       VARCHAR (50)  NOT NULL,
    [TemplateID]            VARCHAR (200) NOT NULL,
    [SharedTradingAccounts] VARCHAR (20)  NULL,
    [Exchanges]             VARCHAR (50)  NULL,
    [AssetID]               INT           NOT NULL,
    [UnderLyingID]          INT           NOT NULL,
    [UserID]                INT           NOT NULL,
    [BasketColumns]         NCHAR (500)   NULL,
    [BenchMark]             FLOAT (53)    NULL,
    [HasWaves]              VARCHAR (5)   NULL,
    CONSTRAINT [PK_T_SavedBasket] PRIMARY KEY CLUSTERED ([SavedBasketID] ASC),
    FOREIGN KEY ([TemplateID]) REFERENCES [dbo].[T_BTTemplateList] ([TemplateID])
);

