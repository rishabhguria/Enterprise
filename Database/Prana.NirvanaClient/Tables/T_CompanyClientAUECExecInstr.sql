CREATE TABLE [dbo].[T_CompanyClientAUECExecInstr] (
    [CompanyClientExecInstrID] INT NOT NULL,
    [CompanyClientAUECID]      INT NOT NULL,
    [ExecutionInstructionsID]  INT NOT NULL,
    CONSTRAINT [PK_T_CompanyClientAUECExecInstr] PRIMARY KEY CLUSTERED ([CompanyClientExecInstrID] ASC),
    CONSTRAINT [FK_T_CompanyClientAUECExecInstr_T_ExecutionInstructions] FOREIGN KEY ([ExecutionInstructionsID]) REFERENCES [dbo].[T_ExecutionInstructions] ([ExecutionInstructionsID])
);

