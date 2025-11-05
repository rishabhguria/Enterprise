CREATE TABLE [dbo].[T_SMCorporateactions_Fix] (
    [CorpActionID]        UNIQUEIDENTIFIER NOT NULL,
    [CorporateAction]     VARCHAR (MAX)    NOT NULL,
    [EffectiveDate]       DATETIME         NOT NULL,
    [CorporateActionType] NCHAR (10)       NULL,
    [IsApplied]           BIT              NOT NULL,
    [Symbol]              VARCHAR (20)     NULL
);

