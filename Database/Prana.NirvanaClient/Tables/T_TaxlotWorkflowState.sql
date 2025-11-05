CREATE TABLE [dbo].[T_TaxlotWorkflowState] (
    [PK]              INT            IDENTITY (1, 1) NOT NULL,
    [TaxlotID]        NVARCHAR (100) NULL,
    [WorkflowStateID] INT            NULL,
    [LastUpdatedTime] DATETIME       NULL,
    [Comments]        NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_T_TaxlotWorkflowState] PRIMARY KEY CLUSTERED ([PK] ASC)
);

