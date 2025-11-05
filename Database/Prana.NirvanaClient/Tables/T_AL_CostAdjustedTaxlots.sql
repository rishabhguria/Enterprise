CREATE TABLE [dbo].[T_AL_CostAdjustedTaxlots] (
    [ID]               BIGINT           IDENTITY (1, 1) NOT NULL,
    [CostAdjustmentID] UNIQUEIDENTIFIER NOT NULL,
    [FKID]             BIGINT           NULL,
    [ParentRow_Pk]     BIGINT           NULL,
    [TaxlotID]         VARCHAR (50)     NULL,
    [GroupID]          VARCHAR (50)     NULL,
    [ClosingTaxlotID]  VARCHAR (50)     NULL,
    [ClosingID]        UNIQUEIDENTIFIER NULL,
    [UTCInsertionTime] DATETIME         CONSTRAINT [DF_PM_CostAdjustedTaxlots_InsertionTime] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_PM_CostAdjustedTaxlots] PRIMARY KEY CLUSTERED ([ID] ASC)
);

