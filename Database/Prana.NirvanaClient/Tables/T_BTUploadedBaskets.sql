CREATE TABLE [dbo].[T_BTUploadedBaskets] (
    [BasketID]           VARCHAR (50)  NOT NULL,
    [UpLoadedBasketID]   VARCHAR (50)  NOT NULL,
    [HasWaves]           VARCHAR (5)   NOT NULL,
    [TradedBasketID]     VARCHAR (50)  NULL,
    [UserID]             INT           NOT NULL,
    [ISSaved]            VARCHAR (5)   NOT NULL,
    [UploadedBasketName] VARCHAR (20)  NOT NULL,
    [TimeOfSave]         DATETIME      NULL,
    [TemplateID]         VARCHAR (200) NULL,
    [AssetID]            INT           NULL,
    [UnderLyingID]       INT           NULL,
    [BenchMark]          FLOAT (53)    NULL,
    CONSTRAINT [PK_T_BTUploadedBaskets] PRIMARY KEY CLUSTERED ([BasketID] ASC)
);

