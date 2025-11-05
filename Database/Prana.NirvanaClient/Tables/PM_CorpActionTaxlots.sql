CREATE TABLE [dbo].[PM_CorpActionTaxlots] (
    [Id]               BIGINT           IDENTITY (1, 1) NOT NULL,
    [CorpActionId]     UNIQUEIDENTIFIER NOT NULL,
    [FKId]             BIGINT           NULL,
    [ParentRow_Pk]     BIGINT           NULL,
    [TaxlotId]         VARCHAR (50)     NULL,
    [L1AllocationID]   VARCHAR (50)     NULL,
    [GroupId]          VARCHAR (50)     NULL,
    [ClosingTaxlotId]  VARCHAR (50)     NULL,
    [ClosingId]        UNIQUEIDENTIFIER NULL,
    [UTCInsertionTime] DATETIME         CONSTRAINT [DF_PM_CorpActionTaxlots_InsertionTime] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_PM_CorpActionTaxlots] PRIMARY KEY CLUSTERED ([Id] ASC)
);

