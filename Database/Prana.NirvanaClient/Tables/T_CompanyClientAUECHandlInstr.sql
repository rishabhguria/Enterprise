CREATE TABLE [dbo].[T_CompanyClientAUECHandlInstr] (
    [CompanyClientAUECHandlInstrID] INT NOT NULL,
    [CompanyClientAUECID]           INT NOT NULL,
    [HandlingInstructionsID]        INT NOT NULL,
    CONSTRAINT [PK_T_CompanyClientAUECHandlInstr] PRIMARY KEY CLUSTERED ([CompanyClientAUECHandlInstrID] ASC),
    CONSTRAINT [FK_T_CompanyClientAUECHandlInstr_T_HandlingInstructions] FOREIGN KEY ([HandlingInstructionsID]) REFERENCES [dbo].[T_HandlingInstructions] ([HandlingInstructionsID])
);

