CREATE TABLE [dbo].[T_SMCorporateactions] (
    [CorpActionID]        UNIQUEIDENTIFIER NOT NULL,
    [CorporateAction]     XML              NOT NULL,
    [EffectiveDate]       DATETIME         NOT NULL,
    [CorporateActionType] NCHAR (10)       NULL,
    [IsApplied]           BIT              NOT NULL,
    [Symbol]              VARCHAR (20)     NULL,
    [UTCInsertionTime]    DATETIME         NOT NULL,
    CONSTRAINT [PK_T_SMCorporateactions] PRIMARY KEY CLUSTERED ([CorpActionID] ASC)
);

