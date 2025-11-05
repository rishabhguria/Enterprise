CREATE TABLE [dbo].[CorporateActionsTemporaryTest] (
    [EffectiveDate]         DATETIME         NULL,
    [OrigSymbol]            VARCHAR (100)    NULL,
    [NewSymbol]             VARCHAR (100)    NULL,
    [CorporateActionType]   VARCHAR (5)      NULL,
    [NewCompanyName]        VARCHAR (100)    NULL,
    [CorpActionID]          UNIQUEIDENTIFIER NULL,
    [Symbology]             VARCHAR (50)     NULL,
    [CorporateActionString] VARCHAR (MAX)    NULL
);

